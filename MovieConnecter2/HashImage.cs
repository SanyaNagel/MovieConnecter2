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
    class HashImage
    {
        public HashImage()
        {

        }

        
        public long getHashScreen()
        {
            int sizeBitmap = 8;
            Bitmap bmpImg1 = getScreen();                    //Получаем скрин экрана
            Bitmap bmpImg = new Bitmap(bmpImg1, new Size(sizeBitmap, sizeBitmap));    //Уменьшаем размер
            bmpImg1.Dispose();   //Освобождение памяти
            
            int count = 0;
            Color color;
            BitArray bits = new BitArray(sizeBitmap * sizeBitmap);
            for (int j = 0; j < bmpImg.Height; j++)
            {
                for (int i = 0; i < bmpImg.Width; i++)
                {
                    color = bmpImg.GetPixel(i, j);
                    int K = (color.R + color.G + color.B) / 3;
                    bits[count++] = K <= 64 ? true : false;
                }
            }

            var bytes = new byte[8];
            bits.CopyTo(bytes, 0);
            var has = BitConverter.ToInt64(bytes, 0);
            
            bmpImg.Dispose();
            return has;
        }


        //Делаем и возращаем скриншот заданной области
        public Bitmap getScreen()
        {
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height); //PixelFormat.Format32bppArgb);
            Graphics graph = Graphics.FromImage(bmp);
            graph.CopyFromScreen(0, 0, 0, 0, bmp.Size);
            graph.Dispose();
            return bmp;
        }
    }
}
