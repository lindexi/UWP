using System.ComponentModel;
using System.Runtime.CompilerServices;
using lindexi.MVVM.Framework.Annotations;

#if WINDOWS_UWP&&false
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
#elif wpf
using System.Threading;
using System.Windows;
using System.Windows.Threading;
#endif


namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    ///     提供继承通知UI改变值
    /// </summary>
    public abstract class NotifyProperty : INotifyPropertyChanged
    {
        /// <summary>
        /// 更新值，如果两个值相同就不更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="properValue"></param>
        /// <param name="newValue"></param>
        /// <param name="properName"></param>
        protected void UpdateProper<T>(ref T properValue, T newValue, [CallerMemberName] string properName = "")
        {
            if (Equals(properValue, newValue))
            {
                return;
            }

            properValue = newValue;
            OnPropertyChanged(properName);
        }

//        public async void OnPropertyChanged([CallerMemberName] string name = "")
//        {
//            PropertyChangedEventHandler handler = PropertyChanged;
//#if WINDOWS_UWP&&false
//               await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
//                () => { handler?.Invoke(this, new PropertyChangedEventArgs(name)); });
//#elif wpf
//            SynchronizationContext.SetSynchronizationContext(new
//   DispatcherSynchronizationContext(Application.Current.Dispatcher));
//            SynchronizationContext.Current.Send(obj =>
//            {
//                handler?.Invoke(this, new PropertyChangedEventArgs(name));
//            }, null);
//#endif

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 通知属性修改
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}