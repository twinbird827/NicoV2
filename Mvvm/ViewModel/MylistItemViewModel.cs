using NicoV2.Mvvm.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace NicoV2.Mvvm.ViewModel
{
    public class MylistItemViewModel : BindableBase
    {
        public MenuItemModel Parent { get; set; }

        public MylistModel Source { get; set; }

        public MylistItemViewModel(MenuItemModel parent, MylistModel model)
        {
            Parent = parent;
            Source = model;
            Source.PropertyChanged += OnPropertyChanged;

            MylistUrl = Source.MylistUrl;
            MylistTitle = Source.MylistTitle;
            MylistCreator = Source.MylistCreator;
            MylistDescription = Source.MylistDescription;
            UserId = Source.UserId;
            UserThumbnailUrl = Source.UserThumbnailUrl;
            UserThumbnail = Source.UserThumbnail;
            MylistDate = Source.MylistDate;
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
                    this.UserThumbnail = Source.UserThumbnail;
                    break;
            }
        }

        /// <summary>
        /// ﾏｲﾘｽﾄUrl
        /// </summary>
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
        /// 項目ﾀﾞﾌﾞﾙｸﾘｯｸ時ｲﾍﾞﾝﾄ
        /// </summary>
        public ICommand OnDoubleClick
        {
            get
            {
                return _OnDoubleClick = _OnDoubleClick ?? new RelayCommand(
              _ =>
              {
                  MainWindowViewModel.Instance.Current = new SearchByMylistViewModel(new SearchByMylistModel(Source.MylistUrl));
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
                  OnDoubleClick.Execute(null);
              },
              e =>
              {
                  return e.Key == Key.Enter;
              });
            }
        }
        public ICommand _OnKeyDown;

        /// <summary>
        /// 右ｸﾘｯｸﾒﾆｭｰ -> ｺﾋﾟｰ時ｲﾍﾞﾝﾄ
        /// </summary>
        public ICommand OnCopy
        {
            get
            {
                return _OnCopy = _OnCopy ?? new RelayCommand(
              _ =>
              {
                  Clipboard.SetText(Source.MylistUrl);
              },
              _ =>
              {
                  return true;
              });
            }
        }
        public ICommand _OnCopy;

        /// <summary>
        /// 右ｸﾘｯｸﾒﾆｭｰ -> 削除時ｲﾍﾞﾝﾄ
        /// </summary>
        public ICommand OnDelete
        {
            get
            {
                return _OnDelete = _OnDelete ?? new RelayCommand(
              _ =>
              {
                  Parent.Items.Remove(Source);
              },
              _ =>
              {
                  return true;
              });
            }
        }
        public ICommand _OnDelete;

        protected override void OnDisposed()
        {
            base.OnDisposed();

            Source.PropertyChanged -= OnPropertyChanged;
        }
    }
}
