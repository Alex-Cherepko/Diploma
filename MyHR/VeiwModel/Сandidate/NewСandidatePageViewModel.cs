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
        private ConnectionToDB connectionToDB;
        private Сandidate mСandidate;
        private DataLogger Logger;

        #endregion

        #region Public Members

        public string Title { get; set; } = "Кандидат: Новый";

        public string FullName { get; set; }

        public int СandidateId { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public string Description { get; set; }

        public DateTime BrdDate { get; set; }

        public CondidateStatus Status { get; set; }

        public List<CondidateStatus> CondidateStatusList { get; set; }

        #endregion

        #region Commands

        public ICommand CommandOK { get; set; }

        public ICommand CommandSave { get; set; }

        public ICommand ClosePage { get; set; }

        //public ICommand SelectVacancy { get; set; }

        public ICommand FindUploadCommand { get; set; }

        #endregion

        #region Constructor

        public NewСandidatePageViewModel(PropertyChangeModel PropertyChangeModel, ApplicationPageCommands applicationPageCommands, Сandidate CurrentVacancy = null)
        {
            mPropertyChangeModel = PropertyChangeModel;

            //mPropertyChangeModel.SendValueEvent += PropertyChangeModelSendValue;
            mApplicationPageCommands = applicationPageCommands;

            Logger = new DataLogger();
            connectionToDB = new ConnectionToDB(true);

            CommandOK = new RelayCommand(() => SaveChangesAndClose());
            CommandSave = new RelayCommand(() => SaveChanges());
            ClosePage = new RelayCommand(() => CloseNewPage());
            FindUploadCommand = new RelayCommand(()=> EnterInputed());

            CondidateStatusViewModel condidateStatusViewModel = new CondidateStatusViewModel();
            CondidateStatusList = condidateStatusViewModel.CondidateStatusList;
            Status = condidateStatusViewModel.CondidateStatus;

            if (mApplicationPageCommands == ApplicationPageCommands.New)
            {
                mСandidate = new Сandidate();
                СandidateId = GetNewCode();

            }
            else if (mApplicationPageCommands == ApplicationPageCommands.Edit)
            {
                Title = "Кандидат: Создан";
                mСandidate = CurrentVacancy;
                СandidateId = mСandidate.Code;
                FullName = mСandidate.FullName;
                Name = mСandidate.Name;
                Surname = mСandidate.Surname;
                Patronymic = mСandidate.Patronymic;
                Description = mСandidate.Description;
                BrdDate = mСandidate.BrdDate;
                Status = condidateStatusViewModel.GetByName(mСandidate.Status);

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
                mСandidate.BrdDate = CurrentVacancy.BrdDate;
                mСandidate.BrdDate = CurrentVacancy.BrdDate;
                mСandidate.Status = CurrentVacancy.Status;

                FullName = mСandidate.FullName;
                Name = mСandidate.Name;
                Surname = mСandidate.Surname;
                Patronymic = mСandidate.Patronymic;
                Description = mСandidate.Description;
                BrdDate = mСandidate.BrdDate;
                Status = condidateStatusViewModel.GetByName(mСandidate.Status);
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
            try
            {
                using (EntityContext context = new EntityContext(connectionToDB.ConnectionString))
                {
                    if (context.Сandidates.Count() > 0)
                    {
                        return context.Сandidates.Max(c => c.Code) + 1;
                    }
                }

            }catch(Exception e)
            {
                Logger.WriteToLog(@"Соискатель: не удалось получить данные из базы");
                Logger.WriteToLog(e.Message);
            }
            return 1;
        }

        //private void SelectVacancyCommand()
        //{
        //    mPropertyChangeModel.SendValue(ApplicationMenuControl.SelectVacancy, ApplicationMenuControl.SelectVacancy, this);
        //}

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
            if (BrdDate == null)
            {
                MessageBox.Show("Укажите дату рождения кандидата", "Ошибка");
                return false;
            }
            return true;
        }

        private bool SaveChanges()
        {
            if (!ChecFields())
            { return false;}

            try
            {
                using (EntityContext context = new EntityContext(connectionToDB.ConnectionString))
                {
                    Сandidate currVal = context.Сandidates.Where(c => c.Code == СandidateId).FirstOrDefault();
                    if (currVal == null)
                    {
                        mСandidate.Code = СandidateId;
                        mСandidate.FullName = FullName;
                        mСandidate.Name = Name;
                        mСandidate.Surname = Surname;
                        mСandidate.Patronymic = Patronymic;
                        mСandidate.Description = Description;
                        mСandidate.BrdDate = BrdDate;
                        mСandidate.Status = Status.Name;

                        context.Сandidates.Add(mСandidate);
                    }
                    else
                    {
                        currVal.Code = СandidateId;
                        currVal.FullName = FullName;
                        currVal.Name = Name;
                        currVal.Surname = Surname;
                        currVal.Patronymic = Patronymic;
                        currVal.Description = Description;
                        currVal.BrdDate = BrdDate;
                        currVal.Status = Status.Name;
                    }
                    context.SaveChanges();
                }

            }catch(Exception e)
            {
                Logger.WriteToLog(@"Соискатель: не удалось записать данные в базу");
                Logger.WriteToLog(e.Message);
            }
            return true;
        }

        private void SaveChangesAndClose()
        {

            if (SaveChanges())
            {
                mPropertyChangeModel.ClosePage(null);
            }
        }

        //private void PropertyChangeModelSendValue(object sender, object Value)
        //{
        //    if (Value is Vacancy)
        //    {
        //        Vacancy = (Vacancy)Value;
        //    }
        //}


        #endregion

    }
}
