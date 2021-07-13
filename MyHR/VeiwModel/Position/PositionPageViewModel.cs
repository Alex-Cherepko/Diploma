using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyHR
{
    public class PositionPageViewModel : BaseViewModel
    {
        #region Private Members

        private readonly PropertyChangeModel mPropertyChangeModel;

        private EntityContext context;

        #endregion

        #region Public Members

        public ObservableCollection<Position> GridDataContext { get; set; }

        public Position SelectedPosition { get; set; }

        #endregion

        #region Commands

        public ICommand New { get; set; }

        public ICommand Edit { get; set; }

        public ICommand Copy { get; set; }

        public ICommand ClosePage { get; set; }

        public ICommand SelectPosition { get; set; }

        #endregion

        #region Constructor

        public PositionPageViewModel()
        {

        }

        public PositionPageViewModel(PropertyChangeModel PropertyChangeModel)
        {
            mPropertyChangeModel = PropertyChangeModel;

            New = new RelayCommand(() => NewPosition());
            Edit = new RelayCommand(() => EditPosition());
            Copy = new RelayCommand(() => CopyPosition());
            ClosePage = new RelayCommand(() => ClosePagePosition());
            SelectPosition = new RelayCommand(() => SelectPositionCommand());

            context = new EntityContext("ConnectionToDB");
            context.Positions.Load();
            GridDataContext = context.Positions.Local;
            context.Dispose();
        }

        private void SelectPositionCommand()
        {
            mPropertyChangeModel.SendValueToOwner(SelectedPosition);
            mPropertyChangeModel.ClosePage(null);
        }

        private void ClosePagePosition()
        {
            mPropertyChangeModel.ClosePage(null);
        }

        private void CopyPosition()
        {
            if(SelectedPosition != null)
                mPropertyChangeModel.SendValue(ApplicationMenuControl.NewPosition, ApplicationPageCommands.Copy, SelectedPosition);
        }

        private void EditPosition()
        {
            if (SelectedPosition != null)
                mPropertyChangeModel.SendValue(ApplicationMenuControl.NewPosition, ApplicationPageCommands.Edit, SelectedPosition);
        }

        private void NewPosition()
        {
            mPropertyChangeModel.SendValue(ApplicationMenuControl.NewPosition, ApplicationPageCommands.New, null);
        }

        #endregion

    }
}
