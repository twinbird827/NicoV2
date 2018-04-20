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
    public class MenuItemViewModel : BindableBase
    {
        /// <summary>
        /// ﾒﾆｭｰ構成を全て管理するMenuViewModel
        /// </summary>
        public MenuViewModel Menu { get; set; }

        /// <summary>
        /// 本ﾒﾆｭｰの親ﾒﾆｭｰ
        /// </summary>
        public MenuItemViewModel Parent { get; set; }

        /// <summary>
        /// 本ﾒﾆｭｰの情報を管理するModel
        /// </summary>
        public MenuItemModel Source { get; set; }

        /// <summary>
        /// ｱｲﾃﾑ名
        /// </summary>
        public string Name
        {
            get { return Source.Name; }
            set { SetProperty(ref _Name, value); Source.Name = _Name; }
        }
        private string _Name = null;

        /// <summary>
        /// ﾒﾆｭｰの種類
        /// </summary>
        public MenuItemType Type
        {
            get { return Source.Type; }
            set { SetProperty(ref _Type, value); Source.Type = _Type; }
        }
        private MenuItemType _Type;

        /// <summary>
        /// ﾒﾆｭｰが紐付くﾜｰｸｽﾍﾟｰｽのｲﾝｽﾀﾝｽ
        /// </summary>
        public WorkSpaceViewModel WorkSpace
        {
            //get { return _WorkSpace; }
            //set { SetProperty(ref _WorkSpace, value); }
            get
            {
                return _WorkSpace = _WorkSpace ?? CreateWorkSpaceViewModel();
            }
        }
        private WorkSpaceViewModel _WorkSpace;

        /// <summary>
        /// 本ﾒﾆｭｰｱｲﾃﾑの子要素
        /// </summary>
        public SynchronizationContextCollection<MenuItemViewModel> Children
        {
            get { return _Children; }
            set { SetProperty(ref _Children, value); }
        }
        private SynchronizationContextCollection<MenuItemViewModel> _Children;

        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        /// <param name="menu">ﾒﾆｭｰ構成を全て管理するMenuViewModel</param>
        /// <param name="parent">本ﾒﾆｭｰの親ﾒﾆｭｰ</param>
        /// <param name="source">本ﾒﾆｭｰの情報を管理するModel</param>
        public MenuItemViewModel(MenuViewModel menu, MenuItemViewModel parent, MenuItemModel source)
        {
            this.Menu = menu;
            this.Parent = parent;
            this.Source = source;

            // TODO NameとTypeは設定必要？
            //Name = Source.Name;
            //Type = Source.Type;
            Children = source.Children.ToSyncedSynchronizationContextCollection(
                m => new MenuItemViewModel(menu, this, m),
                SynchronizationContext.Current
            );
        }

        private WorkSpaceViewModel CreateWorkSpaceViewModel()
        {
            switch (Type)
            {
                case MenuItemType.Setting:
                    return new SettingViewModel();
                default:
                    return null;
            }
        }

        #region IDisposable Support

        public void Test1()
        {
            Console.WriteLine(Name);
        }
        protected override void OnDisposing()
        {
            base.OnDisposing();

            //TODO // ｸﾘｯｸｲﾍﾞﾝﾄの購読解除
            //Click -= VM.OnCurrentChanging;
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            Menu = null;
            Parent = null;
            Source = null;
        }

        #endregion
    }
}
