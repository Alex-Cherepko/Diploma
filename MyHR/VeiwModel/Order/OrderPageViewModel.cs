using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private ConnectionToDB connectionToDB;

        private DataLogger Logger;

        #endregion

        #region Public Members

        public ObservableCollection<DataOrderList> DataContext { get; set; } = new ObservableCollection<DataOrderList>();

        public DataOrderList SelectedPosition { get; set; }

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
            connectionToDB = new ConnectionToDB(true);

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
                try
                {
                    using (EntityContext context = new EntityContext(connectionToDB.ConnectionString))
                    {
                        //DataContext = context.Orders.ToList();
                        var cont = (from o in context.Orders
                                       from ov in context.Order_Vacancy
                                       where o.OrderId == ov.OrderId
                                       select new {o.OrderId, o.Code,o.DocDate,o.Status,o.ExecutionTerm, Vacancy = ov.Vacancy.Name, Order = o }).ToList();

                        foreach(var item in cont)
                        {
                            DataOrderList NewItem = new DataOrderList(item.OrderId, item.Code, item.DocDate, item.Status, item.ExecutionTerm, item.Vacancy, item.Order);
                            DataContext.Add(NewItem);
                        }

                         
                    }
                }catch (Exception e)
                {
                    Logger.WriteToLog(@"Список заявок: не удалось получить данные из базы");
                    Logger.WriteToLog(e.Message);
                }
            }
            catch (Exception e)
            {
                Logger.WriteToLog(@"Список заявок: не удалось получить контекст базы данных");
                Logger.WriteToLog(e.Message);
            }
        }

        public class DataOrderList
        {
            public int OrderId { get; set; }
            public int Code { get; set; }
            public DateTime DocDate { get; set; }
            public string Status { get; set; }
            public DateTime ExecutionTerm { get; set; }
            public string Vacancy { get; set; }
            public Order Order { get; set; }

            public DataOrderList(int orderId, int code, DateTime docDate, string status, DateTime executionTerm, string vacancy, Order order)
            {
                OrderId = orderId;
                Code = code;
                DocDate = docDate;
                Status = status;
                ExecutionTerm = executionTerm;
                Vacancy = vacancy;
                Order = order;

            }

        }
        private void DeleteOrder()
        {
            try
            {
                using (EntityContext context = new EntityContext(connectionToDB.ConnectionString))
                {
                    List<Order_Vacancy> OV = context.Order_Vacancy.Where(o => o.OrderId == SelectedPosition.Order.OrderId).ToList();
                    foreach (Order_Vacancy item in OV)
                    {
                        context.Order_Vacancy.Remove(item);
                    }

                    List<Orders_CandidateForm> _СandidateFormList = context.Orders_CandidateForms.Where(o => o.OrderId == SelectedPosition.Order.OrderId).ToList();
                    foreach (Orders_CandidateForm item in _СandidateFormList)
                    {
                        context.Orders_CandidateForms.Remove(item);
                    }
                    Order order = context.Orders.Where(o => o.OrderId == SelectedPosition.Order.OrderId).FirstOrDefault();
                    if (order != null)
                        context.Orders.Remove(order);
                    context.SaveChanges();

                    //DataContext = context.Orders.ToList();
                    var cont = (from o in context.Orders
                                from ov in context.Order_Vacancy
                                where o.OrderId == ov.OrderId
                                select new { o.OrderId, o.Code, o.DocDate, o.Status, o.ExecutionTerm, Vacancy = ov.Vacancy.Name, Order = o }).ToList();

                    foreach (var item in cont)
                    {
                        DataOrderList NewItem = new DataOrderList(item.OrderId, item.Code, item.DocDate, item.Status, item.ExecutionTerm, item.Vacancy, item.Order);
                        DataContext.Add(NewItem);
                    }

                }
            }
            catch (Exception e)
            {
                Logger.WriteToLog(@"Список заявок: не удалось удалить заявоку");
                Logger.WriteToLog(e.Message);
            }

        }


        private void UpdateListOrder()
        {
            try
            {
                try
                {
                    using (EntityContext context = new EntityContext(connectionToDB.ConnectionString))
                    {
                        //DataContext = context.Orders.ToList();
                        var cont = (from o in context.Orders
                                    from ov in context.Order_Vacancy
                                    where o.OrderId == ov.OrderId
                                    select new { o.OrderId, o.Code, o.DocDate, o.Status, o.ExecutionTerm, Vacancy = ov.Vacancy.Name, Order = o }).ToList();

                        foreach (var item in cont)
                        {
                            DataOrderList NewItem = new DataOrderList(item.OrderId, item.Code, item.DocDate, item.Status, item.ExecutionTerm, item.Vacancy, item.Order);
                            DataContext.Add(NewItem);
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteToLog(@"Список заявок: не удалось получить данные из базы");
                    Logger.WriteToLog(e.Message);
                }
            }
            catch (Exception e)
            {
                Logger.WriteToLog(@"Список заявок: не удалось получить контекст базы данных");
                Logger.WriteToLog(e.Message);
            }
        }

        private void ClosePageOrder()
        {
            mPropertyChangeModel.ClosePage(null);
        }

        private void CopyOrder()
        {
            if (SelectedPosition.Order != null)
                mPropertyChangeModel.SendValue(ApplicationMenuControl.NewOrder, ApplicationPageCommands.Copy, SelectedPosition.Order);
        }

        private void EditOrder()
        {
            if (SelectedPosition.Order != null)
                mPropertyChangeModel.SendValue(ApplicationMenuControl.NewOrder, ApplicationPageCommands.Edit, SelectedPosition.Order);
        }

        private void NewOrder()
        {
            mPropertyChangeModel.SendValue(ApplicationMenuControl.NewOrder, ApplicationPageCommands.New, null);
        }

        #endregion

    }
}
