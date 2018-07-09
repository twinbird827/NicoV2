using Codeplex.Data;
using NicoV2.Common;
using NicoV2.IO;
using NicoV2.Mvvm.Service;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.Model
{
    public class TemporaryModel : HttpModel
    {
        private TemporaryModel()
        {
            this.Method = "GET";
            this.ContentType = "application/x-www-form-urlencoded";

            Reload();
        }

        /// <summary>
        /// TemporaryModelｲﾝｽﾀﾝｽを取得します。
        /// </summary>
        public static TemporaryModel Instance { get; private set; } = new TemporaryModel();

        /// <summary>
        /// ｱｲﾃﾑ構成
        /// </summary>
        public ObservableSynchronizedCollection<VideoModel> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        private ObservableSynchronizedCollection<VideoModel> _Items = new ObservableSynchronizedCollection<VideoModel>();

        private void Reload()
        {
            // ﾛｸﾞｲﾝする。
            LoginModel.Instance.Login();

            // ﾛｸﾞｲﾝできなかった場合
            if (!LoginModel.Instance.IsLogin)
            {
                ServiceFactory.MessageService.Error("Login error");
            }

            string txt = GetSmileVideoHtmlText(Constants.DeflistList);

            if (IsError(txt))
            {
                return;
            }

            var json = DynamicJson.Parse(txt);

            Items.Clear();
            foreach (dynamic data in json["mylistitem"])
            {
                Items.Add(new VideoModel()
                {
                    ContentId = data["item_id"],
                    Title = data["item_data"]["title"],
                    Description = data["description"],
                    //Tags = data["tags"],
                    //CategoryTag = data["categoryTags"],
                    ViewCounter = long.Parse(data["item_data"]["view_counter"]),
                    MylistCounter = long.Parse(data["item_data"]["mylist_counter"]),
                    CommentCounter = long.Parse(data["item_data"]["num_res"]),
                    StartTime = Converter.FromUnixTime((long)data["create_time"]),
                    //LastCommentTime = Converter.ToDatetime(data["lastCommentTime"]),
                    LengthSeconds = long.Parse(data["item_data"]["length_seconds"]),
                    ThumbnailUrl = data["item_data"]["thumbnail_url"],
                    LastResBody = data["item_data"]["last_res_body"],
                    //CommunityIcon = data["communityIcon"]
                });
            }
        }

        public void Add(string id, string description = "")
        {
            if (!Items.Any(v => v.ContentId == id))
            {
                // とりあえずﾏｲﾘｽﾄに追加
                string url = string.Format(Constants.DeflistAdd, id.Split('/').Last(), description, LoginModel.Instance.Token);
                string txt = GetSmileVideoHtmlText(url);

                // 結果追加
                Items.Insert(0, new VideoModel(id));
            }
        }

        public void Delete(string id)
        {
            if (Items.Any(v => v.ContentId == id))
            {
                // とりあえずﾏｲﾘｽﾄから削除
                string url = string.Format(Constants.DeflistDel, id.Split('/').Last(), LoginModel.Instance.Token);
                string txt = GetSmileVideoHtmlText(url);

                Items.Remove(Items.First(v => v.ContentId == id));
            }
        }

        private bool IsError(string txt)
        {
            if (string.IsNullOrWhiteSpace(txt))
            {
                ServiceFactory.MessageService.Error("何らかの通信エラーです。");
                return true;
            }

            //var json = DynamicJson.Parse(txt);

            //if (json["meta"]["status"] == 400d)
            //{
            //    ServiceFactory.MessageService.Error("不正なパラメータです。");
            //    return true;
            //}

            //if (json["meta"]["status"] == 500d)
            //{
            //    ServiceFactory.MessageService.Error("検索サーバの異常です。");
            //    return true;
            //}

            //if (json["meta"]["status"] == 503d)
            //{
            //    ServiceFactory.MessageService.Error("サービスがメンテナンス中です。メンテナンス終了までお待ち下さい。");
            //    return true;
            //}

            //if (json["meta"]["status"] == 410d)
            //{
            //    ServiceFactory.MessageService.Error("検索結果が見つかりませんでした。");
            //    return true;
            //}

            //if (json["meta"]["status"] == 403d)
            //{
            //    ServiceFactory.MessageService.Error("非公開に設定されています。");
            //    return true;
            //}

            //if (json["meta"]["status"] != 200d)
            //{
            //    ServiceFactory.MessageService.Error("何らかの通信エラーです。");
            //    return true;
            //}

            return false;
        }

    }
}
