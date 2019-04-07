using System;
using System.Collections.Generic;
using System.Text;

namespace lindexi.MVVM.Framework.ViewModel
{
  public  class ViewModelCommand
    {
        public ViewModelCommand(Action<object> action)
        {
            this.ViewModelAction = action;
        }

        private Action<object> _viewModelAction;

        public Action<object> ViewModelAction
        {
            get { return _viewModelAction; }
            set { _viewModelAction = value; }
        }

    }
}
