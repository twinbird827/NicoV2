using NicoV2.Mvvm.Model;
using NicoV2.Mvvm.View;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace NicoV2.Mvvm.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public static MainWindowViewModel Instance { get; set; }

        public MenuModel Menu { get; set; }

        public DispatcherTimer Timer { get; set; }

        public DateTime LastConfirmTime { get; set; } = DateTime.Now;

        /// <summary>
        /// ﾒﾆｭｰﾂﾘｰﾋﾞｭｰ構成
        /// </summary>
        public SynchronizationContextCollection<MenuItemViewModel> MenuItems
        {
            get { return _MenuItems; }
            set { SetProperty(ref _MenuItems, value); }
        }
        private SynchronizationContextCollection<MenuItemViewModel> _MenuItems;

        /// <summary>
        /// ｶﾚﾝﾄﾜｰｸｽﾍﾟｰｽ
        /// </summary>
        public WorkSpaceViewModel Current
        {
            get { return _Current; }
            set { SetProperty(ref _Current, value); }
        }
        private WorkSpaceViewModel _Current;

        /// <summary>
        /// ﾒﾆｭｰ表示中かどうか
        /// </summary>
        public bool IsOpenMenu
        {
            get { return _IsOpenMenu; }
            set
            {
                if (!(IsOpenTreeViewDialog && !value))
                {
                    // ﾂﾘｰﾋﾞｭｰ上にﾀﾞｲｱﾛｸﾞ表示中は、Menuを閉じないようにする
                    SetProperty(ref _IsOpenMenu, value);
                }
            }
        }
        private bool _IsOpenMenu;

        /// <summary>
        /// ﾂﾘｰﾋﾞｭｰﾀﾞｲｱﾛｸﾞ表示中かどうか
        /// </summary>
        public bool IsOpenTreeViewDialog
        {
            get { return _IsOpenTreeViewDialog; }
            set { SetProperty(ref _IsOpenTreeViewDialog, value); }
        }
        private bool _IsOpenTreeViewDialog;

        /// <summary>
        /// ﾂﾘｰﾋﾞｭｰﾀﾞｲｱﾛｸﾞの実体
        /// </summary>
        public object TreeViewDialog
        {
            get { return _TreeViewDialog; }
            set { SetProperty(ref _TreeViewDialog, value); }
        }
        private object _TreeViewDialog;

        /// <summary>
        /// ﾒｲﾝﾀﾞｲｱﾛｸﾞ表示中かどうか
        /// </summary>
        public bool IsOpenMainDialog
        {
            get { return _IsOpenMainDialog; }
            set { SetProperty(ref _IsOpenMainDialog, value); }
        }
        private bool _IsOpenMainDialog;

        /// <summary>
        /// ﾒｲﾝﾀﾞｲｱﾛｸﾞの実体
        /// </summary>
        public object MainDialog
        {
            get { return _MainDialog; }
            set { SetProperty(ref _MainDialog, value); }
        }
        private object _MainDialog;


        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public MainWindowViewModel()
        {
            if (Instance != null)
            {
                throw new InvalidOperationException("本ViewModelは複数のｲﾝｽﾀﾝｽを作成することができません。");
            }

            Instance = this;

            // ﾒﾆｭｰ構成を作成
            Menu = MenuModel.Instance;

            MenuItems = Menu.MenuItems.ToSyncedSynchronizationContextCollection(
                m => new MenuItemViewModel(null, m),
                SynchronizationContext.Current
            );

            // TODO 前回起動時の状態を保持する？
            Current = new RankingViewModel();

            // お気に入りを走査して、新しいﾃﾞｰﾀが存在する場合はTempに追加
            Timer = new DispatcherTimer(DispatcherPriority.Normal, App.Current.Dispatcher)
            {
                Interval = TimeSpan.FromMilliseconds(1000 * 10)
            };
            Timer.Tick += (sender, e) =>
            {
                {
                    var lastConfirmTime =
                        MenuModel.Instance.Parameters.ContainsKey("LastConfirmTime")
                        ? (DateTime)MenuModel.Instance.Parameters["LastConfirmTime"]
                        : default(DateTime);

                    MenuModel.Instance.Parameters["LastConfirmTime"] = DateTime.Now;

                    var videos = GetAllMenu(MenuModel.Instance.MenuItems)
                        .SelectMany(m => m.Items)
                        .SelectMany(m => m.GetVideos())
                        .Where(v => lastConfirmTime < v.StartTime)
                        .Where(v => !TemporaryModel.Instance.Items.Any(t => t.VideoId == v.VideoId));

                    foreach (var v in videos)
                    {
                        TemporaryModel.Instance.Add(v.VideoId);
                    }
                }
            };
            Timer.Start();
        }

        private IEnumerable<MenuItemModel> GetAllMenu(IEnumerable<MenuItemModel> menues)
        {
            if (menues.Any())
            {
                return menues.Union(GetAllMenu(menues.SelectMany(m => m.Children)));
            }
            else
            {
                return menues;
            }
        }
        protected override void OnDisposing()
        {
            base.OnDisposing();
        }
        protected override void OnDisposed()
        {
            base.OnDisposed();
        }
    }
}
