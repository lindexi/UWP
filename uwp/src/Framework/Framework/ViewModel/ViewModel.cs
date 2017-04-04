using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace lindexi.uwp.Framework.ViewModel
{
    [ViewModel]
    public class AModel : ViewModelBase
    {
        public string Name { get; set; } = "csdn";

        public override void OnNavigatedFrom(object sender, object obj)
        {
            return;
            throw new NotImplementedException();
        }

        public override void OnNavigatedTo(object sender, object obj)
        {
        }
    }

    [ViewModel]
    public class LinModel : ViewModelBase
    {
        public LinModel()
        {
        }

        public override void OnNavigatedFrom(object sender, object obj)
        {
        }

        public override void OnNavigatedTo(object sender, object obj)
        {
        }
    }

    public class ViewModel : NavigateViewModel
    {
        private Visibility _frameVisibility;

        public ViewModel()
        {
            View = this;
        }

        public AModel AModel { set; get; }

        public LinModel LinModel { set; get; }

        //public CodeStorageModel CodeStorageModel
        //{
        //    set;
        //    get;
        //}

        public Visibility FrameVisibility
        {
            set
            {
                _frameVisibility = value;
                OnPropertyChanged();
            }
            get { return _frameVisibility; }
        }

        public ViewModel View { set; get; }

        public void NavigateToList()
        {
            //Navigate(typeof(CodeStorageModel), null);
        }

        public void NavigateToInfo()
        {
        }

        public void NavigateToAccount()
        {
        }

        public override void OnNavigatedFrom(object sender, object obj)
        {
        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            FrameVisibility = Visibility.Collapsed;
            Content = (Frame) obj;
#if NOGUI
#else
            //Content.Navigate(typeof(SplashPage));
#endif
            if (ViewModel == null)
            {
                ViewModel = new List<ViewModelPage>();
                //加载所有ViewModel
                var applacationAssembly = Application.Current.GetType().GetTypeInfo().Assembly;

                //CodeStorageModel = new CodeStorageModel();
                //ViewModel.Add(new ViewModelPage(CodeStorageModel, typeof(MasterDetailPage))
                //);
                foreach (
                    var temp in applacationAssembly.DefinedTypes.Where(temp => temp.IsSubclassOf(typeof(ViewModelBase)))
                )
                {
                    ViewModel.Add(new ViewModelPage(temp.AsType()));
                }

                foreach (var temp in applacationAssembly.DefinedTypes.Where(temp => temp.IsSubclassOf(typeof(Page))))
                {
                    //获取特性，特性有包含ViewModel
                    var p = temp.GetCustomAttribute<ViewModelAttribute>();

                    var viewmodel = ViewModel.FirstOrDefault(t => t.Equals(p?.ViewModel));
                    if (viewmodel != null)
                    {
                        viewmodel.Page = temp.AsType();
                    }
                }
            }

            FrameVisibility = Visibility.Visible;
            Navigate(typeof(AModel), null);
        }
    }
}