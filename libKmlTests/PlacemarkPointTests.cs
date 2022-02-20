using Microsoft.VisualStudio.TestTools.UnitTesting;
using libKml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace libKml.Tests
{
    [TestClass()]
    public class PlacemarkPointTests
    {
        [TestMethod()]
        public void PointPlacemark出力()
        {
            Placemark<Point> placemark = new Placemark<Point>("nameText", "pdescriptionText");
            placemark.geometry = new Point(-122.082, 37.422, 0);

            MemoryStream ms = new MemoryStream(1024);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UTF8Encoding(false);
            XmlWriter writer = XmlWriter.Create(ms, settings);

            placemark.Write(writer);
            writer.Close();

            string ret = Encoding.UTF8.GetString(ms.ToArray());

            string ans = "<Placemark><name>nameText</name><description>pdescriptionText</description>"
                + "<Point><coordinates>-122.082,37.422,0</coordinates></Point></Placemark>";

            Assert.AreEqual(ans, ret);
        }
    }
}