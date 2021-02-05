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
    public delegate Task<JObject> BinaryOp(long hash);

    class Process
    {
        public HashImage hash = new HashImage();
        public bool isProcess = true;
        public MainWindow mainWindow;
        
        public Process()
        {
            
        }

        public void startProcess()
        {
            mainWindow.Dispatcher.Invoke(() =>
            {
                mainWindow.Status.Content = "Отправка";
            });

            // Асинхронный вызов метода с применением делегата
            BinaryOp bo = setHashOnServerAsync;

            while (isProcess)
            {
                long has = hash.getHashScreen();

                IAsyncResult ar = bo.BeginInvoke(has, null, null);
                Task<JObject> result = bo.EndInvoke(ar);
                //Console.WriteLine(result.Result["command"].ToString());
            }
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
    }
}
