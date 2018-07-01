using System;
using lindexi.MVVM.Framework.Annotations;
using lindexi.uwp.Framework.ViewModel;

namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    ///     组合 Composite 和 Message
    /// </summary>
    public class CombinationComposite : Composite, IMessage, ICombinationComposite
    {
        /// <summary>
        /// 创建组合的Composite用来处理消息
        /// </summary>
        /// <param name="source">发送消息的类</param>
        public CombinationComposite([NotNull] ViewModelBase source)
        {
            if (ReferenceEquals(source, null)) throw new ArgumentNullException(nameof(source));
            ((IMessage) this).Source = source;
        }

        /// <summary>
        /// 创建组合的Composite，并且告诉如何处理
        /// </summary>
        /// <param name="run">如何处理</param>
        /// <param name="source">发送消息的类</param>
        public CombinationComposite([NotNull] Action<ViewModelBase, object> run, [NotNull] ViewModelBase source)
        {
            if (ReferenceEquals(run, null)) throw new ArgumentNullException(nameof(run));
            if (ReferenceEquals(source, null)) throw new ArgumentNullException(nameof(source));

            _run = run;
            ((IMessage) this).Source = source;
        }

        /// <summary>
        /// 开始运行
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public override void Run(ViewModelBase source, IMessage message)
        {
            _run(source, message);
        }

        /// <inheritdoc />
        IViewModel IMessage.Source { get; set; }

        /// <inheritdoc />
        public IPredicateViewModel Goal { set; get; }

        /// <inheritdoc />
        public bool Predicate(IViewModel viewModel)
        {
            if (Goal == null)
            {
                return true;
            }

            return Goal.Predicate(viewModel);
        }

        protected Action<ViewModelBase, object> _run;
    }

    /// <summary>
    /// 组合 Composite 和 Message 用于在发送时告诉如何处理
    /// </summary>
    /// <typeparam name="T">发送到对应类型的 ViewModel ，用来在这个 ViewModel 处理</typeparam>
    public class CombinationComposite<T> : Composite, IMessage, ICombinationComposite
        where T : IViewModel
    {
        /// <summary>
        /// 创建组合 Composite 和 Message 用于在发送时告诉如何处理
        /// </summary>
        /// <param name="source"></param>
        public CombinationComposite([NotNull] ViewModelBase source)
        {
            if (ReferenceEquals(source, null)) throw new ArgumentNullException(nameof(source));
            ((IMessage) this).Source = source;
            Goal = new PredicateInheritViewModel(typeof(T));
        }

        /// <summary>
        ///  创建组合 Composite 和 Message 用于在发送时告诉如何处理
        /// </summary>
        /// <param name="run">发送到对应的 ViewModel 需要如何处理</param>
        /// <param name="source"></param>
        public CombinationComposite([NotNull] Action<T> run, [NotNull] ViewModelBase source) : this(source)
        {
            if (ReferenceEquals(run, null)) throw new ArgumentNullException(nameof(run));
            if (ReferenceEquals(source, null)) throw new ArgumentNullException(nameof(source));
            _run = run;
        }

        /// <inheritdoc />
        public override void Run(IViewModel source, IMessage message)
        {
            if (source is T t)
            {
                _run.Invoke(t);
            }
        }

        /// <inheritdoc />
        IViewModel IMessage.Source { get; set; }

        /// <inheritdoc />
        public IPredicateViewModel Goal { set; get; }

        /// <inheritdoc />
        public bool Predicate(IViewModel viewModel)
        {
            if (Goal == null)
            {
                return viewModel is T;
            }

            return Goal.Predicate(viewModel);
        }

        protected Action<T> _run;
    }

    /// <summary>
    /// 组合 Composite 和 Message
    /// </summary>
    /// <typeparam name="T">发送到的ViewModel是哪个</typeparam>
    /// <typeparam name="U">发送的消息是哪个</typeparam>
    public class CombinationComposite<T, U> : Composite, IMessage, ICombinationComposite
        where U : IMessage where T : IViewModel
    {
        /// <summary>
        /// 创建组合 Composite 和 Message 在发送的时候告诉如何处理
        /// </summary>
        /// <param name="source"></param>
        public CombinationComposite([NotNull] ViewModelBase source)
        {
            if (ReferenceEquals(source, null)) throw new ArgumentNullException(nameof(source));
            ((IMessage) this).Source = source;
            Goal = new PredicateInheritViewModel(typeof(T));
        }

        /// <summary>
        /// 创建组合 Composite 和 Message 在发送的时候告诉如何处理
        /// </summary>
        /// <param name="run"></param>
        /// <param name="source"></param>
        public CombinationComposite([NotNull] Action<T, U> run, ViewModelBase source) : this(source)
        {
            if (ReferenceEquals(run, null)) throw new ArgumentNullException(nameof(run));
            _run = run;
        }

        /// <inheritdoc />
        public override void Run(IViewModel source, IMessage message)
        {
            if (source is T && message is U)
            {
                _run.Invoke((T) source, (U) message);
            }
        }

        /// <inheritdoc />
        IViewModel IMessage.Source { get; set; }

        /// <inheritdoc />
        public IPredicateViewModel Goal { set; get; }

        /// <inheritdoc />
        public bool Predicate(IViewModel viewModel)
        {
            if (Goal == null)
            {
                return viewModel is T;
            }

            return Goal.Predicate(viewModel);
        }

        protected Action<T, U> _run;
    }
}