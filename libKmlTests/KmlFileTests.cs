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
    public class KmlFileTests
    {

        [TestMethod()]
        public void Sample1目印作成()
        {
            //準備
            MemoryStream ms = new MemoryStream(1024);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            XmlWriter writer = XmlWriter.Create(ms, settings);


            //実行
            KmlFile kmlFile = new KmlFile();

            Placemark<Point> placemark = new Placemark<Point>("Simple placemark",
                "Attached to the ground. Intelligently places itself at the height of the underlying terrain.")
                .SetGeometry(new Point(-122.0822035425683, 37.42228990140251, 0));

            kmlFile.item = placemark;

            kmlFile.WriteKml(ms, settings);
            string ret = Encoding.UTF8.GetString(ms.ToArray());

            //確認
            string ans = @"<?xml version=""1.0"" encoding=""utf-8""?><kml xmlns=""http://www.opengis.net/kml/2.2"">"
                + "<Placemark><name>Simple placemark</name>"
                + "<description>Attached to the ground. Intelligently places itself at the height of the underlying terrain.</description>"
                + "<Point><coordinates>-122.0822035425683,37.42228990140251,0</coordinates></Point></Placemark></kml>";

            Assert.AreEqual(ans, ret);
        }


        [TestMethod()]
        public void Sample2地面オーバーレイ作成()
        {
            //データ準備
            KmlFile kmlFile = new KmlFile();

            Container folder = new Folder("Ground Overlays", "Examples of ground overlays");
            kmlFile.item = folder;

            GroundOverlay groundOverlay = new GroundOverlay("Large-scale overlay on terrain", "Overlay shows Mount Etna erupting on July 13th, 2001.");//
                                                                                                                                                       //.AttachTo<GroundOverlay>(folder);
            folder.AddFeature(groundOverlay);

            groundOverlay.icon = new Icon("http://developers.google.com/kml/documentation/images/etna.jpg");

            groundOverlay.latlonBox = new LatLonBox(37.91904192681665, 37.46543388598137, 15.35832653742206, 14.60128369746704, -0.1556640799496235);


            //実行
            MemoryStream ms = new MemoryStream(1024);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            XmlWriter writer = XmlWriter.Create(ms, settings);

            kmlFile.WriteKml(ms, settings);

            string ret = Encoding.UTF8.GetString(ms.ToArray());

            string ans = @"<?xml version=""1.0"" encoding=""utf-8""?><kml xmlns=""http://www.opengis.net/kml/2.2"">"
                + "<Folder><name>Ground Overlays</name><description>Examples of ground overlays</description><GroundOverlay>"
                + "<name>Large-scale overlay on terrain</name><description>Overlay shows Mount Etna erupting on July 13th, 2001.</description>"
                + "<Icon><href>http://developers.google.com/kml/documentation/images/etna.jpg</href></Icon>"
                + "<LatLonBox><north>37.91904192681665</north><south>37.46543388598137</south><east>15.35832653742206</east><west>14.60128369746704</west>"
                + "<rotation>-0.1556640799496235</rotation></LatLonBox></GroundOverlay></Folder></kml>";


            Assert.AreEqual(ans, ret);
        }


        [TestMethod()]
        public void Sample3パス作成()
        {
            //データ準備
            KmlFile kmlFile = new KmlFile();

            string descText = "Examples of paths. Note that the tessellate tag is by default set to 0. "
                + "If you want to create tessellated lines, they must be authored (or edited) directly in KML.";
            Container document = new Document("Paths", descText);

            Style style = new Style("yellowLineGreenPoly");
            style.lineStyle = new LineStyle("7f00ffff", 4);
            style.polyStyle = new PolyStyle("7f00ff00");
            document.AddStyle(style);

            kmlFile.item = document;

            Placemark<LineString> placemark = new Placemark<LineString>("Absolute Extruded", "Transparent green wall with yellow outlines");

            placemark.styleUrl = "#yellowLineGreenPoly";
            placemark.geometry.extrude = true;
            placemark.geometry.tessellate = true;
            placemark.geometry.altitudeMode = AltitudeMode.absolute;

            List<Coordinate> coordinates = new List<Coordinate>() {
                new Coordinate(-112.2550785337791,36.07954952145647,2357),
                new Coordinate(-112.2549277039738,36.08117083492122,2357),
                new Coordinate(-112.2552505069063,36.08260761307279,2357),
                new Coordinate(-112.2564540158376,36.08395660588506,2357),
                new Coordinate(-112.2580238976449,36.08511401044813,2357),
                new Coordinate(-112.2595218489022,36.08584355239394,2357),
                new Coordinate(-112.2608216347552,36.08612634548589,2357),
                new Coordinate(-112.262073428656,36.08626019085147,2357),
                new Coordinate(-112.2633204928495,36.08621519860091,2357),
                new Coordinate(-112.2644963846444,36.08627897945274,2357),
                new Coordinate(-112.2656969554589,36.08649599090644,2357)
            };

            placemark.geometry.SetGeometry(coordinates);
            document.AddFeature(placemark);


            //実行
            MemoryStream ms = new MemoryStream(2048);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);

            kmlFile.WriteKml(ms, settings);

            string ret = Encoding.UTF8.GetString(ms.ToArray());

            string ans = @"<?xml version=""1.0"" encoding=""utf-8""?><kml xmlns=""http://www.opengis.net/kml/2.2"">"
            + "<Document><name>Paths</name><description>Examples of paths. Note that the tessellate tag is by default set to 0. If you want to create tessellated lines, they must be authored (or edited) directly in KML.</description>"
            + @"<Style id=""yellowLineGreenPoly""><LineStyle><color>7f00ffff</color><width>4</width></LineStyle><PolyStyle><color>7f00ff00</color></PolyStyle></Style>"
            + "<Placemark><name>Absolute Extruded</name><description>Transparent green wall with yellow outlines</description>"
            + "<styleUrl>#yellowLineGreenPoly</styleUrl><LineString><extrude>1</extrude><tessellate>1</tessellate><altitudeMode>absolute</altitudeMode>"
            + "<coordinates>-112.2550785337791,36.07954952145647,2357"
            + " -112.2549277039738,36.08117083492122,2357"
            + " -112.2552505069063,36.08260761307279,2357"
            + " -112.2564540158376,36.08395660588506,2357"
            + " -112.2580238976449,36.08511401044813,2357"
            + " -112.2595218489022,36.08584355239394,2357"
            + " -112.2608216347552,36.08612634548589,2357"
            + " -112.262073428656,36.08626019085147,2357"
            + " -112.2633204928495,36.08621519860091,2357"
            + " -112.2644963846444,36.08627897945274,2357"
            + " -112.2656969554589,36.08649599090644,2357</coordinates></LineString></Placemark></Document></kml>";


            Assert.AreEqual(ans, ret);
        }


        [TestMethod()]
        public void Sample4ポリゴン作成()
        {
            //データ準備
            KmlFile kmlFile = new KmlFile();

            Placemark<Polygon> placemark = new Placemark<Polygon>("The Pentagon");
            kmlFile.item = placemark;

            placemark.geometry.extrude = true;
            placemark.geometry.altitudeMode = AltitudeMode.relativeToGround;

            placemark.geometry.SetOuterBoundary(new List<Coordinate>() {
                new Coordinate(-77.0578845766096,38.8725325989282,100),
                new Coordinate(-77.0546597375670,38.8729101628170,100),
                new Coordinate(-77.0531553685479,38.8705326779438,100),
                new Coordinate(-77.0555262249351,38.86875780125,100),
                new Coordinate(-77.0584405629039,38.8699620650694,100),
                new Coordinate(-77.0578845766096,38.8725325989282,100) });

            placemark.geometry.SetInnerBoundary(new List<Coordinate>() {
                new Coordinate(-77.0566805501912, 38.8715423979845, 100),
                new Coordinate(-77.0554262596081, 38.8716789034407, 100),
                new Coordinate(-77.0548512590102, 38.8707653539779, 100),
                new Coordinate(-77.0557767743315, 38.8700868658144, 100),
                new Coordinate(-77.0569116201754, 38.8705444696335, 100),
                new Coordinate(-77.0566805501912, 38.8715423979845, 100) });

            //実行
            MemoryStream ms = new MemoryStream(2048);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            XmlWriter writer = XmlWriter.Create(ms, settings);

            kmlFile.WriteKml(ms, settings);

            string ret = Encoding.UTF8.GetString(ms.ToArray());

            string ans = @"<?xml version=""1.0"" encoding=""utf-8""?><kml xmlns=""http://www.opengis.net/kml/2.2"">" +
                "<Placemark><name>The Pentagon</name><Polygon><extrude>1</extrude><altitudeMode>relativeToGround</altitudeMode><outerBoundaryIs><LinearRing>" +
                "<coordinates>-77.0578845766096,38.8725325989282,100 -77.054659737567,38.872910162817,100 " +
                "-77.0531553685479,38.8705326779438,100 -77.0555262249351,38.86875780125,100 " +
                "-77.0584405629039,38.8699620650694,100 -77.0578845766096,38.8725325989282,100</coordinates></LinearRing></outerBoundaryIs>" +
                "<innerBoundaryIs><LinearRing><coordinates>-77.0566805501912,38.8715423979845,100 -77.0554262596081,38.8716789034407,100 " +
                "-77.0548512590102,38.8707653539779,100 -77.0557767743315,38.8700868658144,100 " +
                "-77.0569116201754,38.8705444696335,100 -77.0566805501912,38.8715423979845,100</coordinates></LinearRing></innerBoundaryIs>" +
                "</Polygon></Placemark></kml>";

            Assert.AreEqual(ans, ret);
        }


        [TestMethod()]
        public void Sample5ポリゴン作成2()
        {
            //データ準備
            KmlFile kmlFile = new KmlFile();

            Container document = new Document();
            kmlFile.item = document;

            List<ColorStyle> styles = new List<ColorStyle>() {
                new LineStyle(null, (float)1.5),
                new PolyStyle("7dff0000") };
            document.AddStyle(new Style("transBluePoly", styles));

            Placemark<Polygon> placemark = new Placemark<Polygon>("Building 41");
            document.AddFeature(placemark);

            placemark.styleUrl = "#transBluePoly";
            placemark.geometry.extrude = true;
            placemark.geometry.altitudeMode = AltitudeMode.relativeToGround;

            placemark.geometry.SetOuterBoundary(new List<Coordinate>() {
                new Coordinate( -122.0857412771483,37.42227033155257,17),
                new Coordinate( -122.0858169768481,37.42231408832346,17),
                new Coordinate( -122.085852582875,37.42230337469744,17),
                new Coordinate( -122.0858799945639,37.42225686138789,17),
                new Coordinate( -122.0858860101409,37.4222311076138,17),
                new Coordinate( -122.0858069157288,37.42220250173855,17),
                new Coordinate( -122.0858379542653,37.42214027058678,17),
                new Coordinate( -122.0856732640519,37.42208690214408,17),
                new Coordinate( -122.0856022926407,37.42214885429042,17),
                new Coordinate( -122.0855902778436,37.422128290487,17),
                new Coordinate( -122.0855841672237,37.42208171967246,17),
                new Coordinate( -122.0854852065741,37.42210455874995,17),
                new Coordinate( -122.0855067264352,37.42214267949824,17),
                new Coordinate( -122.0854430712915,37.42212783846172,17),
                new Coordinate( -122.0850990714904,37.42251282407603,17),
                new Coordinate( -122.0856769818632,37.42281815323651,17),
                new Coordinate( -122.0860162273783,37.42244918858722,17),
                new Coordinate( -122.0857260327004,37.42229239604253,17),
                new Coordinate( -122.0857412771483,37.42227033155257,17) });


            //実行
            MemoryStream ms = new MemoryStream(2048);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            XmlWriter writer = XmlWriter.Create(ms, settings);

            kmlFile.WriteKml(ms, settings);

            string ret = Encoding.UTF8.GetString(ms.ToArray());

            string ans = @"<?xml version=""1.0"" encoding=""utf-8""?><kml xmlns=""http://www.opengis.net/kml/2.2"">"
                + @"<Document><Style id=""transBluePoly""><LineStyle><width>1.5</width></LineStyle><PolyStyle><color>7dff0000</color></PolyStyle></Style>"
                + "<Placemark><name>Building 41</name><styleUrl>#transBluePoly</styleUrl><Polygon><extrude>1</extrude><altitudeMode>relativeToGround</altitudeMode>"
                + "<outerBoundaryIs><LinearRing><coordinates>" +
                "-122.0857412771483,37.42227033155257,17 -122.0858169768481,37.42231408832346,17 " +
                "-122.085852582875,37.42230337469744,17 -122.0858799945639,37.42225686138789,17 " +
                "-122.0858860101409,37.4222311076138,17 -122.0858069157288,37.42220250173855,17 " +
                "-122.0858379542653,37.42214027058678,17 -122.0856732640519,37.42208690214408,17 " +
                "-122.0856022926407,37.42214885429042,17 -122.0855902778436,37.422128290487,17 " +
                "-122.0855841672237,37.42208171967246,17 -122.0854852065741,37.42210455874995,17 " +
                "-122.0855067264352,37.42214267949824,17 -122.0854430712915,37.42212783846172,17 " +
                "-122.0850990714904,37.42251282407603,17 -122.0856769818632,37.42281815323651,17 " +
                "-122.0860162273783,37.42244918858722,17 -122.0857260327004,37.42229239604253,17 " +
                "-122.0857412771483,37.42227033155257,17" +
                "</coordinates></LinearRing></outerBoundaryIs></Polygon></Placemark></Document></kml>";

            Assert.AreEqual(ans, ret);
        }

        [TestMethod()]
        public void KmlFile出力()
        {
            //データ準備
            KmlFile kmlFile = new KmlFile();

            Placemark<Point> placeMark = new Placemark<Point>("Simple placemark",
                "Attached to the ground. Intelligently places itself at the height of the underlying terrain.");
            placeMark.geometry = new Point(-122.0822035425683, 37.42228990140251, 0);

            kmlFile.item = placeMark;
                        
            int ret = kmlFile.WriteKml(new FileStream(@"..\..\..\TestData\testSample1.kml", FileMode.Create));

            Assert.AreEqual(0, ret);
        }

    }

}