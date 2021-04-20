using System;
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
using System.Windows.Forms;

namespace MovieConnecter2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

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
            //currentHost = hostLocal;
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
            process.CreateRoom("Александр");
        }


        private static bool flagStart = true;
        public static IntPtr hookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                
                if(vkCode == 163)   //Если правый контрл
                {
                    _ = process.debAsync();
                    //Получаем координаты курсора
                    //currentTime.setCoord(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
                }
                else if(vkCode == 161) //Нажат правый шифт
                {
                    if (flagStart == true)
                    {
                        _ = process.setReadyOnServer(true);             //Статус готовности "готов"    
                        myThread = new Thread(process.startProcess);    //Создаем новый объект потока (Thread)
                        myThread.Start(); //запускаем поток
                        flagStart = false;
                    }
                    else
                    {
                        flagStart = true;
                        _ = process.setReadyOnServer(false);    //Отмена готовности
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
            _ = process.debAsync();
        }
       
        //Подключение к комнате и регистрация пользователя
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            _ = process.connectUserToRoom();
        }

        internal Process _
        {
            get => default;
            set
            {
            }
        }
    }
}
    