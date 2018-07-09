using NicoV2.Mvvm.Model;
using NicoV2.Mvvm.View;
using StatefulModel;
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
    public class SearchByMylistViewModel : WorkSpaceViewModel
    {
        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public SearchByMylistViewModel() : this(new SearchByMylistModel())
        {

        }

        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public SearchByMylistViewModel(SearchByMylistModel model)
        {
            Source = model;
            Source.PropertyChanged += OnPropertyChanged;
            Word = Source.Word;
            Items = Source.Items.ToSyncedSynchronizationContextCollection(m => new SearchByMylistItemViewModel(m), AnonymousSynchronizationContext.Current);
            IsCreatorVisible = Items.Any();
            SortItems = ComboSortMylistModel
                .Instance
                .Items
                .ToSyncedSynchronizationContextCollection(m => m, AnonymousSynchronizationContext.Current);
            SelectedSortItem = SortItems.First();

        }

        /// <summary>
        /// 本ｲﾝｽﾀﾝｽのﾃﾞｰﾀ実体
        /// </summary>
        public SearchByMylistModel Source { get; set; }

        /// <summary>
        /// ｿｰﾄﾘｽﾄ
        /// </summary>
        public SynchronizationContextCollection<ComboBoxItemModel> SortItems
        {
            get { return _SortItems; }
            set { SetProperty(ref _SortItems, value); }
        }
        private SynchronizationContextCollection<ComboBoxItemModel> _SortItems;

        /// <summary>
        /// 選択中のｿｰﾄ項目
        /// </summary>
        public ComboBoxItemModel SelectedSortItem
        {
            get { return _SelectedSortItem; }
            set { SetProperty(ref _SelectedSortItem, value); }
        }
        private ComboBoxItemModel _SelectedSortItem;

        /// <summary>
        /// ﾒｲﾝ項目ﾘｽﾄ
        /// </summary>
        public SynchronizationContextCollection<SearchByMylistItemViewModel> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        private SynchronizationContextCollection<SearchByMylistItemViewModel> _Items;

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
        /// 作成者情報を表示するか
        /// </summary>
        public bool IsCreatorVisible
        {
            get { return _IsCreatorVisible; }
            set { SetProperty(ref _IsCreatorVisible, value); }
        }
        private bool _IsCreatorVisible = default(bool);

        /// <summary>
        /// 検索処理
        /// </summary>
        public ICommand OnSearch
        {
            get
            {
                return _OnSearch = _OnSearch ?? new RelayCommand(
              _ =>
              {
                  // 入力値をﾓﾃﾞﾙにｾｯﾄ
                  Source.Word = this.Word;
                  Source.OrderBy = this.SelectedSortItem.Value;

                  // 検索実行
                  this.Source.Reload();

                  if (Source.Items.Any())
                  {
                      // ｵｰﾅｰ情報をｾｯﾄ
                      this.MylistCreator = this.Source.MylistCreator;
                      this.MylistDate = this.Source.MylistDate;
                      this.MylistTitle = this.Source.MylistTitle;
                      this.MylistDescription = this.Source.MylistDescription;
                      this.UserId = this.Source.UserId;
                      this.UserThumbnail = this.Source.UserThumbnail;

                      this.IsCreatorVisible = true;
                  }
                  else
                  {
                      this.IsCreatorVisible = false;
                  }

              },
              _ => {
                  return !string.IsNullOrWhiteSpace(Word);
              });
            }
        }
        public ICommand _OnSearch;

        /// <summary>
        /// 検索処理(ﾃｷｽﾄﾎﾞｯｸｽでENTER時)
        /// </summary>
        public ICommand OnSearchByEnter
        {
            get
            {
                return _OnSearchByEnter = _OnSearchByEnter ?? new RelayCommand<string>(
              s =>
              {
                  // 入力値をﾌﾟﾛﾊﾟﾃｨにｾｯﾄ
                  this.Word = s;

                  // 検索処理実行
                  if (OnSearch.CanExecute(null))
                  {
                      OnSearch.Execute(null);
                  }
              },
              s => {
                  return !string.IsNullOrWhiteSpace(s);
              });
            }
        }
        public ICommand _OnSearchByEnter;

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
        /// ｲﾝｽﾀﾝｽ破棄時ｲﾍﾞﾝﾄ
        /// </summary>
        protected override void OnDisposed()
        {
            base.OnDisposed();

            Source.PropertyChanged -= OnPropertyChanged;
        }

        #region "add mylist"

        /// <summary>
        /// ﾏｲﾘｽﾄ追加処理
        /// </summary>
        public ICommand OnAddMylist
        {
            get
            {
                return _OnAddMylist = _OnAddMylist ?? new RelayCommand(
                _ =>
                {
                    // ﾏｲﾘｽﾄﾌｫﾙﾀﾞを取得
                    var items = MenuModel.Instance.MenuItems
                        .First(m => m.Type == MenuItemType.SearchByMylist)
                        .Children;

                    if (items == null)
                    {
                        // TODO ｴﾗｰﾒｯｾｰｼﾞ表示
                        return;
                    }

                    // ﾀﾞｲｱﾛｸﾞで表示するﾏｲﾘｽﾄﾌｫﾙﾀﾞ情報を初期化
                    MylistItems = items.ToSyncedSynchronizationContextCollection(
                        m => new MenuItemViewModel(null, m),
                        AnonymousSynchronizationContext.Current
                    );

                    // ﾀﾞｲｱﾛｸﾞを開く
                    MainWindowViewModel.Instance.IsOpenMainDialog = true;
                    MainWindowViewModel.Instance.MainDialog = new SearchByMylistAddDialog(this);
                },
                _ => {
                    return IsCreatorVisible;
                });
            }
        }
        public ICommand _OnAddMylist;

        public ICommand OnAcceptDialog
        {
            get
            {
                return _OnAcceptDialog = _OnAcceptDialog ?? new RelayCommand(
              _ =>
              {
                  // ﾂﾘｰﾋﾞｭｰを閉じる
                  MainWindowViewModel.Instance.IsOpenMainDialog = false;
                  MainWindowViewModel.Instance.MainDialog = null;

                  // 選択されたﾌｫﾙﾀﾞにﾏｲﾘｽﾄを追加
                  var item = GetSelectedMylist(MylistItems);

                  if (item == null) return;

                  // ﾓﾃﾞﾙにﾏｲﾘｽﾄ情報を追加
                  item.Source.Items.Add(new MylistModel(Word));
              },
              _ =>
              {
                  return true;
              });
            }
        }
        public ICommand _OnAcceptDialog;

        public ICommand OnCancelDialog
        {
            get
            {
                return _OnCancelDialog = _OnCancelDialog ?? new RelayCommand(
              _ =>
              {
                  // ﾂﾘｰﾋﾞｭｰを閉じる
                  MainWindowViewModel.Instance.IsOpenMainDialog = false;
                  MainWindowViewModel.Instance.MainDialog = null;
              },
              _ =>
              {
                  return true;
              });
            }
        }
        public ICommand _OnCancelDialog;

        /// <summary>
        /// ﾏｲﾘｽﾄﾌｫﾙﾀﾞ項目ﾘｽﾄ
        /// </summary>
        public SynchronizationContextCollection<MenuItemViewModel> MylistItems
        {
            get { return _MylistItems; }
            set { SetProperty(ref _MylistItems, value); }
        }
        private SynchronizationContextCollection<MenuItemViewModel> _MylistItems;

        /// <summary>
        /// ﾀﾞｲｱﾛｸﾞで選択されたﾏｲﾘｽﾄを取得する。
        /// </summary>
        /// <param name="items"><code>MylistItems</code></param>
        /// <returns><code>MenuItemViewModel</code></returns>
        private MenuItemViewModel GetSelectedMylist(IEnumerable<MenuItemViewModel> items)
        {
            if (items == null || !items.Any())
            {
                return null;
            }

            var item = items.FirstOrDefault(i => i.IsSelected);

            if (item != null)
            {
                return item;
            }
            else
            {
                return GetSelectedMylist(items.SelectMany(i => i.Children));
            }
        }
        #endregion
    }
}
