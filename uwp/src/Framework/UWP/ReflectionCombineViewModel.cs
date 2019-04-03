using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using lindexi.MVVM.Framework.Annotations;
using lindexi.MVVM.Framework.ViewModel;

namespace lindexi.uwp.Framework
{
    /// <summary>
    /// 反射组合 ViewModel 和页面
    /// </summary>
    public static class ReflectionCombineViewModel
    {
        /// <summary>
        /// 组合 ViewModel 和页面，调用这个方法会读取输入程序集的所有 ViewModel 找到标记对应  <see cref="ViewModelAttribute"/> 的页面
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="assembly"></param>
        public static void CombineViewModel([NotNull] this NavigateViewModel viewModel, [NotNull] Assembly assembly)
        {
            if (ReferenceEquals(viewModel, null)) throw new ArgumentNullException(nameof(viewModel));

            if (ReferenceEquals(assembly, null)) throw new ArgumentNullException(nameof(assembly));

            if (viewModel.ViewModelPageCollection == null)
            {
                viewModel.ViewModelPageCollection = new List<ViewModelPage>();
            }

            viewModel.ViewModelPageCollection.Clear();

            foreach (var temp in assembly.DefinedTypes.Where(temp => typeof(IViewModel).IsAssignableFrom(temp.AsType()))
            )
            {
                viewModel.ViewModelPageCollection.Add(new ViewModelPage(new NavigatableViewModel(temp)));
            }

            foreach (var temp in assembly.DefinedTypes.Where(temp =>
                temp.IsSubclassOf(typeof(Windows.UI.Xaml.Controls.Page))))
            {
                var p = temp.GetCustomAttribute<ViewModelAttribute>();
                if (p != null)
                {
                    var viewmodel =
                        viewModel.ViewModelPageCollection.FirstOrDefault(t => t.ViewModel.Name.Equals(p.ViewModel.Name));

                    if (viewmodel != null)
                    {
                        viewModel.ViewModelPageCollection.Remove(viewmodel);
                        viewModel.ViewModelPageCollection.Add(new ViewModelPage(viewmodel.ViewModel, new NavigatablePage(temp)));
                    }
                }
            }
        }
    }
}