using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyHR
{
    public class OpenPageListViewModel : BaseViewModel
    {
        #region Public Members

        public ObservableCollection<OpenPageItemControl> ItemPages { get; set; }

        #endregion

        #region Constructor

        public OpenPageListViewModel()
        {
            ItemPages = new ObservableCollection<OpenPageItemControl>();
        }

        #endregion

        #region Helpers

        public void AddPageToList(OpenPageItemControl openPageItem)
        {
            var VM = (OpenPageItemViewModel)openPageItem.DataContext;
            SetCurrentPageAvailable(VM.CurrentPage);

            ItemPages.Add(openPageItem);

        }

        public void RemovePageFromList(UserControl DeletedPage, UserControl CurrPage)
        {
            ItemPages.Remove(GetPageItemViewModel(DeletedPage));
            RebuildLinksInViewModels(DeletedPage, CurrPage);
        }

        private void RebuildLinksInViewModels(UserControl deletedPage, UserControl currPage)
        {

            if (ItemPages.Count == 1)
            {
                var VM = (OpenPageItemViewModel)(ItemPages[0].DataContext);
                VM.NextPage = null;
            }
            else
            {
                foreach (var Item in ItemPages)
                {
                    var VM = (OpenPageItemViewModel)(Item.DataContext);
                    if (VM.NextPage == null)
                    { VM.NextPage = currPage; }
                    if (VM.NextPage.Equals(deletedPage))
                    {
                        VM.NextPage = currPage;
                         
                    }
                    
                }
            }
        }

        public void RebuildLinks(UserControl currPage)
        {

            if (ItemPages.Count == 1)
            {
                var VM = (OpenPageItemViewModel)(ItemPages[0].DataContext);
                VM.NextPage = null;
            }
            else
            {
                foreach (var Item in ItemPages)
                {
                    var VM = (OpenPageItemViewModel)(Item.DataContext);
                    if (VM.NextPage == null)
                    { VM.NextPage = currPage; }
                    
                }
            }
        }

        public bool NotFindPage(Type type)
        {
            foreach (var Item in ItemPages)
            {
                var VM = (OpenPageItemViewModel)(Item.DataContext);
                if (VM.CurrentPage.GetType() == type)
                    return false;
            }
            return true;
        }

        public UserControl GetOpendPage(Type type)
        {
            foreach (var Item in ItemPages)
            {
                var VM = (OpenPageItemViewModel)(Item.DataContext);
                if (VM.CurrentPage.GetType() == type)
                    return VM.CurrentPage;
            }

            return null;//new UserControl();
        }

        public UserControl GetNextPage(UserControl Val)
        {
            if (ItemPages.Count == 1)
            {
                var VM = (OpenPageItemViewModel)(ItemPages[0].DataContext);
                return VM.CurrentPage;
            }
            foreach (var Item in ItemPages)
            {
                var VM = (OpenPageItemViewModel)(Item.DataContext);
                if (VM.CurrentPage.Equals(Val))
                    return VM.NextPage;

                
            }

            return null;
        }

        public OpenPageItemControl GetPageItemViewModel(UserControl userControl)
        {
            foreach (var Item in ItemPages)
            {
                var VM = (OpenPageItemViewModel)(Item.DataContext);
                if (VM.CurrentPage.Equals(userControl))
                    return Item;
            }
            return null;//new OpenPageItemControl(null,null,null,null);
        }

        public void SetCurrentPageAvailable(UserControl openPageItem)
        {
            foreach (var Item in ItemPages)
            {
                var VM = (OpenPageItemViewModel)(Item.DataContext);

                if (openPageItem != null)
                {
                    var CP = VM.CurrentPage;
                    if (CP.Equals(openPageItem))
                    {
                        VM.SetCurrentPageAvailable(true);
                        continue;
                    }
                }
                
                VM.SetCurrentPageAvailable(false);
            }
        }
        #endregion
    }
}
