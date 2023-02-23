using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GridTools
{
    public class Tile
    {
        public Vector2 position { get; private set; }
        public int value { get; private set; }
        public bool isSelected { get; private set; }

        public Tile(int X, int Y, int val, bool isSelected)
        {
            position = new Vector2(X, Y);
            value = val;
            this.isSelected = isSelected;
        }

        public void SetSelected(bool isSelected)
        {
            this.isSelected = isSelected;
        }
    }

    public class Grid
    {
        private Tile[,] tiles;

        public Grid(Tile[] tiles)
        {
            this.tiles = TileArrayToGrid(tiles);
        }

        public Grid(int X, int Y)
        {
            this.tiles = new Tile[X, Y];
        }

        private Tile[,] TileArrayToGrid(Tile[] tiles)
        {
            int Y = (int)tiles.MaxBy(x => x.position.Y).position.Y;
            int X = (int)tiles.MaxBy(x => x.position.X).position.X;
            Tile[,] temp = new Tile[X,Y];

            foreach (Tile t in tiles)
            {
                temp[(int)t.position.X, (int)t.position.Y] = t;
            }

            return temp;
        }

        public Tile GetTile(int X, int Y)
        {
            return tiles[X, Y];
        }

        public void SetTile(int X, int Y, Tile tile)
        {
            this.tiles[X, Y] = tile;
        }

        public void DrawGrid(Graphics g, Vector2 StartPoint)
        {

        }
    }
}
