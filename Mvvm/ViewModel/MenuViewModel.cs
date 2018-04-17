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
        /// <summary>
        /// ﾒﾆｭｰ構成のｿｰｽとなるﾓﾃﾞﾙを設定、または取得します。
        /// </summary>
        private MenuModel Source { get; set; }

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
        public MenuViewModel()
        {
            Source = MenuModel.Instance;

            MenuItems = Source.MenuItems.ToSyncedSynchronizationContextCollection(
                model => new MenuItemViewModel(this, null, model),
                SynchronizationContext.Current
            );
        }

    }
}
