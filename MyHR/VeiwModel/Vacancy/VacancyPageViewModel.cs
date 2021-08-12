using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyHR
{
    public class VacancyPageViewModel : BaseViewModel
    {
        #region Private Members

        private readonly PropertyChangeModel mPropertyChangeModel;

        private EntityContext context;

        private DataLogger Logger;

        #endregion

        #region Public Members

        public List<Vacancy> GridDataContext { get; set; }

        public Vacancy SelectedPosition { get; set; }

        #endregion

        #region Commands

        public ICommand New { get; set; }

        public ICommand Edit { get; set; }

        public ICommand Copy { get; set; }

        public ICommand Delete { get; set; }

        public ICommand UpdateList { get; set; }

        public ICommand ClosePage { get; set; }

        public ICommand SelectVacancy { get; set; }

        #endregion

        #region Constructor

        public VacancyPageViewModel(PropertyChangeModel PropertyChangeModel)
        {
            Logger = new DataLogger();

            mPropertyChangeModel = PropertyChangeModel;

            New = new RelayCommand(() => NewVacancy());
            Edit = new RelayCommand(() => EditVacancy());
            Copy = new RelayCommand(() => CopyVacancy());
            Delete = new RelayCommand(() => DeleteVacancy());
            UpdateList = new RelayCommand(() => UpdateListVacancy());
            ClosePage = new RelayCommand(() => ClosePageVacancy());
            SelectVacancy = new RelayCommand(() => SelectPositionCommand());

            try
            {
                context = new EntityContext("ConnectionToDB");
            }
            catch(Exception e)
            {
                Logger.WriteToLog(@"Список вакансий: не удалось получить контекст базы данных");
                Logger.WriteToLog(e.Message);
            }
            try
            {

                context.Vacancies.Load();
                GridDataContext = context.Vacancies.Include(v=>v.Position).ToList();
                context.Dispose();

            }catch(Exception e)
            {
                Logger.WriteToLog(@"Список вакансий: не удалось получить данные из базы");
                Logger.WriteToLog(e.Message);
            }
            
        }

        private void DeleteVacancy()
        {
            context = new EntityContext("ConnectionToDB");

            try
            {

                Vacancy vacancy = context.Vacancies.Where(o => o.VacancyId == SelectedPosition.VacancyId).FirstOrDefault();
                if (vacancy != null)
                    context.Vacancies.Remove(vacancy);
                context.SaveChanges();

                GridDataContext = context.Vacancies.Include(v => v.Position).ToList();
            }
            catch { MessageBox.Show("Удалить не возможно. Есть ссылки на другие объекты", "Ошибка удаления"); }

            context.Dispose();
        }

        private void UpdateListVacancy()
        {
            context = new EntityContext("ConnectionToDB");

            context.Vacancies.Load();
            GridDataContext = context.Vacancies.Include(v => v.Position).ToList();
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
