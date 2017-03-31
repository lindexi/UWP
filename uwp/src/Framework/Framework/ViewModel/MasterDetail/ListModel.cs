using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Framework.Model;

namespace Framework.ViewModel
{
    [CodeStorage]
    public class ListModel : ViewModelBase,ISendMessage
    {
        public ListModel()
        {

        }

        public ObservableCollection<KeySecret> KeySecret
        {
            set
            {
                _keySecret = value;
                OnPropertyChanged();
            }
            get
            {
                return _keySecret;
            }
        }

        public void ListClick(object sender, ItemClickEventArgs e)
        {
            var keySecret = e.ClickedItem as KeySecret;
            Message temp=new Message()
            {
                Content = keySecret,
                Goal = nameof(ContentModel),
                Key = "点击列表",
                Source = this
            };

            //SendMessage?.SendMessage(temp);
            SendMessageHandler?.Invoke(this,temp);
        }

        //public ISendMessage SendMessage
        //{
        //    set;
        //    get;
        //}

        private ObservableCollection<KeySecret> _keySecret;

        public override void OnNavigatedFrom(object obj)
        {

        }

        public override void OnNavigatedTo(object obj)
        {
            KeySecret = new ObservableCollection<KeySecret>()
            {
                new KeySecret()
                {
                    Name = "林德熙",
                    Key = "lindexi.oschina.io"
                },
                new KeySecret()
                {
                    Name = "csdn",
                    Key = "blog.csdn.net/lindexi_gd"
                },
                new KeySecret()
                {
                    Name = "九幽",
                    Key = "win10.cm"
                }
            };

        }

        public EventHandler<Message> SendMessageHandler
        {
            get;
            set;
        }
    }
}