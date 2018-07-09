using NicoV2.Properties;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.Model
{
    public class ComboRankCategoryModel : BindableBase
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
        public static ComboRankCategoryModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ComboRankCategoryModel();
                }
                return _Instance;
            }
        }
        private static ComboRankCategoryModel _Instance;

        private ComboRankCategoryModel()
        {
            _Items = new ObservableSynchronizedCollection<ComboBoxItemModel>
            {
                new ComboBoxItemModel() { Value = "all", Description = Resources.VM01034 },
                new ComboBoxItemModel() { Value = "music", Description = Resources.VM01035 },
                new ComboBoxItemModel() { Value = "ent", Description = Resources.VM01036 },
                new ComboBoxItemModel() { Value = "anime", Description = Resources.VM01037 },
                new ComboBoxItemModel() { Value = "game", Description = Resources.VM01038 },
                new ComboBoxItemModel() { Value = "animal", Description = Resources.VM01039 },
                new ComboBoxItemModel() { Value = "que", Description = Resources.VM01040 },
                new ComboBoxItemModel() { Value = "radio", Description = Resources.VM01041 },
                new ComboBoxItemModel() { Value = "sport", Description = Resources.VM01042 },
                new ComboBoxItemModel() { Value = "politics", Description = Resources.VM01043 },
                new ComboBoxItemModel() { Value = "chat", Description = Resources.VM01044 },
                new ComboBoxItemModel() { Value = "science", Description = Resources.VM01045 },
                new ComboBoxItemModel() { Value = "history", Description = Resources.VM01046 },
                new ComboBoxItemModel() { Value = "cooking", Description = Resources.VM01047 },
                new ComboBoxItemModel() { Value = "nature", Description = Resources.VM01048 },
                new ComboBoxItemModel() { Value = "diary", Description = Resources.VM01049 },
                new ComboBoxItemModel() { Value = "dance", Description = Resources.VM01050 },
                new ComboBoxItemModel() { Value = "sing", Description = Resources.VM01051 },
                new ComboBoxItemModel() { Value = "play", Description = Resources.VM01052 },
                new ComboBoxItemModel() { Value = "lecture", Description = Resources.VM01053 },
                new ComboBoxItemModel() { Value = "tw", Description = Resources.VM01054 },
                new ComboBoxItemModel() { Value = "other", Description = Resources.VM01055 },
                new ComboBoxItemModel() { Value = "test", Description = Resources.VM01056 },
                new ComboBoxItemModel() { Value = "r18", Description = Resources.VM01057 },
            };
        }
    }
}
