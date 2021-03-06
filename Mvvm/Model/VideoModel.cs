﻿using NicoV2.Common;
using NicoV2.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace NicoV2.Mvvm.Model
{
    public class VideoModel : HttpModel
    {
        public VideoModel()
        {
            this.Method = "GET";
            this.ContentType = "application/x-www-form-urlencoded";
        }

        public VideoModel(string id)
            : this()
        {
            this.ContentId = id;

            // ﾋﾞﾃﾞｵ詳細情報取得
            var url = string.Format(Constants.VideoDetailUrl, this.VideoId);
            var txt = GetSmileVideoHtmlText(url);
            var xml = XDocument.Load(new StringReader(txt)).Root.Element("thumb");

            Title = xml.Element("title").Value;
            ViewCounter = long.Parse(xml.Element("view_counter").Value);
            MylistCounter = long.Parse(xml.Element("mylist_counter").Value);
            CommentCounter = long.Parse(xml.Element("comment_num").Value);
            StartTime = DateTime.Parse(xml.Element("first_retrieve").Value);
            ThumbnailUrl = xml.Element("thumbnail_url").Value;
            LengthSeconds = Converter.ToLengthSeconds(xml.Element("length").Value);
            LastResBody = xml.Element("last_res_body").Value;

        }

        /// <summary>
        /// ｺﾝﾃﾝﾂId (http://nico.ms/ の後に連結することでコンテンツへのURLになります)
        /// </summary>
        public string ContentId
        {
            get
            {
                return "http://nico.ms/" + _ContentId;
            }
            set
            {
                if (value.Contains("/"))
                {
                    value = value.Split('/').Last();
                }
                SetProperty(ref _ContentId, value);
            }
        }
        private string _ContentId = null;

        /// <summary>
        /// 動画ID
        /// </summary>
        public string VideoId
        {
            get { return ContentId.Split('/').Last(); }
        }


        /// <summary>
        /// ﾀｲﾄﾙ
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }
        private string _Title = null;

        /// <summary>
        /// ｺﾝﾃﾝﾂの説明文
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set { SetProperty(ref _Description, value); }
        }
        private string _Description = null;

        /// <summary>
        /// ﾀｸﾞ (空白区切り)
        /// </summary>
        public string Tags
        {
            get { return _Tags; }
            set { SetProperty(ref _Tags, value); }
        }
        private string _Tags = null;

        /// <summary>
        /// ｶﾃｺﾞﾘﾀｸﾞ
        /// </summary>
        public string CategoryTag
        {
            get { return _CategoryTag; }
            set { SetProperty(ref _CategoryTag, value); }
        }
        private string _CategoryTag = null;

        /// <summary>
        /// 再生数
        /// </summary>
        public double ViewCounter
        {
            get { return _ViewCounter; }
            set { SetProperty(ref _ViewCounter, value); }
        }
        private double _ViewCounter = default(int);

        /// <summary>
        /// ﾏｲﾘｽﾄ数
        /// </summary>
        public double MylistCounter
        {
            get { return _MylistCounter; }
            set { SetProperty(ref _MylistCounter, value); }
        }
        private double _MylistCounter = default(int);

        /// <summary>
        /// ｺﾒﾝﾄ数
        /// </summary>
        public double CommentCounter
        {
            get { return _CommentCounter; }
            set { SetProperty(ref _CommentCounter, value); }
        }
        private double _CommentCounter = default(int);

        /// <summary>
        /// ｺﾝﾃﾝﾂの投稿時間
        /// </summary>
        public DateTime StartTime
        {
            get { return _StartTime; }
            set { SetProperty(ref _StartTime, value); }
        }
        private DateTime _StartTime = default(DateTime);

        /// <summary>
        /// 最終ｺﾒﾝﾄ時間
        /// </summary>
        public DateTime LastCommentTime
        {
            get { return _LastCommentTime; }
            set { SetProperty(ref _LastCommentTime, value); }
        }
        private DateTime _LastCommentTime = default(DateTime);

        /// <summary>
        /// 再生時間 (秒)
        /// </summary>
        public double LengthSeconds
        {
            get { return _LengthSeconds; }
            set { SetProperty(ref _LengthSeconds, value); }
        }
        private double _LengthSeconds = default(double);

        /// <summary>
        /// ｻﾑﾈｲﾙUrl
        /// </summary>
        public string ThumbnailUrl
        {
            get { return _ThumbnailUrl; }
            set
            {
                SetProperty(ref _ThumbnailUrl, value);

                // TODO ｻﾑﾈ取得失敗時にﾃﾞﾌｫﾙﾄURLで再取得
                // TODO ｻﾑﾈ中/大を選択時、取得失敗した場合はﾃﾞﾌｫﾙﾄｻﾑﾈを拡大する
                HttpUtil.GetThumbnail(_ThumbnailUrl)
                    .ContinueWith(
                        t => Thumbnail = t.Result,
                        TaskScheduler.FromCurrentSynchronizationContext()
                    );
            }
        }
        private string _ThumbnailUrl = null;

        /// <summary>
        /// ｻﾑﾈｲﾙ
        /// </summary>
        public BitmapImage Thumbnail
        {
            get
            {
                return _Thumbnail;
            }
            set { SetProperty(ref _Thumbnail, value); }
        }
        private BitmapImage _Thumbnail;

        /// <summary>
        /// ｺﾐｭﾆﾃｨｱｲｺﾝのUrl
        /// </summary>
        public string CommunityIcon
        {
            get { return _CommunityIcon; }
            set { SetProperty(ref _CommunityIcon, value); }
        }
        private string _CommunityIcon = null;

        /// <summary>
        /// 最終更新時間
        /// </summary>
        public DateTime LastUpdateTime
        {
            get { return _LastUpdateTime; }
            set { SetProperty(ref _LastUpdateTime, value); }
        }
        private DateTime _LastUpdateTime = DateTime.Now;

        /// <summary>
        /// 最新ｺﾒﾝﾄ
        /// </summary>
        public string LastResBody
        {
            get { return _LastResBody; }
            set { SetProperty(ref _LastResBody, value); }
        }
        private string _LastResBody = null;

        /// <summary>
        /// ｺﾝﾃﾝﾂをﾌﾞﾗｳｻﾞ起動する。
        /// </summary>
        public void StartBrowser()
        {
            Process.Start(Variables.BrowserPath, ContentId);
        }
    }
}
