using System;
using System.Threading.Tasks;
using lindexi.MVVM.Framework.Annotations;

namespace PandocMarkdown2Docx
{
    public class RequiringTask
    {
        /// <inheritdoc />
        public RequiringTask([NotNull] Action action, TimeSpan delayTime)
        {
            if (ReferenceEquals(action, null)) throw new ArgumentNullException(nameof(action));

            _action = action;
            _delayTime = delayTime;
        }

        public void InvalidateTask()
        {
            if (_running)
            {
                return;
            }

            _running = true;
            Run();
        }

        private Action _action;

        private TimeSpan _delayTime;

        private bool _running;

        private async void Run()
        {
            await Task.Delay(_delayTime);
            try
            {
                _action();
            }
            finally
            {
                _running = false;
            }
        }
    }
}