using Codeplex.Data;
using NicoV2.IO;
using NicoV2.Mvvm.Service;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NicoV2.Mvvm.Model
{
    public class MylistOfMeModel : HttpModel
    {
        private MylistOfMeModel()
        {
            this.Method = "GET";
            this.ContentType = "application/x-www-form-urlencoded";
        }

        public static MylistOfMeModel Instance { get; private set; } = new MylistOfMeModel();

        public ObservableSynchronizedCollection<MylistModel> Reload()
        {
            // ﾛｸﾞｲﾝする。
            LoginModel.Instance.Login();

            // ﾛｸﾞｲﾝできなかった場合
            if (!LoginModel.Instance.IsLogin)
            {
                ServiceFactory.MessageService.Error("Login error");
            }

            var items = new ObservableSynchronizedCollection<MylistModel>();
            var url = string.Format(Constants.MylistOfMe);
            var txt = GetSmileVideoHtmlText(url);

            ServiceFactory.MessageService.Debug(txt);

            if (IsError(txt))
            {
                return items;
            }

            var json = DynamicJson.Parse(txt);

            foreach (dynamic data in json["mylistgroup"])
            {
                if ((string)data["public"] == "1")
                {
                    items.Add(new MylistModel(data["id"]));
                }
            }

            return items;
            //// 他人のﾘｽﾄを検索した。
            //var result = XDocument.Load(new StringReader(txt)).Root;
            //var channel = result.Descendants("channel").First();

            //// ﾏｲﾘｽﾄ情報を取得
            //MylistHeader?.Dispose();
            //MylistHeader = new MylistModel(result);
            //MylistHeader.PropertyChanged += OnPropertyChanged;

            //// ﾏｲﾘｽﾄ情報を本ｲﾝｽﾀﾝｽのﾌﾟﾛﾊﾟﾃｨに転記
            //this.MylistTitle = MylistHeader.MylistTitle;
            //this.MylistCreator = MylistHeader.MylistCreator;
            //this.MylistDescription = MylistHeader.MylistDescription;
            //this.UserId = MylistHeader.UserId;
            //this.UserThumbnailUrl = MylistHeader.UserThumbnailUrl;
            //this.UserThumbnail = MylistHeader.UserThumbnail;
            //this.MylistDate = MylistHeader.MylistDate;

            //foreach (var item in channel.Descendants("item"))
            //{
            //    var desc = XDocument.Load(new StringReader("<root>" + item.Element("description").Value + "</root>")).Root;
            //    var lengthSecondsStr = (string)desc
            //            .Descendants("strong")
            //            .Where(x => (string)x.Attribute("class") == "nico-info-length")
            //            .First();
            //    Items.Add(new VideoModel()
            //    {
            //        ContentId = item.Element("link").Value,
            //        Title = item.Element("title").Value,
            //        ViewCounter = GetCounter(desc, "nico-numbers-view"),
            //        MylistCounter = GetCounter(desc, "nico-numbers-mylist"),
            //        CommentCounter = GetCounter(desc, "nico-numbers-res"),
            //        StartTime = DateTime.Parse(channel.Element("pubDate").Value),
            //        ThumbnailUrl = (string)desc.Descendants("img").First().Attribute("src"),
            //        LengthSeconds = Converter.ToLengthSeconds(lengthSecondsStr),
            //    });
            //}

            //ServiceFactory.MessageService.Info(url);
        }

        /// <summary>
        /// 検索結果にｴﾗｰがないか確認します。
        /// </summary>
        /// <param name="txt">検索結果</param>
        /// <returns>ｴﾗｰ：true / 正常：false</returns>
        private bool IsError(string txt)
        {
            if (string.IsNullOrWhiteSpace(txt))
            {
                ServiceFactory.MessageService.Error("何らかの通信エラーです。");
                return true;
            }
            else if (txt.Contains("error:410"))
            {
                ServiceFactory.MessageService.Error("検索結果が見つかりませんでした。");
                return true;
            }
            else if (txt.Contains("error:403"))
            {
                ServiceFactory.MessageService.Error("非公開に設定されています。");
                return true;
            }
            else if (txt.Contains("お探しのページは、表示を許可していない可能性があります。"))
            {
                ServiceFactory.MessageService.Error("お探しのページは、表示を許可していない可能性があります。");
                return true;
            }
            else if (txt.Contains("error:"))
            {
                ServiceFactory.MessageService.Error("検索結果が見つかりません。ログイン状態か通信状況を確認してください。");
                return true;
            }
            else if (txt.Contains("<?xml ") && txt.Contains("\"item_type\""))
            {
                ServiceFactory.MessageService.Error("検索結果が見つかりません。ログイン状態か通信状況を確認してください。");
                return true;
            }

            return false;
        }
    }
}
