using Mage.Common;

namespace Mage.Figure
{
    public interface IFiguresComparison
    {
        float Comparison(FigureData target, FigureData figure);
        FigureData PrepareFigure(FigureData figure);
    }
}