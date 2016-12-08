# win10 uwp 图床


## smms图床

### Nuget 安装

搜索imageShack，打开`Imageshack.lindexi`

### 使用

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

## 七牛图床

qiniu图床上从官方的qiniu sdk 修改的。

功能：上传图片、获取外链

实际功能可以上传文件。

### Nuget安装

![](http://7xqpl8.com1.z0.glb.clouddn.com/b16ecaae-ee31-47c8-9f80-8da6618f39e42016129152514.jpg)

搜索`imageShack`，第一个`lindexi.uwp.ImageShack.Thirdqiniucs`

### 使用

图床在https://github.com/lindexi/UWP/tree/master/uwp/src/Imageshack/cloundes

先写配置，配置是需要AK和SK，空间名，域名。

配置是新建`lindexi.uwp.ImageShack.Model.CloundesAccound`，CloundesAccound需要

- AccessKey

- SecretKey

- Bucket 空间名

- Url 域名

- Pname [可选]文件名前缀

Bucket 空间名在注册[七牛](http://www.qiniu.com/)后可以通过添加空间来获得。

![](http://ooo.0o0.ooo/2016/11/20/583185c409c66.jpg)

域名可以在命名空间页面获得

![](http://ooo.0o0.ooo/2016/11/20/5831860f0ca4d.jpg)

AK和SK可以在个人面板密钥获得

![](http://ooo.0o0.ooo/2016/11/20/58318633510e5.jpg)

![](http://ooo.0o0.ooo/2016/11/20/5831865e1883a.jpg)

填写完，新建`lindexi.uwp.ImageShack.Model.QnUploadImage`

传入你要上传的文件，并且把配置也传进去。

如果没有传入配置，会使用默认配置，默认配置写在Appid，我没有传出来，需要自己写。

然后添加OnUploaded事件，接着上传UploadImage。

上传完成，可以通过URL获得图片地址。


```

            QnUploadImage uploadImage = new QnUploadImage(file);
            uploadImage.OnUploaded += (s, e) =>
            {
			    //上传成功
                if (e)
                {
                    Address = uploadImage.Url;
                }
            };
            uploadImage.UploadImage();

```



