using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Mntone.SvgForXaml;
using Mntone.SvgForXaml.UI.Xaml;
using UwpConvertSimplifie.ViewModel;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace ScalableVectorGraphic.ViewModel
{
    public class ViewModel : NotifyProperty
    {
        public ViewModel()
        {
            //Svgimage();
            Read();
        }

        private void Read()
        {
            string str = @"<?xml version=""1.0"" encoding=""utf-8""?>
<!-- Generator: Adobe Illustrator 16.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.1//EN"" ""http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd"">
<svg version=""1.1"" id=""Layer_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px""
	 width=""64px"" height=""64px"" viewBox=""0 0 64 64"" enable-background=""new 0 0 64 64"" xml:space=""preserve"">
<g>
	<circle fill=""none"" stroke=""#000000"" stroke-width=""2"" stroke-miterlimit=""10"" cx=""32"" cy=""32"" r=""16""/>
	<line fill=""none"" stroke=""#000000"" stroke-width=""2"" stroke-miterlimit=""10"" x1=""32"" y1=""10"" x2=""32"" y2=""0""/>
	<line fill=""none"" stroke=""#000000"" stroke-width=""2"" stroke-miterlimit=""10"" x1=""32"" y1=""64"" x2=""32"" y2=""54""/>
	<line fill=""none"" stroke=""#000000"" stroke-width=""2"" stroke-miterlimit=""10"" x1=""54"" y1=""32"" x2=""64"" y2=""32""/>
	<line fill=""none"" stroke=""#000000"" stroke-width=""2"" stroke-miterlimit=""10"" x1=""0"" y1=""32"" x2=""10"" y2=""32""/>
	<line fill=""none"" stroke=""#000000"" stroke-width=""2"" stroke-miterlimit=""10"" x1=""48"" y1=""16"" x2=""53"" y2=""11""/>
	<line fill=""none"" stroke=""#000000"" stroke-width=""2"" stroke-miterlimit=""10"" x1=""11"" y1=""53"" x2=""16"" y2=""48""/>
	<line fill=""none"" stroke=""#000000"" stroke-width=""2"" stroke-miterlimit=""10"" x1=""48"" y1=""48"" x2=""53"" y2=""53""/>
	<line fill=""none"" stroke=""#000000"" stroke-width=""2"" stroke-miterlimit=""10"" x1=""11"" y1=""11"" x2=""16"" y2=""16""/>
</g>
</svg>
";
            //XmlReader read = XmlReader.Create(new StringReader(str));
            XDocument xml = XDocument.Load(new StringReader(str));
            ConvertSvgXaml(xml);

            //XElement svg = xml.Root;
            //Attribute(svg);
            //XAttribute id = svg.Attribute("id");

            //XAttribute href = svg.Attribute(XName.Get("href", "http://www.w3.org/1999/xlink"));
            //if (href != null)
            //{
            //    string reference = href.Value;
            //    if (reference.StartsWith("#"))
            //    {
            //        reference = reference.Substring(1);
            //    }
            //}


        }

        private void ConvertSvgXaml(XDocument xml)
        {
            if (xml == null)
            {
                return;
            }
            if (xml.Root.Name.NamespaceName != "http://www.w3.org/2000/svg")
            {
                throw new ArgumentException("命名空间错");
            }

            if (xml.Root.Name.LocalName.ToLower() != "svg")
            {
                throw new ArgumentException("不是svg");
            }

            XElement svgRootNode = xml.Root;
            XAttribute sizeAttrib = svgRootNode.Attributes().ToList().FirstOrDefault(a => a.Name == "viewBox");
            string size = sizeAttrib?.Value.Split(' ').ElementAtOrDefault(3);

            string width = sizeAttrib?.Value.Split(' ').ElementAtOrDefault(2);
            string height = sizeAttrib?.Value.Split(' ').ElementAtOrDefault(3);
            //Viewbox viewbox=new Viewbox();
            Canvas canvas = new Canvas()
            {
                Width = double.Parse(width),
                Height = double.Parse(height)
            };

            foreach (var temp in svgRootNode.Elements())
            {

            }

            //check color
            XAttribute styleAttrib = svgRootNode.Attributes().ToList().FirstOrDefault(a => a.Name == "style");

            if (styleAttrib != null)
            {
                int fillStartIndex = styleAttrib.Value.IndexOf("fill:") + 5;
                int searchFinishIndex = styleAttrib.Value.IndexOf(";", fillStartIndex);
                string fillAttributte = styleAttrib.Value.Substring(fillStartIndex, searchFinishIndex - fillStartIndex).Trim();


                string color = DetermineFillString(fillAttributte);

                //path
                string pathToAdd = svgRootNode.Elements().FirstOrDefault(d => d.Name.LocalName == "path")?.Attributes().FirstOrDefault(a => a.Name.LocalName == "d")?.Value;
                if (!string.IsNullOrEmpty(pathToAdd))
                {
                    XDocument xamlDoc = ComposeIcon(pathToAdd, color, size);
                }
            }


        }

        //private static void ConvertFile(string filename, double fileCount, double total)
        //{
        //    string svgFilename = (filename.EndsWith(".svg")) ? filename : $"{filename}.svg";
        //    string xamlFilename = svgFilename.Replace(".svg", ".xaml");

        //    double percent = Math.Round((fileCount / total * 100), 0);
        //    XDocument svgFile;
        //    try
        //    {
        //        svgFile = XDocument.Load(svgFilename);
        //    }
        //    catch
        //    {
        //        //Console.WriteLine($">> SvgToXaml > Error with file '{svgFilename}' > File not found or invalid!");
        //        return;
        //    }

        //    XElement svgRootNode = svgFile.Root;
        //    // check size
        //    XAttribute sizeAttrib = svgRootNode.Attributes().ToList().FirstOrDefault(a => a.Name == "viewBox");
        //    string size = (sizeAttrib != null) ? sizeAttrib.Value.Split(' ').ElementAtOrDefault(3) : null;

        //    //check color
        //    XAttribute styleAttrib = svgRootNode.Attributes().ToList().FirstOrDefault(a => a.Name == "style");
        //    int fillStartIndex = styleAttrib.Value.IndexOf("fill:") + 5;
        //    int searchFinishIndex = styleAttrib.Value.IndexOf(";", fillStartIndex);
        //    string fillAttributte = styleAttrib.Value.Substring(fillStartIndex, searchFinishIndex - fillStartIndex).Trim();
        //    string color = DetermineFillString(fillAttributte);

        //    //path
        //    string pathToAdd = svgRootNode.Elements().FirstOrDefault(d => d.Name.LocalName == "path")?.Attributes().FirstOrDefault(a => a.Name.LocalName == "d")?.Value;
        //    if (!string.IsNullOrEmpty(pathToAdd))
        //    {
        //        try
        //        {
        //            XDocument xamlDoc = ComposeIcon(pathToAdd, color, size);
        //            xamlDoc.Save(xamlFilename);
        //            string percentString = $"{ percent.ToString("N0") }%";
        //            percentString = percentString.PadLeft(4);
        //            //Console.WriteLine($">> SvgToXaml > ({percentString}) File '{xamlFilename}' saved! ...");
        //        }
        //        catch (Exception ex)
        //        {
        //            //Console.WriteLine($">> SvgToXaml > Error  > Conversion failed: {ex.Message} ");
        //            File.Delete(xamlFilename);
        //        }
        //    }
        //    else
        //    {
        //        //Console.WriteLine($">> SvgToXaml > Error with file '{svgFilename}' > No Path found!");
        //    }
        //}

        private static XDocument ComposeIcon(string pathToAdd, string color, string size = null)
        {
            //double check for empty
            color = !string.IsNullOrEmpty(color) ? color : "White";

            string iconSize = size ?? "171";
            XNamespace ns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

            XDocument xmlIcon = new XDocument
            (
                new XElement(ns + "Viewbox",
                                              new XAttribute("Height", iconSize),
                                              new XAttribute("Width", iconSize),
                            new XElement(ns + "Canvas",
                                                      new XAttribute("Height", iconSize),
                                                      new XAttribute("Width", iconSize),
                                        new XElement(ns + "Path",
                                                                  new XAttribute("Fill", color),
                                                                  new XAttribute("Data", pathToAdd)
                                                    )
                                        )
                            )
            );
            return xmlIcon;
        }



        private static string DetermineFillString(string colorString)
        {
            Regex regHEX = new Regex("^#([0-9a-fA-F]{2}){3}$");
            Match match = regHEX.Match(colorString);
            if (match.Success)
            {
                return colorString;
            }
            else
            {
                Regex regRGB = new Regex(@"rgb\([ ]*(?<r>[0-9]{1,3})[ ]*,[ ]*(?<g>[0-9]{1,3})[ ]*,[ ]*(?<b>[0-9]{1,3})[ ]*\)");
                match = regRGB.Match(colorString);
                if (match.Success)
                {
                    return "#" + Int32.Parse(match.Groups["r"].Value).ToString("X2") +
                                 Int32.Parse(match.Groups["g"].Value).ToString("X2") +
                                 Int32.Parse(match.Groups["b"].Value).ToString("X2");

                }
            }
            //default for white
            return "White";
        }

        private static void Attribute(XElement svg)
        {
            XAttribute attribute = svg.Attribute("style");
            if (attribute != null)
            {
                foreach (var temp in attribute.Value.Split(';'))
                {
                    try
                    {
                        var str = temp.Split(':');
                        if (str.Length == 2)
                        {
                            svg.SetAttributeValue(str[0], str[1]);
                        }
                    }
                    catch
                    {

                    }
                }
                attribute.Remove();
            }
        }

        private void Svgimage()
        {
            string str = @"<?xml version=""1.0"" encoding=""utf-8""?>
<!-- Generator: Adobe Illustrator 16.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->
<!DOCTYPE svg PUBLIC ""-//W3C//DTD SVG 1.1//EN"" ""http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd"">
<svg version=""1.1"" id=""Layer_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px""
	 width=""64px"" height=""64px"" viewBox=""0 0 64 64"" enable-background=""new 0 0 64 64"" xml:space=""preserve"">
<g>
	<circle fill=""none"" stroke=""#000000"" stroke-width=""2"" stroke-miterlimit=""10"" cx=""32"" cy=""32"" r=""16""/>
	<line fill=""none"" stroke=""#000000"" stroke-width=""2"" stroke-miterlimit=""10"" x1=""32"" y1=""10"" x2=""32"" y2=""0""/>
	<line fill=""none"" stroke=""#000000"" stroke-width=""2"" stroke-miterlimit=""10"" x1=""32"" y1=""64"" x2=""32"" y2=""54""/>
	<line fill=""none"" stroke=""#000000"" stroke-width=""2"" stroke-miterlimit=""10"" x1=""54"" y1=""32"" x2=""64"" y2=""32""/>
	<line fill=""none"" stroke=""#000000"" stroke-width=""2"" stroke-miterlimit=""10"" x1=""0"" y1=""32"" x2=""10"" y2=""32""/>
	<line fill=""none"" stroke=""#000000"" stroke-width=""2"" stroke-miterlimit=""10"" x1=""48"" y1=""16"" x2=""53"" y2=""11""/>
	<line fill=""none"" stroke=""#000000"" stroke-width=""2"" stroke-miterlimit=""10"" x1=""11"" y1=""53"" x2=""16"" y2=""48""/>
	<line fill=""none"" stroke=""#000000"" stroke-width=""2"" stroke-miterlimit=""10"" x1=""48"" y1=""48"" x2=""53"" y2=""53""/>
	<line fill=""none"" stroke=""#000000"" stroke-width=""2"" stroke-miterlimit=""10"" x1=""11"" y1=""11"" x2=""16"" y2=""16""/>
</g>
</svg>
";
            Svg = SvgDocument.Parse(str);

        }

        public SvgDocument Svg
        {
            set
            {
                _svg = value;
                OnPropertyChanged();
            }
            get
            {
                return _svg;
            }
        }

        private SvgDocument _svg;
    }
}
