using System.Text.Json.Serialization;

namespace CS3500.GUI.Client.Models
{
    /// <summary>
    /// Model to represent the powerups for the game. Has an id, location, and state of life.
    /// </summary>
    public class Powerup
    {

        /// <summary>
        /// Unique identifier for the powerup object.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("power")]
        public int power { get; private set; }

        /// <summary>
        /// Location of the powerup to be drawn.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("loc")]
        public Point2D location { get; private set; }

        /// <summary>
        /// Value representing if the powerup is still "alive".
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("died")]
        public bool died { get; private set; }


        /// <summary>
        /// Null constructor used by JSON deserializer to create the powerup object.
        /// </summary>
        public Powerup()
        {

        }
    }
}
