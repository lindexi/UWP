##smms图床

使用

```
            //传入文件
            smms.Model.Imageshack imageshack = new Imageshack()
            {
                File=File,
            };
            //上传完成事件，其中str为sm.ms返回，一般为json
            //Reminder是例子，可以根据具体修改，注意要同步CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync
            imageshack.OnUploadedEventHandler += (sender, str) => Reminder = str.Replace("\\/","/");
            //上传
            imageshack.UpLoad();
```

##七牛图床

