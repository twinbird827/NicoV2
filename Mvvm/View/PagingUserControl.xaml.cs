using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NicoV2.Mvvm.View
{
    /// <summary>
    /// PagingUserControl.xaml の相互作用ロジック
    /// </summary>
    /// <remarks>
    /// ・TODOリスト
    /// Current：表示位置（ページ数換算）
    /// Offset：表示位置（データ数）
    /// Limit：1ページに表示する件数
    /// PageLength：ページングボタンの数
    /// DataLength：データ件数
    /// 
    /// ・発生すべきイベント
    /// Current.Changed - Current位置によってデザイン再描写
    /// Initialize - Current=1として再処理 - Limit or PageLength or DataLength が変更したときに実行する (すべての変数>0であること)
    /// 
    /// </remarks>
    public partial class PagingUserControl : UserControl
    {
        #region Properties

        /// <summary>
        /// 現在の表示位置をページ数換算で取得、または設定します。
        /// </summary>
        public int Current
        {
            get { return (int)GetValue(CurrentProperty); }
            set { SetValueAndRaiseEvent(CurrentProperty, value, Current, CurrentChanged); }
        }

        public static readonly DependencyProperty CurrentProperty =
            DependencyProperty.Register("Current", typeof(int), typeof(PagingUserControl), new PropertyMetadata());

        /// <summary>
        /// 現在の表示位置をデータ件数換算で取得、または設定します。
        /// </summary>
        public int Offset
        {
            get { return (int)GetValue(OffsetProperty); }
            set { SetValue(OffsetProperty, value); }
        }

        public static readonly DependencyProperty OffsetProperty =
            DependencyProperty.Register("Offset", typeof(int), typeof(PagingUserControl), new PropertyMetadata());

        /// <summary>
        /// 1ページに表示する件数を取得、または設定します。
        /// </summary>
        public int Limit
        {
            get { return (int)GetValue(LimitProperty); }
            set { SetValueAndRaiseEvent(LimitProperty, value, Limit, PropertyInitialized); }
        }

        public static readonly DependencyProperty LimitProperty =
            DependencyProperty.Register("Limit", typeof(int), typeof(PagingUserControl), new PropertyMetadata());

        /// <summary>
        /// ページングボタンの数を取得、または設定します。
        /// </summary>
        public int PageLength
        {
            get { return (int)GetValue(PageLengthProperty); }
            set { SetValueAndRaiseEvent(PageLengthProperty, value, PageLength, PropertyInitialized); }
        }

        public static readonly DependencyProperty PageLengthProperty =
            DependencyProperty.Register("PageLength", typeof(int), typeof(PagingUserControl), new PropertyMetadata());

        /// <summary>
        /// データ件数を取得、または設定します。
        /// </summary>
        public int DataLength
        {
            get { return (int)GetValue(DataLengthProperty); }
            set { SetValueAndRaiseEvent(DataLengthProperty, value, DataLength, PropertyInitialized); }
        }

        public static readonly DependencyProperty DataLengthProperty =
            DependencyProperty.Register("DataLength", typeof(int), typeof(PagingUserControl), new PropertyMetadata());

        #endregion

        public event EventHandler CurrentChanged;

        public event EventHandler PropertyInitialized;

        public PagingUserControl()
        {
            InitializeComponent();

            this.DataContext = this;
            this.CurrentChanged += OnCurrentChanged;
            this.PropertyInitialized += OnPropertyInitialized;
        }

        private void SetValueAndRaiseEvent(DependencyProperty dp, object n, object o, EventHandler e)
        {
            SetValue(dp, n);
            if (o != null && !o.Equals(n))
            {
                e(this, new EventArgs());
            }
        }

        private void OnCurrentChanged(object sender, EventArgs e)
        {

        }

        private void OnPropertyInitialized(object sender, EventArgs e)
        {

        }


    }
}
