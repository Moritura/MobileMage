using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShapeComparison : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public float CompareShapes(List<Vector2> shape1, List<Vector2> shape2)
    {
        int resolution = shape1.Count * shape2.Count;

        shape1 = Resample(shape1, resolution);
        shape2 = Resample(shape2, resolution);

        shape1 = CenterAtOrigin(shape1);
        shape2 = CenterAtOrigin(shape2);

        shape1 = ScaleToUnitSize(shape1);
        shape2 = ScaleToUnitSize(shape2);

        float bestDifference = float.MaxValue;

        for (int i = 0; i < resolution; i++)
        {
            List<Vector2> rotatedShape2 = RotateToMatch(shape1, shape2);
            float difference = ComputeDifference(shape1, rotatedShape2);

            if (difference < bestDifference)
            {
                bestDifference = difference;
            }

            shape2 = RotateByOnePoint(shape2);
        }

        _text.text = bestDifference.ToString();

        return bestDifference;
    }

    private List<Vector2> Resample(List<Vector2> shape, int n)
    {
        float I = PathLength(shape) / (n - 1);
        float D = 0;
        List<Vector2> resampled = new List<Vector2>();
        resampled.Add(shape[0]);

        for (int i = 1; i < shape.Count; i++)
        {
            Vector2 currentPoint = shape[i];
            Vector2 prevPoint = shape[i - 1];
            float d = Vector2.Distance(prevPoint, currentPoint);

            if (D + d >= I)
            {
                Vector2 q = prevPoint + ((I - D) / d) * (currentPoint - prevPoint);
                resampled.Add(q);
                shape.Insert(i, q);
                D = 0.0f;
            }
            else
            {
                D += d;
            }
        }

        if (resampled.Count < n) 
        {
            resampled.Add(shape[shape.Count - 1]);
        }

        return resampled;
    }

    private float PathLength(List<Vector2> shape)
    {
        float distance = 0;
        for (int i = 1; i < shape.Count; i++)
        {
            distance += Vector2.Distance(shape[i], shape[i - 1]);
        }
        return distance;
    }

    private List<Vector2> CenterAtOrigin(List<Vector2> shape)
    {
        Vector2 centroid = new Vector2();
        foreach (Vector2 point in shape)
        {
            centroid += point;
        }
        centroid /= shape.Count;

        List<Vector2> centeredShape = new List<Vector2>();
        foreach (Vector2 point in shape)
        {
            centeredShape.Add(point - centroid);
        }
        return centeredShape;
    }

    private List<Vector2> ScaleToUnitSize(List<Vector2> shape)
    {
        float scale = 0;
        foreach (Vector2 point in shape)
        {
            scale += point.sqrMagnitude;
        }
        scale = Mathf.Sqrt(scale / shape.Count);

        List<Vector2> scaledShape = new List<Vector2>();
        foreach (Vector2 point in shape)
        {
            scaledShape.Add(point / scale);
        }
        return scaledShape;
    }

    private List<Vector2> RotateToMatch(List<Vector2> shape1, List<Vector2> shape2)
    {
        float angle = Mathf.Atan2(shape2[0].y, shape2[0].x) - Mathf.Atan2(shape1[0].y, shape1[0].x);
        List<Vector2> rotatedShape = new List<Vector2>();

        foreach (Vector2 point in shape2)
        {
            float rotatedX = Mathf.Cos(angle) * point.x - Mathf.Sin(angle) * point.y;
            float rotatedY = Mathf.Sin(angle) * point.x + Mathf.Cos(angle) * point.y;
            rotatedShape.Add(new Vector2(rotatedX, rotatedY));
        }

        return rotatedShape;
    }

    private List<Vector2> RotateByOnePoint(List<Vector2> shape)
    {
        Vector2 firstPoint = shape[0];
        shape.RemoveAt(0);
        shape.Add(firstPoint);
        return shape;
    }

    private static float ComputeDifference(List<Vector2> shape1, List<Vector2> shape2)
    {
        float difference = 0;
        for (int i = 0; i < shape1.Count; i++)
        {
            difference += Vector2.SqrMagnitude(shape1[i] - shape2[i]);
        }
        return difference;
    }

    //private float ComputeDifference(List<Vector2> shape1, List<Vector2> shape2)
    //{
    //    float difference = 0;
    //    for (int i = 1; i < shape1.Count; i++)
    //    {
    //        difference += DistanceBetweenLines(shape1[i - 1], shape1[i], shape2[i - 1], shape2[i]);
    //    }

    //    return difference;
    //}

    //private float DistanceBetweenLines(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    //{
    //    float distanceA = PointToLineDistance(a1, a2, b1) + PointToLineDistance(a1, a2, b2);
    //    float distanceB = PointToLineDistance(b1, b2, a1) + PointToLineDistance(b1, b2, a2);

    //    return (distanceA + distanceB) * 0.5f;
    //}

    //private float PointToLineDistance(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
    //{
    //    float num = Mathf.Abs((lineEnd.y - lineStart.y) * point.x - (lineEnd.x - lineStart.x) * point.y + lineEnd.x * lineStart.y - lineEnd.y * lineStart.x);
    //    float den = Mathf.Sqrt((lineEnd.y - lineStart.y) * (lineEnd.y - lineStart.y) + (lineEnd.x - lineStart.x) * (lineEnd.x - lineStart.x));
    //    return num / den;
    //}
}