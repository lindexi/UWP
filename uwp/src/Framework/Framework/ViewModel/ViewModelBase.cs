using System;
using System.Collections.Generic;
using lindexi.MVVM.Framework.Annotations;
using lindexi.uwp.Framework.ViewModel;

namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    /// 提供基础的 ViewModel 包含跳转
    /// </summary>
    [PublicAPI]
    public abstract class ViewModelBase : NotifyProperty, INavigable, ILoadableMode, IViewModel
    {
        /// <summary>
        ///     表示当前ViewModel是否处于进入状态
        ///     用于命令判断是否可用
        /// </summary>
        public bool IsLoaded { get; set; }

        /// <inheritdoc />
        public virtual void NavigatedFrom(object sender, object obj)
        {
            OnNavigatedFrom(sender, obj);
            IsLoaded = false;
        }

        /// <inheritdoc />
        public virtual void NavigatedTo(object sender, object obj)
        {
            IsLoaded = true;
            OnNavigatedTo(sender, obj);
        }

        /// <summary>
        ///     从其他页面跳转出
        ///     需要释放页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="obj"></param>
        public abstract void OnNavigatedFrom(object sender, object obj);

        /// <summary>
        ///     从其他页面跳转到
        ///     在这里初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="obj"></param>
        public abstract void OnNavigatedTo(object sender, object obj);
    }

    /// <summary>
    ///     可以接收发送消息的页面
    /// </summary>
    public abstract class ViewModelMessage : ViewModelBase, IAdapterMessage
    {
        /// <summary>
        ///     发送消息
        /// </summary>
        EventHandler<IMessage> ISendMessage.Send { set; get; }

        /// <summary>
        ///     接收信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public virtual void ReceiveMessage(object sender, IMessage message)
        {
            ViewModelBase viewModel = this;
            var composite = message as ICombinationComposite;
            composite?.Run(viewModel, message);

            ViewModel.Composite.Run(viewModel, message, Composite);
        }

        /// <summary>
        ///     命令合成
        ///     全部调用发送信息的处理在<see cref="Composite" />
        /// </summary>
        public List<Composite> Composite { set; get; } = new List<Composite>();

        /// <inheritdoc />
        public sealed override void NavigatedFrom(object sender, object obj)
        {
            base.NavigatedFrom(sender, obj);
            ((ISendMessage) this).Send = null;
        }

        /// <inheritdoc />
        public sealed override void NavigatedTo(object sender, object obj)
        {
            if (sender is IReceiveMessage viewmodel)
            {
                ((ISendMessage) this).Send += viewmodel.ReceiveMessage;
            }

            base.NavigatedTo(sender, obj);
        }

        /// <summary>
        ///     获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="continus"></param>
        public void GetValue<T>(Action<T> continus)
        {
            ((ISendMessage) this).Send?.Invoke(this, new GetValueCombinationComposite<T>(this, continus));
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="run"></param>
        public virtual void SendCombinationComposite<T, U>(Action<T, U> run)
            where U : IMessage where T : IViewModel
        {
            ((ISendMessage) this).Send?.Invoke(this, new CombinationComposite<T, U>(run, this));
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public virtual void Send(IMessage message)
        {
            ((ISendMessage) this).Send?.Invoke(this, message);
        }
    }
}