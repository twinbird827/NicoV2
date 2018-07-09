using NicoV2.Properties;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.Model
{
    public class ComboRankTargetModel : BindableBase
    {
        /// <summary>
        /// ｿｰﾄﾘｽﾄ構成
        /// </summary>
        public ObservableSynchronizedCollection<ComboBoxItemModel> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        private ObservableSynchronizedCollection<ComboBoxItemModel> _Items;

        /// <summary>
        /// ｲﾝｽﾀﾝｽ (ｼﾝｸﾞﾙﾄﾝﾊﾟﾀｰﾝ)
        /// </summary>
        public static ComboRankTargetModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ComboRankTargetModel();
                }
                return _Instance;
            }
        }
        private static ComboRankTargetModel _Instance;

        private ComboRankTargetModel()
        {
            _Items = new ObservableSynchronizedCollection<ComboBoxItemModel>
            {
                new ComboBoxItemModel() { Value = "fav", Description = Resources.VM01063 },
                new ComboBoxItemModel() { Value = "view", Description = Resources.VM01031 },
                new ComboBoxItemModel() { Value = "res", Description = Resources.VM01032 },
                new ComboBoxItemModel() { Value = "mylist", Description = Resources.VM01033 },
            };
        }
    }
}
