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
    public class CoordinateTests
    {
        [TestMethod()]
        public void Write3Test()
        {

            Coordinate crd = new Coordinate(-122.0822035425683, 37.42228990140251, 0);

            MemoryStream ms = new MemoryStream(1024);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UTF8Encoding(false);
            XmlWriter writer = XmlWriter.Create(ms, settings);

            crd.Write3(writer);
            writer.Close();

            string ret = Encoding.UTF8.GetString(ms.ToArray());

            Assert.AreEqual("<coordinates>-122.0822035425683,37.42228990140251,0</coordinates>", ret);
        }
    }
}