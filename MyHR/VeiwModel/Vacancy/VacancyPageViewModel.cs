using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyHR
{
    public class VacancyPageViewModel : BaseViewModel
    {
        #region Private Members

        private readonly PropertyChangeModel mPropertyChangeModel;

        private EntityContext context;

        #endregion

        #region Public Members

        public List<Vacancy> GridDataContext { get; set; }

        public Vacancy SelectedPosition { get; set; }

        #endregion

        #region Commands

        public ICommand New { get; set; }

        public ICommand Edit { get; set; }

        public ICommand Copy { get; set; }

        public ICommand ClosePage { get; set; }

        public ICommand SelectVacancy { get; set; }

        #endregion

        #region Constructor

        public VacancyPageViewModel(PropertyChangeModel PropertyChangeModel)
        {
            mPropertyChangeModel = PropertyChangeModel;

            New = new RelayCommand(() => NewVacancy());
            Edit = new RelayCommand(() => EditVacancy());
            Copy = new RelayCommand(() => CopyVacancy());
            ClosePage = new RelayCommand(() => ClosePageVacancy());
            SelectVacancy = new RelayCommand(() => SelectPositionCommand());

            context = new EntityContext("ConnectionToDB");
            context.Vacancies.Load();
            GridDataContext = context.Vacancies.Include(v=>v.Position).ToList();
            context.Dispose();
        }

        private void SelectPositionCommand()
        {
            mPropertyChangeModel.SendValueToOwner(SelectedPosition);
            mPropertyChangeModel.ClosePage(null);
        }

        private void ClosePageVacancy()
        {
            mPropertyChangeModel.ClosePage(null);
        }

        private void CopyVacancy()
        {
            if (SelectedPosition != null)
                mPropertyChangeModel.SendValue(ApplicationMenuControl.NewVacancy, ApplicationPageCommands.Copy, SelectedPosition);
        }

        private void EditVacancy()
        {
            if (SelectedPosition != null)
                mPropertyChangeModel.SendValue(ApplicationMenuControl.NewVacancy, ApplicationPageCommands.Edit, SelectedPosition);
        }

        private void NewVacancy()
        {
            mPropertyChangeModel.SendValue(ApplicationMenuControl.NewVacancy, ApplicationPageCommands.New, null);
        }

        #endregion

    }
}
