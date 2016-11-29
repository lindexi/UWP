# Uwp

[中文](#中文)
[English](#English)

# 中文

常用代码和控件

## 控件

 - [白天黑夜按钮](uwp/control/NightDayThemeToggleButton)

 ![](uwp/control/NightDayThemeToggleButton/NightDayThemeToggleButton/Assets/NightDayThemeToggleButton.gif)

 - [进度条](uwp/control/Progress)

 ![](http://img.blog.csdn.net/20160815151046014)

 - [变大数字颜色按钮](uwp/control/RountGradualFigure)

 ![](uwp/control/RountGradualFigure/RountGradualFigure/Assets/RountGradual.gif)

### 图

 - DataGrid（没做）

## 软件

 - [win10 uwp 水印图床](uwp/control/BitStamp)

   参见 ：[win10 uwp 水印图床](uwp/control/BitStamp/【广告】win10 uwp 水印图床 含代码.md)

   [安装](ms-windows-store://pdp/?productid=9nblggh562r2) https://www.microsoft.com/store/apps/9nblggh562r2

## 代码

 - [DetailMaster](uwp/src/DetailMaster)

 ![](http://img.blog.csdn.net/20160806130438076)

 - [图床](uwp/src/Imageshack)

   图床是把图片上传到云，然后获取图片链接的开发包，我将繁琐的过程写成一个简单的类。
   上传的服务器现在有[sm.ms](https://sm.ms/)和[七牛图床](http://www.qiniu.com/)。其中[七牛sdk UWP](uwp/src/Imageshack/cloundes)，
   我只有简单文件上传，好多还没写。代码是从其他大神改出

   七牛图床上传到Nuget，搜索`lindexi.uwp.ImageShack.Thirdqiniucs`或
   控制台`Install-Package lindexi.uwp.ImageShack.Thirdqiniucs`


 - [显示svg](uwp/src/ScalableVectorGraphic)

 - [SplitView](uwp/src/SplitView)
   
   汉堡菜单

 - [ViewModel](uwp/src/ViewModel)

 - [隐私策略](uwp/src/隐私策略)

 - [径向规](uwp/src/RadialGauge)

 - 图片存放本地
   
   输入Uri打开，第一次从网络打开，之后在本地打开。

   先判断本地存在图片，不存在就从网络下载

   `BitmapImage img = await ImageStorage.GetImage(uri);`

   上传到Nuget，可以搜索`lindexi.uwp.src.ImageStorage `或控制台
   `Install-Package lindexi.uwp.src.ImageStorage`

# English

Some controls and common codes



