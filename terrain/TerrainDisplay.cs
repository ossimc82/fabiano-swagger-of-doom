#region

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace terrain
{
    internal class TerrainDisplay : Form
    {
        private readonly PictureBox pic;
        private readonly PictureBox pic2;
        private readonly TerrainTile[,] tilesBak;
        private Bitmap bmp;
        private Mode mode;
        private Panel panel;
        private TerrainTile[,] tiles;

        public TerrainDisplay(TerrainTile[,] tiles)
        {
            mode = Mode.Erase;
            tilesBak = (TerrainTile[,]) tiles.Clone();
            this.tiles = tiles;
            ClientSize = new Size(800, 800);
            BackColor = Color.Black;
            WindowState = FormWindowState.Maximized;
            panel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Controls =
                {
                    (pic = new PictureBox
                    {
                        Image = bmp = RenderColorBmp(tiles),
                        SizeMode = PictureBoxSizeMode.AutoSize,
                    })
                }
            };
            panel.HorizontalScroll.Enabled = true;
            panel.VerticalScroll.Enabled = true;
            panel.HorizontalScroll.Visible = true;
            panel.VerticalScroll.Visible = true;
            Controls.Add(panel);
            pic2 = new PictureBox
            {
                Image = bmp,
                Width = 250,
                Height = 250,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            Controls.Add(pic2);
            pic2.BringToFront();

            Text = mode.ToString();

            pic.MouseMove += pic_MouseMove;
            pic.MouseDoubleClick += pic_MouseDoubleClick;
        }

        private void pic_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tiles[e.X, e.Y].Region = TileRegion.Spawn;
            bmp.SetPixel(e.X, e.Y, Color.FromArgb((int) GetColor(tiles[e.X, e.Y])));
            pic.Invalidate();
            pic2.Invalidate();
        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (mode == Mode.Erase && (MouseButtons & MouseButtons.Left) != 0)
            {
                Point center = e.Location;
                for (int y = -10; y <= 10; y++)
                    for (int x = -10; x <= 10; x++)
                    {
                        if (x*x + y*y <= 10*10)
                        {
                            tiles[center.X + x, center.Y + y].Terrain = TerrainType.None;
                            tiles[center.X + x, center.Y + y].Elevation = 0;
                            tiles[center.X + x, center.Y + y].Biome = "ocean";
                            tiles[center.X + x, center.Y + y].TileObj = "";
                            tiles[center.X + x, center.Y + y].TileId = TileTypes.DeepWater;
                            tiles[center.X + x, center.Y + y].Region = TileRegion.None;
                            tiles[center.X + x, center.Y + y].Name = "";
                            bmp.SetPixel(center.X + x, center.Y + y, Color.FromArgb(
                                (int) GetColor(tiles[center.X + x, center.Y + y])));
                        }
                    }
                pic.Invalidate();
                pic2.Invalidate();
            }
            else if (mode == Mode.Average && (MouseButtons & MouseButtons.Left) != 0)
            {
                Point center = e.Location;
                Dictionary<TerrainTile, int> dict = new Dictionary<TerrainTile, int>();
                for (int y = -10; y <= 10; y++)
                    for (int x = -10; x <= 10; x++)
                        if (x*x + y*y <= 10*10)
                        {
                            TerrainTile t = tiles[center.X + x, center.Y + y];
                            if (dict.ContainsKey(t))
                                dict[t]++;
                            else
                                dict[t] = 0;
                        }
                int maxOccurance = dict.Values.Max();
                TerrainTile targetTile = dict.First(t => t.Value == maxOccurance).Key;
                for (int y = -10; y <= 10; y++)
                    for (int x = -10; x <= 10; x++)
                        if (x*x + y*y <= 10*10)
                        {
                            tiles[center.X + x, center.Y + y] = targetTile;
                            bmp.SetPixel(center.X + x, center.Y + y, Color.FromArgb(
                                (int) GetColor(tiles[center.X + x, center.Y + y])));
                        }
                pic.Invalidate();
                pic2.Invalidate();
            }
        }

        private static uint GetColor(TerrainTile tile)
        {
            if (tile.Region == TileRegion.Spawn)
                return 0xffff0000;
            return TileTypes.color[tile.TileId];
        }

        private static Bitmap RenderColorBmp(TerrainTile[,] tiles)
        {
            int w = tiles.GetLength(0);
            int h = tiles.GetLength(1);
            Bitmap bmp = new Bitmap(w, h);
            BitmapBuffer buff = new BitmapBuffer(bmp);
            buff.Lock();
            for (int y = 0; y < w; y++)
                for (int x = 0; x < h; x++)
                {
                    buff[x, y] = GetColor(tiles[x, y]);
                }
            buff.Unlock();
            return bmp;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) mode = mode + 1;
            if (mode == Mode.MAX) mode = 0;
            Text = mode.ToString();

            if (e.KeyCode == Keys.S)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "WMap files (*.wmap)|*.wmap|All Files (*.*)|*.*";
                if (sfd.ShowDialog() != DialogResult.Cancel)
                    WorldMapExporter.Export(tiles, sfd.FileName);
            }
            else if (e.KeyCode == Keys.R)
            {
                tiles = (TerrainTile[,]) tilesBak.Clone();
                bmp = RenderColorBmp(tiles);
                pic.Image = pic2.Image = bmp;
            }
            base.OnKeyUp(e);
        }

        private enum Mode
        {
            None,
            Erase,
            Average,
            MAX
        }
    }
}