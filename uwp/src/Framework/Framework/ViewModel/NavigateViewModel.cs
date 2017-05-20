using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

#if WINDOWS_UWP
#elif wpf
#endif

#if WINDOWS_UWP
using Windows.UI.Xaml.Controls;
#elif wpf
using System.Windows.Controls;
#endif

namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    /// 包含有页面的ViewModel
    /// </summary>
    public abstract class NavigateViewModel : ViewModelMessage, IKeyNavigato, INavigateable
    {
        //当前ViewModel
        private ViewModelBase _viewModel;

        /// <summary>
        /// 正在跳转事件
        /// </summary>
        public event EventHandler<ViewModelPage> Navigating;

        /// <summary>
        /// 跳转完成
        /// </summary>
        public event EventHandler<ViewModelPage> Navigated;

        /// <summary>
        /// 自动组合
        /// </summary>
        protected void CombineViewModel()
        {
#if wpf
            Assembly assembly = Assembly.GetCallingAssembly();
            if (ViewModel == null)
            {
                ViewModel = new List<ViewModelPage>();
            }
            ViewModel = new List<ViewModelPage>();
            foreach (var temp in assembly.GetTypes().Where(temp => temp.IsSubclassOf(typeof(ViewModelBase))))
            {
                ViewModel.Add(new ViewModelPage(temp));
            }

            foreach (var temp in assembly.GetTypes().Where(temp => temp.IsSubclassOf(typeof(Page))))
            {
                var viewmodel = temp.GetCustomAttribute<ViewModelAttribute>();
                if (viewmodel != null)
                {
                    var view = ViewModel.FirstOrDefault(t => t.Equals(viewmodel.ViewModel));
                    if (view != null)
                    {
                        view.Page = temp;
                    }
                }
            }
#endif
        }
        /// <summary>
        /// 获取所有的处理
        /// </summary>
        protected void AllAssemblyComposite()
        {
#if wpf
            Assembly assembly = Assembly.GetCallingAssembly();
            foreach (var temp in assembly.GetTypes().Where(temp => temp.IsSubclassOf(typeof(Composite)) && !temp.IsSubclassOf(typeof(CombinationComposite)) &&
                    temp != typeof(CombinationComposite)))
            {
                Composite.Add(temp.Assembly.CreateInstance(temp.FullName) as Composite);
            }
#endif
        }

        public ViewModelBase this[string str]
        {
            get
            {
                return ViewModel.FirstOrDefault(temp => temp.Key == str)?.ViewModel;
            }
        }

        /// <summary>
        /// 包含的页面集合
        /// </summary>
        public List<ViewModelPage> ViewModel
        {
            set; get;
        }


        /// <summary>
        /// 提供跳转的控件
        /// </summary>
        public Frame Content
        {
            set; get;
        }

        /// <summary>
        /// 跳转到页面
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parameter"></param>
        public void Navigate(string key, object parameter = null)
        {
            var viewModel = ViewModel.FirstOrDefault(temp => temp.Key == key);
            if (viewModel != null)
            {
                Navigate(viewModel, parameter);
            }
            else
            {
                throw new ArgumentException();
            }
        }


        /// <summary>
        /// 跳转到页面
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="paramter"></param>
        /// <param name="content"></param>
        public async void Navigate(ViewModelPage viewModel, object paramter, Frame content = null)
        {
            await Navigate(paramter, viewModel, content);
        }

        /// <summary>
        /// 跳转到页面
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="paramter"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async void Navigate(Type viewModel, object paramter, Frame content = null)
        {
            ViewModelPage view = ViewModel.Find(temp => temp.Equals(viewModel));
            await Navigate(paramter, view, content);
        }

        /// <summary>
        /// 跳转到页面
        /// </summary>
        /// <param name="paramter"></param>
        /// <param name="view"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private async Task Navigate(object paramter, ViewModelPage view, Frame content)
        {
            Navigating?.Invoke(this, view);
            if (content == null)
            {
                content = Content;
            }
            _viewModel?.NavigatedFrom(this, null);
            await view.Navigate(content, this, paramter);
            _viewModel = view.ViewModel;
            Navigated?.Invoke(this, view);
        }


        /// <inheritdoc />
        public override void ReceiveMessage(object sender, IMessage message)
        {
            //var viewModel = string.IsNullOrEmpty(message.Goal) ? this : ViewModel.FirstOrDefault(temp => temp.Key == message.Goal)?.ViewModel;
            ViewModelBase viewModel = this;
            if (!message.Predicate(new ViewModelPage(this)))
            {
                viewModel = ViewModel.FirstOrDefault(temp => message.Predicate(temp))?.ViewModel;
            }
            if (viewModel == null)
            {
                return;
            }
            var composite = message as ICombinationComposite;
            composite?.Run(viewModel, message);
            Composite.FirstOrDefault(temp => temp.Message == message.GetType())?.Run(viewModel, message);
        }


    }
}