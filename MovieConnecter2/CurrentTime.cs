using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.Util;
using Rectangle = System.Drawing.Rectangle;
using System.Windows.Forms;
using System.Diagnostics;
using Size = System.Drawing.Size;
using Image = System.Drawing.Image;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace MovieConnecter2
{
    class CurrentTime
    {
        //Флаг инициализации координат
        private bool firstInitCoord = true;
       
        //Размеры скрина
        public int Width;
        public int Height;
        
        //Кординаты начала скрина
        public int x;       
        public int y;

        public CurrentTime()
        {

        }


        //Задаём координаты 
        public void setCoord(int X, int Y)
        {
            if(firstInitCoord == true) //Если это первая инициализаци
            {
                firstInitCoord = false;
                x = X;
                y = Y;
            }
            else  //Если это вторая инициализация
            {
                firstInitCoord = true;  //Для повторной отправки кординат
                if (X > x && Y > y)
                {
                    Width = X - x;
                    Height = Y - y;
                }else if(X < x && Y < y)
                {
                    Width = x - X;
                    Height = y - Y;
                    x = X;
                    y = Y;
                }else if(X > x && Y < y)
                {
                    Width = X - x;
                    Height = y - Y;
                    y = Y;
                }else if(X < x && Y > y)
                {
                    Width = x - X;
                    Height = Y - y;
                    x = X;
                }else if(checkingTimer())  //Проверка на читаемость таймера
                {
                    Console.WriteLine("Отлично, таймер найден!");
                }
                else
                {
                    Console.WriteLine("Таймер не найден, повторите попытку!");
                }
            }
        }

        //Проверка на корректность чтения таймера со скрина
        public bool checkingTimer()
        {
            String time = readScreen(getScreen());

            //Здесь проверить на корректность

            return true;
        }

        //Делаем и возращаем скриншот заданной области
        public Bitmap getScreen()
        {
            Bitmap bmp = new Bitmap(Width, Height); //PixelFormat.Format32bppArgb);
            Graphics graph = Graphics.FromImage(bmp);
            graph.CopyFromScreen(x, y, 0, 0, bmp.Size);

            bmp.Save("filename.jpg");   ///////////////////Потом убрать///////////////////////

            graph.Dispose();
            return bmp;
        }

        public String readScreen(Bitmap bmp)
        {
            Tesseract tesseract = new Tesseract(@"C:\lang", "rus", OcrEngineMode.TesseractLstmCombined);
            tesseract.SetImage(bmp.ToImage<Bgr, byte>());
            tesseract.Recognize();
            String text = tesseract.GetUTF8Text();
            tesseract.Dispose();    //Очистка памяти
            return text;
        }
    }
}
