using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using lindexi.MVVM.Framework.Annotations;
using lindexi.uwp.Framework.ViewModel;

namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    /// 处理消息
    /// </summary>
    public class Composite : IComposite
    {
        /// <inheritdoc />
        public Composite()
        {
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="message">处理的消息类型</param>
        public Composite([NotNull] Type message)
        {
            if (ReferenceEquals(message, null)) throw new ArgumentNullException(nameof(message));
            Message = message;
        }

        /// <summary>
        /// 处理什么消息
        /// </summary>
        public Type Message { get; set; }

        public string Key { get; set; }

        /// <inheritdoc />
        public virtual void Run([NotNull] IViewModel source, [NotNull] IMessage message)
        {
            if (ReferenceEquals(source, null)) throw new ArgumentNullException(nameof(source));
            if (ReferenceEquals(message, null)) throw new ArgumentNullException(nameof(message));
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

        /// <inheritdoc />
        public virtual void Run(ViewModelBase source, IMessage message)
        {
            if (_run)
            {
                return;
            }

            _run = true;
            Run((IViewModel) source, message);
            _run = false;
        }

        static Composite()
        {
            CompositeList = new List<Composite>()
            {
                new NavigateComposite()
            };
        }

        /// <summary>
        /// 获取现有的处理
        /// </summary>
        /// <returns></returns>
        public static List<Composite> GetCompositeList()
        {
            return CompositeList.ToList();
        }

        /// <summary>
        /// 查找可以运行的消息
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="message"></param>
        /// <param name="compositeList"></param>
        [PublicAPI]
        public static bool Run([NotNull] IViewModel viewModel, [NotNull] IMessage message, IEnumerable<Composite> compositeList = null)
        {
            if (ReferenceEquals(viewModel, null)) throw new ArgumentNullException(nameof(viewModel));
            if (ReferenceEquals(message, null)) throw new ArgumentNullException(nameof(message));
            if (!message.Goal.Predicate(viewModel))
            {
                return true;
            }

            if (compositeList == null || !compositeList.Any())
            {
                compositeList = CompositeList;
            }

            var composite = false;
            var exceptionList = new List<Exception>();

            var t = message.GetType();
            foreach (var temp in compositeList.Where(temp => temp.Message == t))
            {
                try
                {
                    temp.Run(viewModel, message);
                }
                catch (Exception e)
                {
                    Trace.Write(e);
                    exceptionList.Add(e);
                }

                composite = true;
            }

            if (exceptionList.Any())
            {
                if (exceptionList.Count == 1)
                {
                    throw exceptionList[0];
                }

                throw new AggregateException(
                    "调用 static bool Run(IViewModel viewModel, IMessage message, IEnumerable<Composite> compositeList) 发现异常",
                    exceptionList);
            }

            return composite;
        }

        private static IReadOnlyCollection<Composite> CompositeList { get; }
    }
}