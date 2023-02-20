using System.Diagnostics;
using GridTools;

namespace PathfindingTest
{
    public partial class Form1 : Form
    {
        Graphics g;
        Tile[] tiles;
        public Form1()
        {
            InitializeComponent();
            GenerateTable();
        }

        void GenerateTable()
        {
            string d = GetDungeon();
            d = d.Replace("[", "").Replace("]", "_");
            List<string> temp = d.Split("_").ToList();
            Debug.WriteLine(temp.Count);

            List<string> removeList = new List<string>();
            for (int i = 0; i < temp.Count; i++)
            {
                Debug.WriteLine(i);
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
                    Tile tile = new Tile(e, i, int.Parse(s[e]));
                    tiles.Add(tile);
                }
            }

            return tiles.ToArray();
        }

        string GetDungeon()
        {
            return File.ReadAllLines("C:\\Programmieren\\Test\\Dungeon.txt")[0];
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ButtonImport_Click(object sender, EventArgs e)
        {

        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            Solver solver = new Solver(tiles, new System.Numerics.Vector2(), new System.Numerics.Vector2());
            solver.FormBorderStyle= FormBorderStyle.Fixed3D;
            solver.Show();
        }
    }
}