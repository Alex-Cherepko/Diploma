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

        public int VacancyId { get; set; } = 1;

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
                //context.Vacancies.Add(mVacancy);
                //context.SaveChanges();
                //VacancyId = mVacancy.VacancyId;

            }
            else if (mApplicationPageCommands == ApplicationPageCommands.Edit)
            {
                Title = "Вакансия: Создан";
                mVacancy = CurrentPosition;
                VacancyId = mVacancy.VacancyId;
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

                VacancyId = mVacancy.VacancyId;
                Name = mVacancy.Name;
                Description = mVacancy.Description;
                PositionId = mVacancy.PositionId;
                Position = mVacancy.Position;
            }
        }

        private Position GetPosition(int positionId)
        {
            return context.Positions.Where(p => p.PositionId == positionId).FirstOrDefault();
        }

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
        private void SaveChanges()
        {
            if (!ChecFields())
                return;

            var currVal = context.Vacancies.Where(c => c.VacancyId == VacancyId).FirstOrDefault();
            if(currVal == null)
            {
                mVacancy.VacancyId = VacancyId;
                mVacancy.Name = Name;
                mVacancy.Description = Description;
                mVacancy.PositionId = Position.PositionId;

                context.Vacancies.Add(mVacancy);
            }
            else
            {
            currVal.VacancyId = VacancyId;
            currVal.Name = Name;
            currVal.Description = Description;
            currVal.PositionId = Position.PositionId;
            }


            context.SaveChanges();
            
        }

        private void SaveChangesAndClose()
        {
            SaveChanges();
            context.Dispose();

            mPropertyChangeModel.ClosePage(null);
        }

        #endregion

    }
}
