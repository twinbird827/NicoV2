using NicoV2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NicoV2.Mvvm.View
{
    public static class WebBrowserBehaviors
    {
        public static readonly DependencyProperty DocumentTextProperty =
        DependencyProperty.RegisterAttached(
          "DocumentText", typeof(string),
          typeof(WebBrowserBehaviors),
          new UIPropertyMetadata(string.Empty, DocumentTextChanged)
        );

        [AttachedPropertyBrowsableForType(typeof(WebBrowser))]
        public static string GetDocumentText(DependencyObject obj)
        {
            return obj.GetValue(DocumentTextProperty).ToString();
        }

        [AttachedPropertyBrowsableForType(typeof(WebBrowser))]
        public static void SetDocumentText(DependencyObject obj, string value)
        {
            obj.SetValue(DocumentTextProperty, value);
        }

        private static void DocumentTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var browser = sender as WebBrowser;
            if (browser == null) return;

            var window = WpfUtil.GetActiveWindow();
            var textblock = new TextBlock();
            var document = e.NewValue.ToString();
            var mawari = @"
                <html>
                    <head>
                        <meta http-equiv='Content-Type' content='text/html;charset=UTF-8'>
                    </head>
                    <body scroll=no><div style=""font-size: {0}px; font-family: '{1}';"">{2}</font></body>
                </html>";

            if (window != null)
            {
                browser.NavigateToString(string.Format(mawari, textblock.FontSize, textblock.FontFamily.Source, document));
            }
            else
            {
                browser.NavigateToString(string.Format(mawari, 12, "メイリオ", document));
            }
        }
    }
}
