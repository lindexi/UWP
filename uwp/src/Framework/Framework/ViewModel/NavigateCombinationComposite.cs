using System;

namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    /// 提供跳转消息，自动找到<see cref="NavigateViewModel"/>让他跳转
    /// </summary>
    public class NavigateCombinationComposite : CombinationComposite<NavigateViewModel>
    {
        public NavigateCombinationComposite(ViewModelBase source, Type view, object paramter = null) : base(source)
        {
            _run = viewModel =>
            {
                viewModel.Navigate(view, paramter);
            };
        }
    }
}