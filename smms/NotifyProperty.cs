using System;
using System.ComponentModel;
using System.Text;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace smms
{
    /// <summary>
    /// 提供继承通知UI改变值
    /// </summary>
    public class NotifyProperty : INotifyPropertyChanged
    {
        public NotifyProperty()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;


        public void UpdateProper<T>(ref T properValue, T newValue, [System.Runtime.CompilerServices.CallerMemberName] string properName = "")
        {
            if (object.Equals(properValue, newValue))
                return;

            properValue = newValue;
            OnPropertyChanged(properName);
        }
        public async void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    handler?.Invoke(this, new PropertyChangedEventArgs(name));
                });
        }
    }
}
