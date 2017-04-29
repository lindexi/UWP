﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace lindexi.uwp.Framework.ViewModel
{
    public abstract class ViewModelBase : NotifyProperty, INavigable, ISendMessage, IReceiveMessage
    {
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
        /// <param name="source"></param>
        /// <param name="e"></param>
        public abstract void OnNavigatedTo(object sender, object obj);

        public EventHandler<IMessage> SendMessageHandler { set; get; }
        protected List<Composite> Composite = new List<Composite>();
        public virtual void ReceiveMessage(object sender, IMessage message)
        {
            Composite.FirstOrDefault(temp => temp.Message == message.GetType())?.Run(this, message);
        }
    }


    public abstract class ViewModelMessage : IAdapterMessage, INotifyPropertyChanged
    {
        /// <summary>
        ///     命令合成
        ///     全部调用发送信息的处理在<see cref="Composite" />
        /// </summary>
        protected static List<Composite> Composite
        {
            set; get;
        }

        /// <summary>
        ///     发送信息
        /// </summary>
        public EventHandler<Message> Send
        {
            get; set;
        }

        /// <summary>
        ///     接收信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public abstract void Receive(object source, Message message);

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}