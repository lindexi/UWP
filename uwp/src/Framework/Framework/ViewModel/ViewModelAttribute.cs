using System;

namespace Framework.ViewModel
{
    public class ViewModelAttribute : Attribute
    {
        public Type ViewModel { get; set; }
    }
}