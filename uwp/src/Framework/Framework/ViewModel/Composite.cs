using System;

namespace lindexi.uwp.Framework.ViewModel
{
    public class Composite
    {
        public Type Message { get; set; }
        public string Key { get; set; }

        public virtual void Run(ViewModelBase source, IMessage o)
        {
        }
    }
}