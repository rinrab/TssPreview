using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows.Media;

namespace TssPreview
{
    public class Boat
    {
        [JsonPropertyName("turns")]
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

        [JsonPropertyName("x")]
        public float X { get; set; }

        [JsonPropertyName("y")]
        public float Y { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("tack")]
        public bool tack { get; set; }

        [JsonPropertyName("color")]
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

    public class Story
    {
        [JsonPropertyName("points")]
        public Point[]? Points { get; set; }

        public Story() { }
    }

    public class Point
    {
        [JsonPropertyName("x")]
        public float X { get; set; }

        [JsonPropertyName("y")]
        public float Y { get; set; }

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Point() { }
    }
}
