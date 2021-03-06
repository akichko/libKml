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
