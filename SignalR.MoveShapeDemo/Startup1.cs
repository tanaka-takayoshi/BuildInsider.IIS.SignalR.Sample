using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SignalR.MoveShapeDemo.Startup1))]
namespace SignalR.MoveShapeDemo
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            //この1行を追加。サーバー名はAzureで指定したDNS。
            //デフォルト設定ではパスワードなし。
            //ポートはAzure上でPublicに公開したものを指定している
            GlobalHost.DependencyResolver.UseRedis("ubuntu-redis.cloudapp.net", 9826, "", "BuildInsider7");
            //SignalRの初期化。/signalr へのリクエストをSignalRで処理するようにする。
            app.MapSignalR();
        }
    }
}
