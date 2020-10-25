namespace lindexi.uwp.control.Button.ViewModel
{
    public class ViewModel : NotifyProperty
    {
        public ViewModel()
        {
            Complete = true;
        }

        public bool Complete
        {
            set
            {
                _complete = value;
                OnPropertyChanged();
            }
            get
            {
                return _complete;
            }
        }



        private bool _complete;
    }
}
