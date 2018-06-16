using System;
using lindexi.uwp.Framework.ViewModel;

namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    /// 尝试从指定的 ViewModel 获取值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class GetValueCombinationComposite<T> : CombinationComposite
    {
        /// <summary>
        /// 尝试从指定的 ViewModel 获取值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="continueWith">在获取值之后做的</param>
        public GetValueCombinationComposite(ViewModelBase source, Action<T> continueWith) : base(source)
        {
            ContinueWith = continueWith;
        }

        private Action<T> ContinueWith { get; set; }

        public override void Run(IViewModel source, IMessage message)
        {
            if (source is IViewModelValue<T> viewmodel)
            {
                ContinueWith(viewmodel.Value);
            }
        }
    }
}