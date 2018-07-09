using NicoV2.Mvvm.Model;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.ViewModel
{
    public class MylistViewModel : WorkSpaceViewModel
    {
        public MenuItemModel Source { get; set; }

        public MylistViewModel(MenuItemModel model)
        {
            Source = model;

            Items = Source.Items.ToSyncedSynchronizationContextCollection(
                m => new MylistItemViewModel(this.Source, m), 
                AnonymousSynchronizationContext.Current
            );

        }

        /// <summary>
        /// ﾒｲﾝ項目ﾘｽﾄ
        /// </summary>
        public SynchronizationContextCollection<MylistItemViewModel> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        private SynchronizationContextCollection<MylistItemViewModel> _Items;

    }
}
