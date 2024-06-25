using Cysharp.Threading.Tasks;
using Mage.Common;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Mage.Draw
{
    public class DrawService : IDrawServiceInternal, IDisposable
    {
        #region Declaration

        public event Action<FigureData> DrawEvent;

        public event Action ClearDrawEvent;
        public event Action<bool> BlockDrawEvent;

        private readonly int _maxCountLine;
        private readonly int _timeBetweenLines;

        private CancellationTokenSource _cancellationTokenSource;

        private FigureData _currentDraw;

        private int _countDrawingLine;

        #endregion

        public DrawService(
            int maxCountLine,
            float timeBetweenLines)
        {
            _maxCountLine = maxCountLine;
            _timeBetweenLines = (int)(timeBetweenLines * 1000);

            // TODO: generate new keys
            _currentDraw = new FigureData(new Key<FigureType>(FigureType.UserDraw, 0), new List<LineData>());
            _countDrawingLine = 0;
        }

        public void BlockDraw(bool isBlock)
        {
            BlockDrawEvent?.Invoke(isBlock);
        }

        public void StartLine()
        {
            if (_countDrawingLine == 0)
            {
                CancelAndDisposeToken();
            }

            _countDrawingLine++;
        }

        public void InputLine(LineData line)
        {
            _countDrawingLine--;
            _currentDraw.Lines.Add(line);
            if (_countDrawingLine == 0)
            {
                WaitNewLine(_cancellationTokenSource.Token).Forget();
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private async UniTask WaitNewLine(CancellationToken token)
        {
            if (_currentDraw.Lines.Count < _maxCountLine)
            {
                await UniTask.Delay(_timeBetweenLines);
                if (token.IsCancellationRequested)
                {
                    return;
                }
            }

            ClearDrawEvent?.Invoke();
            DrawEvent?.Invoke(_currentDraw);
            _currentDraw = new FigureData(new Key<FigureType>(FigureType.UserDraw, _currentDraw.Key.Id), new List<LineData>()); // TODO
        }

        private void CancelAndDisposeToken()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }
}