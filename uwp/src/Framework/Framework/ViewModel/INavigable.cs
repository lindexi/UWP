namespace Framework.ViewModel
{
    public interface INavigable
    {
        /// <summary>
        ///     不使用这个页面
        ///     清理页面
        /// </summary>
        /// <param name="obj"></param>
        void OnNavigatedFrom(object obj);

        /// <summary>
        ///     跳转到
        /// </summary>
        /// <param name="obj"></param>
        void OnNavigatedTo(object obj);
    }
}