using lindexi.MVVM.Framework.ViewModel;

namespace lindexi.uwp.Framework.ViewModel
{
    public interface IComposite
    {
        void Run(IViewModel source, IMessage message);
    }
}