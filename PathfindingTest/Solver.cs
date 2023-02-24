using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        readonly Point drawPoint = new Point(10, 10);

        public Solver(Tile[] tiles, Point startPoint, Point EndPoint)
        {
            InitializeComponent();
            this.tiles = tiles;
            this.startPoint = startPoint;
            this.endPoint = EndPoint;
            grid = new Grid(tiles);
        }

        private void Solver_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            PaintGrid(tiles);
            Solve();
        }

        void PaintGrid(Tile[] tiles)
        {
            grid.DrawGrid(g, ClientSize, new Point(25, 25));
        }

        void Solve()
        {
            Tile tile = new Tile(startPoint.X, startPoint.Y, 0, false);
            Tile tile2 = new Tile(endPoint.X, endPoint.Y, 0, false);
            grid.DrawTile(tile, Color.Green, g, ClientSize, new Point(25, 25));
            grid.DrawTile(tile2, Color.Orange, g, ClientSize, new Point(25, 25));
        }
    }
}
