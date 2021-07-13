using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyHR
{
    public class UserControlViewModel : BaseViewModel
    {
        #region Private Members

        private readonly PropertyChangeModel mPropertyChangeModel;

        private ApplicationUserControl CurrentPanel;

        #endregion

        #region Commands

        public ICommand OpenPositionCommand { get; set; }

        public ICommand OpenVacancyCommand { get; set; }

        public ICommand OpenСandidateCommand { get; set; }

        public ICommand OpenOrderCommand { get; set; }

        #endregion

        #region Constructor

        public UserControlViewModel(PropertyChangeModel PropertyChangeModel, ApplicationUserControl applicationUserControl)
        {
            mPropertyChangeModel = PropertyChangeModel;
            CurrentPanel = applicationUserControl;

            OpenPositionCommand = new RelayCommand(() => OpenPositionList());
            OpenVacancyCommand = new RelayCommand(() => OpenVacancyList());
            OpenСandidateCommand = new RelayCommand(() => OpenСandidateList());
            OpenOrderCommand = new RelayCommand(() => OpenOrderList());
        }

        private void OpenOrderList()
        {
            mPropertyChangeModel.SendValue(ApplicationMenuControl.Order, ApplicationMenuControl.Order,null);
        }

        private void OpenСandidateList()
        {
            mPropertyChangeModel.SendValue(ApplicationMenuControl.Сandidate, ApplicationMenuControl.Сandidate, null);
        }

        private void OpenVacancyList()
        {
            mPropertyChangeModel.SendValue(ApplicationMenuControl.Vacancy, ApplicationMenuControl.Vacancy, null);
        }

        private void OpenPositionList()
        {
            mPropertyChangeModel.SendValue(ApplicationMenuControl.Position, ApplicationMenuControl.Position, null);
        }

        #endregion

    }
}
