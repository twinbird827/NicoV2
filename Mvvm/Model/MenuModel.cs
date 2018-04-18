using NicoV2.Common;
using NicoV2.IO;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.Model
{
    public class MenuModel : BindableBase
    {
        /// <summary>
        /// ｼﾝｸﾞﾙﾄﾝﾊﾟﾀｰﾝのｲﾝｽﾀﾝｽ
        /// </summary>
        public static MenuModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    // ﾓﾃﾞﾙを設定ﾌｧｲﾙから取得する。取得できなかった場合はﾃﾞﾌｫﾙﾄﾒﾆｭｰとする。
                    _Instance = JsonUtil.Deserialize<MenuModel>(Constants.MenuModelPath) ?? CreateDefault();
                }
                return _Instance;
            }
        }
        private static MenuModel _Instance;

        /// <summary>
        /// ﾌﾟﾗｲﾍﾞｰﾄｺﾝｽﾄﾗｸﾀ
        /// </summary>
        private MenuModel()
        {

        }

        /// <summary>
        /// ﾃﾞﾌｫﾙﾄﾒﾆｭｰを作成する。
        /// </summary>
        /// <returns></returns>
        private static MenuModel CreateDefault()
        {
            // ﾃﾞﾌｫﾙﾄﾒﾆｭｰの作成
            var model = new MenuModel()
            {
                Parameters = new Dictionary<string, object>()
            };

            model.MenuItems = new ObservableSynchronizedCollection<MenuItemModel>();
            model.MenuItems.Add(new MenuItemModel("TEST1", MenuItemType.SearchByWord));
            model.MenuItems.Add(new MenuItemModel("TEST2", MenuItemType.Ranking));
            model.MenuItems.Add(new MenuItemModel("FAV0", MenuItemType.MylistOfOther)
            {
                Children = new ObservableSynchronizedCollection<MenuItemModel>
                {
                    new MenuItemModel("FAV1", MenuItemType.MylistOfOther),
                    new MenuItemModel("FAV2", MenuItemType.MylistOfOther),
                    new MenuItemModel("FAV3", MenuItemType.MylistOfOther)
                }
            });
            model.MenuItems.Add(new MenuItemModel("TEST4", MenuItemType.Temporary));
            model.MenuItems.Add(new MenuItemModel("TEST5", MenuItemType.SearchByMylist));
            model.MenuItems.Add(new MenuItemModel("TEST6", MenuItemType.MylistOfMe));
            model.MenuItems.Add(new MenuItemModel("TEST7", MenuItemType.Setting));

            model.Parameters.Add("key1", "value1");
            model.Parameters.Add("key2", "value2");
            model.Parameters.Add("key3", "value3");
            return model;
        }

        /// <summary>
        /// ﾒﾆｭｰﾂﾘｰﾋﾞｭｰ構成
        /// </summary>
        public ObservableSynchronizedCollection<MenuItemModel> MenuItems
        {
            get { return _MenuItems; }
            set { SetProperty(ref _MenuItems, value); }
        }
        private ObservableSynchronizedCollection<MenuItemModel> _MenuItems;

        public Dictionary<string, object> Parameters
        {
            get { return _Parameters; }
            set { SetProperty(ref _Parameters, value); }
        }
        private Dictionary<string, object> _Parameters;

        #region IDisposable Support

        protected override void OnDisposing()
        {
            base.OnDisposing();

            // ｲﾝｽﾀﾝｽのﾃﾞｰﾀを保存する。
            JsonUtil.Serialize(Constants.MenuModelPath, this);
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            if (MenuItems != null)
            {
                MenuItems.Clear();
                MenuItems = null;
            }
        }

        #endregion
    }
}
