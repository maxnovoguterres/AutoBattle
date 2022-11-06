using AutoBattle.Controllers;

namespace AutoBattle.Managers
{
    public static class GridManager
    {
        private static Grid Grid;

        public static void CreateGrid (int gridSizeX, int gridSizeY) => Grid = new Grid(gridSizeX, gridSizeY);

        public static Grid GetGrid () => Grid;
    }
}
