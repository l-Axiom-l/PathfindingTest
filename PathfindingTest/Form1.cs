using System.Diagnostics;
using GridTools;

namespace PathfindingTest
{
    public partial class Form1 : Form
    {
        Graphics g;
        Tile[] tiles;
        Point start;
        Point end;
        string DungeonPath = "";
        public Form1()
        {
            InitializeComponent();
        }

        void GenerateTable()
        {
            tiles = null;
            table.Rows.Clear();
            table.Columns.Clear();
            string d = GetDungeon();
            d = d.Replace("[", "").Replace("]", "_");
            List<string> temp = d.Split("_").ToList();

            List<string> removeList = new List<string>();
            for (int i = 0; i < temp.Count; i++)
            {
                string s = temp[i];

                s = s.Trim(',');
                temp[i] = s;

                if (s.Length < 1)
                {
                    removeList.Add(temp[i]);
                }
            }

            foreach(string s in removeList)
            {
                temp.Remove(s);
            }

            for (int i = 0; i < temp[0].Split(",").Length; i++)
            {
                table.Columns.Add(i.ToString(), i.ToString());
            }

            for (int i = 0; i < temp.Count; i++)
            {
                table.Rows.Add(temp[i].Split(","));
            }

            tiles = GenerateTilesArray(temp.ToArray());
        }

        Tile[] GenerateTilesArray(string[] temp)
        {
            List<Tile> tiles = new List<Tile>();

            for (int i = 0; i < temp.Length; i++)
            {
                string[] s = temp[i].Split(",");

                for (int e = 0; e < s.Length; e++)
                {
                    Tile tile = new Tile(e, i, int.Parse(s[e]), false);
                    tiles.Add(tile);
                }
            }

            return tiles.ToArray();
        }

        string GetDungeon()
        {
            return File.ReadAllLines(DungeonPath)[0];
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ButtonImport_Click(object sender, EventArgs ea)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DungeonPath = openFileDialog.FileName;

                string[] temp = File.ReadAllLines(DungeonPath);
                string[] s = temp[1].Split(",");
                string[] e = temp[2].Split(",");
                start = new Point(int.Parse(s[0]), int.Parse(s[1]));
                end = new Point(int.Parse(e[0]), int.Parse(e[1]));

                GenerateTable();
            }

        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            Solver solver = new Solver(tiles, start, end);
            solver.FormBorderStyle= FormBorderStyle.Fixed3D;
            solver.Show();
        }
    }
}