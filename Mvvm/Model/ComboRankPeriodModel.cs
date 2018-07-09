using NicoV2.Properties;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.Model
{
    public class ComboRankPeriodModel : BindableBase
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
        public static ComboRankPeriodModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ComboRankPeriodModel();
                }
                return _Instance;
            }
        }
        private static ComboRankPeriodModel _Instance;

        private ComboRankPeriodModel()
        {
            _Items = new ObservableSynchronizedCollection<ComboBoxItemModel>
            {
                new ComboBoxItemModel() { Value = "hourly", Description = Resources.VM01058 },
                new ComboBoxItemModel() { Value = "daily", Description = Resources.VM01059 },
                new ComboBoxItemModel() { Value = "weekly", Description = Resources.VM01060 },
                new ComboBoxItemModel() { Value = "monthly", Description = Resources.VM01061 },
                new ComboBoxItemModel() { Value = "total", Description = Resources.VM01062 },
            };
        }
    }
}
