using NicoV2.IO;
using NicoV2.Common;
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
    public class RankingModel : HttpModel
    {
        public RankingModel()
        {
            this.Method = "GET";
            this.ContentType = "application/x-www-form-urlencoded";
        }

        /// <summary>
        /// 対象
        /// </summary>
        public string Target
        {
            get { return _Target; }
            set { SetProperty(ref _Target, value); }
        }
        private string _Target = null;

        /// <summary>
        /// 期間
        /// </summary>
        public string Period
        {
            get { return _Period; }
            set { SetProperty(ref _Period, value); }
        }
        private string _Period = null;

        /// <summary>
        /// ｶﾃｺﾞﾘ
        /// </summary>
        public string Category
        {
            get { return _Category; }
            set { SetProperty(ref _Category, value); }
        }
        private string _Category = null;

        /// <summary>
        /// ｱｲﾃﾑ構成
        /// </summary>
        public ObservableSynchronizedCollection<VideoModel> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        private ObservableSynchronizedCollection<VideoModel> _Items = new ObservableSynchronizedCollection<VideoModel>();

        public void Reload()
        {
            if (string.IsNullOrWhiteSpace(Target) || string.IsNullOrWhiteSpace(Period) || string.IsNullOrWhiteSpace(Category))
            {
                ServiceFactory.MessageService.Error("検索ワードが入力されていません。");
                return;
            }

            Items.Clear();

            var url = string.Format(Constants.RankingUrl, Target, Period, Category);
            var txt = GetSmileVideoHtmlText(url);

            if (IsError(txt))
            {
                return;
            }

            // ﾗﾝｷﾝｸﾞを検索した。
            var result = XDocument.Load(new StringReader(txt)).Root;
            var channel = result.Descendants("channel").First();

            foreach (var item in channel.Descendants("item"))
            {
                var desc = XDocument.Load(new StringReader("<root>" + item.Element("description").Value + "</root>")).Root;
                var lengthSecondsStr = (string)desc
                        .Descendants("strong")
                        .Where(x => (string)x.Attribute("class") == "nico-info-length")
                        .First();
                Items.Add(new VideoModel()
                {
                    ContentId = item.Element("link").Value,
                    Title = item.Element("title").Value,
                    ViewCounter = GetCounter(desc, "nico-info-total-view"),
                    MylistCounter = GetCounter(desc, "nico-info-total-res"),
                    CommentCounter = GetCounter(desc, "nico-info-total-mylist"),
                    StartTime = DateTime.Parse(channel.Element("pubDate").Value),
                    ThumbnailUrl = (string)desc.Descendants("img").First().Attribute("src"),
                    LengthSeconds = Converter.ToLengthSeconds(lengthSecondsStr),
                });
            }

            ServiceFactory.MessageService.Info(url);
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

        private long GetCounter(XElement e, string name)
        {
            // TODO SearchByMylistModelのﾒｿｯﾄﾞと統合する
            var s = (string)e
                .Descendants("strong")
                .Where(x => (string)x.Attribute("class") == name)
                .FirstOrDefault();
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }
            else
            {
                return Converter.ToLong(s);
            }
        }

    }
}
