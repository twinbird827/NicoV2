using NicoV2.Mvvm.Model;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.ViewModel
{
    public class MenuViewModel : BindableBase
    {
        public MainWindowViewModel MainWindow { get; set; }

        /// <summary>
        /// ﾒﾆｭｰ構成のｿｰｽとなるﾓﾃﾞﾙを設定、または取得します。
        /// </summary>
        public MenuModel Source { get; private set; }

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
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public MenuViewModel(MainWindowViewModel mainWindow)
        {
            Source = MenuModel.Instance;
            MainWindow = mainWindow;

            MenuItems = Source.MenuItems.ToSyncedSynchronizationContextCollection(
                model => new MenuItemViewModel(this, null, model),
                SynchronizationContext.Current
            );
        }
    }
}
