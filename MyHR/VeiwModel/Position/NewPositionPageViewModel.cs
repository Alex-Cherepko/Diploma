using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyHR
{
    public class NewPositionPageViewModel : BaseViewModel
    {
        #region Private Members

        private readonly PropertyChangeModel mPropertyChangeModel;
        private readonly ApplicationPageCommands mApplicationPageCommands;
        private EntityContext context;
        private Position mPosition;

        #endregion

        #region Public Members

        public string Title { get; set; } = "Должность: Новый";

        public int PositionId { get; set; }

        public string Name { get; set; }

        #endregion

        #region Commands

        public ICommand CommandOK { get; set; }

        public ICommand CommandSave { get; set; }

        public ICommand ClosePage { get; set; }

        #endregion

        #region Constructor

        public NewPositionPageViewModel(PropertyChangeModel PropertyChangeModel, ApplicationPageCommands applicationPageCommands, Position CurrentPosition = null)
        {
            mPropertyChangeModel = PropertyChangeModel;
            mApplicationPageCommands = applicationPageCommands;

            context = new EntityContext("ConnectionToDB");

            CommandOK = new RelayCommand(()=>SaveChangesAndClose());
            CommandSave = new RelayCommand(() => SaveChanges());
            ClosePage = new RelayCommand(() => CloseNewPage());

            if (mApplicationPageCommands == ApplicationPageCommands.New)
            {
                mPosition = new Position();
                context.Positions.Add(mPosition);
                context.SaveChanges();
                PositionId = mPosition.PositionId;

            }
            else if(mApplicationPageCommands == ApplicationPageCommands.Edit)
            {
                Title = "Должность: Создан";
                mPosition = CurrentPosition;
                PositionId = mPosition.PositionId;
                Name = mPosition.Name;
            }
            else if(mApplicationPageCommands == ApplicationPageCommands.Copy)
            {
                mPosition = new Position();
                mPosition.Name = CurrentPosition.Name;
                context.Positions.Add(mPosition);
                context.SaveChanges();
                PositionId = mPosition.PositionId;
                Name = mPosition.Name;

            }

        }

        private bool ChecFields()
        {
            if (Name == null)
            {
                MessageBox.Show("Укажите наименование должности","Ошибка");
                return false;
            }

            return true;
        }
        private void CloseNewPage()
        {
            mPropertyChangeModel.ClosePage(null);
        }

        private void SaveChanges()
        {
            if (!ChecFields())
                return;

            var currVal = context.Positions.Where(c => c.PositionId == PositionId).FirstOrDefault();
            currVal.PositionId = PositionId;
            currVal.Name = Name;

            context.SaveChanges();
            context.Dispose();
        }

        private void SaveChangesAndClose()
        {
            var currVal = context.Positions.Where(c => c.PositionId == PositionId).FirstOrDefault();
            currVal.PositionId = PositionId;
            currVal.Name = Name;

            context.SaveChanges();
            context.Dispose();

            mPropertyChangeModel.ClosePage(null);
        }

        #endregion

    }
}
