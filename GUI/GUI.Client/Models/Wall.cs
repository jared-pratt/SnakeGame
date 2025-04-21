using System.Text.Json.Serialization;

namespace CS3500.GUI.Client.Models
{
    /// <summary>
    /// Model for the game walls. Including id and wall coordinates.
    /// </summary>
    public class Wall
    {
        /// <summary>
        /// Unique identifier for the wall objects used for drawing the game.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("wall")]
        public int id { get; private set; }

        /// <summary>
        /// 2D point representing the first coordinate of the wall.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("p1")]
        public Point2D p1 { get; private set; }

        /// <summary>
        /// 2D point representing the second coordinate of the wall.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("p2")]
        public Point2D p2 { get; private set; }

        /// <summary>
        /// Null constructor for the wall object, used by JSON deserializer to create the walls.
        /// </summary>
        public Wall()
        { }
    }
}
