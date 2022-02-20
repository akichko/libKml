using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace libKml
{
    public abstract class Overlay : Feature
    {
        public string color;
        public int? drawOrder;
        public Icon icon;

        public Overlay(string name = null, string description = null) : base(name, description) { }



        public override void Write(XmlWriter writer)
        {
            base.Write(writer);
            icon?.Write(writer);
        }

    }

    public class Icon
    {
        public string href;

        public Icon(string href)
        {
            this.href = href;
        }
        public void Write(XmlWriter writer)
        {
            writer.WriteStartElement("Icon");

            if (href != null) writer.WriteElementString("href", href);

            writer.WriteEndElement(); //Icon
        }
    }


    public class GroundOverlay : Overlay
    {
        public LatLonBox latlonBox;

        protected override string TagName => "GroundOverlay";
        public GroundOverlay(string name = null, string description = null) : base(name, description) { }

        public override void Write(XmlWriter writer)
        {
            base.Write(writer);  //includes WriteStartElement

            latlonBox?.Write(writer);


            writer.WriteEndElement();

        }
    }
}
