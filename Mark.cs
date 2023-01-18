using System.Text.Json.Serialization;

namespace TssPreview
{
    public class Mark
    {
        [JsonPropertyName("x")]
        public float X { get; set; }

        [JsonPropertyName("y")]
        public float Y { get; set; }

        public Mark(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Mark() { }
    }
}
