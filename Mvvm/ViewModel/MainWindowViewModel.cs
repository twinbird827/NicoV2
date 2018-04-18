using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public MenuViewModel Menu
        {
            get { return _Menu; }
            set { SetProperty(ref _Menu, value); }
        }
        private MenuViewModel _Menu;

        public WorkSpaceViewModel Current
        {
            get { return _Current; }
            set { SetProperty(ref _Current, value); }
        }
        private WorkSpaceViewModel _Current;

        public MainWindowViewModel()
        {
            // ﾒﾆｭｰ構成を作成
            Menu = new MenuViewModel(this);

            // TODO 初期起動時のﾜｰｸｽﾍﾟｰｽを決定
            // TODO 前回起動時の状態を保持する？
            Current = null;
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
