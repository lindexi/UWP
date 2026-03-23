// See https://aka.ms/new-console-template for more information

using System.Text;

var text = 
"""
宸ュ叿鈥渕icrosoft.dotnet-interactive鈥?鐗堟湰鈥?.0.616301鈥?宸叉垚鍔熷畨瑁呫€傛潯鐩皢娣诲姞鍒版竻鍗曟枃浠
""";
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var gbkEncoding = Encoding.GetEncoding("GBK");
var binaryData = gbkEncoding.GetBytes(text);
var utf8Text = Encoding.UTF8.GetString(binaryData);

Console.WriteLine($"GBK->UTF8 猜测： \"{text}\" 为 \"{utf8Text}\"");

binaryData = Encoding.UTF8.GetBytes(text);
var gbkText = gbkEncoding.GetString(binaryData);

Console.WriteLine($"UTF8->GBK 猜测： \"{text}\" 为 \"{gbkText}\"");

Console.WriteLine("Hello, World!");