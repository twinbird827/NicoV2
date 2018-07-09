using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

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

        /// <summary>
        /// 現在メッセージ待ち行列の中にある全てのUIメッセージを処理します。
        /// </summary>
        public static void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            var callback = new DispatcherOperationCallback(obj =>
            {
                ((DispatcherFrame)obj).Continue = false;
                return null;
            });
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, callback, frame);
            Dispatcher.PushFrame(frame);
        }
    }
}
