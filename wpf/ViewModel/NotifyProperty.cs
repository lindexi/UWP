// lindexi
// 15:58

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace lindexi.wpf.ViewModel
{
    /// <summary>
    ///     提供继承通知UI改变值
    /// </summary>
    public class NotifyProperty : INotifyPropertyChanged
    {
        public NotifyProperty()
        {
        }

        public void UpdateProper<T>(ref T properValue, T newValue, [CallerMemberName] string properName = "")
        {
            if (Equals(properValue, newValue))
            {
                return;
            }

            properValue = newValue;
            OnPropertyChanged(properName);
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
           
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}