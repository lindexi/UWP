using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace lindexi.MVVM.Framework.ViewModel
{
   public abstract  class MapEventToCommandBase<TEventArgs>:TriggerAction<DependencyObject> where TEventArgs:EventArgs
    {

        protected override void Invoke(object parameter)
        {
            if (base.AssociatedObject!=null)
            {
                ICommand command = GetCommand();
                EventInfomation<TEventArgs> eventInfomation = new EventInfomation<TEventArgs>()
                {
                    Sender=base.AssociatedObject,
                    EventArgs=parameter as TEventArgs,
                    CommandParameter=GetValue(CommandParameterProperty)
                };
                if (command!=null&&command.CanExecute(eventInfomation))
                {
                    command.Execute(eventInfomation);
                }
            }
        }


        private ICommand GetCommand()
        {
            ICommand command = null;
            if (this.Command!=null)
            {
                command = this.Command;
            }
            else if(base.AssociatedObject!=null)
            {
                Type type = base.AssociatedObject.GetType();
                PropertyInfo[] propertyInfoArray = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                for (int i = 0; i < propertyInfoArray.Length; i++)
                {
                    PropertyInfo propertyInfo = propertyInfoArray[i];
                    if (typeof(ICommand).IsAssignableFrom(propertyInfo.PropertyType)&&string.Equals(propertyInfo.Name,this.CommandName,StringComparison.Ordinal))
                    {
                        command = (ICommand)propertyInfo.GetValue(base.AssociatedObject, null);
                    }
                }
            }
            return command;
        }




        private string _commandName;

        public string CommandName
        {
            get
            {
                base.ReadPreamble();
                return this._commandName;
            }
            set
            {
                if (this._commandName != value)
                {
                    base.WritePreamble();
                    this._commandName = value;
                    base.WritePostscript();
                }
                this._commandName = value;
            }
        }


        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(MapEventToCommandBase<TEventArgs>), null);


        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(MapEventToCommandBase<TEventArgs>), null);




    }
}
