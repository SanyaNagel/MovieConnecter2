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

        //Уменьшить размер
        //Убрать цвет - перевести изображение в градации серого
        //Вычислить среднее значение цвета для всех пикселей
        //Заменить пиксели на ч/б, в зависимости больше пиксель или меньше среднего
        //Построить хэш
        public override string getHash()
        {
            int sizeBitmap = 8;
            Bitmap bmpImg1 = getScreen();                    //Получаем скрин экрана
            Bitmap bmpImg = resizeImage(bmpImg1, sizeBitmap, sizeBitmap);    //Уменьшаем размер
            bmpImg1.Dispose();

            Color color;
            string hash = "";
            SetGrayscale(bmpImg);   //Преобразование в градации серого
            int middle = MiddleColor(bmpImg);
            for (int j = 0; j < bmpImg.Height; j++)
            {
                for (int i = 0; i < bmpImg.Width; i++)
                {
                    color = bmpImg.GetPixel(i, j);
                    int K = (color.R + color.G + color.B) / 3;
                    hash += K <= middle ? "1" : "0";
                }
            }
            bmpImg.Dispose();

            return hash;
        }


    }
}
