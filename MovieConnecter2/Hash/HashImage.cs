using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieConnecter2
{
    abstract class HashImage
    {
        public HashImage()
        {

        }
        public abstract string getHash();

        //Делаем и возращаем скриншот заданной области
        protected Bitmap getScreen()
        {
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height); //PixelFormat.Format32bppArgb);
            Graphics graph = Graphics.FromImage(bmp);
            graph.CopyFromScreen(0, 0, 0, 0, bmp.Size);
            graph.Dispose();
            return bmp;
        }

        protected int MiddleColor(Bitmap bmpImg)
        {
            Color color;
            int midle = 0;
            for (int j = 0; j < bmpImg.Height; j++)
            {
                for (int i = 0; i < bmpImg.Width; i++)
                {
                    color = bmpImg.GetPixel(i, j);
                    midle += (color.R + color.G + color.B) / 3;
                }
            }
            return midle / (bmpImg.Width * bmpImg.Height);
        }

        protected void SetGrayscale(Bitmap bmap)
        {
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    byte gray = (byte)(.299 * c.R + .587 * c.G + .114 * c.B);

                    bmap.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
        }

        protected Bitmap resizeImage(Bitmap bmp, int x, int y)
        {
            return new Bitmap(bmp, new Size(x, y));
        }
    }
}
