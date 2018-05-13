using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.Model
{
    public class SortModel : BindableBase
    {
        /// <summary>
        /// ｿｰﾄﾘｽﾄ構成
        /// </summary>
        public ObservableSynchronizedCollection<SortItemModel> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        private ObservableSynchronizedCollection<SortItemModel> _Items;

        /// <summary>
        /// ｲﾝｽﾀﾝｽ (ｼﾝｸﾞﾙﾄﾝﾊﾟﾀｰﾝ)
        /// </summary>
        public static SortModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SortModel();
                }
                return _Instance;
            }
        }
        private static SortModel _Instance;

        private SortModel()
        {
            _Items = new ObservableSynchronizedCollection<SortItemModel>
            {
                //new SortItemModel() { Keyword = "-title", Description = "-title" },
                //new SortItemModel() { Keyword = "+title", Description = "+title" },
                //new SortItemModel() { Keyword = "-description", Description = "-description" },
                //new SortItemModel() { Keyword = "+description", Description = "+description" },
                //new SortItemModel() { Keyword = "-tags", Description = "-tags" },
                //new SortItemModel() { Keyword = "+tags", Description = "+tags" },
                //new SortItemModel() { Keyword = "-categoryTags", Description = "-categoryTags" },
                //new SortItemModel() { Keyword = "+categoryTags", Description = "+categoryTags" },
                new SortItemModel() { Keyword = "-viewCounter", Description = "-viewCounter" },
                new SortItemModel() { Keyword = "+viewCounter", Description = "+viewCounter" },
                new SortItemModel() { Keyword = "-mylistCounter", Description = "-mylistCounter" },
                new SortItemModel() { Keyword = "+mylistCounter", Description = "+mylistCounter" },
                new SortItemModel() { Keyword = "-commentCounter", Description = "-commentCounter" },
                new SortItemModel() { Keyword = "+commentCounter", Description = "+commentCounter" },
                new SortItemModel() { Keyword = "-startTime", Description = "-startTime" },
                new SortItemModel() { Keyword = "+startTime", Description = "+startTime" },
                new SortItemModel() { Keyword = "-lastCommentTime", Description = "-lastCommentTime" },
                new SortItemModel() { Keyword = "+lastCommentTime", Description = "+lastCommentTime" },
                new SortItemModel() { Keyword = "-lengthSeconds", Description = "-lengthSeconds" },
                new SortItemModel() { Keyword = "+lengthSeconds", Description = "+lengthSeconds" }
            };
        }
    }
}
