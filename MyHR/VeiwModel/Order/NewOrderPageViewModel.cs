using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyHR
{
    public class NewOrderPageViewModel : BaseViewModel
    {
        #region Private Members

        private readonly PropertyChangeModel mPropertyChangeModel;
        private readonly ApplicationPageCommands mApplicationPageCommands;
        private EntityContext context;
        private Order mOrder;
        private Order_Vacancy mOrder_Vacancy;
        private Orders_CandidateForm mOrders_CandidateForm;

        #endregion

        #region Public Members

        public string Title { get; set; } = "Заявка: Новый";

        public int OrderId { get; set; }

        public DateTime DocDate { get; set; } = DateTime.Now;

        public DateTime ExecutionTerm { get; set; } = DateTime.Now;

        public Vacancy Vacancy { get; set; }

        public List<OrderStatus> OrderList { get; set; }
        public OrderStatus Status { get; set; }

        public ObservableCollection<СandidateForm> СandidateFormList { get; set; } = new ObservableCollection<СandidateForm>();


        #endregion

        #region Commands

        public ICommand CommandOK { get; set; }

        public ICommand CommandSave { get; set; }

        public ICommand ClosePage { get; set; }

        public ICommand SelectVacancy { get; set; }

        public ICommand SelectCanditateForm { get; set; }

        #endregion

        #region Constructor

        public NewOrderPageViewModel(PropertyChangeModel PropertyChangeModel, ApplicationPageCommands applicationPageCommands, Order CurrentOrder = null)
        {
            mPropertyChangeModel = PropertyChangeModel;

            mPropertyChangeModel.SendValueEvent += PropertyChangeModelSendValue;
            mApplicationPageCommands = applicationPageCommands;

            context = new EntityContext("ConnectionToDB");

            CommandOK = new RelayCommand(() => SaveChangesAndClose());
            CommandSave = new RelayCommand(() => SaveChanges());
            ClosePage = new RelayCommand(() => CloseNewPage());
            SelectVacancy = new RelayCommand(() => SelectVacancyCommand());
            SelectCanditateForm = new RelayCommand(() => SelectCandidateFormCommand());

            OrderStatusViewModel orderStatusViewModel = new OrderStatusViewModel();
            OrderList = orderStatusViewModel.OrderStatusList;
            Status = orderStatusViewModel.OrderStatus;

            if (mApplicationPageCommands == ApplicationPageCommands.New)
            {
                mOrder = new Order();
                OrderId = GetNewCode();

                mOrder_Vacancy = new Order_Vacancy();


            }
            else if (mApplicationPageCommands == ApplicationPageCommands.Edit)
            {
                Title = "Заявка: Создан";
                mOrder = CurrentOrder;
                OrderId = mOrder.Code;
                DocDate = mOrder.DocDate;

                Status = orderStatusViewModel.GetByName(mOrder.Status);
                ExecutionTerm = mOrder.ExecutionTerm;

                mOrder_Vacancy = context.Order_Vacancy.Where(o => o.OrderId==CurrentOrder.OrderId).FirstOrDefault();
                if (mOrder_Vacancy != null)
                {
                    Vacancy = mOrder_Vacancy.Vacancy;
                    
                }
                else
                {
                    mOrder_Vacancy = new Order_Vacancy();
                }

                List<Orders_CandidateForm> _СandidateFormList = context.Orders_CandidateForms.Where(o => o.OrderId == mOrder.OrderId).ToList();
                foreach (Orders_CandidateForm item in _СandidateFormList)
                {
                    СandidateFormList.Add(item.CandidateForm);
                }

            }
            else if (mApplicationPageCommands == ApplicationPageCommands.Copy)
            {
                var Order_Vacancy = context.Order_Vacancy.Where(o => o.OrderId == CurrentOrder.OrderId).FirstOrDefault();
                if (Order_Vacancy != null)
                    Vacancy = Order_Vacancy.Vacancy;

                mOrder = new Order();
                OrderId = GetNewCode();
                mOrder.Code = OrderId;
                mOrder.DocDate = CurrentOrder.DocDate;
                mOrder.Status = CurrentOrder.Status;
                mOrder.ExecutionTerm = CurrentOrder.ExecutionTerm;

                mOrder_Vacancy = new Order_Vacancy();

                DocDate = mOrder.DocDate;
                Status = orderStatusViewModel.GetByName(mOrder.Status);
                ExecutionTerm = mOrder.ExecutionTerm;

                List<Orders_CandidateForm> _СandidateFormList = context.Orders_CandidateForms.Where(o => o.OrderId == mOrder.OrderId).ToList();
                foreach (Orders_CandidateForm item in _СandidateFormList)
                {
                    СandidateFormList.Add(item.CandidateForm);
                }


            }
        }

        private void SelectVacancyCommand()
        {
            mPropertyChangeModel.SendValue(ApplicationMenuControl.SelectVacancy, ApplicationMenuControl.SelectVacancy, this);
        }

        private int GetNewCode()
        {
            if (context.Orders.Count() > 0)
            {
                return context.Orders.Max(c => c.Code) + 1;
            }
            return 1;
        }

        private void SelectCandidateFormCommand()
        {
            mPropertyChangeModel.SendValue(ApplicationMenuControl.SelectedСandidateForm, ApplicationMenuControl.SelectedСandidateForm, this);
        }

        private void CloseNewPage()
        {
            context.Dispose();
            mPropertyChangeModel.ClosePage(null);
        }

        private bool ChecFields()
        {

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

            var currVal = context.Orders.Where(c => c.Code == OrderId).FirstOrDefault();
            if (currVal == null)
            {
                mOrder.Code = OrderId;
                mOrder.DocDate = DocDate;
                mOrder.ExecutionTerm = ExecutionTerm;
                mOrder.Status = Status.Name;
                // mOrder.VacancyId = Vacancy.VacancyId;

                context.Orders.Add(mOrder);

            }
            else
            {
                currVal.Code = OrderId;
                currVal.DocDate = DocDate;
                currVal.ExecutionTerm = ExecutionTerm;
                currVal.Status = Status.Name;
                // currVal.VacancyId = Vacancy.VacancyId;

            }

            context.SaveChanges();

            Order_Vacancy order_s = (from c in context.Order_Vacancy where c.Order.OrderId == mOrder.OrderId select c).FirstOrDefault();
            if (order_s == null)
            {
                //mOrder_Vacancy.Order = mOrder;
                mOrder_Vacancy.OrderId = mOrder.OrderId;
                //mOrder_Vacancy.Vacancy = Vacancy;
                mOrder_Vacancy.VacancyId = Vacancy.VacancyId;
                context.Order_Vacancy.Add(mOrder_Vacancy);
            }
            else
            {
                order_s.Vacancy = Vacancy;
            }

            //context.SaveChanges();

            List<Orders_CandidateForm> _СandidateFormList = context.Orders_CandidateForms.Where(o => o.OrderId == mOrder.OrderId).ToList();
            foreach (Orders_CandidateForm item in _СandidateFormList)
            {
                context.Orders_CandidateForms.Remove(item);
            }

            //context.SaveChanges();

            foreach (СandidateForm item in СandidateFormList)
            {
                mOrders_CandidateForm = new Orders_CandidateForm
                {

                    //Order = mOrder,
                    OrderId = mOrder.OrderId,
                    //CandidateForm = item,
                    СandidateFormId = item.СandidateFormId

                };
                context.Orders_CandidateForms.Add(mOrders_CandidateForm);
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
            if (Value is Vacancy)
            {
                Vacancy = (Vacancy)Value;
            }
            if (Value is СandidateForm)
            {
                СandidateFormList.Add((СandidateForm)Value);
            }
        }


        #endregion

        
    }
}
