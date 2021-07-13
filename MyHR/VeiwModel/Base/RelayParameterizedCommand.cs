using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyHR
{
    /// <summary>
    /// Basic commands that run Action
    /// </summary>
    class RelayParameterizedCommand : ICommand
    {
        #region Private Members

        /// <summary>
        /// The action to run
        /// </summary>
        private Action<object> mAction;

        #endregion

        #region Public ivents

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        #endregion

        #region Constructor

        public RelayParameterizedCommand(Action<object> action)
        {
            mAction = action;
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Передает(Relay) команду, которая может всегда выполняться(execute)
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            mAction(parameter);
        }

        #endregion
    }

}
