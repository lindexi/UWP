namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    /// 表示 ViewModel 包含属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IViewModelValue<out T> : IViewModel
    {
        /// <summary>
        /// 属性
        /// </summary>
        T Value { get; }
    }
}
