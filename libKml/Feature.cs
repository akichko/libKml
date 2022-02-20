using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace libKml
{


    public abstract class Feature : KmlObj
    {
        public string name;
        public bool? visibility;
        public bool? open;
        //author
        //href
        public string address;
        //AddressDetails
        public string phoneNumber;
        public string snippet;
        public string description;
        public string abstractView;
        public string timePrimitive;
        public string styleUrl;
        public List<StyleSelector> styleSelectors;
        public string region;
        public string metadata;
        public string extendedData;


        public Feature(string name = null, string description = null)
        {
            this.name = name;
            this.description = description;
        }

        public void AddStyle(StyleSelector style)
        {
            if (styleSelectors == null)
                styleSelectors = new List<StyleSelector>();

            styleSelectors.Add(style);
        }

        public override void Write(XmlWriter writer)
        {
            base.Write(writer);

            if (name != null)
                writer.WriteElementString("name", $"{name}");
            if (visibility != null)
                writer.WriteElementString("visibility", visibility == true ? "1" : "0");
            if (open != null)
                writer.WriteElementString("open", open == true ? "1" : "0");
            if (description != null)
                writer.WriteElementString("description", $"{description}");
            if (styleUrl != null)
                writer.WriteElementString("styleUrl", $"{styleUrl}");

            styleSelectors?.ForEach(x => x.Write(writer));

            elements?.ForEach(x => x.Write(writer));

        }

    }



}
