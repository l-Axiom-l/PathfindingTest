using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GridTools
{
    public delegate void TileChangeEvent(Tile tile);

    public class Tile
    {
        public Point position { get; private set; }
        public int value { get; private set; }
        public bool isSelected { get; private set; }


        public event TileChangeEvent Changed;

        public Tile(int X, int Y, int val, bool isSelected)
        {
            position = new Point(X, Y);
            value = val != null ? val : 0;
            this.isSelected = isSelected;
        } 

        public void SetSelected(bool isSelected)
        {
            this.isSelected = isSelected;
            Changed?.Invoke(this);
        }
    }

    public class Grid
    {
        private Tile[,] tiles;
        const int offset = 5;

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
            int Y = (int)tiles.MaxBy(x => x.position.Y).position.Y + 1;
            int X = (int)tiles.MaxBy(x => x.position.X).position.X + 1;
            Debug.WriteLine(Y + " " + X);
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

        public Tile GetTile(Point point)
        {
            if (tiles.GetLength(0) <= point.X || tiles.GetLength(1) <= point.Y)
                return new Tile(0,0,9999, false);

            return tiles[point.X, point.Y];
        }

        public void SetTile(int X, int Y, Tile tile)
        {
            this.tiles[X, Y] = tile;
        }

        public void SetTile(Point point, Tile tile)
        {
            this.tiles[point.X, point.Y] = tile;
        }

        public void DrawGrid(Graphics g, Size size, Point StartPoint)
        {
            int GridColumnCount = tiles.GetLength(0);
            int GridRowCount = tiles.GetLength(1);
            int sizeX = (size.Width - StartPoint.X * 2 - (offset * (GridColumnCount - 1))) / GridColumnCount;
            int sizeY = (size.Height - StartPoint.Y * 2 - (offset * (GridRowCount - 1))) / GridRowCount;

            
            Rectangle[] recs = new Rectangle[GridColumnCount * GridRowCount];
            int id = 0;
            for (int i = 0; i < GridRowCount; i++)
            {
                for (int e = 0; e < GridColumnCount; e++)
                {
                    Point P = new Point(StartPoint.X + ((sizeX + offset) * e), StartPoint.Y + ((sizeY + offset) * i));
                    recs[id] = new Rectangle(P, new Size(sizeX, sizeY));
                    id++;
                }
            }
            g.FillRectangles(GridBrushes.mainBrush, recs);
            g.DrawRectangles(GridBrushes.blackPen, recs);
        }

        public void DrawTile(Tile tile, Color color, Graphics g, Size size, Point StartPoint)
        {
            int GridColumnCount = tiles.GetLength(0);
            int GridRowCount = tiles.GetLength(1);
            int sizeX = (size.Width - StartPoint.X * 2 - (offset * (GridColumnCount - 1))) / GridColumnCount;
            int sizeY = (size.Height - StartPoint.Y * 2 - (offset * (GridRowCount - 1))) / GridRowCount;

            Point P = new Point(StartPoint.X + ((sizeX + offset) * tile.position.X), StartPoint.Y + ((sizeY + offset) * tile.position.Y));

            g.FillRectangle(new SolidBrush(color), new Rectangle(P, new Size(sizeX, sizeY)));
            g.DrawRectangle(GridBrushes.blackPen, new Rectangle(P, new Size(sizeX, sizeY)));

            if (tile.isSelected)
                CrossTile(tile, g, size, StartPoint);
        }

        public void CrossTile(Tile tile, Graphics g, Size size, Point StartPoint)
        {
            int GridColumnCount = tiles.GetLength(0);
            int GridRowCount = tiles.GetLength(1);
            int sizeX = (size.Width - StartPoint.X * 2 - (offset * (GridColumnCount - 1))) / GridColumnCount;
            int sizeY = (size.Height - StartPoint.Y * 2 - (offset * (GridRowCount - 1))) / GridRowCount;
            Point P = new Point(StartPoint.X + ((sizeX + offset) * tile.position.X), StartPoint.Y + ((sizeY + offset) * tile.position.Y));

            g.DrawLine(GridBrushes.blackPen, P, new Point(P.X + sizeX, P.Y + sizeY));
            g.DrawLine(GridBrushes.blackPen, new Point(P.X, P.Y + sizeY), new Point(P.X + sizeX, P.Y));
        }
    }

    public readonly struct GridBrushes
    {
        public GridBrushes() { }

        public static readonly SolidBrush mainBrush = new SolidBrush(Color.DarkCyan);
        public static readonly SolidBrush secondaryBrush = new SolidBrush(Color.DarkRed);
        public static readonly SolidBrush fontBrush = new SolidBrush(Color.DarkGreen);
        public static readonly Pen blackPen = new Pen(Color.Black, 3);
    }

    public class TileChangeEventArgs : EventArgs
    {
        public Tile tile { get; private set; }
        public TileChangeEventArgs(Tile tile)
        {
            this.tile = tile;
        }
    }
}
