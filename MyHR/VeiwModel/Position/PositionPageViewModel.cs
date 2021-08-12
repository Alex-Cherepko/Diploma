using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyHR
{
    public class PositionPageViewModel : BaseViewModel
    {
        #region Private Members

        private readonly PropertyChangeModel mPropertyChangeModel;

        private EntityContext context;

        private DataLogger Logger;

        #endregion

        #region Public Members

        public ObservableCollection<Position> GridDataContext { get; set; }

        public Position SelectedPosition { get; set; }

        #endregion

        #region Commands

        public ICommand New { get; set; }

        public ICommand Edit { get; set; }

        public ICommand Copy { get; set; }

        public ICommand Delete { get; set; }

        public ICommand UpdateList { get; set; }

        public ICommand ClosePage { get; set; }

        public ICommand SelectPosition { get; set; }

        #endregion

        #region Constructor

        public PositionPageViewModel()
        {

        }

        public PositionPageViewModel(PropertyChangeModel PropertyChangeModel)
        {
            Logger = new DataLogger();

            mPropertyChangeModel = PropertyChangeModel;

            New = new RelayCommand(() => NewPosition());
            Edit = new RelayCommand(() => EditPosition());
            Copy = new RelayCommand(() => CopyPosition());
            Delete = new RelayCommand(() => DeletePosition());
            UpdateList = new RelayCommand(() => UpdateListPosition());
            ClosePage = new RelayCommand(() => ClosePagePosition());
            SelectPosition = new RelayCommand(() => SelectPositionCommand());

            try
            {
                context = new EntityContext("ConnectionToDB");

            }catch( Exception e)
            {
                Logger.WriteToLog(@"Список должностей: не удалось получить контекст базы данных");
                Logger.WriteToLog(e.Message);
            }
            try
            { 

                context.Positions.Load();
                GridDataContext = context.Positions.Local;

            }catch(Exception e)
            {
                Logger.WriteToLog(@"Список должностей: не удалось получить данные из базы");
                Logger.WriteToLog(e.Message);
            }

            try { context.Dispose(); } catch { }
            
        }

        private void DeletePosition()
        {
            context = new EntityContext("ConnectionToDB");

            try
            {

                Position position = context.Positions.Where(o => o.PositionId == SelectedPosition.PositionId).FirstOrDefault();
                if (position != null)
                    context.Positions.Remove(position);
                context.SaveChanges();

                GridDataContext = context.Positions.Local;
            }
            catch { MessageBox.Show("Удалить не возможно. Есть ссылки на другие объекты", "Ошибка удаления"); }

            context.Dispose();

        }

        private void UpdateListPosition()
        {
            context = new EntityContext("ConnectionToDB");
            context.Positions.Load();
            GridDataContext = context.Positions.Local;
            context.Dispose();
        }

        private void SelectPositionCommand()
        {
            mPropertyChangeModel.SendValueToOwner(SelectedPosition);
            mPropertyChangeModel.ClosePage(null);
        }

        private void ClosePagePosition()
        {
            mPropertyChangeModel.ClosePage(null);
        }

        private void CopyPosition()
        {
            if(SelectedPosition != null)
                mPropertyChangeModel.SendValue(ApplicationMenuControl.NewPosition, ApplicationPageCommands.Copy, SelectedPosition);
        }

        private void EditPosition()
        {
            if (SelectedPosition != null)
                mPropertyChangeModel.SendValue(ApplicationMenuControl.NewPosition, ApplicationPageCommands.Edit, SelectedPosition);
        }

        private void NewPosition()
        {
            mPropertyChangeModel.SendValue(ApplicationMenuControl.NewPosition, ApplicationPageCommands.New, null);
        }

        #endregion

    }
}
