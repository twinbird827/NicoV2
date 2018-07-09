using NicoV2.Properties;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.Model
{
    public class ComboSortModel : BindableBase
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
        public static ComboSortModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ComboSortModel();
                }
                return _Instance;
            }
        }
        private static ComboSortModel _Instance;

        private ComboSortModel()
        {
            _Items = new ObservableSynchronizedCollection<ComboBoxItemModel>
            {
                //new SortItemModel() { Keyword = "-title", Description = "-title" },
                //new SortItemModel() { Keyword = "%2btitle", Description = "+title" },
                //new SortItemModel() { Keyword = "-description", Description = "-description" },
                //new SortItemModel() { Keyword = "%2bdescription", Description = "+description" },
                //new SortItemModel() { Keyword = "-tags", Description = "-tags" },
                //new SortItemModel() { Keyword = "%2btags", Description = "+tags" },
                //new SortItemModel() { Keyword = "-categoryTags", Description = "-categoryTags" },
                //new SortItemModel() { Keyword = "%2bcategoryTags", Description = "+categoryTags" },
                new ComboBoxItemModel() { Value = "-viewCounter", Description = Resources.VM01001 },
                new ComboBoxItemModel() { Value = "%2bviewCounter", Description = Resources.VM01002 },
                new ComboBoxItemModel() { Value = "-mylistCounter", Description = Resources.VM01003 },
                new ComboBoxItemModel() { Value = "%2bmylistCounter", Description = Resources.VM01004 },
                new ComboBoxItemModel() { Value = "-commentCounter", Description = Resources.VM01005 },
                new ComboBoxItemModel() { Value = "%2bcommentCounter", Description = Resources.VM01006 },
                new ComboBoxItemModel() { Value = "-startTime", Description = Resources.VM01007 },
                new ComboBoxItemModel() { Value = "%2bstartTime", Description = Resources.VM01008 },
                new ComboBoxItemModel() { Value = "-lastCommentTime", Description = Resources.VM01009 },
                new ComboBoxItemModel() { Value = "%2blastCommentTime", Description = Resources.VM01010 },
                new ComboBoxItemModel() { Value = "-lengthSeconds", Description = Resources.VM01011 },
                new ComboBoxItemModel() { Value = "%2blengthSeconds", Description = Resources.VM01012 }
            };
        }
    }
}
