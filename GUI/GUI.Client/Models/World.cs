namespace CS3500.GUI.Client.Models
{
    /// <summary>
    /// Represents all of the objects in the game.
    /// </summary>
    public class World
    {
        /// <summary>
        /// The players of the game.
        /// </summary>
        public Dictionary<int, Snake> Players;

        /// <summary>
        /// The powerups in the game.
        /// </summary>
        public Dictionary<int, Powerup> Powerups;

        /// <summary>
        /// The walls in the game.
        /// </summary>
        public Dictionary<int, Wall> Walls;

        /// <summary>
        /// The size of a single side of the square world.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Creates a new world with the given size.
        /// </summary>
        /// <param name="_size">Size of the world to be created</param>
        public World(int _size)
        {
            Players = new Dictionary<int, Snake>();
            Powerups = new Dictionary<int, Powerup>();
            Walls = new Dictionary<int, Wall>();
            Size = _size;
        }

        /// <summary>
        /// Shallow copy constructor.
        /// </summary>
        /// <param name="world">Existing world object to copy</param>
        public World(World world)
        {
            Players = new(world.Players);
            Powerups = new(world.Powerups);
            Walls = new(world.Walls);
            Size = world.Size;
        }
    }
}
