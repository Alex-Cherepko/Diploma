using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyHR
{
    public class СandidateFormPageViewModel : BaseViewModel
    {
        #region Private Members

        private readonly PropertyChangeModel mPropertyChangeModel;

       //private EntityContext context;

        private DataLogger Logger;

        #endregion

        #region Public Members

        public List<СandidateForm> DataContext { get; set; }

        public СandidateForm SelectedPosition { get; set; }

        #endregion

        #region Commands

        public ICommand New { get; set; }

        public ICommand Edit { get; set; }

        public ICommand Copy { get; set; }

        public ICommand Delete { get; set; }

        public ICommand UpdateList { get; set; }

        public ICommand ClosePage { get; set; }

        public ICommand SelectCandidateForm { get; set; }


        #endregion

        #region Constructor

        public СandidateFormPageViewModel(PropertyChangeModel PropertyChangeModel)
        {
            Logger = new DataLogger();

            mPropertyChangeModel = PropertyChangeModel;

            New = new RelayCommand(() => NewСandidate());
            Edit = new RelayCommand(() => EditСandidate());
            Copy = new RelayCommand(() => CopyСandidate());
            Delete = new RelayCommand(() => DeleteСandidate());
            UpdateList = new RelayCommand(() => UpdateListСandidate());
            ClosePage = new RelayCommand(() => ClosePageСandidate());
            SelectCandidateForm = new RelayCommand(() => SelectCandidateFormCommand());

            try
            {
                try
                {
                    using (EntityContext context = new EntityContext("ConnectionToDB"))
                    {
                        DataContext = context.СandidateFormes.Include(v => v.Vacancy).Include(c => c.Сandidate).ToList();
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteToLog(@"Список анкет: не удалось получить данные из базы");
                    Logger.WriteToLog(e.Message);
                }
            }
            catch (Exception e)
            {
                Logger.WriteToLog(@"Список анкет: не удалось получить контекст базы данных");
                Logger.WriteToLog(e.Message);
            }
            
        }

        private void DeleteСandidate()
        {
            try
            {
                using (EntityContext context = new EntityContext("ConnectionToDB"))
                {
                    СandidateForm candidateForm = context.СandidateFormes.Where(o => o.СandidateFormId == SelectedPosition.СandidateFormId).FirstOrDefault();
                    if (candidateForm != null)
                        context.СandidateFormes.Remove(candidateForm);
                    context.SaveChanges();

                    DataContext = context.СandidateFormes.Include(v => v.Vacancy).Include(c => c.Сandidate).ToList();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Удалить не возможно. Есть ссылки на другие объекты", "Ошибка удаления");
                Logger.WriteToLog(@"Список анкет: не удалось удалить должность базы данных");
                Logger.WriteToLog(e.Message);
            }
        }

        private void SelectCandidateFormCommand()
        {
            mPropertyChangeModel.SendValueToOwner(SelectedPosition);
            mPropertyChangeModel.ClosePage(null);
        }

        private void UpdateListСandidate()
        {
            try
            {
                using (EntityContext context = new EntityContext("ConnectionToDB")) {

                    DataContext = context.СandidateFormes.Include(v => v.Vacancy).Include(c => c.Сandidate).ToList();
                }
            }catch(Exception e)
            {
                Logger.WriteToLog(@"Список анкет: не удалось получить данные из базы");
                Logger.WriteToLog(e.Message);
            }
        }

        private void ClosePageСandidate()
        {
            mPropertyChangeModel.ClosePage(null);
        }

        private void CopyСandidate()
        {
            if (SelectedPosition != null)
                mPropertyChangeModel.SendValue(ApplicationMenuControl.NewСandidateForm, ApplicationPageCommands.Copy, SelectedPosition);
        }

        private void EditСandidate()
        {
            if (SelectedPosition != null)
                mPropertyChangeModel.SendValue(ApplicationMenuControl.NewСandidateForm, ApplicationPageCommands.Edit, SelectedPosition);
        }

        private void NewСandidate()
        {
            mPropertyChangeModel.SendValue(ApplicationMenuControl.NewСandidateForm, ApplicationPageCommands.New, null);
        }

        #endregion

    }
}
