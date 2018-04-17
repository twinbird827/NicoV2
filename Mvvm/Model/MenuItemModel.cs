using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.Model
{
    public class MenuItemModel : BindableBase
    {
        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public MenuItemModel(string name, MenuItemType type)
        {
            Name = name;
            Type = type;
        }

        public MenuItemModel()
        {

        }

        /// <summary>
        /// ｱｲﾃﾑ名
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }
        private string _Name = null;

        /// <summary>
        /// ﾒﾆｭｰの種類
        /// </summary>
        public MenuItemType Type
        {
            get { return _Type; }
            set { SetProperty(ref _Type, value); }
        }
        private MenuItemType _Type;

        /// <summary>
        /// 子ﾒﾆｭｰ
        /// </summary>
        public ObservableSynchronizedCollection<MenuItemModel> Children
        {
            get { return _Children; }
            set { SetProperty(ref _Children, value); }
        }
        private ObservableSynchronizedCollection<MenuItemModel> _Children = new ObservableSynchronizedCollection<MenuItemModel>();

    }
}
