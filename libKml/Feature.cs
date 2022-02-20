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
