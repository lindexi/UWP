using System;

namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    /// 消息符合
    /// </summary>
    public interface IPredicateMessage
    {
        /// <summary>
        /// 判断消息是否符合
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Predicate(IMessage message);
    }

    /// <inheritdoc />
    internal class PredicateMessage : IPredicateMessage
    {
        /// <inheritdoc />
        private PredicateMessage()
        {
        }

        /// <inheritdoc />
        public bool Predicate(IMessage message)
        {
            return true;
        }

        /// <summary>
        /// 创建默认的
        /// </summary>
        public static PredicateMessage Instance { get; } = new PredicateMessage();
    }

    /// <summary>
    /// 通过类型判断消息是否符合
    /// </summary>
    public class TypePredicateMessage : IPredicateMessage
    {
        /// <inheritdoc />
        public TypePredicateMessage(Type message)
        {
            Message = message;
        }

        /// <inheritdoc />
        public bool Predicate(IMessage message)
        {
            return message.GetType() == Message;
        }

        /// <summary>
        /// 用于比较的消息
        /// </summary>
        public Type Message { get; }
    }

    /// <summary>
    /// 通过类型判断消息是否符合
    /// </summary>
    public class TypePredicateMessage<T> : IPredicateMessage where T : IMessage
    {
        /// <inheritdoc />
        public bool Predicate(IMessage message)
        {
            return message is T;
        }
    }
}
