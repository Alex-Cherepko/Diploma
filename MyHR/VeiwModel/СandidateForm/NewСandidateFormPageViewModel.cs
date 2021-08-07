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
    public class NewСandidateFormPageViewModel : BaseViewModel
    {
        #region Private Members

        private readonly PropertyChangeModel mPropertyChangeModel;
        private readonly ApplicationPageCommands mApplicationPageCommands;
        private EntityContext context;
        private СandidateForm mСandidateForm;

        #endregion

        #region Public Members

        public string Title { get; set; } = "Анкета: Новый";

        public int СandidateFormId { get; set; }

        public DateTime DocDate { get; set; } = DateTime.Now;

        public string Sity { get; set; }

        public string Description { get; set; }

        public List<CondidateFormStatus> CondidateFormList { get; set; }
        public CondidateFormStatus Status { get; set; }

        public Сandidate Сandidate { get; set; }

        public Vacancy Vacancy { get; set; }

        #endregion

        #region Commands

        public ICommand CommandOK { get; set; }

        public ICommand CommandSave { get; set; }

        public ICommand ClosePage { get; set; }

        public ICommand SelectVacancy { get; set; }

        public ICommand SelectedCandidate { get; set; }

        #endregion

        #region Constructor

        public NewСandidateFormPageViewModel(PropertyChangeModel PropertyChangeModel, ApplicationPageCommands applicationPageCommands, СandidateForm CurrentСandidateForm = null)
        {
            mPropertyChangeModel = PropertyChangeModel;

            mPropertyChangeModel.SendValueEvent += PropertyChangeModelSendValue;
            mApplicationPageCommands = applicationPageCommands;

            context = new EntityContext("ConnectionToDB");

            CommandOK = new RelayCommand(() => SaveChangesAndClose());
            CommandSave = new RelayCommand(() => SaveChanges());
            ClosePage = new RelayCommand(() => CloseNewPage());
            SelectVacancy = new RelayCommand(() => SelectVacancyCommand());
            SelectedCandidate = new RelayCommand(() => SelectCandidateCommand());

            CondidateFormStatusViewModel condidateFormStatusViewModel = new CondidateFormStatusViewModel();
            CondidateFormList = condidateFormStatusViewModel.CondidateFormStatusList;
            Status = condidateFormStatusViewModel.CondidateFormStatus;

            if (mApplicationPageCommands == ApplicationPageCommands.New)
            {
                mСandidateForm = new СandidateForm();
                СandidateFormId = GetNewCode();

            }
            else if (mApplicationPageCommands == ApplicationPageCommands.Edit)
            {
                Title = "Анкета: Создан";
                mСandidateForm = CurrentСandidateForm;
                СandidateFormId = mСandidateForm.Code;
                DocDate = mСandidateForm.DocDate;

                Status = condidateFormStatusViewModel.GetByName(mСandidateForm.Status);
                Sity = mСandidateForm.Sity;
                Description = mСandidateForm.Description;

                Vacancy = mСandidateForm.Vacancy;
                Сandidate = mСandidateForm.Сandidate;

            }
            else if (mApplicationPageCommands == ApplicationPageCommands.Copy)
            {
                mСandidateForm = new СandidateForm();
                СandidateFormId = GetNewCode();
                mСandidateForm.Sity = CurrentСandidateForm.Sity;
                mСandidateForm.Status = CurrentСandidateForm.Status;
                mСandidateForm.Description = CurrentСandidateForm.Description;
                mСandidateForm.Vacancy = CurrentСandidateForm.Vacancy;
                mСandidateForm.Сandidate = CurrentСandidateForm.Сandidate;

                Sity = mСandidateForm.Sity;
                Status = condidateFormStatusViewModel.GetByName(mСandidateForm.Status);
                Description = mСandidateForm.Description;
                Vacancy = mСandidateForm.Vacancy;
                Сandidate = mСandidateForm.Сandidate;

            }
        }

        private void SelectVacancyCommand()
        {
            mPropertyChangeModel.SendValue(ApplicationMenuControl.SelectVacancy, ApplicationMenuControl.SelectVacancy, this);
        }

        private int GetNewCode()
        {
            if (context.СandidateFormes.Count() > 0)
            {
                return context.СandidateFormes.Max(c => c.Code) + 1;
            }
            return 1;
        }

        private void SelectCandidateCommand()
        {
            mPropertyChangeModel.SendValue(ApplicationMenuControl.SelectCandidate, ApplicationMenuControl.SelectCandidate, this);
        }

        private void CloseNewPage()
        {
            context.Dispose();
            mPropertyChangeModel.ClosePage(null);
        }

        private bool ChecFields()
        {
            if (Сandidate == null)
            {
                MessageBox.Show("Укажите кандидата", "Ошибка");
                return false;
            }

            if (Vacancy == null)
            {
                MessageBox.Show("Укажите вакансию кандидата", "Ошибка");
                return false;
            }

            return true;
        }

        private bool SaveChanges()
        {
            if (!ChecFields())
                return false;

            var currVal = context.СandidateFormes.Where(c => c.Code == СandidateFormId).FirstOrDefault();
            if (currVal == null)
            {
                mСandidateForm.Code = СandidateFormId;
                mСandidateForm.DocDate = DocDate;
                mСandidateForm.Sity = Sity;
                mСandidateForm.Status = Status.Name;
                mСandidateForm.Description = Description;
                mСandidateForm.VacancyId = Vacancy.VacancyId;
                mСandidateForm.СandidateId = Сandidate.СandidateId;

                context.СandidateFormes.Add(mСandidateForm);
            }
            else
            {
                currVal.Code = СandidateFormId;
                currVal.DocDate = DocDate;
                currVal.Sity = Sity;
                currVal.Status = Status.Name;
                currVal.Description = Description;
                currVal.VacancyId = Vacancy.VacancyId;
                currVal.СandidateId = Сandidate.СandidateId;

            }
            context.SaveChanges();

            return true;
            
        }

        private void SaveChangesAndClose()
        {
            if (SaveChanges())
            {
                context.Dispose();

                mPropertyChangeModel.ClosePage(null);
            }
        }

        private void PropertyChangeModelSendValue(object sender, object Value)
        {
            if (Value is Vacancy vacancy)
            {
                Vacancy = vacancy;
            }
            if (Value is Сandidate)
            {
                Сandidate = (Сandidate)Value;
            }

        }


        #endregion

    }
}
