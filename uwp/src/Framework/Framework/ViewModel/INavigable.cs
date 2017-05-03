namespace lindexi.uwp.Framework.ViewModel
{
    public interface INavigable: IViewModel
    {
        /// <summary>
        ///     不使用这个页面
        ///     清理页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="obj"></param>
        void OnNavigatedFrom(object sender, object obj);

        /// <summary>
        ///     跳转到
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="obj"></param>
        void OnNavigatedTo(object sender, object obj);
    }
}