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
    public class SearchByWordModel : HttpModel
    {
        /// <summary>
        /// 検索ﾜｰﾄﾞ
        /// </summary>
        public string Word
        {
            get { return _Word; }
            set { SetProperty(ref _Word, value); }
        }
        private string _Word = null;

        /// <summary>
        /// ﾀｸﾞ検索 or ｷｰﾜｰﾄﾞ検索
        /// </summary>
        public bool IsTag
        {
            get { return _IsTag; }
            set { SetProperty(ref _IsTag, value); }
        }
        private bool _IsTag = true;

        /// <summary>
        /// ﾘﾐｯﾄ (何件取得するか)
        /// </summary>
        public int Limit
        {
            get { return _Limit; }
            set { SetProperty(ref _Limit, value); }
        }
        private int _Limit = 100;

        /// <summary>
        /// ｵﾌｾｯﾄ (取得する開始位置)
        /// </summary>
        public int Offset
        {
            get { return _Offset; }
            set { SetProperty(ref _Offset, value); }
        }
        private int _Offset = 0;

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
        /// ﾃﾞｰﾀ件数
        /// </summary>
        public int DataLength
        {
            get { return _DataLength; }
            set { SetProperty(ref _DataLength, value); }
        }
        private int _DataLength = 0;

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

            string targets = IsTag ? Constants.TargetTag : Constants.TargetKeyword;
            string q = HttpUtil.ToUrlEncode(Word);
            string fields = Constants.Fields;
            string offset = Offset.ToString();
            string limit = Limit.ToString();
            string context = Constants.Context;
            string sort = OrderBy;
            string url = String.Format(Constants.SearchByWordUrl, q, targets, fields, sort, offset, limit, context);
            string txt = GetSmileVideoHtmlText(url);

            if (IsError(txt))
            {
                return;
            }

            var json = DynamicJson.Parse(txt);

            foreach (dynamic data in json["data"])
            {
                Items.Add(new VideoModel()
                {
                    ContentId = data["contentId"],
                    Title = data["title"],
                    Description = data["description"],
                    Tags = data["tags"],
                    CategoryTag = data["categoryTag"],
                    ViewCounter = data["viewCounter"],
                    MylistCounter = data["mylistCounter"],
                    CommentCounter = data["commentCounter"],
                    StartTime = DateTime.Parse(data["startTime"]),
                    LastCommentTime = new DateTime(data["lastCommentTime"] * 1000),
                    LengthSeconds = data["lengthSeconds"],
                    ThumbnailUrl = data["thumbnailUrl"],
                    CommunityIcon = data["communityIcon"]
                });
            }

            DataLength = json["meta"]["totalCount"];

            ServiceFactory.MessageService.Info(url);
        }

        private bool IsError(string txt)
        {
            if (string.IsNullOrWhiteSpace(txt))
            {
                ServiceFactory.MessageService.Error("何らかの通信エラーです。");
                return true;
            }

            var json = DynamicJson.Parse(txt);

            if (json["meta"]["status"] == "400")
            {
                ServiceFactory.MessageService.Error("不正なパラメータです。");
                return true;
            }

            if (json["meta"]["status"] == "500")
            {
                ServiceFactory.MessageService.Error("検索サーバの異常です。");
                return true;
            }

            if (json["meta"]["status"] == "503")
            {
                ServiceFactory.MessageService.Error("サービスがメンテナンス中です。メンテナンス終了までお待ち下さい。");
                return true;
            }

            if (json["meta"]["status"] == "410")
            {
                ServiceFactory.MessageService.Error("検索結果が見つかりませんでした。");
                return true;
            }

            if (json["meta"]["status"] == "403")
            {
                ServiceFactory.MessageService.Error("非公開に設定されています。");
                return true;
            }

            if (json["meta"]["status"] != "200")
            {
                ServiceFactory.MessageService.Error("何らかの通信エラーです。");
                return true;
            }

            return false;
        }
    }
}
