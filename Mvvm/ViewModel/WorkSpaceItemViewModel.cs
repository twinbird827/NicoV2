using NicoV2.Mvvm.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace NicoV2.Mvvm.ViewModel
{
    public class WorkSpaceItemViewModel : BindableBase
    {
        public VideoModel Source { get; private set; }

        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        /// <param name="m"></param>
        public WorkSpaceItemViewModel(VideoModel m)
        {
            Source = m;
            Source.PropertyChanged += OnPropertyChanged;

            this.ContentId = Source.ContentId;
            this.Title = Source.Title;
            this.CategoryTag = Source.CategoryTag;
            this.ViewCounter = Source.ViewCounter;
            this.MylistCounter = Source.MylistCounter;
            this.CommentCounter = Source.CommentCounter;
            this.StartTime = Source.StartTime;
            this.LengthSeconds = Source.LengthSeconds;
            this.Thumbnail = Source.Thumbnail;
        }

        /// <summary>
        /// ﾌﾟﾛﾊﾟﾃｨ変更時ｲﾍﾞﾝﾄ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Thumbnail):
                    this.Thumbnail = Source.Thumbnail;
                    break;
            }
        }

        /// <summary>
        /// ｺﾝﾃﾝﾂId
        /// </summary>
        public string ContentId
        {
            get { return _ContentId; }
            set { SetProperty(ref _ContentId, value); }
        }
        private string _ContentId = null;

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
        /// 再生時間 (秒)
        /// </summary>
        public double LengthSeconds
        {
            get { return _LengthSeconds; }
            set { SetProperty(ref _LengthSeconds, value); }
        }
        private double _LengthSeconds = default(double);

        /// <summary>
        /// ｻﾑﾈｲﾙ
        /// </summary>
        public BitmapImage Thumbnail
        {
            get { return _Thumbnail; }
            set { SetProperty(ref _Thumbnail, value); }
        }
        private BitmapImage _Thumbnail;

        /// <summary>
        /// 選択されているかどうか
        /// </summary>
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { SetProperty(ref _IsSelected, value); }
        }
        private bool _IsSelected = false;

        /// <summary>
        /// 項目ﾀﾞﾌﾞﾙｸﾘｯｸ時ｲﾍﾞﾝﾄ
        /// </summary>
        public ICommand OnDoubleClick
        {
            get
            {
                return _OnDoubleClick = _OnDoubleClick ?? new RelayCommand(
              _ =>
              {
                  // ﾛｸﾞｲﾝ実行
                  Source.StartBrowser();
              },
              _ =>
              {
                  return true;
              });
            }
        }
        public ICommand _OnDoubleClick;

        /// <summary>
        /// 項目ｷｰ入力時ｲﾍﾞﾝﾄ
        /// </summary>
        public ICommand OnKeyDown
        {
            get
            {
                return _OnKeyDown = _OnKeyDown ?? new RelayCommand<KeyEventArgs>(
              e =>
              {
                  // ﾛｸﾞｲﾝ実行
                  Source.StartBrowser();
              },
              e =>
              {
                  return e.Key == Key.Enter;
              });
            }
        }
        public ICommand _OnKeyDown;

        /// <summary>
        /// 項目をﾃﾝﾎﾟﾗﾘに追加する
        /// </summary>
        public ICommand OnTemporaryAdd
        {
            get
            {
                return _OnTemporaryAdd = _OnTemporaryAdd ?? new RelayCommand(
              e =>
              {
                  TemporaryModel.Instance.Add(Source.ContentId);
              },
              e =>
              {
                  return true;
              });
            }
        }
        public ICommand _OnTemporaryAdd;

        /// <summary>
        /// 項目をﾃﾝﾎﾟﾗﾘから削除する
        /// </summary>
        public ICommand OnTemporaryDel
        {
            get
            {
                return _OnTemporaryDel = _OnTemporaryDel ?? new RelayCommand(
              e =>
              {
                  TemporaryModel.Instance.Delete(Source.ContentId);
              },
              e =>
              {
                  return true;
              });
            }
        }
        public ICommand _OnTemporaryDel;

        /// <summary>
        /// ｲﾝｽﾀﾝｽ破棄時ｲﾍﾞﾝﾄ
        /// </summary>
        protected override void OnDisposed()
        {
            base.OnDisposed();

            Source.PropertyChanged -= OnPropertyChanged;
            Source = null;
        }
    }
}
