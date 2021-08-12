using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyHR
{
    public class OrderPageViewModel : BaseViewModel
    {
        #region Private Members

        private readonly PropertyChangeModel mPropertyChangeModel;

        private EntityContext context;

        private DataLogger Logger;

        #endregion

        #region Public Members

        public List<Order> DataContext { get; set; }

        public Order SelectedPosition { get; set; }

        #endregion

        #region Commands

        public ICommand New { get; set; }

        public ICommand Edit { get; set; }

        public ICommand Copy { get; set; }

        public ICommand UpdateList { get; set; }

        public ICommand ClosePage { get; set; }

        public ICommand Delete { get; set; }


        #endregion

        #region Constructor

        public OrderPageViewModel(PropertyChangeModel PropertyChangeModel)
        {
            Logger = new DataLogger();

            mPropertyChangeModel = PropertyChangeModel;

            New = new RelayCommand(() => NewOrder());
            Edit = new RelayCommand(() => EditOrder());
            Copy = new RelayCommand(() => CopyOrder());
            Delete = new RelayCommand(() => DeleteOrder());

            UpdateList = new RelayCommand(() => UpdateListOrder());
            ClosePage = new RelayCommand(() => ClosePageOrder());
            //SelectCandidateForm = new RelayCommand(() => SelectCandidateFormCommand());

            try
            {
                context = new EntityContext("ConnectionToDB");
            }
            catch (Exception e)
            {
                Logger.WriteToLog(@"Список анкет: не удалось получить контекст базы данных");
                Logger.WriteToLog(e.Message);
            }
            try
            {

                DataContext = context.Orders.ToList();
                context.Dispose();

            }
            catch (Exception e)
            {
                Logger.WriteToLog(@"Список анкет: не удалось получить данные из базы");
                Logger.WriteToLog(e.Message);
            }
        }

        private void DeleteOrder()
        {
            context = new EntityContext("ConnectionToDB");

            List<Order_Vacancy> OV = context.Order_Vacancy.Where(o => o.OrderId == SelectedPosition.OrderId).ToList();
            foreach(Order_Vacancy item in OV)
            {
                context.Order_Vacancy.Remove(item);
            } 

            List<Orders_CandidateForm> _СandidateFormList = context.Orders_CandidateForms.Where(o => o.OrderId == SelectedPosition.OrderId).ToList();
            foreach (Orders_CandidateForm item in _СandidateFormList)
            {
                context.Orders_CandidateForms.Remove(item);
            }
            Order order = context.Orders.Where(o => o.OrderId == SelectedPosition.OrderId).FirstOrDefault();
            if(order != null)
            context.Orders.Remove(order);
            context.SaveChanges();

            DataContext = context.Orders.ToList();
            context.Dispose();

        }

        //private void SelectCandidateFormCommand()
        //{
        //    mPropertyChangeModel.SendValueToOwner(SelectedPosition);
        //    mPropertyChangeModel.ClosePage(null);
        //}

        private void UpdateListOrder()
        {
            context = new EntityContext("ConnectionToDB");

            DataContext = context.Orders.ToList();
            context.Dispose();
        }

        private void ClosePageOrder()
        {
            mPropertyChangeModel.ClosePage(null);
        }

        private void CopyOrder()
        {
            if (SelectedPosition != null)
                mPropertyChangeModel.SendValue(ApplicationMenuControl.NewOrder, ApplicationPageCommands.Copy, SelectedPosition);
        }

        private void EditOrder()
        {
            if (SelectedPosition != null)
                mPropertyChangeModel.SendValue(ApplicationMenuControl.NewOrder, ApplicationPageCommands.Edit, SelectedPosition);
        }

        private void NewOrder()
        {
            mPropertyChangeModel.SendValue(ApplicationMenuControl.NewOrder, ApplicationPageCommands.New, null);
        }

        #endregion

    }
}
