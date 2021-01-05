using System;
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

        //Делаем и возращаем скриншот заданной области
        public Bitmap getScreen()
        {
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height); //PixelFormat.Format32bppArgb);
            Graphics graph = Graphics.FromImage(bmp);
            graph.CopyFromScreen(0, 0, 0, 0, bmp.Size);
            //Invert(bmp);
            //bmp.Save("filename.jpg");   ///////////////////Потом убрать///////////////////////
            graph.Dispose();
            return bmp;
        }
        
        public int getHashScreen()
        {
            Bitmap bmpImg = getScreen();                    //Получаем скрин экрана
            bmpImg = new Bitmap(bmpImg, new Size(8, 8));    //Уменьшаем размер
            
            Bitmap result = new Bitmap(bmpImg.Width, bmpImg.Height);
            Color color = new Color();
            for (int j = 0; j < bmpImg.Height; j++)
            {
                for (int i = 0; i < bmpImg.Width; i++)
                {
                    color = bmpImg.GetPixel(i, j);
                    int K = (color.R + color.G + color.B) / 3;
                    result.SetPixel(i, j, K <= 64 ? Color.Black : Color.White);
                }
            }


            //////////////Нужно исправить получение хэша///////////////
            ///
            int has = result.GetHashCode();
            bmpImg.Dispose();
            result.Dispose();
            return has;
        }

        public Bitmap Invert(Bitmap bitmap)
        {
            int X;
            int Y;
            for (X = 0; X < bitmap.Width; X++)
            {
                for (Y = 0; Y < bitmap.Height; Y++)
                {
                    Color oldColor = bitmap.GetPixel(X, Y);
                    Color newColor;
                    newColor = Color.FromArgb(oldColor.A, 255 - oldColor.R, 255 - oldColor.G, 255 - oldColor.B);
                    bitmap.SetPixel(X, Y, newColor);
                    Application.DoEvents();
                }
            }
            return bitmap;
        }
    }
}
