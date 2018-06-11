using System;

namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    /// 提供跳转消息，自动找到<see cref="NavigateViewModel"/>让他跳转
    /// </summary>
    public class NavigateCombinationComposite : CombinationComposite<NavigateViewModel>
    {
        /// <summary>
        /// 提供跳转消息，自动找到<see cref="NavigateViewModel"/>让他跳转
        /// </summary>
        public NavigateCombinationComposite(ViewModelBase source, Type view, object paramter = null) : base(source)
        {
            _run = viewModel => { viewModel.Navigate(view, paramter); };
        }

        /// <summary>
        /// 提供跳转消息，自动找到<see cref="NavigateViewModel"/>让他跳转
        /// </summary>
        public NavigateCombinationComposite(ViewModelBase source, string key, object paramter = null) : base(source)
        {
            _run = viewModel => { viewModel.Navigate(key, paramter); };
        }
    }

    /// <summary>
    /// 跳转的消息
    /// </summary>
    public class NavigateMessage : Message
    {
        /// <inheritdoc />
        public NavigateMessage(ViewModelBase source, string key, object parameter = null) : base(source)
        {
            Parameter = parameter;
            Goal = new PredicateInheritViewModel(typeof(IKeyNavigato));
            Key = key;
        }

        /// <summary>
        /// 跳转参数
        /// </summary>
        public object Parameter { get; }
    }

    /// <summary>
    /// 跳转消息
    /// </summary>
    public class NavigateComposite : Composite
    {
        /// <inheritdoc />
        public NavigateComposite() : base(typeof(NavigateMessage))
        {
        }

        /// <inheritdoc />
        public override void Run(IViewModel source, IMessage e)
        {
            if (source is IKeyNavigato naviagateViewModel
                && e is NavigateMessage message)
            {
                // 只有可以跳转的 ViewModel 才可以使用
                naviagateViewModel.Navigate(message.Key, message.Parameter);
            }
        }
    }
}