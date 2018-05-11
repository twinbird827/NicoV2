using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV2.Mvvm.Model
{
    public class SortItemModel : BindableBase
    {
        /// <summary>
        /// Urlﾊﾟﾗﾒｰﾀに渡すｷｰﾜｰﾄﾞ
        /// </summary>
        public string Keyword
        {
            get { return _Keyword; }
            set { SetProperty(ref _Keyword, value); }
        }
        private string _Keyword = null;

        /// <summary>
        /// ｷｰﾜｰﾄﾞの説明
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set { SetProperty(ref _Description, value); }
        }
        private string _Description = null;
    }
}
