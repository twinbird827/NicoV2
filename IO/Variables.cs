using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.IO
{
    public class Variables : Ini
    {
        private static Variables Instance { get; set; } = new Variables();

        protected Variables() : base(Constants.AppConfig, Constants.AppConfigDefaultSection)
        {
        }

        public static string MailAddress {
            get
            {
                var tmp = Instance["MAIL_ADDRESS"];
                return 
            }
            set
            {
            }
        }
        public static string Password { get; set; }
    }
}
