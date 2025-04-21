using System.Text.Json.Serialization;

namespace CS3500.GUI.Client.Models
{
    /// <summary>
    /// Model to represent a snake object, players in the game, with various properties.
    /// </summary>
    public class Snake
    {
        /// <summary>
        /// Unique identifier for the snakes.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("snake")]
        public int id { get; private set; }

        /// <summary>
        /// Name given by client to the snake.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("name")]
        public string name { get; private set; }

        /// <summary>
        /// Body of the snake, a list of coordinates representing its vertices.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("body")]
        public List<Point2D> body { get; private set; }

        /// <summary>
        /// Direction the snake is currently moving. This is changed by the player by requesting movement from the server.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("dir")]
        public Point2D dir { get; private set; }

        /// <summary>
        /// Number representing the snake's current score (# of powerups eaten).
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("score")]
        public int score { get; private set; }

        /// <summary>
        /// Value representing if the snake has died.
        /// </summary>
        [JsonPropertyName("died")]
        [JsonInclude]
        public bool died { get; private set; }

        /// <summary>
        /// Value representing if the snake is living.
        /// </summary>
        [JsonPropertyName("alive")]
        [JsonInclude]
        public bool alive { get; private set; }

        /// <summary>
        /// Value representing if the snake ahs been disconnected from the game.
        /// </summary>
        [JsonPropertyName("dc")]
        [JsonInclude]
        public bool disconnected { get; private set; }

        /// <summary>
        /// Value indicating if the player has joined on that frame.
        /// </summary>
        [JsonPropertyName("join")]
        [JsonInclude]
        public bool join { get; private set; }

        /// <summary>
        /// Null constructor used by JSON deserializer to create a snake object with all necessary fields.
        /// </summary>
        public Snake()
        {
        }
    }
}
