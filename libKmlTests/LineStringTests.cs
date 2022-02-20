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
    public class LineStringTests
    {
        [TestMethod()]
        public void LineString出力()
        {

            LineString lineString = new LineString();
            lineString.extrude = true;
            lineString.tessellate = true;
            lineString.altitudeMode = AltitudeMode.absolute;

            List<Coordinate> coordinates = new List<Coordinate>() {
                new Coordinate(-112.2550785337791,36.07954952145647,2357),
                new Coordinate(-112.2549277039738,36.08117083492122,2357)
            };
            lineString.coordinates = coordinates;



            MemoryStream ms = new MemoryStream(1024);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UTF8Encoding(false);
            XmlWriter writer = XmlWriter.Create(ms, settings);

            lineString.Write(writer);
            writer.Close();

            string ret = Encoding.UTF8.GetString(ms.ToArray());

            string ans = "<LineString><extrude>1</extrude><tessellate>1</tessellate><altitudeMode>absolute</altitudeMode>"
                + "<coordinates>-112.2550785337791,36.07954952145647,2357 "
                + "-112.2549277039738,36.08117083492122,2357"
                + "</coordinates></LineString>";

            Assert.AreEqual(ans, ret);
        }
    }
}