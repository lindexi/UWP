using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DetailMaster.Model;
using DetailMaster.View;

namespace DetailMaster.ViewModel
{
    public class DetailMasterModel : NotifyProperty
    {
        public DetailMasterModel()
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;
           
            EccryptAddress = new ObservableCollection<AddressBook>()
            {
                new AddressBook()
                {
                    Id = "1",
                    Name = "德熙",
                    Str = ""
                },
                new AddressBook()
                {
                    Id = "2",
                    Name = "win10 uwp 列表模板选择器",
                    Str =
                        @"本文主要讲ListView等列表可以根据内容不同，使用不同模板的列表模板选择器，DataTemplateSelector。本文首发Win10.me，因为csdn总是被盗，上次维权失败，所以选择九幽首发，Voidcn也是，有我的连接，那些垃圾网站很都是修改文章不写出错。

好啦，我们先来说下我们在什么下需要使用，其实就是当我们的数据有多样。

例如我们做了一个类，叫做 人，这时我们继承人做出来 男生 和女生，那么男生的属性可能和女生的不同

假设我们的 人有个属性叫做名称，而男生有个属性叫身高，女孩有个属性叫年龄，当然女生年龄放出来并不好，不过我也没找到别的。"
                },
                new AddressBook()
                {
                    Id = "3",
                    Name = "csdn",
                    Str = "本软件重视用户隐私，本软件尊重并保护所有使用服务用户的个人隐私权。为了给您提供更准确、更有个性化的服务，本软件会按照本隐私权政策的规定使用和披露您的个人信息。"
                }
            };


            ZListView = 0;
            ZFrame = 1;

            MasterGrid = GridLength.Auto;
            DetailGrid = new GridLength(1, GridUnitType.Star);

            GridInt = 1;
        }

        public bool HasFrame { set; get; }

        public GridLength MasterGrid
        {
            set
            {
                _masterGrid = value;
                OnPropertyChanged();
            }
            get
            {
                return _masterGrid;
            }
        }

        public GridLength DetailGrid
        {
            set
            {
                _detailGrid = value;
                OnPropertyChanged();
            }
            get
            {
                return _detailGrid;
            }
        }

        public int ZListView
        {
            set
            {
                _zListView = value;
                OnPropertyChanged();
            }
            get
            {
                return _zListView;
            }
        }

        public int GridInt
        {
            set
            {
                _gridInt = value;
                OnPropertyChanged();
            }
            get
            {
                return _gridInt;
            }
        }

        public int ZFrame
        {
            set
            {
                _zFrame = value;
                OnPropertyChanged();
            }
            get
            {
                return _zFrame;
            }
        }

        public ObservableCollection<AddressBook> EccryptAddress { set; get; }

        public Frame Detail { set; get; }

        public void MasterClick(object o, ItemClickEventArgs e)
        {
            AddressBook temp = e.ClickedItem as AddressBook;
            if (temp == null)
            {
                return;
            }
            HasFrame = true;
            Detail.Navigate(typeof(DetailPage), temp.Str);
            Narrow();
        }

        public void NarrowVisual(object sender, VisualStateChangedEventArgs e)
        {
            Narrow();
        }

        private GridLength _detailGrid;

        private int _gridInt;

        private GridLength _masterGrid;

        private int _zFrame;
        private int _zListView;

        private void Narrow()
        {
            if (Window.Current.Bounds.Width < 720)
            {
                MasterGrid = new GridLength(1, GridUnitType.Star);
                DetailGrid = GridLength.Auto;
                GridInt = 0;
                if (HasFrame)
                {
                    ZListView = 0;
                }
                else
                {
                    ZListView = 2;
                }
            }
            else
            {
                MasterGrid = GridLength.Auto;
                DetailGrid = new GridLength(1, GridUnitType.Star);
                GridInt = 1;
            }
        }

        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
            HasFrame = false;
            Narrow();
        }
    }
}