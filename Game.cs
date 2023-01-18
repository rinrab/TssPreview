using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace TssPreview
{
    public class Game
    {
        [JsonPropertyName("magic")]
        public string Magic { get; set; }

        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("players")]
        public Boat[] Boats { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }
        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("wind")]
        public int[] Wind { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }

        //[JsonPropertyName("turncount")]
        [JsonIgnore]
        public int TurnCount { 
            get
            {
                return Boats[0].Turns.Length;
            }
        }

        [JsonPropertyName("marks")]
        public Mark[] Marks { get; set; }

        public Game()
        {
            Boats = new Boat[0];
        }

        public static Game Load(string path)
        {
            using FileStream file = File.OpenRead(path);
            return JsonSerializer.Deserialize<Game>(file);
        }
    }
}
