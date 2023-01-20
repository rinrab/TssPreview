using System.Runtime.Serialization;
using System.Xml.Linq;

namespace TssPreview
{
    [DataContract]
    public class Mark
    {
        [DataMember(Name = "x")]
        public float X { get; set; }

        [DataMember(Name = "y")]
        public float Y { get; set; }

        public Mark(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Mark() { }
    }
}
