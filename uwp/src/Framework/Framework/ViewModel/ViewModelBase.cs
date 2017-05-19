using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace lindexi.uwp.Framework.ViewModel
{
    public abstract class ViewModelBase : NotifyProperty, INavigable, IViewModel
    {
        /// <summary>
        /// 表示当前ViewModel是否处于进入状态
        /// 用于命令判断是否可用
        /// </summary>
        public bool IsLoaded
        {
            get; set;
        }



        /// <summary>
        ///     从其他页面跳转出
        ///     需要释放页面
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public abstract void OnNavigatedFrom(object sender, object obj);

        /// <summary>
        ///     从其他页面跳转到
        ///     在这里初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="obj"></param>
        public abstract void OnNavigatedTo(object sender, object obj);

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
    }

    /// <summary>
    /// 可以接收发送消息的页面
    /// </summary>
    public abstract class ViewModelMessage : ViewModelBase, IAdapterMessage
    {
        /// <inheritdoc />
        public sealed override void NavigatedFrom(object sender, object obj)
        {
            base.NavigatedFrom(sender, obj);
            Send = null;
        }

        /// <inheritdoc />
        public sealed override void NavigatedTo(object sender, object obj)
        {
            base.NavigatedTo(sender, obj);
        }


        /// <summary>
        /// 发送消息
        /// </summary>
        public EventHandler<IMessage> Send
        {
            set; get;
        }

        /// <summary>
        ///     命令合成
        ///     全部调用发送信息的处理在<see cref="Composite" />
        /// </summary>
        protected List<Composite> Composite { set; get; } = new List<Composite>();

        /// <summary>
        /// 接收信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public virtual void ReceiveMessage(object sender, IMessage message)
        {
            var composite = message as CombinationComposite;
            composite?.Run(this, composite);
            Composite.FirstOrDefault(temp => temp.Message == message.GetType())?.Run(this, message);
        }

    }
}