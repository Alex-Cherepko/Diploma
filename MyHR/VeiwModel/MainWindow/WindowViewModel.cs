using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyHR
{
    /// <summary>
    /// The View Model for the custom flat window
    /// </summary>
    public class WindowViewModel : BaseViewModel
    {
        #region Private Member

        /// <summary>
        /// The window this view model controls
        /// </summary>
        private Window mWindow;

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        private int mOuterMarginSize = 10;

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        private int mWindowRadius = 10;

        /// <summary>
        /// The last known dock position
        /// </summary>
        private WindowDockPosition mDockPosition = WindowDockPosition.Undocked;

        private List<ApplicationUserControl> PromUserControlsList = new List<ApplicationUserControl>()
        {
           ApplicationUserControl.StaffRecruitment,
           ApplicationUserControl.Adaptation
        };

        private readonly PropertyChangeModel mPropertyChangeModel;

        private UserControl PrevPage;

        #endregion

        #region Public Properties

        /// <summary>
        /// The smallest width the window can go to
        /// </summary>
        public double WindowMinimumWidth { get; set; } = 400;

        /// <summary>
        /// The smallest height the window can go to
        /// </summary>
        public double WindowMinimumHeight { get; set; } = 400;

        /// <summary>
        /// True if the window should be borderless because it is docked or maximized
        /// </summary>
        public bool Borderless { get { return (mWindow.WindowState == WindowState.Maximized || mDockPosition != WindowDockPosition.Undocked); } }

        /// <summary>
        /// The size of the resize border around the window
        /// </summary>
        public int ResizeBorder { get { return Borderless ? 0 : 6; } }

        /// <summary>
        /// The size of the resize border around the window, taking into account the outer margin
        /// </summary>
        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorder + OuterMarginSize); } }

        /// <summary>
        /// The padding of the inner content of the main window
        /// </summary>
        public Thickness InnerContentPadding { get; set; } = new Thickness(0);

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        public int OuterMarginSize
        {
            get
            {
                // If it is maximized or docked, no border
                return Borderless ? 0 : mOuterMarginSize;
            }
            set
            {
                mOuterMarginSize = value;
            }
        }

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        public Thickness OuterMarginSizeThickness { get { return new Thickness(OuterMarginSize); } }

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public int WindowRadius
        {
            get
            {
                // If it is maximized or docked, no border
                return Borderless ? 0 : mWindowRadius;
            }
            set
            {
                mWindowRadius = value;
            }
        }

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public CornerRadius WindowCornerRadius { get { return new CornerRadius(WindowRadius); } }

        /// <summary>
        /// The height of the title bar / caption of the window
        /// </summary>
        public int TitleHeight { get; set; } = 42;

        /// <summary>
        /// The height of the title bar / caption of the window
        /// </summary>
        public GridLength TitleHeightGridLength { get { return new GridLength(TitleHeight + ResizeBorder); } }


        public ApplicationUserControl CurrentSection { get; set; } = ApplicationUserControl.StaffRecruitment;

        public List<ApplicationUserControl> UserControlsList => PromUserControlsList.ToList();

        public ApplicationMenuControl CurrentMenuControl { get; set; }

        public UserControl ContentControlPanel { get; set; }

        public UserControl CurrentPage { get; set; }

        public OpenPageListViewModel OpenPageListViewModel { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// The command to minimize the window
        /// </summary>
        public ICommand MinimizeCommand { get; set; }

        /// <summary>
        /// The command to maximize the window
        /// </summary>
        public ICommand MaximizeCommand { get; set; }

        /// <summary>
        /// The command to close the window
        /// </summary>
        public ICommand CloseCommand { get; set; }

        public ICommand SectionButtonCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WindowViewModel(Window window, PropertyChangeModel propertyChangeModel)
        {

            mPropertyChangeModel = propertyChangeModel;
            mPropertyChangeModel.ValueChanged += PropertyChangeModelValueChanged;
            mPropertyChangeModel.ClosePageIvent += PropertyChangeModelClosePage;
            mPropertyChangeModel.ChagePageIvent += PropertyChangeModelChagePage;

            if (CurrentSection == ApplicationUserControl.StaffRecruitment)
                ContentControlPanel = new StaffRecruitmentPanel(mPropertyChangeModel, CurrentSection);

            mWindow = window;

            // Listen out for the window resizing
            mWindow.StateChanged += (sender, e) =>
            {
                // Fire off events for all properties that are affected by a resize
                WindowResized();
            };

            // Create commands
            MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => mWindow.Close());
            SectionButtonCommand = new RelayParameterizedCommand((parameter) => ChangeSection(parameter));

            // Fix window resize issue
            var resizer = new WindowResizer(mWindow);

            // Listen out for dock changes
            resizer.WindowDockChanged += (dock) =>
            {
                // Store last position
                mDockPosition = dock;

                // Fire off resize events
                WindowResized();
            };

            OpenPageListViewModel = new OpenPageListViewModel();
        }

        private void PropertyChangeModelChagePage(object sender, UserControl CurrPage)
        {
            CurrentPage = CurrPage;
            OpenPageListViewModel.SetCurrentPageAvailable(CurrentPage);
        }

        private void PropertyChangeModelClosePage(object sender, UserControl CurrPage)
        {
            var mCurrentPage = CurrentPage;
            bool ChangeCurrPage = true;

            if(CurrPage != null)
            {
                mCurrentPage = CurrPage;
                ChangeCurrPage = false;
            }
            if (mCurrentPage.Equals(CurrentPage))
            {
                CurrentPage = OpenPageListViewModel.GetNextPage(CurrentPage);
                if(OpenPageListViewModel.ItemPages.Count != 1)
                {
                    ChangeCurrPage = false;
                }
            }

            OpenPageListViewModel.RemovePageFromList(mCurrentPage, CurrentPage);

            if (ChangeCurrPage) { CurrentPage = OpenPageListViewModel.GetNextPage(CurrentPage); }
            if(OpenPageListViewModel.ItemPages.Count == 0) { CurrentPage = null; }
            OpenPageListViewModel.SetCurrentPageAvailable(CurrentPage);
        }

        private void PropertyChangeModelValueChanged(object sender, object valueName, object newValue, object senderedValue)
        {
            if(valueName is ApplicationMenuControl)
            {
                    switch ((ApplicationMenuControl)valueName)
                    {
                        case ApplicationMenuControl.Position:
                            {
                                if (OpenPageListViewModel.NotFindPage(typeof(PositionPage)))
                                {
                                var NextPage = CurrentPage;
                                PrevPage = CurrentPage;
                                CurrentPage = new PositionPage(mPropertyChangeModel);
                                OpenPageListViewModel.AddPageToList(new OpenPageItemControl("Должности", CurrentPage, NextPage, mPropertyChangeModel));
                                OpenPageListViewModel.RebuildLinks(CurrentPage);
                            }
                                else
                                {
                                    var cPG = OpenPageListViewModel.GetOpendPage(typeof(PositionPage));
                                    if (!CurrentPage.Equals(cPG))
                                    {CurrentPage = cPG; OpenPageListViewModel.SetCurrentPageAvailable(CurrentPage); }    
                                        
                                }
                                break;
                            }
                    case ApplicationMenuControl.SelectPosition:
                        {
                            if (OpenPageListViewModel.NotFindPage(typeof(SelectPositionPage)))
                            {
                                var NextPage = CurrentPage;
                                PrevPage = CurrentPage;
                                CurrentPage = new SelectPositionPage(mPropertyChangeModel);
                                OpenPageListViewModel.AddPageToList(new OpenPageItemControl("Должности", CurrentPage, NextPage, mPropertyChangeModel));
                                OpenPageListViewModel.RebuildLinks(CurrentPage);
                            }
                            else
                            {
                                var cPG = OpenPageListViewModel.GetOpendPage(typeof(SelectPositionPage));
                                if (!CurrentPage.Equals(cPG))
                                { CurrentPage = cPG; OpenPageListViewModel.SetCurrentPageAvailable(CurrentPage); }
                            }
                            break;
                        }
                    case ApplicationMenuControl.Vacancy:
                            {
                                if (OpenPageListViewModel.NotFindPage(typeof(VacancyPage)))
                                {
                                var NextPage = CurrentPage;
                                PrevPage = CurrentPage;
                                CurrentPage = new VacancyPage(mPropertyChangeModel);
                                OpenPageListViewModel.AddPageToList(new OpenPageItemControl("Вакансии", CurrentPage, NextPage, mPropertyChangeModel));
                                OpenPageListViewModel.RebuildLinks(CurrentPage);
                            }
                                else
                                {
                                    var cPG = OpenPageListViewModel.GetOpendPage(typeof(VacancyPage));
                                    if (!CurrentPage.Equals(cPG))
                                       { CurrentPage = cPG; OpenPageListViewModel.SetCurrentPageAvailable(CurrentPage); }
                            }
                                break;
                            }
                    case ApplicationMenuControl.SelectVacancy:
                        {
                            if (OpenPageListViewModel.NotFindPage(typeof(SelectVacancyPage)))
                            {
                                var NextPage = CurrentPage;
                                PrevPage = CurrentPage;
                                CurrentPage = new SelectVacancyPage(mPropertyChangeModel);
                                OpenPageListViewModel.AddPageToList(new OpenPageItemControl("Вакансии", CurrentPage, NextPage, mPropertyChangeModel));
                                OpenPageListViewModel.RebuildLinks(CurrentPage);
                            }
                            else
                            {
                                var cPG = OpenPageListViewModel.GetOpendPage(typeof(SelectVacancyPage));
                                if (!CurrentPage.Equals(cPG))
                                { CurrentPage = cPG; OpenPageListViewModel.SetCurrentPageAvailable(CurrentPage); }
                            }
                            break;
                        }
                    case ApplicationMenuControl.Сandidate:
                            {
                                if (OpenPageListViewModel.NotFindPage(typeof(СandidatePage)))
                                {
                                var NextPage = CurrentPage;
                                PrevPage = CurrentPage;
                                CurrentPage = new СandidatePage(mPropertyChangeModel);
                                OpenPageListViewModel.AddPageToList(new OpenPageItemControl("Соискатели", CurrentPage, NextPage, mPropertyChangeModel));
                                OpenPageListViewModel.RebuildLinks(CurrentPage);
                            }
                                else
                                {
                                    var cPG = OpenPageListViewModel.GetOpendPage(typeof(СandidatePage));
                                    if (!CurrentPage.Equals(cPG))
                                         { CurrentPage = cPG; OpenPageListViewModel.SetCurrentPageAvailable(CurrentPage); }
                            }
                                break;
                            }

                    case ApplicationMenuControl.SelectCandidate:
                        {
                            if (OpenPageListViewModel.NotFindPage(typeof(SelectСandidatePage)))
                            {
                                var NextPage = CurrentPage;
                                PrevPage = CurrentPage;
                                CurrentPage = new SelectСandidatePage(mPropertyChangeModel);
                                OpenPageListViewModel.AddPageToList(new OpenPageItemControl("Соискатели", CurrentPage, NextPage, mPropertyChangeModel));
                                OpenPageListViewModel.RebuildLinks(CurrentPage);
                            }
                            else
                            {
                                var cPG = OpenPageListViewModel.GetOpendPage(typeof(SelectСandidatePage));
                                if (!CurrentPage.Equals(cPG))
                                { CurrentPage = cPG; OpenPageListViewModel.SetCurrentPageAvailable(CurrentPage); }
                            }
                            break;
                        }

                    case ApplicationMenuControl.СandidateForm:
                        {
                            if (OpenPageListViewModel.NotFindPage(typeof(СandidateFormPage)))
                            {
                                var NextPage = CurrentPage;
                                PrevPage = CurrentPage;
                                CurrentPage = new СandidateFormPage(mPropertyChangeModel);
                                OpenPageListViewModel.AddPageToList(new OpenPageItemControl("Анкеты", CurrentPage, NextPage, mPropertyChangeModel));
                                OpenPageListViewModel.RebuildLinks(CurrentPage);
                            }
                            else
                            {
                                var cPG = OpenPageListViewModel.GetOpendPage(typeof(СandidateFormPage));
                                if (!CurrentPage.Equals(cPG))
                                { CurrentPage = cPG; OpenPageListViewModel.SetCurrentPageAvailable(CurrentPage); }
                            }
                            break;
                        }

                    case ApplicationMenuControl.SelectedСandidateForm:
                        {
                            if (OpenPageListViewModel.NotFindPage(typeof(SelectСandidateFormPage)))
                            {
                                var NextPage = CurrentPage;
                                PrevPage = CurrentPage;
                                CurrentPage = new SelectСandidateFormPage(mPropertyChangeModel);
                                OpenPageListViewModel.AddPageToList(new OpenPageItemControl("Анкеты", CurrentPage, NextPage, mPropertyChangeModel));
                                OpenPageListViewModel.RebuildLinks(CurrentPage);
                            }
                            else
                            {
                                var cPG = OpenPageListViewModel.GetOpendPage(typeof(SelectСandidateFormPage));
                                if (!CurrentPage.Equals(cPG))
                                { CurrentPage = cPG; OpenPageListViewModel.SetCurrentPageAvailable(CurrentPage); }
                            }
                            break;
                        }

                    case ApplicationMenuControl.Order:
                            {
                                if (OpenPageListViewModel.NotFindPage(typeof(OrderPage)))
                                {
                                var NextPage = CurrentPage;
                                PrevPage = CurrentPage;
                                CurrentPage = new OrderPage(mPropertyChangeModel);
                                OpenPageListViewModel.AddPageToList(new OpenPageItemControl("Заявки", CurrentPage, NextPage, mPropertyChangeModel));
                                OpenPageListViewModel.RebuildLinks(CurrentPage);
                            }
                                else
                                {
                                    var cPG = OpenPageListViewModel.GetOpendPage(typeof(OrderPage));
                                    if (!CurrentPage.Equals(cPG))
                                         { CurrentPage = cPG; OpenPageListViewModel.SetCurrentPageAvailable(CurrentPage); }
                            }
                                break;
                            }
                    case ApplicationMenuControl.NewOrder:
                        {
                            if (OpenPageListViewModel.NotFindPage(typeof(NewOrderPage)))
                            {
                                var NextPage = CurrentPage;
                                PrevPage = CurrentPage;
                                CurrentPage = new NewOrderPage(mPropertyChangeModel, (ApplicationPageCommands)newValue, (Order)senderedValue);
                                OpenPageListViewModel.AddPageToList(new OpenPageItemControl("Заявка", CurrentPage, NextPage, mPropertyChangeModel));
                                OpenPageListViewModel.RebuildLinks(CurrentPage);
                            }
                            else
                            {
                                var cPG = OpenPageListViewModel.GetOpendPage(typeof(NewOrderPage));
                                if (!CurrentPage.Equals(cPG))
                                { CurrentPage = cPG; OpenPageListViewModel.SetCurrentPageAvailable(CurrentPage); }
                            }
                            break;
                        }

                    case ApplicationMenuControl.NewPosition:
                            {
                                if (OpenPageListViewModel.NotFindPage(typeof(NewPositionPage)))
                                {
                                var NextPage = CurrentPage;
                                PrevPage = CurrentPage;
                                CurrentPage = new NewPositionPage(mPropertyChangeModel, (ApplicationPageCommands)newValue, (Position)senderedValue);
                                OpenPageListViewModel.AddPageToList(new OpenPageItemControl("Должность", CurrentPage, NextPage, mPropertyChangeModel));
                                OpenPageListViewModel.RebuildLinks(CurrentPage);
                            }
                                else
                                {
                                    var cPG = OpenPageListViewModel.GetOpendPage(typeof(NewPositionPage));
                                    if (!CurrentPage.Equals(cPG))
                                         { CurrentPage = cPG; OpenPageListViewModel.SetCurrentPageAvailable(CurrentPage); }
                            }
                                break;
                            }
                        case ApplicationMenuControl.NewVacancy:
                            {
                                if (OpenPageListViewModel.NotFindPage(typeof(NewVacancyPage)))
                                {
                                var NextPage = CurrentPage;
                                PrevPage = CurrentPage;
                                CurrentPage = new NewVacancyPage(mPropertyChangeModel, (ApplicationPageCommands)newValue, (Vacancy)senderedValue);
                                OpenPageListViewModel.AddPageToList(new OpenPageItemControl("Вакансия", CurrentPage, NextPage, mPropertyChangeModel));
                                OpenPageListViewModel.RebuildLinks(CurrentPage);
                            }
                                else
                                {
                                    var cPG = OpenPageListViewModel.GetOpendPage(typeof(NewVacancyPage));
                                    if (!CurrentPage.Equals(cPG))
                                { CurrentPage = cPG; OpenPageListViewModel.SetCurrentPageAvailable(CurrentPage); }
                            }
                                break;
                            }
                        case ApplicationMenuControl.NewСandidate:
                            {
                                if (OpenPageListViewModel.NotFindPage(typeof(NewСandidatePage)))
                                {
                                var NextPage = CurrentPage;
                                PrevPage = CurrentPage;
                                CurrentPage = new NewСandidatePage(mPropertyChangeModel, (ApplicationPageCommands)newValue, (Сandidate)senderedValue);
                                OpenPageListViewModel.AddPageToList(new OpenPageItemControl("Соискатель", CurrentPage, NextPage, mPropertyChangeModel));
                                OpenPageListViewModel.RebuildLinks(CurrentPage);
                            }
                                else
                                {
                                    var cPG = OpenPageListViewModel.GetOpendPage(typeof(NewСandidatePage));
                                    if (!CurrentPage.Equals(cPG))
                                { CurrentPage = cPG; OpenPageListViewModel.SetCurrentPageAvailable(CurrentPage); }
                            }
                                break;
                            }

                        case ApplicationMenuControl.NewСandidateForm:
                            {
                                if (OpenPageListViewModel.NotFindPage(typeof(NewСandidateFormPage)))
                                {
                                    var NextPage = CurrentPage;
                                    PrevPage = CurrentPage;
                                    CurrentPage = new NewСandidateFormPage(mPropertyChangeModel, (ApplicationPageCommands)newValue, (СandidateForm)senderedValue);
                                    OpenPageListViewModel.AddPageToList(new OpenPageItemControl("Анкета", CurrentPage, NextPage, mPropertyChangeModel));
                                    OpenPageListViewModel.RebuildLinks(CurrentPage);
                            }
                                else
                                {
                                    var cPG = OpenPageListViewModel.GetOpendPage(typeof(NewСandidateFormPage));
                                    if (!CurrentPage.Equals(cPG))
                                    { CurrentPage = cPG; OpenPageListViewModel.SetCurrentPageAvailable(CurrentPage); }
                                }
                                break;
                            }
                    default:
                            break;
                    }
            }
        }

        private void ChangeSection(object parameter)
        {
            // If nothing changed 
            if (CurrentSection == (ApplicationUserControl)parameter)
                return;

            CurrentSection = (ApplicationUserControl)parameter;
            if (CurrentSection == ApplicationUserControl.Adaptation)
            { ContentControlPanel = new AdaptationPanel(mPropertyChangeModel, CurrentSection); }
            else
            { ContentControlPanel = new StaffRecruitmentPanel(mPropertyChangeModel, CurrentSection); }
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// If the window resizes to a special position (docked or maximized)
        /// this will update all required property change events to set the borders and radius values
        /// </summary>
        private void WindowResized()
        {
            // Fire off events for all properties that are affected by a resize
            OnPropertyChanged(nameof(Borderless));
            OnPropertyChanged(nameof(ResizeBorderThickness));
            OnPropertyChanged(nameof(OuterMarginSize));
            OnPropertyChanged(nameof(OuterMarginSizeThickness));
            OnPropertyChanged(nameof(WindowRadius));
            OnPropertyChanged(nameof(WindowCornerRadius));
        }

        
        #endregion
    }
}
