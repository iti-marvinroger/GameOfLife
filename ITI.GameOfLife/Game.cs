using System;

namespace ITI.GameOfLife
{
    public class Game
    {
        bool[,] _grid;
        public Game( int width, int height )
        {
            if (width <= 0 || height <= 0) throw new ArgumentException("The grid must have positive dimensions.");
            _grid = new bool[width, height];
        }

        public int Width
        {
            get { return _grid.GetLength(0); }
        }

        public int Height
        {
            get { return _grid.GetLength(1); }
        }

        bool CoordinatesExist(int x, int y)
        {
            return (x >= 0 && x <= Width - 1) && (y >= 0 && y <= Height - 1);
        }

        public bool IsAlive( int x, int y )
        {
            if (!CoordinatesExist(x, y)) throw new ArgumentException("The coordinates must exist on the grid.");
            return _grid[x, y];
        }

        public void GiveLive( int x, int y )
        {
            if (!CoordinatesExist(x, y)) throw new ArgumentException("The coordinates must exist on the grid.");
            _grid[x, y] = true;
        }

        public void Kill( int x, int y )
        {
            if (!CoordinatesExist(x, y)) throw new ArgumentException("The coordinates must exist on the grid.");
            _grid[x, y] = false;
        }

        struct Coordinate
        {
            public int x;
            public int y;
        }

        int Modulo (int first, int second)
        {
            int result = first % second;
            if (result < 0) return second + result;
            return result;
        }

        Coordinate[] GetNeighbors(int x, int y)
        {
            Coordinate[] neighbors = new Coordinate[8];
            int neighborIndex = 0;

            for (int offsetX = -1; offsetX <= 1; offsetX++)
            {
                for (int offsetY = -1; offsetY <= 1; offsetY++)
                {
                    if (offsetX == 0 && offsetY == 0) continue;

                    Coordinate neighbor = new Coordinate { x = Modulo(x + offsetX, Width), y = Modulo(y + offsetY, Height) };
                    
                    neighbors[neighborIndex++] = neighbor;
                }
            }

            return neighbors;
        }

        public bool NextTurn()
        {
            bool stateChanged = false;

            bool[,] tmpGrid = new bool[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Coordinate[] neighbors = GetNeighbors(x, y);
                    int aliveNeighbors = 0;

                    for (int iNeighbor = 0; iNeighbor < neighbors.Length; iNeighbor++)
                    {
                        if (IsAlive(neighbors[iNeighbor].x, neighbors[iNeighbor].y)) aliveNeighbors++;
                    }

                    if (!IsAlive(x, y) && aliveNeighbors == 3)
                    {
                        stateChanged = true;
                        tmpGrid[x, y] = true;
                    }
                    else if (IsAlive(x, y) && (aliveNeighbors < 2 || aliveNeighbors > 3))
                    {
                        stateChanged = true;
                        tmpGrid[x, y] = false;
                    } else
                    {
                        tmpGrid[x, y] = _grid[x, y];
                    }
                }
            }

            _grid = tmpGrid;

            return stateChanged;
        }
    }
}
