using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


#if WINDOWS_UWP&&false
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
#elif wpf
using System.Threading;
using System.Windows;
using System.Windows.Threading;
#endif


namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    ///     提供继承通知UI改变值
    /// </summary>
    public class NotifyProperty : INotifyPropertyChanged
    {
        public NotifyProperty()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void UpdateProper<T>(ref T properValue, T newValue, [CallerMemberName] string properName = "")
        {
            if (Equals(properValue, newValue))
            {
                return;
            }

            properValue = newValue;
            OnPropertyChanged(properName);
        }

        public async void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
#if WINDOWS_UWP&&false
               await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () => { handler?.Invoke(this, new PropertyChangedEventArgs(name)); });
#elif wpf
            SynchronizationContext.SetSynchronizationContext(new
   DispatcherSynchronizationContext(Application.Current.Dispatcher));
            SynchronizationContext.Current.Send(obj =>
            {
                handler?.Invoke(this, new PropertyChangedEventArgs(name));
            }, null);
#endif

        }
    }
}