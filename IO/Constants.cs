using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.IO
{
    public class Constants
    {
        /**************************************************
         * 共通
         **************************************************/

        /// <summary>
        /// ｱﾌﾟﾘｹｰｼｮﾝ固有のId
        /// </summary>
        public const string ApplicationId = "P&imsbQSWKKQ";

        /// <summary>
        /// 設定ﾌｧｲﾙのﾊﾟｽ
        /// </summary>
        public const string AppConfigPath = @".\lib\app-config.ini";

        /// <summary>
        /// ﾒﾆｭｰ構成のﾊﾟｽ
        /// </summary>
        public const string MenuModelPath = @".\lib\menu-model.json";

        /// <summary>
        /// 設定ﾌｧｲﾙのﾃﾞﾌｫﾙﾄｾｸｼｮﾝ
        /// </summary>
        public const string AppConfigDefaultSection = "NicoV2";

        /// <summary>
        /// ｴﾝｺｰﾃﾞｨﾝｸﾞ
        /// </summary>
        public static Encoding Encoding {
            get
            {
                _Encoding = _Encoding ?? Encoding.GetEncoding(Variables.Encoding);
                return _Encoding;
            }
            private set
            {
                _Encoding = value;
            }
            
        }
        private static Encoding _Encoding;

        /**************************************************
         * HTTPS関連の定数
         **************************************************/

        /// <summary>
        /// ﾛｸﾞｲﾝUrl
        /// </summary>
        public const string LoginUrl = "https://secure.nicovideo.jp/secure/login";

        /// <summary>
        /// ﾛｸﾞｲﾝﾊﾟﾗﾒｰﾀ
        /// </summary>
        public const string LoginParameter = "site=niconico&mail={0}&password={1}&next_url=http://flapi.nicovideo.jp/api/getflv/sm9";

        /// <summary>
        /// ﾀｲﾑｱｳﾄ
        /// </summary>
        public const int Timeout = 100000;

        /// <summary>
        /// ﾕｰｻﾞｴｰｼﾞｪﾝﾄ
        /// </summary>
        public const string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:17.0) Gecko/20100101 Firefox/17.0";

        /// <summary>
        /// ﾘﾌｧﾗ
        /// </summary>
        public const string Referer = "http://www.nicovideo.jp/";

        /// <summary>
        /// ｸｯｷｰUrl
        /// </summary>
        public const string CookieUrl = "http://nicovideo.jp/";

        /// <summary>
        /// ｸｯｷｰﾃﾞｰﾀ
        /// </summary>
        public const string CookieData = "{0}; expires = {1}";

        /// <summary>
        /// ﾏｲﾘｽﾄUrl
        /// </summary>
        public const string MylistUrl = "http://www.nicovideo.jp/mylist/";

        /// <summary>
        /// ﾏｲﾘｽﾄﾃﾞﾌｫﾙﾄ
        /// </summary>
        public const string MylistDefault = "http://www.nicovideo.jp/api/deflist/list";

        /**************************************************
         * 検索処理関連の定数
         **************************************************/

        public const string TargetTag = "tagsExact";

        public const string TargetKeyword = "title,description,tags";

        public const string Fields = "contentId,title,description,tags,categoryTags,viewCounter,mylistCounter,commentCounter,startTime,lastCommentTime,lengthSeconds,thumbnailUrl,communityIcon";

        public const string Context = "kaz.server-on.net/NicoV2";

        public const string SearchByWordUrl = "http://api.search.nicovideo.jp/api/v2/video/contents/search?q={0}&targets={1}&fields={2}&_sort={3}&_offset={4}&_limit={5}&_context={6}";

    }
}
