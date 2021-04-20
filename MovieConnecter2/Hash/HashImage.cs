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
        public abstract long getHash();

        //Делаем и возращаем скриншот заданной области
        protected Bitmap getScreen()
        {
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height); //PixelFormat.Format32bppArgb);
            Graphics graph = Graphics.FromImage(bmp);
            graph.CopyFromScreen(0, 0, 0, 0, bmp.Size);
            graph.Dispose();
            return bmp;
        }

        protected Bitmap resizeImage(Bitmap bmp, int x, int y)
        {
            return new Bitmap(bmp, new Size(x, y));
        }
    }
}
