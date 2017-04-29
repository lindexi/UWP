using System;
using Framework.Model;
using lindexi.uwp.Framework.ViewModel;

namespace Framework.ViewModel
{
    [CodeStorage]
    public class ContentModel : ViewModelBase
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

        public override void OnNavigatedFrom(object sender, object obj)
        {
        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            Key = obj as KeySecret;
        }


        private KeySecret _key;

        public override void ReceiveMessage(object sender, IMessage e)
        {
            Message message = e as Message;
            if (message?.Key == "点击列表")
            {
                Key = message.Content as KeySecret;
            }
        }
    }
}