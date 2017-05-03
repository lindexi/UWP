using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace lindexi.uwp.Framework.ViewModel
{
    public abstract class ViewModelBase : NotifyProperty, INavigable, ISendMessage, IReceiveMessage
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

        public EventHandler<IMessage> SendMessageHandler
        {
            set; get;
        }
        protected List<Composite> Composite = new List<Composite>();
        public virtual void ReceiveMessage(object sender, IMessage message)
        {
            if (message is CombinationComposite)
            {
                ((CombinationComposite)message).Run(this, message);
            }
            Composite.FirstOrDefault(temp => temp.Message == message.GetType())?.Run(this, message);
        }
    }


    public abstract class ViewModelMessage : ViewModelBase,IAdapterMessage
    {
        /// <summary>
        ///     命令合成
        ///     全部调用发送信息的处理在<see cref="Composite" />
        /// </summary>
        protected static List<Composite> Composite
        {
            set; get;
        }

        public void Navigateto(object source, object e)
        {
            IsLoaded = true;
            OnNavigatedTo(source, e);
        }

        public void NavigateFrom(object source, object e)
        {
            OnNavigatedFrom(source, e);
            IsLoaded = false;
            Send = null;
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        public EventHandler<IMessage> Send
        {
            get; set;
        }

        /// <summary>
        /// 接收信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public abstract void Receive(object source, IMessage message);


    }
}