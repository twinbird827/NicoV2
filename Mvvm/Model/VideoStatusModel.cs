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
    public class VideoStatusModel : BindableBase
    {
        /// <summary>
        /// ｼﾝｸﾞﾙﾄﾝﾊﾟﾀｰﾝのｲﾝｽﾀﾝｽ
        /// </summary>
        public static VideoStatusModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    // ﾓﾃﾞﾙを設定ﾌｧｲﾙから取得する。取得できなかった場合はﾃﾞﾌｫﾙﾄﾒﾆｭｰとする。
                    _Instance = JsonUtil.Deserialize<VideoStatusModel>(Constants.VideoStatusModelPath) ?? CreateDefault();
                }
                return _Instance;
            }
        }
        private static VideoStatusModel _Instance;

        /// <summary>
        /// ﾌﾟﾗｲﾍﾞｰﾄｺﾝｽﾄﾗｸﾀ
        /// </summary>
        private VideoStatusModel()
        {

        }

        /// <summary>
        /// ﾃﾞﾌｫﾙﾄｲﾝｽﾀﾝｽを作成する。
        /// </summary>
        /// <returns></returns>
        private static VideoStatusModel CreateDefault()
        {
            // ﾃﾞﾌｫﾙﾄの作成
            return new VideoStatusModel();
        }

        /// <summary>
        /// ﾒﾆｭｰﾂﾘｰﾋﾞｭｰ構成
        /// </summary>
        public ObservableSynchronizedCollection<VideoStatusItemModel> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        private ObservableSynchronizedCollection<VideoStatusItemModel> _Items;

        #region IDisposable Support

        protected override void OnDisposing()
        {
            base.OnDisposing();

            // ｲﾝｽﾀﾝｽのﾃﾞｰﾀを保存する。
            JsonUtil.Serialize(Constants.VideoStatusModelPath, this);
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();

            if (Items != null)
            {
                Items.Clear();
                Items = null;
            }
        }

        #endregion

    }
}
