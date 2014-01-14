using System;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;

namespace SignalRClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //サーバー名を指定
            using (var conn = new HubConnection("http://signalr-i1.cloudapp.net/"))
            {
                //サーバーで作成したHubの名前を指定
                var proxy = conn.CreateHubProxy("moveShapeHub");
                //受信する
                proxy.On<ShapeModel>("updateShape", m => Console.WriteLine(m.Left + ", " + m.Top));
                conn.Start().Wait();
                //Transportを指定することも可能
                //conn.Start(new LongPollingTransport()).Wait();

                //送信する
                var random = new Random();
                proxy.Invoke("updateModel", new ShapeModel()
                {
                    Top = random.NextDouble()*100,
                    Left = random.NextDouble()*400
                }).Wait();

                Console.ReadKey();

                // 終了
                conn.Stop();
            }
        }
    }

    /// <summary>
    /// サーバー側で定義しているのと同じ、やりとりするJSONデータの定義クラス。
    /// </summary>
    public class ShapeModel
    {
        [JsonProperty("left")]
        public double Left { get; set; }

        [JsonProperty("top")]
        public double Top { get; set; }

        [JsonIgnore]
        public string LastUpdatedBy { get; set; }
    }

}
