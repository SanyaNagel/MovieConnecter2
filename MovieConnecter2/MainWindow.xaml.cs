﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json.Linq;

namespace MovieConnecter2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);
        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);
        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        const int WH_KEYBOARD_LL = 13; // Номер глобального LowLevel-хука на клавиатуру
        const int WM_KEYDOWN = 0x100; // Сообщения нажатия клавиши
        private LowLevelKeyboardProc _proc = hookProc;
        private static IntPtr hhook = IntPtr.Zero;

        //private static CurrentTime currentTime = new CurrentTime();
        private static Process process;
        private static Thread myThread;
        private string hostLocal = "http://localhost:8080";
        private string hostGlobal = "https://movieconnecter.herokuapp.com";
        public string currentHost = "";
        public MainWindow()
        {
            InitializeComponent();
            currentHost = hostGlobal;
            IntPtr hInstance = LoadLibrary("User32");
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, hInstance, 0);
            process = new Process();
            process.mainWindow = this;
        }

        ~MainWindow()
        {
            UnhookWindowsHookEx(hhook);   //убираем хук
        }

        //Кнопка создания комнаты
        private void Button_Click(object sender, RoutedEventArgs e)
        {
             process.CreatRoom("Александр");
        }


        private static bool flagStart = true;
        public static IntPtr hookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                
                if(vkCode == 163)   //Если правый контрл
                {
                    //Получаем координаты курсора
                    //currentTime.setCoord(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
                }else if(vkCode == 161) //Нажат правый шифт
                {
                    if (flagStart == true)
                    {//Запуск отправки хешей
                        myThread = new Thread(process.startProcess); //Создаем новый объект потока (Thread)
                        myThread.Start(); //запускаем поток
                        flagStart = false;
                    }
                    else
                    {
                        flagStart = true;
                        myThread.Abort();
                    }
                }
                else
                {
                    return CallNextHookEx(hhook, code, (int)wParam, lParam);
                }

                return (IntPtr)1;
            }
            else
                return CallNextHookEx(hhook, code, (int)wParam, lParam);
        }

        //Кнопка отладки
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            debAsync();
        }
        //Отладка - отображение всех щэшей пользователей комнаты
        public async Task debAsync()
        {   
            WebRequest request = WebRequest.Create(currentHost+"/server/view/" + boxCodeRoom.Text);
            request.Method = "POST";
            WebResponse response = await request.GetResponseAsync();

          
            response.Close();
        }
       
        //Подключение к комнате и регистрация пользователя
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            connectUserToRoom();
        }

        //Подключение к комнате и регистрация пользователя
        public async Task connectUserToRoom()
        {
            WebRequest request = WebRequest.Create(currentHost+"/server/login/" + boxCodeRoom.Text + "/" + NameUser.Content);
            request.Method = "POST";
            WebResponse response = await request.GetResponseAsync();

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    JObject json = JObject.Parse(reader.ReadToEnd());
                    userID.Content = json["id"].ToString();
                }
            }
            response.Close();

        }
    }
}
    