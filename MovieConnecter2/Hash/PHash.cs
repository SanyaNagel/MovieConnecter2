using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;

namespace MovieConnecter2.Hash
{
    class PHash : HashImage
    {
        public PHash() : base()
        {

        }

        //Уменьшить размер
        //Преобразовать в ЧБ
        //Запуск дискретное косинусное преобразование DCT
        //Сократить DCT
        //Вычислить среднее значение
        //Ещё сокращаем DCT
        //Строим ХЭШ
        public override string getHash()
        {
            return "0";
        }
    }
}

//Данный метод позволяет сравнивать изображения спомощью расстояния хэмминга
//
