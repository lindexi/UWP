using System;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace lindexi.uwp.Framework.ViewModel
{
    public class ViewModelPage : IEquatable<Type>
    {
        private Type _viewModel;

        public ViewModelPage()
        {
            //if (ViewModel == null)
            //{
            //    //ViewModel=View.GetConstructor(null)
            //}
        }

        public ViewModelPage(Type viewModel)
        {
            _viewModel = viewModel;
            Key = _viewModel.Name;
        }

        public ViewModelPage(Type viewModel, Type page)
        {
            _viewModel = viewModel;
            Page = page;
            Key = _viewModel.Name;
        }

        public ViewModelPage(ViewModelBase viewModel, Type page)
        {
            ViewModel = viewModel;
            Page = page;
            Key = viewModel.GetType().Name;
            _viewModel = viewModel.GetType();
        }

        public string Key { set; get; }


        public ViewModelBase ViewModel { set; get; }

        public Type Page { set; get; }

        public bool Equals(Type other)
        {
            return _viewModel == other;
        }

        public async Task Navigate(Frame content, object paramter)
        {
            if (ViewModel == null)
            {
                ViewModel = (ViewModelBase) _viewModel.GetConstructor(Type.EmptyTypes).Invoke(null);
            }
            ViewModel.OnNavigatedTo(this, paramter);
#if NOGUI
            return;
#endif
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () => { content.Navigate(Page, ViewModel); });
        }

        protected bool Equals(ViewModelPage other)
        {
            return _viewModel == other._viewModel;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ViewModelPage) obj);
        }

        public override int GetHashCode()
        {
            return _viewModel?.GetHashCode() ?? 0;
        }
    }
}