using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.Model
{
    public class ComboThumbSizeModel : BindableBase
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
        public static ComboThumbSizeModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ComboThumbSizeModel();
                }
                return _Instance;
            }
        }
        private static ComboThumbSizeModel _Instance;

        private ComboThumbSizeModel()
        {
            _Items = new ObservableSynchronizedCollection<ComboBoxItemModel>
            {
                new ComboBoxItemModel() { Value = "", Description = "サムネ小" },
                new ComboBoxItemModel() { Value = ".M", Description = "サムネ中" },
                new ComboBoxItemModel() { Value = ".L", Description = "サムネ大" },
            };
        }
    }
}
