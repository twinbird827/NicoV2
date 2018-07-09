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
using NicoV2.Common;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using System.Net.Http;
using System.ComponentModel;

namespace NicoV2.Mvvm.Model
{
    public class SearchByMylistModel : HttpModel
    {
        public SearchByMylistModel()
        {
            this.Method = "GET";
            this.ContentType = "application/x-www-form-urlencoded";
        }

        public SearchByMylistModel(string word)
            : this()
        {
            this.Word = word;
            this.OrderBy = "0";
            this.Reload();
        }

        /// <summary>
        /// 検索ﾜｰﾄﾞ
        /// </summary>
        public string Word
        {
            get { return _Word; }
            set
            {
                if (value.Contains("/"))
                {
                    value = value.Split('/').Last();
                }
                SetProperty(ref _Word, value);
            }
        }
        private string _Word = null;

        /// <summary>
        /// ｿｰﾄ順
        /// </summary>
        public string OrderBy
        {
            get { return _OrderBy; }
            set { SetProperty(ref _OrderBy, value); }
        }
        private string _OrderBy = null;

        /// <summary>
        /// ﾏｲﾘｽﾄﾍｯﾀﾞ情報
        /// </summary>
        public MylistModel MylistHeader
        {
            get { return _MylistHeader; }
            set { SetProperty(ref _MylistHeader, value); }
        }
        private MylistModel _MylistHeader = new MylistModel();

        /// <summary>
        /// ﾀｲﾄﾙ
        /// </summary>
        public string MylistTitle
        {
            get { return _MylistTitle; }
            set { SetProperty(ref _MylistTitle, value); }
        }
        private string _MylistTitle = null;

        /// <summary>
        /// 作成者
        /// </summary>
        public string MylistCreator
        {
            get { return _MylistCreator; }
            set { SetProperty(ref _MylistCreator, value); }
        }
        private string _MylistCreator = null;

        /// <summary>
        /// ﾏｲﾘｽﾄ詳細
        /// </summary>
        public string MylistDescription
        {
            get { return _MylistDescription; }
            set { SetProperty(ref _MylistDescription, value); }
        }
        private string _MylistDescription = null;

        /// <summary>
        /// 作成者のID
        /// </summary>
        public string UserId
        {
            get { return _UserId; }
            set { SetProperty(ref _UserId, value); }
        }
        private string _UserId = null;

        /// <summary>
        /// 作成者のｻﾑﾈｲﾙUrl
        /// </summary>
        public string UserThumbnailUrl
        {
            get { return _UserThumbnailUrl; }
            set { SetProperty(ref _UserThumbnailUrl, value); }
        }
        private string _UserThumbnailUrl = null;

        /// <summary>
        /// 作成者のｻﾑﾈｲﾙ
        /// </summary>
        public BitmapImage UserThumbnail
        {
            get { return _UserThumbnail; }
            set { SetProperty(ref _UserThumbnail, value); }
        }
        private BitmapImage _UserThumbnail = null;

        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTime MylistDate
        {
            get { return _MylistDate; }
            set { SetProperty(ref _MylistDate, value); }
        }
        private DateTime _MylistDate = default(DateTime);

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
            if (string.IsNullOrWhiteSpace(Word))
            {
                ServiceFactory.MessageService.Error("検索ワードが入力されていません。");
                return;
            }

            Items.Clear();

            var url = string.Format(Constants.MylistUrlRss, Word, OrderBy);
            var txt = GetSmileVideoHtmlText(url);

            if (IsError(txt))
            {
                return;
            }

            // 他人のﾘｽﾄを検索した。
            var result = XDocument.Load(new StringReader(txt)).Root;
            var channel = result.Descendants("channel").First();

            // ﾏｲﾘｽﾄ情報を取得
            MylistHeader?.Dispose();
            MylistHeader = new MylistModel(result);
            MylistHeader.PropertyChanged += OnPropertyChanged;

            // ﾏｲﾘｽﾄ情報を本ｲﾝｽﾀﾝｽのﾌﾟﾛﾊﾟﾃｨに転記
            this.MylistTitle = MylistHeader.MylistTitle;
            this.MylistCreator = MylistHeader.MylistCreator;
            this.MylistDescription = MylistHeader.MylistDescription;
            this.UserId = MylistHeader.UserId;
            this.UserThumbnailUrl = MylistHeader.UserThumbnailUrl;
            this.UserThumbnail = MylistHeader.UserThumbnail;
            this.MylistDate = MylistHeader.MylistDate;

            foreach (var item in channel.Descendants("item"))
            {
                var desc = XDocument.Load(new StringReader("<root>" + item.Element("description").Value + "</root>")).Root;
                var lengthSecondsStr = (string)desc
                        .Descendants("strong")
                        .Where(x => (string)x.Attribute("class") == "nico-info-length")
                        .First();
                Items.Add( new VideoModel()
                {
                    ContentId = item.Element("link").Value,
                    Title = item.Element("title").Value,
                    ViewCounter = GetCounter(desc, "nico-numbers-view"),
                    MylistCounter = GetCounter(desc, "nico-numbers-mylist"),
                    CommentCounter = GetCounter(desc, "nico-numbers-res"),
                    StartTime = DateTime.Parse(channel.Element("pubDate").Value),
                    ThumbnailUrl = (string)desc.Descendants("img").First().Attribute("src"),
                    LengthSeconds = Converter.ToLengthSeconds(lengthSecondsStr),
                });
            }

            ServiceFactory.MessageService.Info(url);
        }

        private long GetCounter(XElement e, string name)
        {
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

        /// <summary>
        /// ﾌﾟﾛﾊﾟﾃｨ変更時ｲﾍﾞﾝﾄ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case nameof(UserThumbnail):
                    this.UserThumbnail = MylistHeader.UserThumbnail;
                    break;
            }
        }
    }
}
