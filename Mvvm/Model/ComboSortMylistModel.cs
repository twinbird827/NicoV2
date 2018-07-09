using NicoV2.Properties;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.Model
{
    public class ComboSortMylistModel : BindableBase
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
        public static ComboSortMylistModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ComboSortMylistModel();
                }
                return _Instance;
            }
        }
        private static ComboSortMylistModel _Instance;

        private ComboSortMylistModel()
        {
            _Items = new ObservableSynchronizedCollection<ComboBoxItemModel>
            {
                new ComboBoxItemModel() { Value = "0", Description = Resources.VM01013 },
                new ComboBoxItemModel() { Value = "1", Description = Resources.VM01014 },
                new ComboBoxItemModel() { Value = "2", Description = Resources.VM01015 },
                new ComboBoxItemModel() { Value = "3", Description = Resources.VM01016 },
                new ComboBoxItemModel() { Value = "4", Description = Resources.VM01017 },
                new ComboBoxItemModel() { Value = "5", Description = Resources.VM01018 },
                new ComboBoxItemModel() { Value = "6", Description = Resources.VM01019 },
                new ComboBoxItemModel() { Value = "7", Description = Resources.VM01020 },
                new ComboBoxItemModel() { Value = "8", Description = Resources.VM01021 },
                new ComboBoxItemModel() { Value = "9", Description = Resources.VM01022 },
                new ComboBoxItemModel() { Value = "10", Description = Resources.VM01023 },
                new ComboBoxItemModel() { Value = "11", Description = Resources.VM01024 },
                new ComboBoxItemModel() { Value = "12", Description = Resources.VM01025 },
                new ComboBoxItemModel() { Value = "13", Description = Resources.VM01026 },
                new ComboBoxItemModel() { Value = "14", Description = Resources.VM01027 },
                new ComboBoxItemModel() { Value = "15", Description = Resources.VM01028 },
                new ComboBoxItemModel() { Value = "16", Description = Resources.VM01029 },
                new ComboBoxItemModel() { Value = "17", Description = Resources.VM01030 },
            };
        }
    }
}
