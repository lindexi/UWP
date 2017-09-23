using System;

namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    /// 处理消息
    /// </summary>
    public class Composite: IComposite
    {
        /// <summary>
        /// 处理什么消息
        /// </summary>
        public Type Message
        {
            get; set;
        }

        public string Key
        {
            get; set;
        }

        public virtual void Run(IViewModel source, IMessage message)
        {
            var viewModel = source as ViewModelBase;
            if (viewModel != null)
            {
                Run(viewModel, message);
            }
        }

        /// <summary>
        /// 是否已经使用函数
        /// </summary>
        private bool _run;

        public virtual void Run(ViewModelBase source, IMessage message)
        {
            if (_run)
            {
                return;
            }
            _run = true;
            Run((IViewModel)source, message);
            _run = false;
        }
    }
}