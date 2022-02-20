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
    public class PointTests
    {
        [TestMethod()]
        public void Coorinate出力()
        {
            Coordinate crd = new Coordinate(-122.0822035425683, 37.42228990140251, 0);

            MemoryStream ms = new MemoryStream(1024);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UTF8Encoding(false);
            XmlWriter writer = XmlWriter.Create(ms, settings);

            crd.Write(writer);
            writer.Close();

            string ret = Encoding.UTF8.GetString(ms.ToArray());

            Assert.AreEqual("<coordinates>-122.0822035425683,37.42228990140251,0</coordinates>", ret);
        }

        [TestMethod()]
        public void 標高ありPoint出力()
        {
            Point point = new Point(-122.0822035425683, 37.42228990140251, 0);

            MemoryStream ms = new MemoryStream(1024);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UTF8Encoding(false);
            XmlWriter writer = XmlWriter.Create(ms, settings);

            point.Write(writer);
            writer.Close();

            string ret = Encoding.UTF8.GetString(ms.ToArray());

            Assert.AreEqual("<Point><coordinates>-122.0822035425683,37.42228990140251,0</coordinates></Point>", ret);
        }

        [TestMethod()]
        public void 標高なしPoint出力()
        {
            Point point = new Point(-122.0822035425683, 37.42228990140251);

            MemoryStream ms = new MemoryStream(1024);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UTF8Encoding(false);
            XmlWriter writer = XmlWriter.Create(ms, settings);

            point.Write(writer);
            writer.Close();

            string ret = Encoding.UTF8.GetString(ms.ToArray());

            Assert.AreEqual("<Point><coordinates>-122.0822035425683,37.42228990140251</coordinates></Point>", ret);
        }
    }
}