using System.Collections.Generic;
using UnityEngine;

namespace Draw
{
    public class LineData
    {
        public float StartTime;
        public float EndTime;

        public List<Vector2Int> Points;

        public LineData()
        {
            Points = new List<Vector2Int>();
        }
    }
}