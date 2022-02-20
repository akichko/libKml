using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace libKml
{
    public enum ContainerType
    {
        Folder,
        Document
    }

    public enum PlacemarkType
    {
        Point,
        Line,
        Polygon
    }

    public enum AltitudeMode
    {
        [Description("clampToGround")] clampToGround,
        [Description("relativeToGround")] relativeToGround,
        [Description("absolute")] absolute
    }

    class AttibuteTypes
    {
    }
}
