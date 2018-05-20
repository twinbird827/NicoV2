using NicoV2.Mvvm.Model;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NicoV2.Mvvm.ViewModel
{
    public class SearchByWordViewModel : WorkSpaceViewModel
    {
        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public SearchByWordViewModel() : this(new SearchByWordModel())
        {

        }

        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public SearchByWordViewModel(SearchByWordModel model)
        {
            Source = model;
            Items = Source.Items.ToSyncedSynchronizationContextCollection(m => m, AnonymousSynchronizationContext.Current);

            SortModel = SortModel.Instance;
            SortItems = SortModel.Items.ToSyncedSynchronizationContextCollection(m => m, AnonymousSynchronizationContext.Current);
            SelectedSortItem = SortItems.First();


        }

        /// <summary>
        /// 本ｲﾝｽﾀﾝｽのﾃﾞｰﾀ実体
        /// </summary>
        public SearchByWordModel Source { get; set; }

        /// <summary>
        /// ｿｰﾄ用ﾓﾃﾞﾙ
        /// </summary>
        public SortModel SortModel
        {
            get { return _SortModel; }
            set { SetProperty(ref _SortModel, value); }
        }
        private SortModel _SortModel;

        /// <summary>
        /// ｿｰﾄﾘｽﾄ
        /// </summary>
        public SynchronizationContextCollection<SortItemModel> SortItems
        {
            get { return _SortItems; }
            set { SetProperty(ref _SortItems, value); }
        }
        private SynchronizationContextCollection<SortItemModel> _SortItems;

        /// <summary>
        /// 選択中のｿｰﾄ項目
        /// </summary>
        public SortItemModel SelectedSortItem
        {
            get { return _SelectedSortItem; }
            set { SetProperty(ref _SelectedSortItem, value); }
        }
        private SortItemModel _SelectedSortItem;

        /// <summary>
        /// ｿｰﾄﾘｽﾄ
        /// </summary>
        public SynchronizationContextCollection<VideoModel> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        private SynchronizationContextCollection<VideoModel> _Items;

        /// <summary>
        /// 検索ﾜｰﾄﾞ
        /// </summary>
        public string Word
        {
            get { return _Word; }
            set { SetProperty(ref _Word, value); }
        }
        private string _Word = null;

        /// <summary>
        /// ﾀｸﾞ検索 or ｷｰﾜｰﾄﾞ検索
        /// </summary>
        public bool IsTag
        {
            get { return _IsTag; }
            set { SetProperty(ref _IsTag, value); }
        }
        private bool _IsTag = true;

        /// <summary>
        /// 現在のﾍﾟｰｼﾞ位置
        /// </summary>
        public int Current
        {
            get { return _Current; }
            set { SetProperty(ref _Current, value); }
        }
        private int _Current = 1;

        /// <summary>
        /// ｵﾌｾｯﾄ (取得する開始位置)
        /// </summary>
        public int Offset
        {
            get { return _Offset; }
            set { SetProperty(ref _Offset, value); }
        }
        private int _Offset = 0;

        /// <summary>
        /// ﾃﾞｰﾀ件数
        /// </summary>
        public double DataLength
        {
            get { return _DataLength; }
            set { SetProperty(ref _DataLength, value); }
        }
        private double _DataLength = 0;

        public ICommand SearchCommand
        {
            get
            {
                return _SearchCommand = _SearchCommand ?? new RelayCommand<bool>(
              b =>
              {
                  // 現在位置をﾘｾｯﾄ
                  this.IsTag = b;

                  // 入力値をﾓﾃﾞﾙにｾｯﾄ
                  Source.Word = this.Word;
                  Source.Offset = 0;
                  Source.IsTag = this.IsTag;
                  Source.OrderBy = this.SelectedSortItem.Keyword;

                  // 検索実行
                  this.Source.Reload();

                  // ﾃﾞｰﾀ件数をVMにｾｯﾄ
                  this.Offset = 0;
                  this.DataLength = Source.DataLength;

                },
              b => {
                  return !string.IsNullOrWhiteSpace(Word);
              });
            }
        }
        public ICommand _SearchCommand;

        public ICommand CurrentChangedCommand
        {
            get
            {
                return _CurrentChangedCommand = _CurrentChangedCommand ?? new RelayCommand(
              _ =>
              {
                  if (Source.Offset != this.Offset)
                  {
                      // ｵﾌｾｯﾄを更新
                      Source.Offset = this.Offset;

                      // 検索実行
                      this.Source.Reload();
                  }
              },
              _ => {
                  return !string.IsNullOrWhiteSpace(Word);
              });
            }
        }
        public ICommand _CurrentChangedCommand;

    }
}
