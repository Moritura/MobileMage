using Mage.Common;
using System.Collections.Generic;

namespace Mage.Figure
{
    public class FigureService
    {
        #region Declaration

        private readonly List<FigureData> _figures;
        private readonly IFiguresComparison _figuresComparison;

        #endregion

        public FigureService(
            IFiguresComparison figuresComparison,
            IReadOnlyList<FigureData> figures)
        {
            _figuresComparison = figuresComparison;
            _figures = new List<FigureData>();

            foreach (FigureData figure in figures)
            {
                _figures.Add(_figuresComparison.PrepareFigure(figure));
            }
        }

        public (Key<FigureType>, float) CalculateSimilarFigure(FigureData target)
        {
            (Key<FigureType> Key, float Similarity) result = (null, float.MaxValue);
            target = _figuresComparison.PrepareFigure(target);

            foreach (FigureData figure in _figures)
            {
                float similarity = _figuresComparison.Copmarison(target, figure);
                if (similarity < result.Similarity)
                {
                    result.Key = figure.Key;
                    result.Similarity = similarity;
                }
            }

            return result;
        }
    }
}