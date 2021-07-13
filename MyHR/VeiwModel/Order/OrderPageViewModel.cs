using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHR
{
    public class OrderPageViewModel : BaseViewModel
    {
        #region Private Members

        private readonly PropertyChangeModel mPropertyChangeModel;

        #endregion

        #region Constructor

        public OrderPageViewModel(PropertyChangeModel PropertyChangeModel)
        {
            mPropertyChangeModel = PropertyChangeModel;
        }

        #endregion

    }
}
