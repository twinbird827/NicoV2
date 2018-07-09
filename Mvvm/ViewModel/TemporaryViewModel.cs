using NicoV2.Mvvm.Model;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NicoV2.Mvvm.ViewModel
{
    public class TemporaryViewModel : WorkSpaceViewModel
    {
        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public TemporaryViewModel() : this(TemporaryModel.Instance)
        {

        }

        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public TemporaryViewModel(TemporaryModel model)
        {
            Source = model;
            Items = Source.Items.ToSyncedSynchronizationContextCollection(m => new TemporaryItemViewModel(m), AnonymousSynchronizationContext.Current);
        }

        /// <summary>
        /// 本ｲﾝｽﾀﾝｽのﾃﾞｰﾀ実体
        /// </summary>
        public TemporaryModel Source { get; set; }

        /// <summary>
        /// ﾒｲﾝ項目ﾘｽﾄ
        /// </summary>
        public SynchronizationContextCollection<TemporaryItemViewModel> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }
        private SynchronizationContextCollection<TemporaryItemViewModel> _Items;

        public ICommand OnTemporaryAdd
        {
            get
            {
                return _OnTemporaryAdd = _OnTemporaryAdd ?? new RelayCommand(
              _ =>
              {
                  var txt = Clipboard.GetText();
                  Source.Add(txt.Split('/').Last());
              },
              _ => {
                  return true;
              });
            }
        }
        public ICommand _OnTemporaryAdd;

        public ICommand OnTemporaryDelAll
        {
            get
            {
                return _OnTemporaryDelAll = _OnTemporaryDelAll ?? new RelayCommand(
              _ =>
              {
                  foreach (var m in Items.Where(m => m.IsSelected))
                  {
                      Source.Delete(m.ContentId);
                  }
              },
              _ => {
                  return true;
              });
            }
        }
        public ICommand _OnTemporaryDelAll;

    }
}
