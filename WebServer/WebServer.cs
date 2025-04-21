// Code implemented by Jared Pratt and Grant Beck
// December 5, 2024

using CS3500.GUI.Client.Controllers;
using CS3500.Networking;
using MySql.Data.MySqlClient;

namespace WebServer
{
    /// <summary>
    /// Class for the web server. This server serves the pages that contain 
    /// information on games and players that are stored in our database. Uses
    /// TCP and HTTP for transmission and transfer of data.
    /// </summary>
    public static class WebServer
    {
        /// <summary>
        /// HTTP header for an OK response.
        /// </summary>
        private const string httpOkHeader =
            "HTTP/1.1 200 OK\r\n" +
            "Connection: close\r\n" +
            "Content-Type: text/html; charset=UTF-8\r\n" +
            "\r\n";

        /// <summary>
        /// HTTP header for a bad response.
        /// </summary>
        private const string httpBadHeader =
            "HTTP/1.1 404 Not Found\r\n" +
            "Connection: close\r\n" +
            "Content-Type: text/html; charset=UTF-8\r\n" +
            "\r\n";

        /// <summary>
        /// Run when the program starts. Starts the server with the provided handler on port 80.
        /// </summary>
        static void Main(string[] args)
        {
            Server.StartServer(HandleHttpConnection, 80);
            Console.Read();
        }

        /// <summary>
        /// Handler for the server. This will take care of receiving and sending requests and responses.
        /// </summary>
        /// <param name="client">NetworkConnection object from the
        /// Library previously created. Wraps a TCP connection.</param>
        private static void HandleHttpConnection(NetworkConnection client)
        {
            // Connection string used to connect to the database with required credentials.
            string connectionString = "server=atr.eng.utah.edu;" +
                "database=u1350242;" +
                "uid=u1350242;" +
                "password=d0MMypASs;" +
                "Pooling=true;";

            string request = String.Empty;

            try
            {
                request = client.ReadLine();
            }
            catch (Exception ex)
            {
            }

            // Return the home page
            if (request.Contains("GET / "))
            {
                string response = String.Empty;

                response += "<html>";
                response += "<h3>Welcome to the Snake Games Database!</h3>";
                response += "<a href=\"/games\">View Games</a>";
                response += "</html>";

                client.Send(httpOkHeader + response);
            }

            // Return a page displaying game information and players for a specific game
            else if (request.Contains("GET /games?gid="))
            {
                // Parse the game number from the request
                string gid = request.Split('=')[1].Split(' ')[0];
                string response = String.Empty;

                // Create connection to database
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand getPlayers = conn.CreateCommand();

                        // Get the player information from the given game id
                        getPlayers.CommandText = $"select PlayerID, PlayerName, MaxScore," +
                            $" EnterTime, LeaveTime from Players where GameID = {gid};";

                        using (MySqlDataReader reader = getPlayers.ExecuteReader())
                        {
                            // HTML for the page with the query results
                            response += "<html>";
                            response += $"<h2>Stats for game {gid}</h2>";
                            response += "<table border=\"1\">";

                            response += "<thead><tr><td>Player ID</td><td>Player Name</td><td>Max " +
                                "Score</td><td>Enter Time</td><td>Leave Time</td></tr></thead>";

                            response += "<tbody>";
                            while (reader.Read())
                            {
                                response += "<tr><td>";
                                response += reader["PlayerID"];
                                response += "</td><td>";
                                response += reader["PlayerName"];
                                response += "</td><td>";
                                response += reader["MaxScore"];
                                response += "</td><td>";
                                response += reader["EnterTime"];
                                response += "</td><td>";
                                response += reader["LeaveTime"];
                                response += "</td></tr>";
                            }
                            response += "</tbody>";

                            response += "</table>";
                            response += "</html>";
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                // Send the ok header with the response containing the data
                client.Send(httpOkHeader + response);
            }

            // Return a page for all the games in the database
            else if (request.Contains("GET /games "))
            {
                string response = String.Empty;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand getGames = conn.CreateCommand();

                        // Get all games in the database
                        getGames.CommandText = $"select * from Games;";

                        using (MySqlDataReader reader = getGames.ExecuteReader())
                        {
                            response += "<html>";
                            response += "<table border=\"1\">";
                            response += "<thead><tr><td>Game ID</td><td>Start Time</td><td>End Time</td></tr></thead>";

                            response += "<tbody>";
                            while (reader.Read())
                            {
                                response += "<tr><td>";
                                response += "<a href =\"/games?gid=" + reader["GameID"] + "\">" + reader["GameID"] + "</a>";
                                response += "</td><td>";
                                response += reader["GameStart"];
                                response += "</td><td>";
                                response += reader["GameEnd"];
                                response += "</td></tr>";
                            }
                            response += "</tbody>";

                            response += "</table>";
                            response += "</html>";
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                // Send the ok header and the data as before
                client.Send(httpOkHeader + response);
            }
            else
            {
                // If no existing page was requested, send the bad headed with 404 error and display 404 page
                client.Send(httpBadHeader + "<html lang=\"en\"> <title>404 Error</title> <img " +
                    "src=\"https://www.blackhillsinfosec.com/wp-content/uploads/2016/07/66619265.jpg\" " +
                    "alt=\"Fun 404 image\"> <p>Don't worry, you can always find your way back home.</p> " +
                    "<a href=\"/\">Go Back to Homepage</a></html>");
            }

            // Once the data has been transferred, disconnect the client and return to listening for requests
            client.Disconnect();
        }
    }
}

