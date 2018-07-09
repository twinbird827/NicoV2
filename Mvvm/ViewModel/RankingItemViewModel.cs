using NicoV2.Mvvm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace NicoV2.Mvvm.ViewModel
{
    public class RankingItemViewModel : WorkSpaceItemViewModel
    {
        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        /// <param name="m"></param>
        public RankingItemViewModel(VideoModel m)
            : base(m)
        {
        }
    }
}
