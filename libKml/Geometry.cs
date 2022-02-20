/*============================================================================
MIT License

Copyright (c) 2022 akichko

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace libKml
{
    public abstract class Geometry : KmlObj { }


    public class Point : Geometry
    {
        Coordinate coordinate;

        public Point() { }
        
        public Point(double lon, double lat, double? z = null)
        {
            coordinate = new Coordinate(lon, lat, z);
        }

        protected override string TagName => "Point";
        
        public override void Write(XmlWriter writer)
        {
            //writer.WriteStartElement("Point");
            base.Write(writer);

            coordinate.Write(writer);

            writer.WriteEndElement(); //Point

        }
    }

    public class LineString : Geometry
    {
        public bool? extrude;
        public bool? tessellate;
        public AltitudeMode? altitudeMode;
        public int? drawOrder;
        public List<Coordinate> coordinates;
        protected override string TagName => "LineString";

        public LineString()
        {
            //coordinates = new Coordinate(lon, lat, z);
        }

        public LineString(List<Coordinate> coordinates)
        {
            this.coordinates = coordinates;
        }

        public void SetGeometry(List<Coordinate> coordinates)
        {
            this.coordinates = coordinates;
        }

        public override void Write(XmlWriter writer)
        {
            base.Write(writer); //WriteStartElement

            if (extrude != null) writer.WriteElementString("extrude", extrude == true ? "1" : "0");
            if (tessellate != null) writer.WriteElementString("tessellate", tessellate == true ? "1" : "0");
            if (altitudeMode != null) writer.WriteElementString("altitudeMode", altitudeMode.ToString());

            writer.WriteStartElement("coordinates");
            foreach (var coordinate in coordinates.Take(1))
            {
                coordinate.WriteRaw(writer);
            }
            foreach (var coordinate in coordinates.Skip(1))
            {
                writer.WriteRaw(" ");
                coordinate.WriteRaw(writer);
            }
            writer.WriteEndElement(); //coordinates
            writer.WriteEndElement(); //LineString

        }
    }

    public class Polygon : Geometry
    {
        public bool? extrude;
        public bool? tessellate;
        public AltitudeMode? altitudeMode;
        public List<Coordinate> OuterBoundary { get; set; }
        public List<Coordinate> InnerBoundary { get; set; }
        protected override string TagName => "Polygon";

        public Polygon() { }


        public override void Write(XmlWriter writer)
        {
            base.Write(writer); //WriteStartElement

            if (extrude != null) writer.WriteElementString("extrude", extrude == true ? "1" : "0");
            if (tessellate != null) writer.WriteElementString("tessellate", tessellate == true ? "1" : "0");
            if (altitudeMode != null) writer.WriteElementString("altitudeMode", altitudeMode.ToString());

            //outer
            if (OuterBoundary != null)
            {
                writer.WriteStartElement("outerBoundaryIs");
                writer.WriteStartElement("LinearRing");
                writer.WriteStartElement("coordinates");
                foreach (var coordinate in OuterBoundary.Take(1))
                {
                    coordinate.WriteRaw(writer);
                }
                foreach (var coordinate in OuterBoundary.Skip(1))
                {
                    writer.WriteRaw(" ");
                    coordinate.WriteRaw(writer);
                }
                writer.WriteEndElement(); //coordinates
                writer.WriteEndElement(); //LinearRing
                writer.WriteEndElement(); //outerBoundaryIs
            }


            //Inner
            if (InnerBoundary != null)
            {
                writer.WriteStartElement("innerBoundaryIs");
                writer.WriteStartElement("LinearRing");
                writer.WriteStartElement("coordinates");

                foreach (var coordinate in InnerBoundary.Take(1))
                {
                    coordinate.WriteRaw(writer);
                }
                foreach (var coordinate in InnerBoundary.Skip(1))
                {
                    writer.WriteRaw(" ");
                    coordinate.WriteRaw(writer);
                }

                writer.WriteEndElement(); //coordinates
                writer.WriteEndElement(); //LinearRing
                writer.WriteEndElement(); //innerBoundaryIs
            }

            writer.WriteEndElement(); //Point

        }

        public void SetOuterBoundary(List<Coordinate> coordinates)
        {
            OuterBoundary = coordinates;
        }
        public void SetInnerBoundary(List<Coordinate> coordinates)
        {
            InnerBoundary = coordinates;
        }

    }


    public class Coordinate
    {
        private double lon;
        private double lat;
        private double? z;


        public Coordinate(double lon, double lat, double? z = null)
        {
            this.lon = lon;
            this.lat = lat;
            this.z = z;
        }

        public void Write(XmlWriter writer)
        {
            if (z != null)
                writer.WriteElementString("coordinates", $"{lon},{lat},{z}");
            else
                writer.WriteElementString("coordinates", $"{lon},{lat}");
        }

        public void Write2(Stream stream)
        {
            XmlDocument xd = new XmlDocument();
            XmlElement xElem = xd.CreateElement("coordinates");

            if (z != null)
            {
                xElem.InnerText = $"{lon},{lat},{z}";
                XmlSerializer xr = new XmlSerializer(typeof(XmlElement));
                    xr.Serialize(stream, xElem);
            }
            else
            {
                xElem.InnerText = $"{lon},{lat}";
                XmlSerializer xr = new XmlSerializer(typeof(XmlElement));
                xr.Serialize(stream, xElem);

            }
        }


        public void Write3(XmlWriter writer)
        {
            XElement xElem = new XElement("coordinates");

            if (z != null)
                xElem.Value = $"{lon},{lat},{z}";
            else
                xElem.Value = $"{lon},{lat}";
            xElem.Save(writer);
        }

        public void WriteRaw(XmlWriter writer)
        {
            writer.WriteRaw($"{lon},{lat},{z}");
        }

    }


    public class LatLonBox : KmlObj
    {
        double north;
        double south;
        double east;
        double west;
        double rotation;

        protected override string TagName => "LatLonBox";
        
        public LatLonBox(double north, double south, double east, double west, double rotation = 0)
        {
            this.north = north;
            this.south = south;
            this.east = east;
            this.west = west;
            this.rotation = rotation;
        }

        public override void Write(XmlWriter writer)
        {
            base.Write(writer);

            writer.WriteElementString("north", $"{north}");
            writer.WriteElementString("south", $"{south}");
            writer.WriteElementString("east", $"{east}");
            writer.WriteElementString("west", $"{west}");
            writer.WriteElementString("rotation", $"{rotation}");

            writer.WriteEndElement(); //LatLonBox

        }

    }
}
