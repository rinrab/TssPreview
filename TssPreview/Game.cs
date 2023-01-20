using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace TssPreview
{
    [DataContract]
    public class Game
    {
        [DataMember(Name = "magic")]
        public string Magic { get; set; }

        [DataMember(Name = "version")]
        public int Version { get; set; }

        [DataMember(Name = "players")]
        public Boat[] Boats { get; set; }

        [DataMember(Name = "width")]
        public int Width { get; set; }
        [DataMember(Name = "height")]
        public int Height { get; set; }

        [DataMember(Name = "wind")]
        public int[] Wind { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        public int TurnCount
        {
            get
            {
                return Boats[0].Turns.Length;
            }
        }

        [DataMember(Name = "marks")]
        public Mark[] Marks { get; set; }

        public Game()
        {
            Boats = new Boat[0];
        }

        public static Game Load(string path)
        {
            using (FileStream file = File.OpenRead(path))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Game));
                return serializer.ReadObject(file) as Game;
            }
        }
    }
}
