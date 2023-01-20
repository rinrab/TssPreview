using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows.Media;

namespace TssPreview
{
    [DataContract]
    public class Boat
    {
        [DataMember(Name = "turns")]
        public Story[] turns { get; set; }
        public Story[] Turns
        {
            get
            {
                if (turns.Length == 0)
                {
                    return new Story[] {
                        new Story()
                        {
                            Points = new Point[]
                            {
                                new Point(X, Y)
                            }
                        }
                    };
                }
                else
                {
                    return turns;
                }
            }
        }

        [DataMember(Name = "x")]
        public float X { get; set; }

        [DataMember(Name = "y")]
        public float Y { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "tack")]
        public bool tack { get; set; }

        [DataMember(Name = "color")]
        public string Color { get; set; }

        public Color GetColor()
        {
            Dictionary<string, Color> colors = new Dictionary<string, Color>
            {
                { "red", Colors.Red },
                { "blue", Colors.Blue },
                { "black", Colors.Black },
                { "green", Colors.Green },
                { "cyan", Colors.Cyan },
                { "magenta", Colors.Magenta },
                { "purple", Colors.Purple },
                { "gray", Colors.Gray },
                { "yellow", Colors.Yellow },
                { "darkred", Colors.DarkRed },
                { "darkblue", Colors.DarkBlue },
                { "goldenrod", Colors.Goldenrod }
            };

            return colors[Color];
        }

        public Boat(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Boat() { }
    }

    [DataContract]
    public class Story
    {
        [DataMember(Name = "points")]
        public Point[] Points { get; set; }

        public Story() { }
    }

    [DataContract]
    public class Point
    {
        [DataMember(Name = "x")]
        public float X { get; set; }

        [DataMember(Name = "y")]
        public float Y { get; set; }

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Point() { }
    }
}
