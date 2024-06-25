using Mage.Common;
using System;

namespace Mage.Draw
{
    public interface IDrawServiceInternal : IDrawService
    {
        event Action ClearDrawEvent;
        event Action<bool> BlockDrawEvent;

        void InputLine(LineData line);
        void StartLine();
    }
}