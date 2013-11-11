using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SignalR.MoveShapeDemo.Startup1))]

namespace SignalR.MoveShapeDemo
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            // アプリケーションの設定方法の詳細については、http://go.microsoft.com/fwlink/?LinkID=316888 を参照してください
            //SignalRの初期化。/signalr へのリクエストをSignalRで処理するようにする。
            app.MapSignalR();
        }
    }
}
