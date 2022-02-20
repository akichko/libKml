using Microsoft.VisualStudio.TestTools.UnitTesting;
using libKml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace libKml.Tests
{
    [TestClass()]
    public class StyleTests
    {
        [TestMethod()]
        public void LineStyle出力()
        {
            LineStyle style = new LineStyle("7f00ffff", 4);

            MemoryStream ms = new MemoryStream(1024);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UTF8Encoding(false);
            XmlWriter writer = XmlWriter.Create(ms, settings);

            style.Write(writer);
            writer.Close();

            string ret = Encoding.UTF8.GetString(ms.ToArray());

            Assert.AreEqual("<LineStyle><color>7f00ffff</color><width>4</width></LineStyle>", ret);
        }

        [TestMethod()]
        public void Style出力()
        {
            LineStyle lineStyle = new LineStyle("7f00ffff", 4);
            PolyStyle polyStyle = new PolyStyle("7f00ff00");

            Style styleSet = new Style();
            styleSet.id = "yellowLineGreenPoly";
            styleSet.lineStyle = lineStyle;
            styleSet.polyStyle = polyStyle;

            MemoryStream ms = new MemoryStream(1024);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UTF8Encoding(false);
            XmlWriter writer = XmlWriter.Create(ms, settings);

            styleSet.Write(writer);
            writer.Close();

            string ret = Encoding.UTF8.GetString(ms.ToArray());

            string ans = @"<Style id=""yellowLineGreenPoly""><LineStyle><color>7f00ffff</color><width>4</width></LineStyle><PolyStyle><color>7f00ff00</color></PolyStyle></Style>";

            Assert.AreEqual(ans, ret);
        }
    }
}