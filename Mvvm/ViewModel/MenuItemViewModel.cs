using NicoV2.Common;
using NicoV2.Mvvm.Model;
using NicoV2.Mvvm.View;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NicoV2.Mvvm.ViewModel
{
    public class MenuItemViewModel : BindableBase
    {
        /// <summary>
        /// 本ﾒﾆｭｰの親ﾒﾆｭｰ
        /// Dispose時に除外する属性をｾｯﾄ
        /// </summary>
        [Exclusion]
        public MenuItemViewModel Parent { get; set; }

        /// <summary>
        /// 本ﾒﾆｭｰの情報を管理するModel
        /// </summary>
        public MenuItemModel Source { get; set; }

        /// <summary>
        /// ｱｲﾃﾑ名
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); Source.Name = value; } // Modelにも通知する
        }
        private string _Name = null;

        /// <summary>
        /// ｱｲﾃﾑ名(ﾘﾈｰﾑ後)
        /// </summary>
        public string Input
        {
            get { return _Input; }
            set { SetProperty(ref _Input, value); }
        }
        private string _Input = null;

        /// <summary>
        /// ﾒﾆｭｰの種類
        /// </summary>
        public MenuItemType Type
        {
            get { return _Type; }
            set { SetProperty(ref _Type, value); }
        }
        private MenuItemType _Type;

        /// <summary>
        /// ﾒﾆｭｰの種類
        /// </summary>
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { SetProperty(ref _IsSelected, value); }
        }
        private bool _IsSelected;

        /// <summary>
        /// ﾒﾆｭｰが紐付くﾜｰｸｽﾍﾟｰｽのｲﾝｽﾀﾝｽ
        /// </summary>
        public WorkSpaceViewModel WorkSpace
        {
            //get { return _WorkSpace; }
            //set { SetProperty(ref _WorkSpace, value); }
            get
            {
                return _WorkSpace = _WorkSpace ?? CreateWorkSpaceViewModel();
            }
        }
        private WorkSpaceViewModel _WorkSpace;

        /// <summary>
        /// 本ﾒﾆｭｰｱｲﾃﾑの子要素
        /// </summary>
        public SynchronizationContextCollection<MenuItemViewModel> Children
        {
            get { return _Children; }
            set { SetProperty(ref _Children, value); }
        }
        private SynchronizationContextCollection<MenuItemViewModel> _Children;

        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        /// <param name="menu">ﾒﾆｭｰ構成を全て管理するMenuViewModel</param>
        /// <param name="parent">本ﾒﾆｭｰの親ﾒﾆｭｰ</param>
        /// <param name="source">本ﾒﾆｭｰの情報を管理するModel</param>
        public MenuItemViewModel(MenuItemViewModel parent, MenuItemModel source)
        {
            this.Parent = parent;
            this.Source = source;
            this.Name = Source.Name;
            this.Type = Source.Type;
            this.IsSelected = false;

            Children = Source.Children.ToSyncedSynchronizationContextCollection(
                m => new MenuItemViewModel(this, m),
                SynchronizationContext.Current
            );
        }

        private WorkSpaceViewModel CreateWorkSpaceViewModel()
        {
            switch (Type)
            {
                case MenuItemType.Setting:
                    return new SettingViewModel();
                case MenuItemType.SearchByWord:
                    return new SearchByWordViewModel();
                case MenuItemType.SearchByMylist:
                    return new SearchByMylistViewModel();
                case MenuItemType.Ranking:
                    return new RankingViewModel();
                case MenuItemType.Temporary:
                    return new TemporaryViewModel();
                case MenuItemType.MylistOfOther:
                    return new MylistViewModel(this.Source);
                case MenuItemType.MylistOfMe:
                    return new MylistViewModel(this.Source);
                default:
                    return null;
            }
        }

        public ICommand OnOpenRenameDialog
        {
            get
            {
                return _OnOpenRenameDialog = _OnOpenRenameDialog ?? new RelayCommand(
              _ =>
              {
                  // 要素名変更用ﾀﾞｲｱﾛｸﾞを開く
                  Input = Name;
                  MainWindowViewModel.Instance.TreeViewDialog = new MainWindowRenameDialog(this);
                  MainWindowViewModel.Instance.IsOpenTreeViewDialog = true;
              },
              _ =>
              {
                  return true;
              });
            }
        }
        public ICommand _OnOpenRenameDialog;

        public ICommand OnAcceptRenameDialog
        {
            get
            {
                return _OnAcceptRenameDialog = _OnAcceptRenameDialog ?? new RelayCommand(
              _ =>
              {
                  // 要素名変更用ﾀﾞｲｱﾛｸﾞで入力された文字でﾘﾈｰﾑ
                  MainWindowViewModel.Instance.IsOpenTreeViewDialog = false;
                  Name = Input;
              },
              _ =>
              {
                  return true;
              });
            }
        }
        public ICommand _OnAcceptRenameDialog;

        public ICommand OnOpenAddDialog
        {
            get
            {
                return _OnOpenAddDialog = _OnOpenAddDialog ?? new RelayCommand(
              _ =>
              {
                  // 子要素追加用ﾀﾞｲｱﾛｸﾞを開く
                  MainWindowViewModel.Instance.TreeViewDialog = new MainWindowAddDialog(this);
                  MainWindowViewModel.Instance.IsOpenTreeViewDialog = true;
              },
              _ =>
              {
                  return true;
              });
            }
        }
        public ICommand _OnOpenAddDialog;

        public ICommand OnAcceptAddDialog
        {
            get
            {
                return _OnAcceptAddDialog = _OnAcceptAddDialog ?? new RelayCommand(
              _ =>
              {
                  // 子要素追加用ﾀﾞｲｱﾛｸﾞで入力された名前で子要素追加
                  MainWindowViewModel.Instance.IsOpenTreeViewDialog = false;
                  Source.Children.Add(new MenuItemModel(Input, MenuItemType.MylistOfOther));
              },
              _ =>
              {
                  return true;
              });
            }
        }
        public ICommand _OnAcceptAddDialog;

        public ICommand OnCancelDialog
        {
            get
            {
                return _OnCancelDialog = _OnCancelDialog ?? new RelayCommand(
              _ =>
              {
                  // ﾂﾘｰﾋﾞｭｰを閉じる
                  MainWindowViewModel.Instance.IsOpenTreeViewDialog = false;
                  MainWindowViewModel.Instance.TreeViewDialog = null;
              },
              _ =>
              {
                  return true;
              });
            }
        }
        public ICommand _OnCancelDialog;

        public ICommand OnDelete
        {
            get
            {
                return _OnDelete = _OnDelete ?? new RelayCommand<string>(
              s =>
              {
                  // 親の子要素から自身を削除する
                  Parent.Source.Children.Remove(Source);
              },
              _ =>
              {
                  return true;
              });
            }
        }
        public ICommand _OnDelete;

        public void OnItemDoubleClick()
        {
            MainWindowViewModel.Instance.Current = WorkSpace;
            MainWindowViewModel.Instance.IsOpenMenu = !MainWindowViewModel.Instance.IsOpenMenu;
        }

        #region IDisposable Support

        protected override void OnDisposed()
        {
            base.OnDisposed();

            Parent = null;
            Source = null;
        }

        #endregion
    }
}
