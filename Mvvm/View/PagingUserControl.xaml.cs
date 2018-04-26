using NicoV2.Mvvm.ViewModel;
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

        private ICommand _NumberingCommand;
        public ICommand NumberingCommand {
            get
            {
                return _NumberingCommand = _NumberingCommand ?? new RelayCommand<Button>(
                    b =>
                    {
                        Current = int.Parse(b.Content.ToString());
                    }
                );
            }
        }

        public event EventHandler CurrentChanged;

        public event EventHandler PropertyInitialized;

        public PagingUserControl()
        {
            InitializeComponent();

            this.DataContext = this;
            this.PropertyInitialized += OnPropertyInitialized;
            this.CurrentChanged += OnCurrentChanged;

            Current = 0;
            Offset = 0;
            Limit = 10;
            PageLength = 5;
            DataLength = 0;
        }

        private void SetValueAndRaiseEvent(DependencyProperty dp, object n, object o, EventHandler e)
        {
            SetValue(dp, n);
            if (n != null && !n.Equals(o))
            {
                e(this, new EventArgs());
            }
        }

        private void OnCurrentChanged(object sender, EventArgs e)
        {
            var maxPage = GetMaxPage();

            // ﾍﾟｰｼﾞｬ開始位置を取得
            var index = (int)(Current - Math.Ceiling((PageLength - 1) / 2d));

            // 最終ﾍﾟｰｼﾞまで移動していた場合の考慮
            index = (index + PageLength - 1) < maxPage ? index : maxPage - PageLength + 1;

            // ﾍﾟｰｼﾞｬ開始位置がﾏｲﾅｽになった場合の考慮
            index = index <= 0 ? 1 : index;

            // ﾍﾟｰｼﾞｬ開始位置によってPAGE1ﾎﾞﾀﾝの活性/非活性を切り替える。
            btnFirst.IsEnabled = (1 < index);

            // ﾍﾟｰｼﾞ番号の更新
            for (int i = 0; i < PageLength; i++)
            {
                var button = GetButton(i + 1);
                button.Content = string.Format("{0}", index + i);
                button.IsEnabled = !((index + i) == Current);
            }

            // TODO PAGE x or x のﾗﾍﾞﾙ変更
            txtPageSize.Text = string.Format("PAGE {0} of {1}", Current, maxPage);

        }

        private void OnPropertyInitialized(object sender, EventArgs e)
        {
            var index = GetMaxPage();
            index = index <= 0 ? 1 : index;
            index = index < PageLength ? index : PageLength;

            // 表示/非表示切替
            for (int i = 1; i <= index; i++)
            {
                GetButton(i).Visibility = Visibility.Visible;
            }
            for (int i = index + 1; i <= 10; i++)
            {
                GetButton(i).Visibility = Visibility.Collapsed;
            }

            // 
        }

        private void UserControl_LayoutUpdated(object sender, EventArgs e)
        {
        }

        private Button GetButton(int index)
        {
            switch (index)
            {
                case 1:
                    return btnPage1;
                case 2:
                    return btnPage2;
                case 3:
                    return btnPage3;
                case 4:
                    return btnPage4;
                case 5:
                    return btnPage5;
                case 6:
                    return btnPage6;
                case 7:
                    return btnPage7;
                case 8:
                    return btnPage8;
                case 9:
                    return btnPage9;
                case 10:
                    return btnPage10;
                default:
                    throw new ArgumentException(string.Format("Button index: ", index));
            }
        }

        private int GetMaxPage()
        {
            if (0 < Limit)
            {
                return (int)Math.Floor(DataLength / Limit * 1d);
            }
            else
            {
                return 0;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("{0} {1} {2}", PageLength, DataLength, Limit);
            PropertyInitialized(this, new EventArgs());
            CurrentChanged(this, new EventArgs());
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            Current--;
        }

        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            Current = 1;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Current++;
        }

    }
}
