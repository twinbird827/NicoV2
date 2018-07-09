using NicoV2.IO;
using NicoV2.Mvvm.Model;
using NicoV2.Mvvm.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NicoV2.Mvvm.ViewModel
{
    public class SettingViewModel : WorkSpaceViewModel
    {
        /// <summary>
        /// ﾛｸﾞｲﾝｿｰｽ
        /// </summary>
        private LoginModel LoginSource { get; set; }

        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public SettingViewModel()
        {
            // ﾛｸﾞｲﾝﾓﾃﾞﾙを取得
            this.LoginSource = LoginModel.Instance;

            // ﾌﾟﾛﾊﾟﾃｨ初期値
            MailAddress = Variables.MailAddress;
            Password = Variables.Password;

            // なぜか Password が string.Empty だと上手くﾊﾞｲﾝﾃﾞｨﾝｸﾞできないので対処
            Password = string.IsNullOrEmpty(Password) ? null : Password;
        }

        /// <summary>
        /// ﾒｰﾙｱﾄﾞﾚｽ
        /// </summary>
        public string MailAddress
        {
            get { return _MailAddress; }
            set { SetProperty(ref _MailAddress, value); }
        }
        private string _MailAddress;

        /// <summary>
        /// ﾊﾟｽﾜｰﾄﾞ
        /// </summary>
        public string Password
        {
            get { return _Password; }
            set { SetProperty(ref _Password, value); }
        }
        private string _Password;

        /// <summary>
        /// ﾛｸﾞｲﾝ処理
        /// </summary>
        public ICommand OnLogin
        {
            get { return _OnLogin = _OnLogin ?? new RelayCommand(
                _ =>
                {
                    // ﾛｸﾞｲﾝ実行
                    this.LoginSource.Login(MailAddress, Password);
                    ServiceFactory.MessageService.Debug(this.LoginSource.IsLogin.ToString());
                },
                _ => {
                    return
                        !string.IsNullOrWhiteSpace(MailAddress) &&
                        !string.IsNullOrWhiteSpace(Password);
                });
            }
        }
        public ICommand _OnLogin;

    }
}
