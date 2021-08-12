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
    public class СandidatePageViewModel : BaseViewModel
    {
        #region Private Members

        private readonly PropertyChangeModel mPropertyChangeModel;

        private EntityContext context;

        private DataLogger Logger;

        #endregion

        #region Public Members

        public List<Сandidate> DataContext { get; set; }

        public Сandidate SelectedPosition { get; set; }

        #endregion

        #region Commands

        public ICommand New { get; set; }

        public ICommand Edit { get; set; }

        public ICommand Copy { get; set; }

        public ICommand Delete { get; set; }

        public ICommand UpdateList { get; set; }

        public ICommand ClosePage { get; set; }

        public ICommand SelectCandidate { get; set; }

        #endregion

        #region Constructor

        public СandidatePageViewModel(PropertyChangeModel PropertyChangeModel)
        {
            Logger = new DataLogger();

            mPropertyChangeModel = PropertyChangeModel;

            New = new RelayCommand(() => NewСandidate());
            Edit = new RelayCommand(() => EditСandidate());
            Copy = new RelayCommand(() => CopyСandidate());
            Delete = new RelayCommand(() => DeleteСandidate());
            UpdateList = new RelayCommand(() => UpdateListСandidate());
            ClosePage = new RelayCommand(() => ClosePageСandidate());
            SelectCandidate = new RelayCommand(() => SelectCandidateCommand());


            try
            {
                context = new EntityContext("ConnectionToDB");
            }
            catch (Exception e)
            {
                Logger.WriteToLog(@"Список соискателей: не удалось получить контекст базы данных");
                Logger.WriteToLog(e.Message);
            }
            try
            {

                context.Сandidates.Load();
                DataContext = context.Сandidates.ToList();
                context.Dispose();

            }
            catch (Exception e)
            {
                Logger.WriteToLog(@"Список соискателей: не удалось получить данные из базы");
                Logger.WriteToLog(e.Message);
            }
        }

        private void DeleteСandidate()
        {
            context = new EntityContext("ConnectionToDB");

            try
            {

                Сandidate candidate = context.Сandidates.Where(o => o.СandidateId == SelectedPosition.СandidateId).FirstOrDefault();
                if (candidate != null)
                    context.Сandidates.Remove(candidate);
                context.SaveChanges();

                DataContext = context.Сandidates.ToList();
            }
            catch { MessageBox.Show("Удалить не возможно. Есть ссылки на другие объекты", "Ошибка удаления"); }

            context.Dispose();
        }

        private void UpdateListСandidate()
        {
            context = new EntityContext("ConnectionToDB");

            context.Сandidates.Load();
            DataContext = context.Сandidates.ToList();
            context.Dispose();
        }

        private void SelectCandidateCommand()
        {
            mPropertyChangeModel.SendValueToOwner(SelectedPosition);
            mPropertyChangeModel.ClosePage(null);
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
