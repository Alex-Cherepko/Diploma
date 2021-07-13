using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyHR
{
    public class OpenPageItemViewModel : BaseViewModel
    {
        #region Private Members

        private readonly PropertyChangeModel mPropertyChangeModel;

        #endregion

        #region Public Members

        public string NamePage { get; set; }

        public UserControl CurrentPage { get; set; }

        public UserControl NextPage { get; set; }

        public bool CurrentPageAvailable { get; set; } = true;

        #endregion

        #region Commands

        public ICommand ClosePage { get; set; }
        public ICommand ChangePage { get; set; }

        #endregion

        #region Constructor

        public OpenPageItemViewModel(string NamePage, UserControl CurrentPage, UserControl NextPage, PropertyChangeModel PropertyChangeModel)
        {
            mPropertyChangeModel = PropertyChangeModel;

            this.CurrentPage = CurrentPage;
            this.NextPage = NextPage;
            this.NamePage = NamePage;

            ClosePage = new RelayCommand(()=>CloseCurrPage());
            ChangePage = new RelayCommand(()=> ChangePageCurrPage());
        }

        private void ChangePageCurrPage()
        {
            mPropertyChangeModel.ChagePage(CurrentPage);
        }

        private void CloseCurrPage()
        {
            //ChangePageCurrPage();
            mPropertyChangeModel.ClosePage(CurrentPage);
        }

        public void SetCurrentPageAvailable(bool Value) { CurrentPageAvailable = Value; }

        #endregion

    }
}
