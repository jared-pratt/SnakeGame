using System.Text.Json.Serialization;

namespace CS3500.GUI.Client.Models
{

    /// <summary>
    /// Model for the Point2D class, representing a 2D point with X and Y component.
    /// </summary>
    public class Point2D
    {

        /// <summary>
        /// X component of the 2D point.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("X")]
        public int X { get; private set; }


        /// <summary>
        /// Y component of the 2D point
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Y")]
        public int Y { get; private set; }

        /// <summary>
        /// Null constructor for the 2D point object, used for JSON deserialization.
        /// </summary>
        public Point2D()
        {

        }
    }
}
