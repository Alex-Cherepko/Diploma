using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyHR
{
    public class NewСandidatePageViewModel : BaseViewModel
    {
        #region Private Members

        private readonly PropertyChangeModel mPropertyChangeModel;
        private readonly ApplicationPageCommands mApplicationPageCommands;
        private EntityContext context;
        private Сandidate mСandidate;

        #endregion

        #region Public Members

        public string Title { get; set; } = "Кандидат: Новый";

        public string FullName { get; set; }

        public int СandidateId { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public string Description { get; set; }

        public Vacancy Vacancy { get; set; }

        

        #endregion

        #region Commands

        public ICommand CommandOK { get; set; }

        public ICommand CommandSave { get; set; }

        public ICommand ClosePage { get; set; }

        public ICommand SelectVacancy { get; set; }

        public ICommand FindUploadCommand { get; set; }

        #endregion

        #region Constructor

        public NewСandidatePageViewModel(PropertyChangeModel PropertyChangeModel, ApplicationPageCommands applicationPageCommands, Сandidate CurrentVacancy = null)
        {
            mPropertyChangeModel = PropertyChangeModel;

            mPropertyChangeModel.SendValueEvent += PropertyChangeModelSendValue;
            mApplicationPageCommands = applicationPageCommands;

            context = new EntityContext("ConnectionToDB");

            CommandOK = new RelayCommand(() => SaveChangesAndClose());
            CommandSave = new RelayCommand(() => SaveChanges());
            ClosePage = new RelayCommand(() => CloseNewPage());
            SelectVacancy = new RelayCommand(() => SelectVacancyCommand());
            FindUploadCommand = new RelayCommand(()=> EnterInputed());

            if (mApplicationPageCommands == ApplicationPageCommands.New)
            {
                mСandidate = new Сandidate();
                СandidateId = GetNewCode();

            }
            else if (mApplicationPageCommands == ApplicationPageCommands.Edit)
            {
                Title = "Кандидат: Создан";
                mСandidate = CurrentVacancy;
                СandidateId = mСandidate.СandidateId;
                FullName = mСandidate.FullName;
                Name = mСandidate.Name;
                Surname = mСandidate.Surname;
                Patronymic = mСandidate.Patronymic;
                Description = mСandidate.Description;
                Vacancy = mСandidate.Vacancy;

            }
            else if (mApplicationPageCommands == ApplicationPageCommands.Copy)
            {
                mСandidate = new Сandidate();
                СandidateId = GetNewCode();
                mСandidate.FullName = CurrentVacancy.FullName;
                mСandidate.Name = CurrentVacancy.Name;
                mСandidate.Surname = CurrentVacancy.Surname;
                mСandidate.Patronymic = CurrentVacancy.Patronymic;
                mСandidate.Description = CurrentVacancy.Description;
                mСandidate.Vacancy = CurrentVacancy.Vacancy;

                FullName = mСandidate.FullName;
                Name = mСandidate.Name;
                Surname = mСandidate.Surname;
                Patronymic = mСandidate.Patronymic;
                Description = mСandidate.Description;
                Vacancy = mСandidate.Vacancy;
            }
        }

        private void EnterInputed()
        {
            if (!string.IsNullOrEmpty(FullName))
            {
                string[] words = FullName.Split(new char[] { ' ' });
                try { Surname = words[0]; }
                catch { }

                try { Name = words[1]; }
                catch { }

                try { Patronymic = words[2]; }
                catch { }
            }
        }

        private int GetNewCode()
        {
            if (context.Сandidates.Count() > 0)
            {
                return context.Сandidates.Max(c => c.СandidateId) + 1;
            }
            return 1;
        }

        private void SelectVacancyCommand()
        {
            mPropertyChangeModel.SendValue(ApplicationMenuControl.SelectVacancy, ApplicationMenuControl.SelectVacancy, this);
        }

        private void CloseNewPage()
        {
            mPropertyChangeModel.ClosePage(null);
        }

        private bool ChecFields()
        {
            if (Name == null)
            {
                MessageBox.Show("Укажите наименование кандидата", "Ошибка");
                return false;
            }

            return true;
        }

        private void SaveChanges()
        {
            if (!ChecFields())
                return;

            var currVal = context.Сandidates.Where(c => c.СandidateId == СandidateId).FirstOrDefault();
            if (currVal == null)
            {
                mСandidate.СandidateId = СandidateId;
                mСandidate.FullName = FullName;
                mСandidate.Name = Name;
                mСandidate.Surname = Surname;
                mСandidate.Patronymic = Patronymic;
                mСandidate.Description = Description;
                mСandidate.VacancyId = Vacancy.VacancyId;

                context.Сandidates.Add(mСandidate);
            }
            else
            {
                currVal.СandidateId = СandidateId;
                currVal.FullName = FullName;
                currVal.Name = Name;
                currVal.Surname = Surname;
                currVal.Patronymic = Patronymic;
                currVal.Description = Description;
                currVal.VacancyId = Vacancy.VacancyId;
            }
            context.SaveChanges();
            
        }

        private void SaveChangesAndClose()
        {
            SaveChanges();
            context.Dispose();

            mPropertyChangeModel.ClosePage(null);
        }

        private void PropertyChangeModelSendValue(object sender, object Value)
        {
            if (Value is Vacancy)
            {
                Vacancy = (Vacancy)Value;
            }
        }


        #endregion

    }
}
