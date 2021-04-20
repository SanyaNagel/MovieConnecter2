using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;

namespace MovieConnecter2.Hash
{

    class SimpleHash : HashImage
    {
        public SimpleHash() : base()
        {

        }

        public override long getHash()
        {
            int sizeBitmap = 8;
            Bitmap bmpImg1 = getScreen();                    //Получаем скрин экрана
            Bitmap bmpImg = resizeImage(bmpImg1, sizeBitmap, sizeBitmap);    //Уменьшаем размер
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

    }
}
