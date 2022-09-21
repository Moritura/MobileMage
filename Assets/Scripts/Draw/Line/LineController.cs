using System;
using UnityEngine;

namespace Draw
{
    public class LineController : MonoBehaviour
    {
        [SerializeField] private InputData input;

        private LineData data;

        public event Action<LineData> UpdateLineEvent;
        public event Action<LineData> EndLineEvent;

        private void Awake()
        {
            input.StartInputEvent += OnStartInput;
            input.InputEvent += OnInput;
            input.EndInputEvent += OnEndInput;
        }

        private void OnStartInput(Vector2Int point)
        {
            data = new LineData();
            data.Points.Add(point);
            data.StartTime = Time.time;
        }

        private void OnInput(Vector2Int point)
        {
            Vector2Int lastPoint = data.Points[data.Points.Count - 1];

            if (lastPoint == point) return;

            int deltaX = point.x - lastPoint.x;
            int deltaY = point.y - lastPoint.y;

            int count;
            int step;

            if (Math.Abs(deltaX) > Math.Abs(deltaY))
            {
                count = deltaX;
                step = Math.Sign(deltaX);
            }
            else
            {
                count = deltaY;
                step = Math.Sign(deltaY);
            }

            Vector2 stepVector = new Vector2((float)deltaX / (float)count * step, (float)deltaY / (float)count * step);

            Vector2 currentPoint = lastPoint;

            for (int i = 0; i != count; i += step)
            {
                currentPoint += stepVector;
                data.Points.Add(new Vector2Int(Mathf.RoundToInt(currentPoint.x), Mathf.RoundToInt(currentPoint.y)));
            }

            UpdateLineEvent?.Invoke(data);
        }

        private void OnEndInput()
        {
            data.EndTime = Time.time;
            EndLineEvent?.Invoke(data);
        }
    }
}