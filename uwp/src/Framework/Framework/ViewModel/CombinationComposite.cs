using System;

namespace lindexi.uwp.Framework.ViewModel
{
    public class CombinationComposite : Composite, IMessage
    {
        public CombinationComposite(ViewModelBase source)
        {
            ((IMessage) this).Source = source;
        }

        public CombinationComposite(Action<ViewModelBase, object> run, ViewModelBase source)
        {
            _run = run;
            ((IMessage) this).Source = source;
        }

        public ViewModelMessage Aim { get; set; }

        ViewModelBase IMessage.Source { get; set; }

        public string Goal { set; get; }

        public override void Run(ViewModelBase source, IMessage message)
        {
            _run.Invoke(source, message);
        }

        protected Action<ViewModelBase, object> _run;
    }
}