using Mage.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mage.Figure
{
    public class FiguresComparison : IFiguresComparison
    {
        public FiguresComparison()
        {
        }

        public FigureData PrepareFigure(FigureData figure)
        {
            var normalizedLines = NormalizeFigure(figure.Lines);
            return new FigureData(figure.Key, normalizedLines);
        }

        private List<LineData> NormalizeFigure(List<LineData> lines)
        {
            Vector2 centroid = new Vector2();
            float scale = 0;
            int totalPoints = 0;

            foreach (var line in lines)
            {
                foreach (var point in line.Points)
                {
                    centroid += point;
                    scale += point.sqrMagnitude;
                    totalPoints++;
                }
            }

            centroid /= totalPoints;
            scale = Mathf.Sqrt(scale / totalPoints);

            var normalizedLines = new List<LineData>();
            foreach (var line in lines)
            {
                var normalizedPoints = new List<Vector2>();
                foreach (var point in line.Points)
                {
                    Vector2 centeredPoint = point - centroid;
                    normalizedPoints.Add(centeredPoint / scale);
                }
                normalizedLines.Add(new LineData(normalizedPoints));
            }
            return normalizedLines;
        }


        public float Comparison(FigureData target, FigureData figure)
        {
            if (target == null || figure == null)
                throw new ArgumentNullException("Both figures must be provided.");

            target = PrepareFigure(target);
            figure = PrepareFigure(figure);

            if (target.Lines.Count == 0 || figure.Lines.Count == 0)
                throw new ArgumentException("Figures should have at least one line.");

            // Assuming only one line for simplicity. For multi-line, you will iterate over all the lines.
            List<Vector2> targetPoints = target.Lines[0].Points;
            List<Vector2> figurePoints = figure.Lines[0].Points;

            if (targetPoints.Count != figurePoints.Count)
                return float.MaxValue;  // Can't compare figures with different point counts.

            float minDifference = float.MaxValue;

            for (int i = 0; i < targetPoints.Count; i++)
            {
                float currentDifference = 0;
                for (int j = 0; j < targetPoints.Count; j++)
                {
                    int figureIndex = (i + j) % targetPoints.Count;
                    Vector2 difference = targetPoints[j] - figurePoints[figureIndex];
                    currentDifference += difference.sqrMagnitude;
                }
                minDifference = Mathf.Min(minDifference, currentDifference);
            }

            return minDifference;
        }

    }
}