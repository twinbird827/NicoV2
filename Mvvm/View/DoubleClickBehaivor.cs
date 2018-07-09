using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NicoV2.Mvvm.View
{
    public class DoubleClickBehaivor
    {
        /// <summary>
        /// Commandの依存関係プロパティ
        /// </summary>
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(DoubleClickBehaivor), new UIPropertyMetadata(OnChangeCommand));

        /// <summary>
        /// コマンドを設定します（添付ビヘイビア）
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="value">コマンド</param>
        public static void SetCommand(DependencyObject target, object value)
        {
            target.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// コマンドを取得します（添付ビヘイビア）
        /// </summary>
        /// <param name="target">対象</param>
        /// <returns>コマンド</returns>
        public static ICommand GetCommand(DependencyObject target)
        {
            return (ICommand)target.GetValue(CommandProperty);
        }

        /// <summary>
        /// コマンドプロパティが変更された際の処理
        /// </summary>
        /// <param name="target">対象</param>
        /// <param name="e">イベント情報</param>
        private static void OnChangeCommand(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            Control control = target as Control;
            if (control != null)
            {
                if (e.OldValue == null && e.NewValue != null)
                {
                    control.MouseDoubleClick += OnDoubleClick;
                }
                else if (e.OldValue != null && e.NewValue == null)
                {
                    control.MouseDoubleClick -= OnDoubleClick;
                }
            }
        }

        /// <summary>
        /// ダブルクリック時の処理
        /// </summary>
        /// <param name="sender">送り先</param>
        /// <param name="e">イベント情報</param>
        private static void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Control control = sender as Control;
            if (control != null)
            {
                ICommand command = (ICommand)control.GetValue(CommandProperty);
                if (command.CanExecute(e)) command.Execute(e);
            }
        }
    }
}
