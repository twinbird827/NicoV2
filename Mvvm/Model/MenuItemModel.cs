using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.Model
{
    [DataContract]
    public class MenuItemModel : BindableBase
    {
        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public MenuItemModel(string name, MenuItemType type)
        {
            Name = name;
            Type = type;

            if (Type == MenuItemType.MylistOfMe)
            {
                foreach (var item in MylistOfMeModel.Instance.Reload())
                {
                    Items.Add(item);
                }
            }
        }

        /// <summary>
        /// ｱｲﾃﾑ名
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }
        private string _Name = null;

        /// <summary>
        /// ﾒﾆｭｰの種類
        /// </summary>
        [DataMember]
        public MenuItemType Type
        {
            get { return _Type; }
            set { SetProperty(ref _Type, value); }
        }
        private MenuItemType _Type;

        /// <summary>
        /// GUID
        /// </summary>
        [DataMember]
        public string Guid { get; set; } = System.Guid.NewGuid().ToString();

        /// <summary>
        /// 子ﾒﾆｭｰ
        /// </summary>
        [DataMember]
        public ObservableSynchronizedCollection<MenuItemModel> Children
        {
            get { return _Children; }
            set { SetProperty(ref _Children, value); }
        }
        private ObservableSynchronizedCollection<MenuItemModel> _Children = new ObservableSynchronizedCollection<MenuItemModel>();

        /// <summary>
        /// ﾒﾆｭｰに紐づくﾏｲﾘｽﾄ情報
        /// </summary>
        [DataMember]
        public ObservableSynchronizedCollection<MylistModel> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        private ObservableSynchronizedCollection<MylistModel> _Items = new ObservableSynchronizedCollection<MylistModel>();

    }
}
