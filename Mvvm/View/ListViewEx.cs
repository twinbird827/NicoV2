using NicoV2.Mvvm.Model;
using NicoV2.Mvvm.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NicoV2.Mvvm.View
{
    public class ListViewEx : ListView
    {
        public VideoModel SelectedFirstItem
        {
            get { return (VideoModel)GetValue(SelectedFirstItemProperty); }
            set { SetValue(SelectedFirstItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedFirstItemProperty =
            DependencyProperty.Register(
                "SelectedFirstItem",
                typeof(VideoModel),
                typeof(ListViewEx),
                new FrameworkPropertyMetadata(
                    new PropertyChangedCallback(ListViewEx.OnSelectedFirstItemChanged)
                )
            );

        private static void OnSelectedFirstItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = sender as ListViewEx;
            if (instance != null)
            {
            }
        }

        public ListViewEx()
        {
            this.SelectionChanged += lvw_SelectionChanged;
        }

        private void lvw_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedFirstItem = null;
            foreach (var item in e.RemovedItems.Cast<WorkSpaceItemViewModel>())
            {
                item.IsSelected = false;
            }
            foreach (var item in e.AddedItems.Cast<WorkSpaceItemViewModel>())
            {
                item.IsSelected = true;
                if (SelectedFirstItem == null) SelectedFirstItem = item.Source;
            }

        }


    }
}
