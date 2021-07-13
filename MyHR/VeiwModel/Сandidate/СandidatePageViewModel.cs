using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyHR
{
    public class СandidatePageViewModel : BaseViewModel
    {
        #region Private Members

        private readonly PropertyChangeModel mPropertyChangeModel;

        private EntityContext context;

        #endregion

        #region Public Members

        public List<Сandidate> DataContext { get; set; }

        public Сandidate SelectedPosition { get; set; }

        #endregion

        #region Commands

        public ICommand New { get; set; }

        public ICommand Edit { get; set; }

        public ICommand Copy { get; set; }

        public ICommand ClosePage { get; set; }

        #endregion

        #region Constructor

        public СandidatePageViewModel(PropertyChangeModel PropertyChangeModel)
        {
            mPropertyChangeModel = PropertyChangeModel;

            New = new RelayCommand(() => NewСandidate());
            Edit = new RelayCommand(() => EditСandidate());
            Copy = new RelayCommand(() => CopyСandidate());
            ClosePage = new RelayCommand(() => ClosePageСandidate());

            context = new EntityContext("ConnectionToDB");
            context.Сandidates.Load();
            DataContext = context.Сandidates.Include(v => v.Vacancy).ToList();
            context.Dispose();
        }

        private void ClosePageСandidate()
        {
            mPropertyChangeModel.ClosePage(null);
        }

        private void CopyСandidate()
        {
            if (SelectedPosition != null)
                mPropertyChangeModel.SendValue(ApplicationMenuControl.NewСandidate, ApplicationPageCommands.Copy, SelectedPosition);
        }

        private void EditСandidate()
        {
            if (SelectedPosition != null)
                mPropertyChangeModel.SendValue(ApplicationMenuControl.NewСandidate, ApplicationPageCommands.Edit, SelectedPosition);
        }

        private void NewСandidate()
        {
            mPropertyChangeModel.SendValue(ApplicationMenuControl.NewСandidate, ApplicationPageCommands.New, null);
        }

        #endregion

    }
}
