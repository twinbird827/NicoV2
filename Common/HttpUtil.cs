using NicoV2.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NicoV2.Common
{
    public class HttpUtil
    {
        /// <summary>
        /// ｸｯｷｰに設定する有効期限を取得します。
        /// </summary>
        /// <param name="d">有効期限の基準となる日付。ﾃﾞﾌｫﾙﾄは現在日時</param>
        /// <returns>有効期限を表す文字列</returns>
        public static string GetExpiresDate(DateTime d = default(DateTime))
        {
            d = default(DateTime) == d ? DateTime.Now : d;
            return d.AddMonths(1).ToString("r", DateTimeFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// HttpWebRequestからHttpWebResponseを表す文字列を取得します。
        /// </summary>
        /// <param name="httpWebRequest"><see cref="HttpWebRequest"/></param>
        /// <returns><see cref="HttpWebResponse"/>のｽﾄﾘｰﾑ文字列</returns>
        public static string GetResponseString(HttpWebRequest httpWebRequest)
        {
            string expression = String.Empty;
            using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                expression = GetResponseString(httpWebResponse);
            }
            return expression;
        }

        /// <summary>
        /// 文字列をUrlｴﾝｺｰﾄﾞ文字列に変換します。
        /// </summary>
        /// <param name="txt">変換前文字列</param>
        /// <returns>変換後文字列</returns>
        public static string ToUrlEncode(string txt)
        {
            return HttpUtility.UrlEncode(txt);
        }

        /// <summary>
        /// HttpWebRequestからHttpWebResponseを表す文字列を取得します。
        /// </summary>
        /// <param name="httpWebRequest"><see cref="HttpWebRequest"/></param>
        /// <returns><see cref="HttpWebResponse"/>のｽﾄﾘｰﾑ文字列</returns>
        public static string GetResponseString(HttpWebResponse httpWebResponse)
        {
            string expression = String.Empty;
            using (Stream responseStream = httpWebResponse.GetResponseStream())
            {
                StreamReader streamReader = new StreamReader(responseStream, Constants.Encoding);
                expression = streamReader.ReadToEnd();
            }
            return expression;
        }

        /// <summary>
        /// IEにｸｯｷｰを設定します。
        /// </summary>
        /// <param name="cookies">設定するｸｯｷｰ</param>
        /// <param name="isDispose">設定後にｸｯｷｰを破棄するかどうか。ﾃﾞﾌｫﾙﾄ=true</param>
        public static void InternetSetCookie(CookieCollection cookies, bool isDispose = true)
        {
            InternetSetCookie(cookies.OfType<Cookie>().ToArray(), isDispose);
        }

        /// <summary>
        /// IEにｸｯｷｰを設定します。
        /// </summary>
        /// <param name="cookies">設定するｸｯｷｰ</param>
        /// <param name="isDispose">設定後にｸｯｷｰを破棄するかどうか。ﾃﾞﾌｫﾙﾄ=true</param>
        public static void InternetSetCookie(Cookie[] cookies, bool isDispose = true)
        {
            // 取得したｸｯｷｰをIEに流用
            Array.ForEach(
                cookies,
                cookie => InternetSetCookie(cookie, isDispose)
            );
        }

        /// <summary>
        /// IEにｸｯｷｰを設定します。
        /// </summary>
        /// <param name="cookies">設定するｸｯｷｰ</param>
        /// <param name="isDispose">設定後にｸｯｷｰを破棄するかどうか。ﾃﾞﾌｫﾙﾄ=true</param>
        public static void InternetSetCookie(Cookie cookie, bool isDispose = true)
        {
            // 取得したｸｯｷｰをIEに流用
            Win32Methods.InternetSetCookie(
                Constants.CookieUrl,
                cookie.Name,
                String.Format(Constants.CookieData,
                    cookie.Value,
                    GetExpiresDate()
                )
            );

            if (isDispose)
            {
                IEnumerable<Cookie> cookies = new Cookie[] { cookie };
                var disposable = cookies.OfType<IDisposable>().FirstOrDefault();
                if (disposable != null)
                {
                    // 破棄可能なｸｯｷｰは破棄する。
                    disposable.Dispose();
                }
            }
        }

        public static string ToLoginParameter(string mailaddress, string password)
        {
            return string.Format(
                Constants.LoginParameter, 
                ToUrlEncode(mailaddress),
                ToUrlEncode(password)
            );
        }
    }
}
