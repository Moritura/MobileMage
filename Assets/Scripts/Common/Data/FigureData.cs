using System.Collections.Generic;

namespace Mage.Common
{
    public class FigureData
    {
        #region Declaration

        public Key<FigureType> Key { get; private set; }
        public List<LineData> Lines { get; private set; }

        #endregion

        public FigureData(Key<FigureType> key, List<LineData> lines)
        {
            Key = key;
            Lines = lines;
        }
    }
}