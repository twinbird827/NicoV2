using NicoV2.Common;
using NicoV2.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace NicoV2.Mvvm.Model
{
    [DataContract]
    public class MylistModel : HttpModel
    {
        public MylistModel()
        {
            this.Method = "GET";
            this.ContentType = "application/x-www-form-urlencoded";
        }

        public MylistModel(XElement xml)
            : this()
        {
            FromXml(xml);
        }

        public MylistModel(string url)
            : this()
        {
            FromUrl(url);
        }

        /// <summary>
        /// ﾏｲﾘｽﾄUrl
        /// </summary>
        [DataMember]
        public string MylistUrl
        {
            get { return _MylistUrl; }
            set { SetProperty(ref _MylistUrl, value); }
        }
        private string _MylistUrl = null;

        /// <summary>
        /// ﾏｲﾘｽﾄId
        /// </summary>
        public string MylistId
        {
            get { return MylistUrl?.Split('/').Last(); }
        }

        /// <summary>
        /// ﾀｲﾄﾙ
        /// </summary>
        [DataMember]
        public string MylistTitle
        {
            get { return _MylistTitle; }
            set { SetProperty(ref _MylistTitle, value); }
        }
        private string _MylistTitle = null;

        /// <summary>
        /// 作成者
        /// </summary>
        [DataMember]
        public string MylistCreator
        {
            get { return _MylistCreator; }
            set { SetProperty(ref _MylistCreator, value); }
        }
        private string _MylistCreator = null;

        /// <summary>
        /// ﾏｲﾘｽﾄ詳細
        /// </summary>
        [DataMember]
        public string MylistDescription
        {
            get { return _MylistDescription; }
            set { SetProperty(ref _MylistDescription, value); }
        }
        private string _MylistDescription = null;

        /// <summary>
        /// 作成者のID
        /// </summary>
        [DataMember]
        public string UserId
        {
            get { return _UserId; }
            set { SetProperty(ref _UserId, value); }
        }
        private string _UserId = null;

        /// <summary>
        /// 作成者のｻﾑﾈｲﾙUrl
        /// </summary>
        [DataMember]
        public string UserThumbnailUrl
        {
            get { return _UserThumbnailUrl; }
            set
            {
                SetProperty(ref _UserThumbnailUrl, value);

                // TODO ｻﾑﾈ取得失敗時にﾃﾞﾌｫﾙﾄURLで再取得
                // TODO ｻﾑﾈ中/大を選択時、取得失敗した場合はﾃﾞﾌｫﾙﾄｻﾑﾈを拡大する
                HttpUtil.GetThumbnail(value).ContinueWith(
                    t => UserThumbnail = t.Result,
                    TaskScheduler.FromCurrentSynchronizationContext()
                );
            }
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
        [DataMember]
        public DateTime MylistDate
        {
            get { return _MylistDate; }
            set { SetProperty(ref _MylistDate, value); }
        }
        private DateTime _MylistDate = default(DateTime);

        public MylistModel FromUrl(string url)
        {
            url = string.Format(Constants.MylistUrlRss, url?.Split('/').Last(), 0);
            return FromXml(XDocument.Load(new StringReader(GetSmileVideoHtmlText(url))).Root);
        }

        public MylistModel FromXml(XElement xml)
        {
            // 他人のﾘｽﾄを検索した。
            var channel = xml.Descendants("channel").First();

            MylistUrl = channel.Element("link").Value;
            MylistTitle = channel.Element("title").Value;
            MylistCreator = channel.Element(XName.Get("creator", "http://purl.org/dc/elements/1.1/")).Value;
            MylistDate = DateTime.Parse(channel.Element("lastBuildDate").Value);
            MylistDescription = channel.Element("description").Value;

            UserId = GetUserId();
            UserThumbnailUrl = GetThumbnailUrl();

            return this;
        }

        private string GetThumbnailUrl()
        {
            var url = string.Format(Constants.UserIframe, UserId);
            var txt = GetSmileVideoHtmlText(url);
            var thumbnail = Regex.Match(txt, Constants.UserThumbnailUrl + "(?<url>[^\"]+)").Groups["url"].Value;
            return Constants.UserThumbnailUrl + thumbnail;
        }

        private string GetUserId()
        {
            var url = string.Format(Constants.MylistUrl, MylistId); // 生のURL 
            var txt = GetSmileVideoHtmlText(url);               // HTMLﾃｷｽﾄを取得 & user id を取得
            var id = Regex.Match(txt, "user_id: (?<user_id>[\\d]+)").Groups["user_id"].Value;
            return id;
        }

        public IEnumerable<VideoModel> GetVideos()
        {
            var model = new SearchByMylistModel(MylistUrl);
            return model.Items.ToArray();
        }
    }
}
