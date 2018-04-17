using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.Service
{
    public interface IMessageService
    {
        /// <summary>
        /// ｴﾗｰをﾒｯｾｰｼﾞ処理します。
        /// </summary>
        /// <param name="message">ﾒｯｾｰｼﾞ</param>
        void Error(string message);
    }
}
