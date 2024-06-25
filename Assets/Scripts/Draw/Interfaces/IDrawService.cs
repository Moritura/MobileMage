using Mage.Common;
using System;

namespace Mage.Draw
{
    public interface IDrawService
    {
        event Action<FigureData> DrawEvent;
        void BlockDraw(bool isBlock);
    }
}