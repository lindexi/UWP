using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using lindexi.MVVM.Framework.Annotations;
using lindexi.MVVM.Framework.ViewModel;

namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    /// 提供支持跳转类
    /// </summary>
    [PublicAPI]
    public abstract class NavigateViewModel : ViewModelMessage, IKeyNavigato, INavigateable
    {
        /// <summary>
        /// 提供 ViewModel 之间跳转
        /// </summary>
        protected ViewModelNavigate ViewModelNavigate { get; set; }

        /// <inheritdoc />
        public override void ReceiveMessage(object sender, IMessage message)
        {
            ViewModelBase viewModel = this;
            var composite = message as ICombinationComposite;
            composite?.Run(viewModel, message);

            var run = ViewModel.Composite.Run(viewModel, message, Composite);

            if (run)
            {
                return;
            }

            // 所有在这个 ViewModel 的 ViewModel 判断是否需要
            // 解决 A B 两个通信
            foreach (var temp in ViewModelPage.
                Where(/*如果 ViewModel 没有使用，就不收消息*/temp => temp.ViewModel.IsLoaded)
                .Select(temp=>temp.ViewModel.GetViewModel()))
            {
                ViewModel.Composite.Run(temp, message, Composite);
            }
        }

        /// <summary>
        /// 集合 ViewModel 和 页面 用来跳转
        /// </summary>
        public List<ViewModelPage> ViewModelPage { get; set; }

        public IViewModel this[string str]
        {
            get { return ViewModelPage.FirstOrDefault(temp => temp.Key == str)?.ViewModel.GetViewModel(); }
        }

        /// <inheritdoc />
        public override void OnNavigatedFrom(object sender, object obj)
        {
        }

        /// <inheritdoc />
        public override void OnNavigatedTo(object sender, object obj)
        {
            if (obj is INavigateFrame content)
            {
                Content = content;
            }
            else if (obj is ViewModelNavigate viewModelNavigate)
            {
                ViewModelNavigate = viewModelNavigate;
            }

            if (ViewModelPage == null)
            {
                ViewModelPage = new List<ViewModelPage>();
            }
        }

        /// <inheritdoc />
        public INavigateFrame Content
        {
            get => ViewModelNavigate?.Frame;
            set
            {
                if (ViewModelNavigate == null || !value.Equals(ViewModelNavigate.Frame))
                {
                    ViewModelNavigate = new ViewModelNavigate(value);
                }
            }
        }

        /// <inheritdoc />
        public void Navigate(Type viewModel, object parameter = null, INavigateFrame content = null)
        {
            Navigate(viewModel.Name, parameter, content);
        }

        /// <inheritdoc />
        public event EventHandler<ViewModelPage> Navigating;

        /// <inheritdoc />
        public event EventHandler<ViewModelPage> Navigated;

        /// <inheritdoc />
        public void Navigate(string key, object parameter, INavigateFrame content = null)
        {
            var viewModel = ViewModelPage.FirstOrDefault(temp => temp.Equals(key));
            var viewModelNavigate = ViewModelNavigate;
            if (viewModelNavigate == null)
            {
                throw new ArgumentException("跳转时，提供跳转的类为空，需要先设置 Content 才可以跳转")
                {
                    Data = {{"Method", " Navigate(string key, object parameter, INavigateFrame content = null)"}}
                };
            }

            if (viewModel == null)
            {
                throw new ArgumentException("找不到要跳转")
                {
                    Data = {{"Method", " Navigate(string key, object parameter, INavigateFrame content = null)"}}
                };
            }

            var frame = viewModelNavigate.Frame;

            if (content != null)
            {
                frame = content;
            }

            if (!ReferenceEquals(viewModelNavigate.Frame, frame))
            {
                viewModelNavigate = new ViewModelNavigate(frame);
            }

            Navigating?.Invoke(this, viewModel);
            viewModelNavigate.Navigate(this, viewModel, parameter);
            Navigated?.Invoke(this, viewModel);
        }

        /// <summary>
        /// 获取所有的处理
        /// </summary>
        protected void AllAssemblyComposite(Assembly assembly)
        {
            foreach (var temp in assembly.GetTypes().Where(IsCompsite))
            {
                try
                {
                    Composite.Add(temp.Assembly.CreateInstance(temp.FullName) as Composite);
                }
                catch (NullReferenceException)
                {
                }
                catch (ArgumentException)
                {
                }
            }

            bool IsCompsite(Type temp)
            {
                return temp.IsSubclassOf(typeof(Composite)) &&
                       !temp.IsAssignableFrom(typeof(ICombinationComposite))
                       && temp != typeof(CombinationComposite)
                       && !temp.IsSubclassOf(typeof(CombinationComposite));
            }
        }
    }

#if wpf
/// <summary>
///     包含有页面的ViewModel
/// </summary>
    public abstract class NavigateViewModel : ViewModelMessage, IKeyNavigato, INavigateable
    {
        public ViewModelBase this[string str]
        {
            get { return ViewModel.FirstOrDefault(temp => temp.Key == str)?.ViewModel; }
        }

        /// <summary>
        ///     包含的页面集合
        /// </summary>
        public List<ViewModelPage> ViewModel { set; get; } = new List<ViewModelPage>();

        /// <summary>
        ///     正在跳转事件
        /// </summary>
        public event EventHandler<ViewModelPage> Navigating;

        /// <summary>
        ///     跳转完成
        /// </summary>
        public event EventHandler<ViewModelPage> Navigated;


        /// <summary>
        ///     提供跳转的控件
        /// </summary>
        public Frame Content { set; get; }

        /// <summary>
        ///     跳转到页面
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
        ///     跳转到页面
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="paramter"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async void Navigate(Type viewModel, object paramter, Frame content = null)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            ViewModelPage view = ViewModel.Find(temp => temp.Equals(viewModel));
            if (view == null)
            {
                throw new ArgumentException("Cant find the ViewModel, please sure that you have add it to ViewModel");
            }
            await Navigate(paramter, view, content);
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
            IComposite composite = message as ICombinationComposite;
            if (composite != null)
            {
                composite.Run(viewModel, message);
            }
            else
            {
                composite = Composite.FirstOrDefault(temp => temp.Message == message.GetType());
                if (composite != null)
                {
                    composite.Run(viewModel, message);
                }
                else
                {
                    if (viewModel == this)
                    {
                        return;
                    }
                    if (viewModel is IReceiveMessage)
                    {
                        ((IReceiveMessage) viewModel).ReceiveMessage(sender, message);
                    }
                }
            }
        }


        /// <summary>
        ///     跳转到页面
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="paramter"></param>
        /// <param name="content"></param>
        public async void Navigate(ViewModelPage viewModel, object paramter, Frame content = null)
        {
            await Navigate(paramter, viewModel, content);
        }

        //当前ViewModel
        private (ViewModelBase viewModel, Frame frame)? _viewModel;

#if wpf
        /// <summary>
        ///     自动组合
        /// </summary>
        protected void CombineViewModel()
        {
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

        }
#elif WINDOWS_UWP
        
        /// <summary>
        ///     自动组合
        /// </summary>
        protected void CombineViewModel(Assembly assembly)
        {
            if (ViewModel == null)
            {
                ViewModel = new List<ViewModelPage>();
            }
            ViewModel.Clear();

            foreach (var temp in assembly.DefinedTypes.Where(temp => typeof(IViewModel).IsAssignableFrom(temp.AsType())))
            {
                ViewModel.Add(new ViewModelPage(temp.AsType()));
            }

            foreach (var temp in assembly.DefinedTypes.Where(temp => temp.IsSubclassOf(typeof(Page))))
            {
                var p = temp.GetCustomAttribute<ViewModelAttribute>();
                var viewmodel = ViewModel.FirstOrDefault(t => t.Equals(p?.ViewModel));
                if (viewmodel != null)
                {
                    viewmodel.Page = temp.AsType();
                }
            }

            //清理
            if (!NoGui.NOGUI)
            {
                ViewModel.RemoveAll(temp => temp.Page == null);
            }
        }
#endif

#if wpf
        /// <summary>
        ///     获取所有的处理
        /// </summary>
        protected void AllAssemblyComposite()
        {

            Assembly assembly = Assembly.GetCallingAssembly();
            foreach (
                var temp in
                assembly.GetTypes()
                    .Where(
                        temp =>
                            temp.IsSubclassOf(typeof(Composite)) &&
                            !temp.IsAssignableFrom(typeof(ICombinationComposite)) &&
                            !temp.IsSubclassOf(typeof(CombinationComposite)) &&
                            temp != typeof(CombinationComposite)))
            {
                Composite.Add(temp.Assembly.CreateInstance(temp.FullName) as Composite);
            }
        }
#elif WINDOWS_UWP
        /// <summary>
        ///     获取所有的处理
        /// </summary>
        protected void AllAssemblyComposite(Assembly assembly)
        {

            foreach (
                var temp in
                assembly.DefinedTypes
                    .Where(
                        temp => 
                            temp.IsSubclassOf(typeof(Composite)) &&
                            !typeof(ICombinationComposite).IsAssignableFrom(temp.AsType()) &&
                            !temp.IsSubclassOf(typeof(CombinationComposite)) && !temp.ContainsGenericParameters &&
                            temp.AsType() != typeof(CombinationComposite)))
            {
               
                Composite.Add(temp.AsType().GetConstructor(Type.EmptyTypes).Invoke(null) as Composite);
            }
        }
#endif

        /// <summary>
        ///     跳转到页面
        /// 只用于测试使用
        /// </summary>
        public void Navigate(ViewModelBase viewModel, object paramter, ViewModelBase lastViewModel = null)
        {
            if (!NoGui.NOGUI)
            {
                throw new Exception("不能在有界面调用这个跳转");
            }
            var view = ViewModel.FirstOrDefault(temp => temp.ViewModel == viewModel);
            Navigating?.Invoke(this, view);
            lastViewModel?.NavigatedFrom(this, null);
            viewModel.NavigatedTo(this, paramter);
            Navigated?.Invoke(this, view);
        }

        /// <summary>
        ///     跳转到页面
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
            if (_viewModel != null && content != null && content == _viewModel?.frame)
            {
                _viewModel?.viewModel?.NavigatedFrom(this, null);
            }
            await view.Navigate(content, this, paramter);
            _viewModel = (view.ViewModel, content);
            Navigated?.Invoke(this, view);
        }
    }
#endif
}