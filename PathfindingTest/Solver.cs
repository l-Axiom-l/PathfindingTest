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
        public Tile[] tiles { get; private set; }
        public Vector2 startPoint { get; private set; }
        public Vector2 endPoint { get; private set; }
        readonly Vector2 drawPoint = new Vector2(10, 10);

        public Solver(Tile[] tiles, Vector2 startPoint, Vector2 EndPoint)
        {
            InitializeComponent();
            this.tiles = tiles;
            this.startPoint = startPoint;
            this.endPoint = EndPoint;
        }

        private void Solver_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            PaintGrid(tiles);
        }

        void PaintGrid(Tile[] tiles)
        {
            Brush b = new SolidBrush(Color.PaleVioletRed);

        }
    }
}
