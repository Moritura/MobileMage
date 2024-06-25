using System.Collections.Generic;
using UnityEngine;

namespace Mage.Common
{
    public class LineData
    {
        #region Declaration

        public List<Vector2> Points { get; private set; }

        #endregion

        public LineData(List<Vector2> points) 
        {
            Points = points;
        }
    }
}