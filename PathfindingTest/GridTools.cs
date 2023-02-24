﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GridTools
{
    public class Tile
    {
        public Point position { get; private set; }
        public int value { get; private set; }
        public bool isSelected { get; private set; }

        public Tile(int X, int Y, int val, bool isSelected)
        {
            position = new Point(X, Y);
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
        SolidBrush mainBrush = new SolidBrush(Color.DarkCyan);
        SolidBrush secondaryBrush = new SolidBrush(Color.DarkRed);
        SolidBrush fontBrush = new SolidBrush(Color.DarkGreen);

        Pen blackPen = new Pen(Color.Black, 3);

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

        public void SetTile(int X, int Y, Tile tile)
        {
            this.tiles[X, Y] = tile;
        }

        public void DrawGrid(Graphics g, Size size, Point StartPoint)
        {
            const int offset = 5;
            int GridColumnCount = tiles.GetLength(0);
            int GridRowCount = tiles.GetLength(1);
            int sizeX = (size.Width - StartPoint.X * 2 - (offset * (GridColumnCount - 1))) / GridColumnCount;
            int sizeY = (size.Height - StartPoint.Y * 2 - (offset * (GridRowCount - 1))) / GridRowCount;
            //Font f = new Font(SystemFonts.DefaultFont.FontFamily, sizeY / 3, FontStyle.Regular);

            
            Rectangle[] recs = new Rectangle[GridColumnCount * GridRowCount];
            //(Point, string)[] temp = new (Point, string)[tiles.Length];
            int id = 0;
            for (int i = 0; i < GridRowCount; i++)
            {
                for (int e = 0; e < GridColumnCount; e++)
                {
                    Point P = new Point(StartPoint.X + ((sizeX + offset) * e), StartPoint.Y + ((sizeY + offset) * i));
                    recs[id] = new Rectangle(P, new Size(sizeX, sizeY));
                    //temp[id] = (new Point(P.X, P.Y), tiles[i,e].value.ToString());
                    id++;
                }
            }
            g.FillRectangles(mainBrush, recs);
            g.DrawRectangles(blackPen, recs);

            //foreach((Point,string) t in temp)
            //{
            //    g.DrawString(t.Item2, f, fontBrush, t.Item1);
            //}
        }

        public void DrawTile(Tile tile, Color color, Graphics g, Size size, Point StartPoint)
        {
            const int offset = 5;
            int GridColumnCount = tiles.GetLength(0);
            int GridRowCount = tiles.GetLength(1);
            int sizeX = (size.Width - StartPoint.X * 2 - (offset * (GridColumnCount - 1))) / GridColumnCount;
            int sizeY = (size.Height - StartPoint.Y * 2 - (offset * (GridRowCount - 1))) / GridRowCount;

            Point P = new Point(StartPoint.X + ((sizeX + offset) * tile.position.X), StartPoint.Y + ((sizeY + offset) * tile.position.Y));

            g.FillRectangle(new SolidBrush(color), new Rectangle(P, new Size(sizeX, sizeY)));
            g.DrawRectangle(blackPen, new Rectangle(P, new Size(sizeX, sizeY)));
        }
    }
}
