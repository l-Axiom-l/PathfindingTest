using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GridTools;

namespace PathfindingTest
{
    public partial class Solver : Form
    {
        [AllowNull] Graphics g;
        Grid grid;
        public Tile[] tiles { get; private set; }
        public Point startPoint { get; private set; }
        public Point endPoint { get; private set; }
        readonly Point drawPoint = new Point(25, 25);

        public Solver(Tile[] tiles, Point startPoint, Point EndPoint)
        {
            InitializeComponent();
            this.tiles = tiles;
            this.startPoint = startPoint;
            this.endPoint = EndPoint;
            grid = new Grid(tiles);
            foreach (Tile t in tiles) t.Changed += TileChanged;
        }

        private void Solver_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            PaintGrid(tiles);
            Solve();
        }

        void PaintGrid(Tile[] tiles)
        {
            grid.DrawGrid(g, ClientSize, drawPoint);
            Tile tile = new Tile(startPoint.X, startPoint.Y, 0, false);
            Tile tile2 = new Tile(endPoint.X, endPoint.Y, 0, false);
            grid.DrawTile(tile, Color.Green, g, ClientSize, drawPoint);
            grid.DrawTile(tile2, Color.Orange, g, ClientSize, drawPoint);
        }

        public void TileChanged(Tile tile)
        {
            if (tile.isSelected)
                grid.CrossTile(tile, g, ClientSize, drawPoint);
            else
                grid.DrawTile(tile, GridBrushes.mainBrush.Color, g, ClientSize, drawPoint);
        }

        void Solve()
        {
            List<Tile> Path = new List<Tile>();
            int currentPathCost = 0;
            int minimalHeatlh = 1;
            Point CurrentTile = startPoint;
            Point target = endPoint;
            Tile[] moveableTiles = new Tile[2];

            List<WaveFunction> wf = new List<WaveFunction>();
            foreach(Tile t in tiles)
            {
                WaveFunction temp = new WaveFunction(t.value, t.position);
                wf.Add(temp);
            }

            WaveFunctionCollapseHandler WFC = new WaveFunctionCollapseHandler(wf.ToArray());

            Path.Add(grid.GetTile(CurrentTile));
            while (true)
            {
                ////moveableTiles[0] = grid.GetTile(new Point(CurrentTile.X + 1, CurrentTile.Y));
                ////moveableTiles[1] = grid.GetTile(new Point(CurrentTile.X, CurrentTile.Y + 1));

                ////moveableTiles[0] ??= new Tile(0, 0, 9999, false);
                ////moveableTiles[1] ??= new Tile(0, 0, 9999, false);

                ////Tile temp = moveableTiles.MinBy(x => x.value);
                ////Debug.WriteLine(temp.value);
                ////CurrentTile = temp.position;

                CurrentTile = WFC.GetNextPoint();
                Tile temp = grid.GetTile(CurrentTile);
                Path.Add(temp);
                grid.DrawTile(temp, Color.DarkRed, g, ClientSize, drawPoint);
                //Invoke(() => temp.SetSelected(true));

                if (CurrentTile.Equals(target) || WFC.waveFunctionCollapsed)
                    break;
            }

            foreach(Point p in WFC.GetBestPath(endPoint))
            {
                grid.CrossTile(grid.GetTile(p), g, ClientSize, drawPoint);
            }

            grid.DrawTile(new Tile(startPoint.X, startPoint.Y, 0, false), Color.Green, g, ClientSize, drawPoint);
            grid.DrawTile(new Tile(endPoint.X, endPoint.Y, 0, false), Color.Orange, g, ClientSize, drawPoint);
        }
    }
}
