using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System.Threading;

namespace SignalR.MoveShapeDemo
{
    public class Broadcaster
    {
        private readonly static Lazy<Broadcaster> instance =
            new Lazy<Broadcaster>(() => new Broadcaster());
        // 全クライアントへのブロードキャスト送信間隔を設定
        private readonly TimeSpan broadcastInterval =
            TimeSpan.FromMilliseconds(40);
        private readonly IHubContext hubContext;
        private Timer broadcastLoop;
        private ShapeModel model;
        private bool modelUpdated;
        public Broadcaster()
        {
            hubContext = GlobalHost.ConnectionManager.GetHubContext<MoveShapeHub>();
            model = new ShapeModel();
            modelUpdated = false;
            // ブロードキャストを一定時間間隔で実行
            broadcastLoop = new Timer(
                BroadcastShape,
                null,
                broadcastInterval,
                broadcastInterval);
        }
        public void BroadcastShape(object state)
        {
            //変更があっとときのみ送信
            if (modelUpdated)
            {
                //クライアント側で定義しているメソッドをRPCスタイルで呼び出し
                //dynamic で定義されています
                hubContext.Clients.AllExcept(model.LastUpdatedBy).updateShape(model);
                modelUpdated = false;
            }
        }
        public void UpdateShape(ShapeModel clientModel)
        {
            model = clientModel;
            modelUpdated = true;
        }
        public static Broadcaster Instance
        {
            get
            {
                return instance.Value;
            }
        }
    }

    public class MoveShapeHub : Hub
    {
        private readonly Broadcaster broadcaster;
        public MoveShapeHub()
            : this(Broadcaster.Instance)
        {
        }
        public MoveShapeHub(Broadcaster broadcaster)
        {
            this.broadcaster = broadcaster;
        }
        public void UpdateModel(ShapeModel clientModel)
        {
            clientModel.LastUpdatedBy = Context.ConnectionId;
            broadcaster.UpdateShape(clientModel);
        }
    }

    /// <summary>
    /// クライアントとサーバーの間で送信するデータのクラス
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