using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieConnecter2
{
    //Класс для связи с API сервера

    public delegate Task<JObject> BinaryOp(long hash);
    class Process
    {
        public HashImage hash = new HashImage();
        public bool isProcess = true;
        public MainWindow mainWindow;
        public string currentCommand = "Ожидаем готовности всех";
        
        public Process()
        {
            
        }

        //Процесс отправки хэшей текущих изображений
        public void startProcess()
        {
            while (isProcess)
            {
                switch (currentCommand)
                {
                    case "Ожидаем готовности всех":
                        setHash(-1);
                        break;

                    case "Кидай хэш":
                        long has = hash.getHashScreen();
                        Console.WriteLine(has);
                        setHash(has);
                        break;
                }

                mainWindow.Dispatcher.Invoke(() =>
                {
                    mainWindow.Status.Content = currentCommand;
                });
            }
        }

        public void setHash(long has)
        {
            // Асинхронный вызов метода с применением делегата
            BinaryOp bo = setHashOnServerAsync;
            
            IAsyncResult ar = bo.BeginInvoke(has, null, null);
            Task<JObject> result = bo.EndInvoke(ar);
            currentCommand = result.Result["command"].ToString();;
            //Thread.Sleep(500);  //Ожидание пол секунды

                //Console.WriteLine(result.Result["command"].ToString());
            
        }


        //Создаём комнату получаем свой ID и код комнаты
        public async void CreatRoom(String name)
        {
            WebRequest request = WebRequest.Create(mainWindow.currentHost + "/server/creatRoom/" + name);
            request.Method = "POST";
            WebResponse response = await request.GetResponseAsync();

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    JObject json = JObject.Parse(reader.ReadToEnd());
                    mainWindow.boxCodeRoom.Text = json["code"].ToString();
                    mainWindow.userID.Content = json["id"].ToString();

                }
            }
            response.Close();
        }

        //Подключение к комнате и регистрация пользователя
        public async Task connectUserToRoom()
        {
            WebRequest request = WebRequest.Create(mainWindow.currentHost + "/server/login/" + mainWindow.boxCodeRoom.Text + "/" + mainWindow.NameUser.Content);
            request.Method = "POST";
            WebResponse response = await request.GetResponseAsync();

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    JObject json = JObject.Parse(reader.ReadToEnd());
                    mainWindow.userID.Content = json["id"].ToString();
                }
            }
            response.Close();
        }

        //Отправка статуса готовности
        public async Task setReadyOnServer(bool ready)
        {
            string zapros = "";
            mainWindow.Dispatcher.Invoke(() =>
            {
                zapros = mainWindow.currentHost + "/server/ready/" + mainWindow.boxCodeRoom.Text + "/" + mainWindow.userID.Content + "/" + ready;
            });

            JObject command;
            WebRequest request = WebRequest.Create(zapros);
            request.Method = "PUT";
            WebResponse response = await request.GetResponseAsync();
          
            response.Close();
        }


        //Полностью асинхронно отправляем хэш
        public async Task<JObject> setHashOnServerAsync(long hash)
        {
            string zapros = "";
            mainWindow.Dispatcher.Invoke(() =>
            {
                zapros = mainWindow.currentHost + "/server/hash/" + mainWindow.boxCodeRoom.Text + "/" + mainWindow.userID.Content + "/" + hash;
            });

            JObject command;
            WebRequest request = WebRequest.Create(zapros);
            request.Method = "POST";
            WebResponse response = await request.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    command = JObject.Parse(reader.ReadToEnd());
                }
            }
            response.Close();
            return command;
        }

        public void stopProcess()
        {
            isProcess = false;
        }


        //Отладка - отображение всех щэшей пользователей комнаты
        public async Task debAsync()
        {
            WebRequest request = WebRequest.Create(mainWindow.currentHost + "/server/view/" + mainWindow.boxCodeRoom.Text);
            JObject debux;
            request.Method = "POST";
            WebResponse response = await request.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    debux = JObject.Parse(reader.ReadToEnd());
                }
            }
            response.Close();

            mainWindow.Dispatcher.Invoke(() =>
            {
                mainWindow.debugView.Text = debux["debux"].ToString();
            });

            response.Close();
        }
    }
}
