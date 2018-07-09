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

                    // ﾏｲﾘｽﾄ更新
                    var items = _Instance.MenuItems.Where(m => m.Type == MenuItemType.MylistOfMe).First().Items;
                    items.Clear();
                    foreach (var item in MylistOfMeModel.Instance.Reload())
                    {
                        items.Add(item);
                    }
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
            model.MenuItems.Add(new MenuItemModel("SearchByWord", MenuItemType.SearchByWord));
            model.MenuItems.Add(new MenuItemModel("Ranking", MenuItemType.Ranking));
            model.MenuItems.Add(new MenuItemModel("Temporary", MenuItemType.Temporary));
            model.MenuItems.Add(new MenuItemModel("MyListOfMe", MenuItemType.MylistOfMe));
            model.MenuItems.Add(new MenuItemModel("SearchByMylist", MenuItemType.SearchByMylist)
            {
                Children = new ObservableSynchronizedCollection<MenuItemModel>
                {
                    new MenuItemModel("MyListOfOther1", MenuItemType.MylistOfOther),
                    new MenuItemModel("MyListOfOther2", MenuItemType.MylistOfOther)
                }
            });
            model.MenuItems.Add(new MenuItemModel("Setting", MenuItemType.Setting));

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
