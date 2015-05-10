#region

using System.Drawing;
using System.Drawing.Imaging;

#endregion

namespace terrain
{
    internal unsafe class BitmapBuffer
    {
        private readonly Bitmap bmp;
        private readonly int h;
        private readonly int w;
        private BitmapData dat;
        private byte* ptr;
        private int s;

        public BitmapBuffer(Bitmap bmp)
        {
            this.bmp = bmp;
            w = bmp.Width;
            h = bmp.Height;
        }

        public uint this[int x, int y]
        {
            get { return *(uint*) (ptr + x*4 + y*s); }
            set { *(uint*) (ptr + x*4 + y*s) = value; }
        }

        public void Lock()
        {
            dat = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);
            s = dat.Stride;
            ptr = (byte*) dat.Scan0;
        }

        public void Unlock()
        {
            bmp.UnlockBits(dat);
        }
    }
}