using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using lindexi.uwp.Framework.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;

namespace lindexi.uwp.Progress.ViewModel
{
    public class ViewModel : NavigateViewModel
    {
        public override void OnNavigatedFrom(object sender, object obj)
        {

        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            AllAssemblyComposite();
            CombineViewModel();
            if (ViewModel == null)
            {
                ViewModel = new List<ViewModelPage>();
            }
            Assembly applacationAssembly = Application.Current.GetType().GetTypeInfo().Assembly;
            foreach (TypeInfo temp in applacationAssembly.DefinedTypes.Where(temp => temp.IsSubclassOf(typeof(ViewModelBase))))
            {
                ViewModel.Add(new ViewModelPage(temp.AsType()));
            }

            foreach (var temp in applacationAssembly.DefinedTypes.Where(temp => temp.IsSubclassOf(typeof(Page))))
            {
                var viewmodel = temp.GetCustomAttribute<ViewModelAttribute>();
                if (viewmodel != null)
                {
                    var view = ViewModel.FirstOrDefault(t => t.Equals(viewmodel.ViewModel));
                    if (view != null)
                    {
                        view.Page = temp.AsType();
                    }
                }
            }



            foreach (var temp in applacationAssembly.DefinedTypes.Where(temp => temp.IsSubclassOf(typeof(Composite)) &&
            !temp.IsAssignableFrom(typeof(ICombinationComposite).GetTypeInfo())))
            {
                Composite.Add(temp.AsType().GetConstructor(Type.EmptyTypes).Invoke(null) as Composite);
            }


            EposeModel = (EposeModel) this[typeof(EposeModel).Name];
            ViewModel.Remove(ViewModel.First(temp => temp.Equals(typeof(ViewModel))));
            Navigate(typeof(EposeModel), ViewModel, EposeFrame);
        }

        public Frame EposeFrame { get; set; }

        public EposeModel EposeModel { set; get; }
    }
}
