namespace Framework.Model
{
    public class KeySecret : NotifyProperty
    {
        public KeySecret()
        {

        }

        public string Name
        {
            set
            {
                _name = value;
                OnPropertyChanged();
            }
            get
            {
                return _name;
            }
        }

        public string Key
        {
            set
            {
                _key = value;
                OnPropertyChanged();
            }
            get
            {
                return _key;
            }
        }



        private string _key;

        private string _name;
    }
}