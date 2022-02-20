using System;
using System.Collections.Generic;
using System.Text;
using Akichko.libGis;
using System.IO;
using System.Xml;
using System.Linq;

namespace libKml
{
    public abstract class KmlObj : KmlElement
    {
        public string id;
        protected abstract string TagName { get; }

        public override void Write(XmlWriter writer)
        {
            writer.WriteStartElement(TagName);
            if(id != null) writer.WriteAttributeString("id", id);

            //WriteEndElementは呼び出し元で実行
        }
    }

    public class KmlElement
    {
        public string tagName;
        public string text;
        public List<KmlElement> elements;


        public KmlElement(string tagName = null, string text = null)
        {
            this.tagName = tagName;
            this.text = text;
        }

        public virtual void Write(XmlWriter writer)
        {
            if(tagName != null)
            {
                writer.WriteStartElement(tagName);

                elements?.ForEach(x => x.Write(writer));

                writer.WriteRaw(text);

                writer.WriteEndElement(); //tagName

            }

        }

        public KmlElement AddElement(KmlElement element)
        {
            if(elements == null)
                elements = new List<KmlElement>();

            elements.Add(element);
            return this;
        }

        public KmlElement AttachTo(KmlElement element)
        {
            element.AddElement(this);
            return this;
        }


        public T AttachTo<T>(KmlElement element) where T : KmlElement
        {
            element.AddElement(this);
            return (T)this;
        }
    }


    public class KmlFile
    {
        public string xmlns;
        public Feature item;

        public KmlFile(string xmlns = "http://www.opengis.net/kml/2.2")
        {
            this.xmlns = xmlns;
        }

        public int WriteKml(Stream stream, XmlWriterSettings settings=null)
        {
            if(settings == null)
            {
                settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "\t";
            }

            XmlWriter writer = XmlWriter.Create(stream, settings);

            writer.WriteStartElement("kml");
            writer.WriteAttributeString("dummy", "xmlns", null, xmlns);

            item?.Write(writer);
            
            writer.WriteEndElement(); //kml

            writer.Close();

            return 0;

        }


    }

}
