using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyHR
{
    public class BaseControl<VM> : UserControl
               where VM : BaseViewModel, new()
        {
            #region Private Members

            /// <summary>
            /// The view model associated with this
            /// </summary>
            private VM mViewModel;

            #endregion

            #region Public Properties

            /// <summary>
            /// Te view model associated with this
            /// </summary>
            public VM ViewModel
            {
                get
                {
                    return mViewModel;
                }
                set
                {
                    // If nothing has changed, return
                    if (mViewModel == value)
                        return;
                    // Update the value
                    mViewModel = value;

                    // Set the data context for this control
                    DataContext = mViewModel;
                }
            }

            #endregion

            #region Constructor

            /// <summary>
            /// Default constructor
            /// </summary>
            public BaseControl()
            {
                ViewModel = new VM();
            }

            #endregion

        }

   
}
