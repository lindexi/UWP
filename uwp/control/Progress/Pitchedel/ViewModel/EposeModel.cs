using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using lindexi.uwp.Framework.ViewModel;
using lindexi.uwp.Progress.Model;
using Windows.UI.Xaml.Controls;

namespace lindexi.uwp.Progress.ViewModel
{
    public class EposeModel : ViewModelMessage
    {
        public override void OnNavigatedFrom(object sender, object obj)
        {

        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            ViewModel = new ObservableCollection<AdaptModel>()
            {
                new AdaptModel("Marquez",typeof(Marquez))
            };

            var viewmodel = obj as List<ViewModelPage>;
            if (viewmodel == null)
            {
                return;
            }

            for (int i = 0; i < ViewModel.Count; i++)
            {
                if (viewmodel.All(temp => !temp.Equals(ViewModel[i].ViewModel)))
                {
                    ViewModel.RemoveAt(i);
                    i--;
                }
            }
        }

        public ObservableCollection<AdaptModel> ViewModel { get; set; }

        public void Navigate(object sender, ItemClickEventArgs e)
        {
            var temp = (AdaptModel) e.ClickedItem;
            Send(new NavigateMessage(this, temp.ViewModel.Name));
        }
    }

    class NavigateMessage : Message
    {
        public NavigateMessage(ViewModelBase source) : base(source)
        {
        }

        public NavigateMessage(ViewModelBase source, string viewModel) : base(source)
        {
            ViewModel = viewModel;
        }

        /// <summary>
        ///     跳转到的页面
        /// </summary>
        public string ViewModel { get; set; }
    }

    class NagivateComposite : Composite
    {
        public NagivateComposite()
        {
            Message = typeof(NavigateMessage);
        }

        public override void Run(ViewModelBase source, IMessage e)
        {
            var viewModel = source as IKeyNavigato;
            var message = e as NavigateMessage;
            if (message != null)
            {
                viewModel?.Navigate(message.ViewModel, null);
            }
        }
    }
}
