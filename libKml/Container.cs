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
