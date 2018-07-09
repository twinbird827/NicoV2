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
    public class RankingViewModel : WorkSpaceViewModel
    {
        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public RankingViewModel() : this(new RankingModel())
        {

        }

        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public RankingViewModel(RankingModel model)
        {
            Source = model;
            Items = Source.Items.ToSyncedSynchronizationContextCollection(m => new RankingItemViewModel(m), AnonymousSynchronizationContext.Current);

            ComboTargetItems = ComboRankTargetModel
                .Instance
                .Items
                .ToSyncedSynchronizationContextCollection(m => m, AnonymousSynchronizationContext.Current);
            SelectedComboTargetItem = ComboTargetItems.First();

            ComboPeriodItems = ComboRankPeriodModel
                .Instance
                .Items
                .ToSyncedSynchronizationContextCollection(m => m, AnonymousSynchronizationContext.Current);
            SelectedComboPeriodItem = ComboPeriodItems.First();

            ComboCategoryItems = ComboRankCategoryModel
                .Instance
                .Items
                .ToSyncedSynchronizationContextCollection(m => m, AnonymousSynchronizationContext.Current);
            SelectedComboCategoryItem = ComboCategoryItems.First();
        }

        /// <summary>
        /// 本ｲﾝｽﾀﾝｽのﾃﾞｰﾀ実体
        /// </summary>
        public RankingModel Source { get; set; }

        /// <summary>
        /// 対象ﾘｽﾄ
        /// </summary>
        public SynchronizationContextCollection<ComboBoxItemModel> ComboTargetItems
        {
            get { return _ComboTargetItems; }
            set { SetProperty(ref _ComboTargetItems, value); }
        }
        private SynchronizationContextCollection<ComboBoxItemModel> _ComboTargetItems;

        /// <summary>
        /// 選択中の対象項目
        /// </summary>
        public ComboBoxItemModel SelectedComboTargetItem
        {
            get { return _SelectedComboTargetItem; }
            set { SetProperty(ref _SelectedComboTargetItem, value); }
        }
        private ComboBoxItemModel _SelectedComboTargetItem;

        /// <summary>
        /// 期間ﾘｽﾄ
        /// </summary>
        public SynchronizationContextCollection<ComboBoxItemModel> ComboPeriodItems
        {
            get { return _ComboPeriodItems; }
            set { SetProperty(ref _ComboPeriodItems, value); }
        }
        private SynchronizationContextCollection<ComboBoxItemModel> _ComboPeriodItems;

        /// <summary>
        /// 選択中の期間項目
        /// </summary>
        public ComboBoxItemModel SelectedComboPeriodItem
        {
            get { return _SelectedComboPeriodItem; }
            set { SetProperty(ref _SelectedComboPeriodItem, value); }
        }
        private ComboBoxItemModel _SelectedComboPeriodItem;

        /// <summary>
        /// ｶﾃｺﾞﾘﾘｽﾄ
        /// </summary>
        public SynchronizationContextCollection<ComboBoxItemModel> ComboCategoryItems
        {
            get { return _ComboCategoryItems; }
            set { SetProperty(ref _ComboCategoryItems, value); }
        }
        private SynchronizationContextCollection<ComboBoxItemModel> _ComboCategoryItems;

        /// <summary>
        /// 選択中のｶﾃｺﾞﾘ項目
        /// </summary>
        public ComboBoxItemModel SelectedComboCategoryItem
        {
            get { return _SelectedComboCategoryItem; }
            set { SetProperty(ref _SelectedComboCategoryItem, value); }
        }
        private ComboBoxItemModel _SelectedComboCategoryItem;

        /// <summary>
        /// ﾒｲﾝ項目ﾘｽﾄ
        /// </summary>
        public SynchronizationContextCollection<RankingItemViewModel> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        private SynchronizationContextCollection<RankingItemViewModel> _Items;

        public ICommand SearchCommand
        {
            get
            {
                return _SearchCommand = _SearchCommand ?? new RelayCommand(
              _ =>
              {
                  // 入力値をﾓﾃﾞﾙにｾｯﾄ
                  Source.Target = this.SelectedComboTargetItem.Value;
                  Source.Period = this.SelectedComboPeriodItem.Value;
                  Source.Category = this.SelectedComboCategoryItem.Value;

                  // 検索実行
                  this.Source.Reload();
              },
              _ => {
                  return true;
              });
            }
        }
        public ICommand _SearchCommand;
    }
}
