using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace libKml
{
    public abstract class Container : Feature
    {
        public List<Feature> features;


        public Container(string name = null, string description = null) : base(name, description) { }


        public void AddFeature(Feature feature)
        {
            if (features == null)
                features = new List<Feature>();

            features.Add(feature);
        }

        public override void Write(XmlWriter writer)
        {
           base.Write(writer);
        }

    }

    public class Folder : Container
    {

        protected override string TagName => "Folder";

        public Folder(string name = null, string description = null) : base(name, description) { }



        public override void Write(XmlWriter writer)
        {
            base.Write(writer); //includes WriteStartElement

            features.ForEach(x => x.Write(writer));

            writer.WriteEndElement();
        }

    }


    public class Document : Container
    {

        protected override string TagName => "Document";

        public Document(string name = null, string description = null) : base(name, description) { }



        public override void Write(XmlWriter writer)
        {
            base.Write(writer); //includes WriteStartElement

            features.ForEach(x => x.Write(writer));

            writer.WriteEndElement();
        }

    }

}
