using NicoV2.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

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

        /// <summary>
        /// ﾛｸﾞｲﾝﾊﾟﾗﾒｰﾀに変換します。
        /// </summary>
        /// <param name="mailaddress">ﾒｰﾙｱﾄﾞﾚｽ</param>
        /// <param name="password">ﾊﾟｽﾜｰﾄﾞ</param>
        /// <returns></returns>
        public static string ToLoginParameter(string mailaddress, string password)
        {
            return string.Format(
                Constants.LoginParameter, 
                ToUrlEncode(mailaddress),
                ToUrlEncode(password)
            );
        }

        /// <summary>
        /// 指定したUrlからｲﾒｰｼﾞをﾊﾞｲﾄ配列として取得します。
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns></returns>
        public static async Task<byte[]> DownLoadImageBytesAsync(string url)
        {
            using (var web = new HttpClient())
            {
                return await web.GetByteArrayAsync(url);
            }
        }

        /// <summary>
        /// 指定したﾊﾞｲﾄ配列からﾋﾞｯﾄﾏｯﾌﾟｲﾒｰｼﾞを作成します。
        /// </summary>
        /// <param name="bytes">ﾊﾞｲﾄ配列</param>
        /// <param name="freezing">Freezeするかどうか</param>
        /// <returns></returns>
        public static BitmapImage CreateBitmap(byte[] bytes, bool freezing = true)
        {
            using (var stream = new WrappingStream(new MemoryStream(bytes)))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                if (freezing && bitmap.CanFreeze)
                {
                    bitmap.Freeze();
                }
                return bitmap;
            }
        }

        /// <summary>
        /// 指定したﾃﾞｨｽﾊﾟｯﾁｬで、指定したUrlからｲﾒｰｼﾞを取得する非同期ﾀｽｸを取得します。
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dispatcher"></param>
        /// <returns></returns>
        public static async Task<BitmapImage> DownloadImageAsync(string url, Dispatcher dispatcher)
        {
            var bytes = await DownLoadImageBytesAsync(url).ConfigureAwait(false);
            return await dispatcher.InvokeAsync(() => CreateBitmap(bytes));
        }

        /// <summary>
        /// 指定したUrlのｻﾑﾈｲﾙを取得する
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns>ｻﾑﾈｲﾙ画像</returns>
        public static async Task<BitmapImage> GetThumbnail(string url)
        {
            return await DownloadImageAsync(url, App.Current.Dispatcher);
        }

    }
}
