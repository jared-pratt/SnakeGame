// Code implemented by Jared Pratt and Grant Beck
// December 5, 2024

using GUI.Client.Pages;
using MySql.Data.MySqlClient;

namespace CS3500.GUI.Client.Controllers
{
    /// <summary>
    /// Contains all queries for adding or updating rows in the Players or Games databases.
    /// </summary>
    public class DataBaseController
    {
        /// <summary>
        /// The connection string.
        /// Your uID login name serves as both your database name and your uid
        /// </summary>
        public const string connectionString = "server=atr.eng.utah.edu;" +
          "database=u1350242;" +
          "uid=u1350242;" +
          "password=d0MMypASs;" +
          "Pooling=true;";

        /// <summary>
        ///  Adds a row to the Games database for the current game ID with the time that 
        ///  the connection to the game was made.
        /// </summary>
        /// <param name="gameId"></param>
        public static void InsertGame(out int gameId)
        {
            gameId = -1;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    MySqlCommand addGame = conn.CreateCommand();

                    addGame.CommandText = $"insert into Games (GameStart) values (\"{DateTime.Now.
                        ToString("yyyy-MM-dd H:mm:ss")}\");";

                    addGame.ExecuteNonQuery();

                    MySqlCommand getGameId = conn.CreateCommand();

                    // Gets the last number that the auto-increment produced for the gameID variable
                    getGameId.CommandText = "select last_insert_id();";

                    // Execute the command and cycle through the DataReader object
                    using (MySqlDataReader reader = getGameId.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            gameId = reader.GetInt32(0);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        ///  Adds a row to the Players database for the current player ID, game ID player
        ///  name, max score, and the time that they entered the game. The game ID is the 
        ///  same as what is inserted into the Games table for the player joining.
        /// </summary>
        /// <param name="playerId">The current snake ID.</param>
        /// <param name="gameId">The current game ID</param>
        /// <param name="name">The inputted name from the user.</param>
        /// <param name="maxScore">The maximum amount of powerups the user collects by the time they have died.</param>
        public static void InsertSnake (int playerId, string name)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    MySqlCommand addSnake = conn.CreateCommand();

                    // Query for adding the new snake to the table
                    addSnake.CommandText = $"insert into Players (PlayerID, GameID, PlayerName, MaxScore, EnterTime) values ({playerId}, {SnakeGUI.currentGameId}, \"{name}\", 0, \"{DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")}\");";

                    addSnake.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Adds a leave time to the Players database for the current player when they disconnect.
        /// </summary>
        /// <param name="playerId">The current player ID.</param>
        public static void EndSnake(int playerId)
        {
            // Connect to the DB
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    MySqlCommand deleteSnake = conn.CreateCommand();

                    // Updates the leave time for the current player in the table.
                    deleteSnake.CommandText = $"update Players set LeaveTime = \"{DateTime.Now.
                        ToString("yyyy-MM-dd H:mm:ss")}\" where PlayerID = {playerId};";

                    deleteSnake.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Updates the Players table for the current player ID with their max score when they died. 
        /// </summary>
        /// <param name="playerId">The current player ID.</param>
        /// <param name="maxScore">The maximum amount of powerups the user collects by the time they have died.</param>
        public static void UpdateMaxScore(int playerId, int maxScore)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    MySqlCommand updateMax = conn.CreateCommand();

                    updateMax.CommandText = $"update Players set MaxScore = {maxScore} where PlayerID = {playerId};";

                    updateMax.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Adds a leave time to the Players table when the game is disconnected,
        /// thus forcing all players to "leave" at the same time.
        /// </summary>
        public static void EndGame()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    string dcTime = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss");

                    conn.Open();

                    MySqlCommand deleteGame = conn.CreateCommand();

                    // Query for updating the Players database for all players left in the game when the game is disconnected
                    deleteGame.CommandText = $"update Games set GameEnd = \"{dcTime}\" where GameID = {SnakeGUI.currentGameId}; " +
                        $"update Players set LeaveTime = \"{dcTime}\" where (LeaveTime is NULL and GameID = {SnakeGUI.currentGameId});";

                    deleteGame.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
