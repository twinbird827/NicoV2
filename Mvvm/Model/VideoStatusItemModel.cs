using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.Model
{
    public class VideoStatusItemModel : BindableBase
    {
        /// <summary>
        /// ｺﾝﾃﾝﾂId
        /// </summary>
        public string ContentId
        {
            get { return _ContentId; }
            set { SetProperty(ref _ContentId, value); }
        }
        private string _ContentId = null;

        /// <summary>
        /// ｽﾃｰﾀｽ
        /// </summary>
        public string Status
        {
            get { return _Status; }
            set { SetProperty(ref _Status, value); }
        }
        private string _Status = null;
    }
}
