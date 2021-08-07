using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyHR
{
    public class NewVacancyPageViewModel : BaseViewModel
    {
        #region Private Members

        private readonly PropertyChangeModel mPropertyChangeModel;
        private readonly ApplicationPageCommands mApplicationPageCommands;
        private EntityContext context;
        private Vacancy mVacancy;

        #endregion

        #region Public Members

        public string Title { get; set; } = "Вакансия: Новый";

        public int VacancyId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int PositionId { get; set; }

        public Position Position { get; set; }

        #endregion

        #region Commands

        public ICommand CommandOK { get; set; }

        public ICommand CommandSave { get; set; }

        public ICommand ClosePage { get; set; }

        public ICommand SelectPosition { get; set; }

        #endregion

        #region Constructor

        public NewVacancyPageViewModel(PropertyChangeModel PropertyChangeModel, ApplicationPageCommands applicationPageCommands, Vacancy CurrentPosition = null)
        {
            mPropertyChangeModel = PropertyChangeModel;
            mPropertyChangeModel.SendValueEvent += PropertyChangeModelSendValue;
            mApplicationPageCommands = applicationPageCommands;

            context = new EntityContext("ConnectionToDB");

            CommandOK = new RelayCommand(() => SaveChangesAndClose());
            CommandSave = new RelayCommand(() => SaveChanges());
            ClosePage = new RelayCommand(() => CloseNewPage());
            SelectPosition = new RelayCommand(() => SelectPositionCommand());

            if (mApplicationPageCommands == ApplicationPageCommands.New)
            {
                mVacancy = new Vacancy();
                VacancyId = GetNewCode();

            }
            else if (mApplicationPageCommands == ApplicationPageCommands.Edit)
            {
                Title = "Вакансия: Создан";
                mVacancy = CurrentPosition;
                VacancyId = mVacancy.Code;
                Name = mVacancy.Name;
                Description = mVacancy.Description;
                PositionId = mVacancy.PositionId;

                //Position = GetPosition(PositionId); 
                Position = mVacancy.Position;

            }
            else if (mApplicationPageCommands == ApplicationPageCommands.Copy)
            {
                mVacancy = new Vacancy();
                mVacancy.Name = CurrentPosition.Name;
                mVacancy.Description = CurrentPosition.Description;
                mVacancy.PositionId = CurrentPosition.PositionId;
                mVacancy.Position = CurrentPosition.Position;

                VacancyId = GetNewCode();
                Name = mVacancy.Name;
                Description = mVacancy.Description;
                PositionId = mVacancy.PositionId;
                Position = mVacancy.Position;
            }
        }

        private int GetNewCode()
        {
            if (context.Vacancies.Count() > 0)
            {
                return context.Vacancies.Max(c => c.Code) + 1;
            }
            return 1;
        }


        //private Position GetPosition(int positionId)
        //{
        //    return context.Positions.Where(p => p.PositionId == positionId).FirstOrDefault();
        //}

        private void PropertyChangeModelSendValue(object sender, object Value)
        {
            if(Value is Position)
            {
                Position = (Position)Value;
            }
        }

        private void SelectPositionCommand()
        {
            mPropertyChangeModel.SendValue(ApplicationMenuControl.SelectPosition, ApplicationMenuControl.SelectPosition, this);
        }

        private void CloseNewPage()
        {
            context.Dispose();
            mPropertyChangeModel.ClosePage(null);
        }

        private bool ChecFields()
        {
            if (Name == null)
            {
                MessageBox.Show("Укажите наименование вакансии", "Ошибка");
                return false;
            }
            if(Position == null)
            {
                MessageBox.Show("Укажите должность", "Ошибка");
                return false;
            }

            return true;
        }
        private bool SaveChanges()
        {
            if (!ChecFields())
                return false;

            var currVal = context.Vacancies.Where(c => c.Code == VacancyId).FirstOrDefault();
            if(currVal == null)
            {
                mVacancy.Code = VacancyId;
                mVacancy.Name = Name;
                mVacancy.Description = Description;
                mVacancy.PositionId = Position.PositionId;

                context.Vacancies.Add(mVacancy);
            }
            else
            {
                currVal.Code = VacancyId;
                currVal.Name = Name;
                currVal.Description = Description;
                currVal.PositionId = Position.PositionId;
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

        #endregion

    }
}
