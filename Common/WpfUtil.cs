using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NicoV2.Common
{
    public static class WpfUtil
    {
        /// <summary>
        /// ｱｸﾃｨﾌﾞｳｨﾝﾄﾞｳを取得する
        /// </summary>
        /// <returns>Windowｲﾝｽﾀﾝｽ。取得できない場合はnull</returns>
        public static Window GetActiveWindow()
        {
            var owner =
                Application.Current.Windows.OfType<Window>().SingleOrDefault(
                    x => x.IsActive
            );
            return owner;
        }

    }
}
