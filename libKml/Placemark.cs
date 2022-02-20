using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace libKml
{

    public class Placemark<TGeometry> : Feature where TGeometry : Geometry, new()
    {
        protected override string TagName => "Placemark";

        public TGeometry geometry;

        public Placemark(string name = null, string description = null) : base(name, description)
        {
            geometry = new TGeometry();
        }

        public Placemark<TGeometry> SetGeometry(TGeometry geometry)
        {
            this.geometry = geometry;
            return this;
        }


        public override void Write(XmlWriter writer)
        {
            base.Write(writer);

            geometry.Write(writer);

            writer.WriteEndElement(); //Placemark
        }
    }


}
