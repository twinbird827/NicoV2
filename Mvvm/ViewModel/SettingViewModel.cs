using NicoV2.IO;
using NicoV2.Mvvm.Model;
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
        private LoginModel LoginSource { get; set; }

        public string MailAddress
        {
            get { return _MailAddress; }
            set { SetProperty(ref _MailAddress, value); }
        }
        private string _MailAddress;

        public string Password
        {
            get { return _Password; }
            set { SetProperty(ref _Password, value); }
        }
        private string _Password;

        public ICommand LoginCommand
        {
            get { return _LoginCommand = _LoginCommand ?? new RelayCommand(
                _ =>
                {
                    // ﾛｸﾞｲﾝ実行
                    this.LoginSource.Login(MailAddress, Password);
                    Console.WriteLine("{0}", this.LoginSource.IsLogin);
                    //Console.WriteLine("mail: {0}, pass: {1}", MailAddress, Password);
                    //Variables.MailAddress = MailAddress;
                    //Variables.Password = Password;
                },
                _ => {
                    return
                        !string.IsNullOrWhiteSpace(MailAddress) &&
                        !string.IsNullOrWhiteSpace(Password);
                });
            }
        }
        public ICommand _LoginCommand;

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
    }
}
