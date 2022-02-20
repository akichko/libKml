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
    public class PlacemarkPolygonTests
    {
        [TestMethod()]
        public void PolyGonPlacemark出力()
        {
            Placemark<Polygon> placemark = new Placemark<Polygon>("Building");

            placemark.geometry.extrude = true;
            placemark.geometry.altitudeMode = AltitudeMode.relativeToGround;

            placemark.geometry.SetOuterBoundary(new List<Coordinate>() {
                new Coordinate(-122.0857412771483,37.42227033155257,17),
                new Coordinate(-122.0858169768481,37.42231408832346,17),
                new Coordinate(-122.085852582875,37.42230337469744,17),
                new Coordinate(-122.0857412771483,37.42227033155257,17)
            });


            MemoryStream ms = new MemoryStream(1024);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = new UTF8Encoding(false);
            XmlWriter writer = XmlWriter.Create(ms, settings);

            placemark.Write(writer);
            writer.Close();

            string ret = Encoding.UTF8.GetString(ms.ToArray());

            string ans = @"<Placemark><name>Building</name><Polygon><extrude>1</extrude><altitudeMode>relativeToGround</altitudeMode>"
            + "<outerBoundaryIs><LinearRing><coordinates>-122.0857412771483,37.42227033155257,17 -122.0858169768481,37.42231408832346,17 "
            + "-122.085852582875,37.42230337469744,17 -122.0857412771483,37.42227033155257,17" +
            "</coordinates></LinearRing></outerBoundaryIs></Polygon></Placemark>";


            Assert.AreEqual(ans, ret);
        }
    }
}