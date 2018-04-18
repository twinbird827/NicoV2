using NicoV2.Common;
using NicoV2.IO;
using NicoV2.Mvvm.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.Model
{
    public class LoginModel : HttpModel
    {
        /// <summary>
        /// ｼﾝｸﾞﾙﾄﾝﾊﾟﾀｰﾝのため、ﾌﾟﾗｲﾍﾞｰﾄｺﾝｽﾄﾗｸﾀ。
        /// </summary>
        private LoginModel()
        {
            this.Method = "POST";
            this.ContentType = "application/x-www-form-urlencoded";
        }

        /// <summary>
        /// LoginModelｲﾝｽﾀﾝｽを取得します。
        /// </summary>
        public static LoginModel Instance { get; private set; } = new LoginModel();

        /// <summary>
        /// ﾛｸﾞｲﾝしているかどうか
        /// </summary>
        public bool IsLogin
        {
            get { return _IsLogin; }
            set { SetProperty(ref _IsLogin, value); }
        }
        private bool _IsLogin = false;

        /// <summary>
        /// ﾌﾟﾚﾐｱﾑかどうか
        /// </summary>
        public bool IsPremium
        {
            get { return _IsPremium; }
            set { SetProperty(ref _IsPremium, value); }
        }
        private bool _IsPremium = false;

        /// <summary>
        /// ｸｯｷｰｺﾝﾃﾅ
        /// </summary>
        public CookieContainer Cookie
        {
            get { return _Cookie; }
            set { SetProperty(ref _Cookie, value); }
        }
        private CookieContainer _Cookie = new CookieContainer();

        /// <summary>
        /// 規定のﾕｰｻﾞ、ﾊﾟｽﾜｰﾄﾞを用いて、ﾛｸﾞｲﾝ処理を実行します。
        /// </summary>
        /// <remarks>既にﾛｸﾞｲﾝ済みの場合は中断します。</remarks>
        public void Login()
        {
            if (IsLogin)
            {
                // ﾛｸﾞｲﾝ済みの場合は中断
                return;
            }

            // ﾛｸﾞｲﾝ処理
            Login(Variables.MailAddress, Variables.Password);
        }

        /// <summary>
        /// 指定したﾕｰｻﾞ、ﾊﾟｽﾜｰﾄﾞを用いて、ﾛｸﾞｲﾝ処理を実行します。
        /// </summary>
        /// <param name="mail">ﾒｰﾙｱﾄﾞﾚｽ</param>
        /// <param name="password">ﾊﾟｽﾜｰﾄﾞ</param>
        /// <remarks>既にﾛｸﾞｲﾝ済みの場合もﾛｸﾞｲﾝし直します。</remarks>
        public void Login(string mail, string password)
        {
            // ﾛｸﾞｲﾝﾃｽﾄ前にﾌﾟﾛﾊﾟﾃｨを初期化する。
            Initialize();

            if (string.IsNullOrWhiteSpace(mail) || string.IsNullOrWhiteSpace(password))
            {
                // ﾛｸﾞｲﾝ情報が指定されていない場合は中断
                ServiceFactory.MessageService.Error("ログイン情報が入力されていません。");
                return;
            }

            try
            {
                // ﾘｸｴｽﾄを取得する。
                var req = GetRequest(Constants.LoginUrl, HttpUtil.ToLoginParameter(mail, password)); // GetRequest(mail, password);
                var res = GetResponse(req, false);

                // ﾘｸｴｽﾄからｸｯｷｰ取得
                CookieCollection cookies = req.CookieContainer.GetCookies(req.RequestUri);

                // TODO これいる？
                //IFormatter formatter = new BinaryFormatter();
                //using (Stream serializationStream = new FileStream(Variable.LOG_FOLDER + "\\Cookie.bin", FileMode.Create, FileAccess.Write, FileShare.None))
                //{
                //    formatter.Serialize(serializationStream, cookies);
                //}

                // ﾚｽﾎﾟﾝｽを用いてﾛｸﾞｲﾝ処理を実行する。
                if (LoginWithResponse(HttpUtil.GetResponseString(res)))
                {
                    // ﾛｸﾞｲﾝが成功したら、ﾛｸﾞｲﾝ情報を保存する。
                    Variables.MailAddress = mail;
                    Variables.Password = password;

                    // 取得したｸｯｷｰをIEに流用
                    HttpUtil.InternetSetCookie(cookies);

                    // 自身のｸｯｷｰｺﾝﾃﾅに追加
                    Cookie.Add(cookies);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());   // TODO
                Initialize();
                ServiceFactory.MessageService.Error("何らかの原因でエラーが発生しました。");
            }

        }

        ///// <summary>
        ///// 指定したUrlとﾊﾟﾗﾒｰﾀでHTTPﾘｸｴｽﾄを取得します。
        ///// </summary>
        ///// <param name="url">URL</param>
        ///// <param name="parameter">ﾊﾟﾗﾒｰﾀ</param>
        ///// <returns><code>HttpWebRequest</code></returns>
        //private HttpWebRequest GetRequest(string mail, string password)
        //{
        //    string url = Constants.LoginUrl;
        //    string parameter = HttpUtil.ToLoginParameter(mail, password);

        //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        //    req.Method = "POST";
        //    req.ContentType = "application/x-www-form-urlencoded";

        //    req.Timeout = Constants.Timeout;
        //    req.ReadWriteTimeout = Constants.Timeout;
        //    req.UserAgent = Constants.UserAgent;
        //    req.Referer = Constants.Referer;

        //    // ﾛｸﾞｲﾝ情報を持つｸｯｷｰをｺﾝﾃﾅに追加する。
        //    req.CookieContainer = new CookieContainer();
        //    req.CookieContainer.Add(Cookie.GetCookies(req.RequestUri));

        //    // ﾊﾟﾗﾒｰﾀが存在する場合、ｽﾄﾘｰﾑに追記する。
        //    if (!string.IsNullOrWhiteSpace(parameter))
        //    {
        //        byte[] bytes = Encoding.ASCII.GetBytes(parameter);
        //        req.ContentLength = bytes.LongLength;
        //        using (Stream stream = req.GetRequestStream())
        //        {
        //            stream.Write(bytes, 0, (int)bytes.LongLength);
        //        }
        //    }

        //    return req;
        //}

        /// <summary>
        /// ﾚｽﾎﾟﾝｽを用いてﾛｸﾞｲﾝ処理を実行します。
        /// </summary>
        /// <param name="expression">ﾚｽﾎﾟﾝｽﾃﾞｰﾀ</param>
        /// <returns>ﾛｸﾞｲﾝ成功：true / 失敗：false</returns>
        private bool LoginWithResponse(string expression)
        {
            if (expression.Contains("ログイン情報が間違っています"))
            {
                ServiceFactory.MessageService.Error("入力されたログイン情報が間違っています。");
            }
            else if (expression.Contains("closed=1&"))
            {
                ServiceFactory.MessageService.Error("入力されたアカウント情報が間違っています。");
            }
            else if (expression.Contains("error=access_locked"))
            {
                ServiceFactory.MessageService.Error("連続アクセス検出のためアカウントロック中\r\nしばらく時間を置いてから試行してください。");
            }
            else if (expression.Contains("is_premium=0&") || expression.Contains("is_premium=1&"))
            {
                // ﾛｸﾞｲﾝ成功
                // ﾛｸﾞｲﾝﾌﾗｸﾞを立てる。
                IsLogin = true;
                IsPremium = expression.Contains("is_premium=1&");
                return true;
            }
            else
            {
                ServiceFactory.MessageService.Error("何らかの原因でログインできませんでした\r\nしばらく時間を置いてから試行してください。");
            }

            return false;
        }

        /// <summary>
        /// ﾌﾟﾛﾊﾟﾃｨを初期化します。
        /// </summary>
        private void Initialize()
        {
            IsLogin = false;
            IsPremium = false;
            Cookie = null;
            Cookie = new CookieContainer();
        }

    }
}
