using Framework.Model;

namespace Framework.ViewModel
{
    [CodeStorage]
    public class ContentModel : ViewModelBase, IReceiveMessage
    {
        public ContentModel()
        {
        }

        public KeySecret Key
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

        public override void OnNavigatedFrom(object obj)
        {
        }

        public override void OnNavigatedTo(object obj)
        {
            Key = obj as KeySecret;
        }


        private KeySecret _key;

        public void ReceiveMessage(Message message)
        {
            if (message.Key == "点击列表")
            {
                Key = message.Content as KeySecret;
            }
        }
    }
}