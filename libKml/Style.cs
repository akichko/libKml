using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace libKml
{
    public abstract class ColorStyle : KmlObj
    {
        public string color;
        public colorModeEnum? colorMode;

        protected ColorStyle(string color = null, colorModeEnum? colorMode = null)
        {
            this.color = color;
            this.colorMode = colorMode;
        }

        public override void Write(XmlWriter writer)
        {
            base.Write(writer); //WriteStartElement

            if (color != null)
                writer.WriteElementString("color", color);
            if (colorMode != null)
                writer.WriteElementString("colorMode", colorMode.ToString());
        }

    }


    public abstract class StyleSelector : KmlObj { }


    public class Style : StyleSelector
    {
        //public IconStyle iconStyle;
        //public LabelStyle labelStyle;
        public LineStyle lineStyle;
        public PolyStyle polyStyle;
        //public BalloonStyle balloonStyle;
        //public ListStyle listStyle;

        protected override string TagName => "Style";

        public Style() { }

        public Style(string id, LineStyle lineStyle = null, PolyStyle polyStyle = null)
        {
            this.id = id;
            this.lineStyle = lineStyle;
            this.polyStyle = polyStyle;
        }


        public Style(string id, List<ColorStyle> styleList)
        {
            this.id = id;
            this.lineStyle = (LineStyle)styleList.Where(x => x is LineStyle).FirstOrDefault();
            this.polyStyle = (PolyStyle)styleList.Where(x => x is PolyStyle).FirstOrDefault();
        }


        public override void Write(XmlWriter writer)
        {
            base.Write(writer); //WriteStartElement

            lineStyle.Write(writer);
            polyStyle.Write(writer);

            writer.WriteEndElement(); //Style

        }
    }


    public class LineStyle : ColorStyle
    {
        public float? width;

        protected override string TagName => "LineStyle";

        public LineStyle(string color = null, colorModeEnum? colorMode = null, float? width = null) : base(color, colorMode)
        {
            this.width = width;
        }

        public LineStyle(string color = null, float? width = null)
        {
            this.color = color;
            this.width = width;
        }

        public override void Write(XmlWriter writer)
        {
            base.Write(writer); //WriteStartElement

            if (width != null)
                writer.WriteElementString("width", width.ToString());

            writer.WriteEndElement(); //tagName
        }
    }


    public class PolyStyle : ColorStyle
    {
        bool? fill;
        bool? outline;

        protected override string TagName => "PolyStyle";
        public PolyStyle(string color = null, colorModeEnum? colorMode = null, bool? fill = null, bool? outline = null) : base(color, colorMode)
        {
            this.fill = fill;
            this.outline = outline;
        }

        public override void Write(XmlWriter writer)
        {
            base.Write(writer); //WriteStartElement

            if (fill != null)
                writer.WriteElementString("fill", fill.ToString());
            if (outline != null)
                writer.WriteElementString("outline", color);

            writer.WriteEndElement(); //tagName
        }
    }



    public enum colorModeEnum
    {
        normal,
        random
    }
}
