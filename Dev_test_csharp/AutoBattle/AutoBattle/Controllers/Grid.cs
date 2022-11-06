using System;
using System.Collections.Generic;
using static AutoBattle.Types;
using AutoBattle.Utils;

namespace AutoBattle.Controllers
{
    public class Grid
    {
        public List<GridBox> grids = new List<GridBox>();
        public Vector2 gridSize;

        public Grid (int Lines, int Columns)
        {
            gridSize = new Vector2(Lines, Columns);

            for (int i = 0; i < Lines; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    GridBox newBox = new GridBox(j, i, false, (Columns * i + j));
                    grids.Add(newBox);
                }
            }
            Console.WriteLine($"The battle field has been created with {Lines * Columns} tiles with {Lines} lines and {Columns} columns.\n");
        }

        /// <summary>
        /// Prints the matrix that indicates the tiles of the battlefield
        /// </summary>
        public void DrawBattlefield()
        {
            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    GridBox currentGrid = grids[gridSize.y * i + j];
                    if (currentGrid.ocupied)
                    {
                        Console.Write("[ X ]\t");
                    }
                    else
                    {
                        Console.Write($"[ {currentGrid.index} ]\t");
                    }
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            Console.Write(Environment.NewLine + Environment.NewLine);
        }
    }
}
