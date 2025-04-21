// Code implemented by Jared Pratt and Grant Beck
// December 5, 2024

using CS3500.GUI.Client.Models;
using GUI.Client.Pages;
using System.Text.Json;

namespace CS3500.GUI.Client.Controllers
{
    /// <summary>
    /// Controller for handling network related operation. Parses data from the server for the game.
    /// </summary>
    public static class NetworkController
    {
        /// <summary>
        /// Dictionary to store the high scores for each of the players.
        /// </summary>
        private static Dictionary<int, int> highScores = new Dictionary<int, int>();

        /// <summary>
        /// Method to establish connection to the server.
        /// </summary>
        /// <param name="network">The NetworkConnection object representing a connection with the server</param>
        /// <param name="GUI">Razor component</param>
        /// <param name="ServerNameOrAddress">Default server name</param>
        /// <param name="ServerPort">Default server port</param>
        /// <param name="Name">The input player name</param>
        /// <param name="id">Id of the current network connection</param>
        /// <param name="WorldSize">Size of the world, as determined by the server</param>
        /// <param name="TheWorld">The (M)odel part of MVC, represents the objects in the game</param>
        /// <param name="AcceptingUserInput">Variable to track if the game should be receiving user input at any given moment</param>
        public static void ConnectToServer(NetworkConnection network, SnakeGUI GUI, string ServerNameOrAddress, int ServerPort, string Name, out int id, int WorldSize, out World TheWorld, out bool AcceptingUserInput)
        {
            network.Connect(ServerNameOrAddress, ServerPort);
            GUI.StateChanged();
            network.Send(Name);
            id = int.Parse(network.ReadLine());
            WorldSize = int.Parse(network.ReadLine());
            TheWorld = new World(WorldSize);
            string current = "";

            // Parse the wall data, then set user input to true and enter loop to parse powerup and snake for the rest of the connection
            while (network.IsConnected)
            {
                try
                {
                    current = network.ReadLine();

                    // Parse wall data
                    if (current.Contains("\"wall\""))
                    {
                        Wall? wall = JsonSerializer.Deserialize<Wall>(current);
                        if (wall != null)
                        {
                            lock (network)
                            {
                                TheWorld.Walls.Add(wall.id, wall);
                            }
                        }
                    }
                    else
                    {
                        AcceptingUserInput = true;
                        break;
                    }
                }
                catch { }
            }

            // Parse powerups and snake until connection is ended
            while (network.IsConnected)
            {
                try
                {
                    // Parse snake data
                    if (current.Contains("\"snake\""))
                    {
                        Snake? snake = JsonSerializer.Deserialize<Snake>(current);
                        if (snake != null)
                        {
                            lock (network)
                            {
                                if (TheWorld.Players.ContainsKey(snake.id))
                                {


                                    if (snake.disconnected)
                                    {
                                        TheWorld.Players.Remove(snake.id);
                                        DataBaseController.EndSnake(snake.id);
                                    }
                                    else
                                    {
                                        // Check if the current score is greater than the high score, if so update it in the db
                                        if (snake.score > highScores[snake.id])
                                        {
                                            highScores[snake.id] = snake.score;
                                            DataBaseController.UpdateMaxScore(snake.id, snake.score);
                                        }
                                        TheWorld.Players[snake.id] = snake;
                                    }
                                }
                                else
                                {
                                    highScores.Add(snake.id, 0);
                                    TheWorld.Players.Add(snake.id, snake);
                                    DataBaseController.InsertSnake(snake.id, snake.name);
                                }
                            }
                        }
                    }

                    // Parse powerup data
                    else if (current.Contains("\"power\""))
                    {
                        Powerup? powerup = JsonSerializer.Deserialize<Powerup>(current);
                        if (powerup != null)
                        {
                            lock (network)
                            {
                                if (TheWorld.Powerups.ContainsKey(powerup.power))
                                {
                                    if (powerup.died)
                                    {
                                        TheWorld.Powerups.Remove(powerup.power);
                                    }
                                }
                                else
                                {
                                    TheWorld.Powerups.Add(powerup.power, powerup);
                                }
                            }
                        }
                    }
                    current = network.ReadLine() ?? string.Empty;
                }
                catch
                {
                    break;
                }
            }
            AcceptingUserInput = false;
        }

        /// <summary>
        /// Disconnect the network object from the server.
        /// </summary>
        public static void DisconnectFromServer(NetworkConnection network, out bool AcceptingUserInput)
        {
            network.Disconnect();
            AcceptingUserInput = false;
            DataBaseController.EndGame();
        }

        /// <summary>
        /// Handle user input. Move based on predefined keys and send to server.
        /// </summary>
        /// <param name="network">NetworkConnection object</param>
        /// <param name="key">Key value pressed by user</param>
        public static void HandleInput(NetworkConnection network, string key)
        {
            if (key.Equals("w"))
            {
                KeyControllers.Up(network);
            }
            else if (key.Equals("s"))
            {
                KeyControllers.Down(network);
            }
            else if (key.Equals("a"))
            {
                KeyControllers.Left(network);
            }
            else if (key.Equals("d"))
            {
                KeyControllers.Right(network);
            }
            else
            {
                KeyControllers.None(network);
            }
        }

    }
}
