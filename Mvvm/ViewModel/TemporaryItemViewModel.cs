using NicoV2.Mvvm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.ViewModel
{
    public class TemporaryItemViewModel : WorkSpaceItemViewModel
    {
        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        /// <param name="m"></param>
        public TemporaryItemViewModel(VideoModel m)
            : base(m)
        {
        }

        /// <summary>
        /// 最新ｺﾒﾝﾄ
        /// </summary>
        public string LastResBody
        {
            get { return Source.LastResBody; }
            set { SetProperty(ref _LastResBody, value); Source.LastResBody = value; }
        }
        private string _LastResBody = null;

    }
}
