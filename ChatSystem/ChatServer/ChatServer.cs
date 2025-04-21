// <copyright file="ChatServer.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// Skeleton code provided by CS3500, implemented by Jared Pratt and Grant Beck
// November 7, 2024

using CS3500.Networking;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks.Dataflow;
using System.Xml.Linq;

namespace CS3500.Chatting;

/// <summary>
///   A simple ChatServer that handles clients separately and replies with a static message.
/// </summary>
public partial class ChatServer
{
    /// <summary>
    /// List containing user object, which store information about connection, name, etc.
    /// </summary>
    private static List<User> users = new();

    /// <summary>
    ///   The main program.
    /// </summary>
    /// <param name="args"> ignored. </param>
    /// <returns> A Task. Not really used. </returns>
    private static void Main(string[] args)
    {
        Server.StartServer(HandleConnect, 11_000);
        Console.Read(); // don't stop the program.
    }

    /// <summary>
    /// Private wrapper class for a NetworkConnection to store information used by the chat like name, number of lines sent, etc.
    /// </summary>
    private class User
    {
        /// <summary>
        /// Member variable used to store the name of a client connected to the chat.
        /// </summary>
        public string userName = string.Empty;

        /// <summary>
        /// NetworkConnection associated with the user.
        /// </summary>
        public NetworkConnection _connection;

        /// <summary>
        /// Number of lines that the client has sent. Helpful for knowing what the first line sent is so we can associate a name with the client.
        /// </summary>
        public int lineCount = 0;

        /// <summary>
        /// Constructor for a User. Stores information necessary for the chat.
        /// </summary>
        /// <param name="connection">NetworkConnection to be used with this user instance.</param>
        public User(NetworkConnection connection)
        {
            _connection = connection;
        }
    }


    /// <summary>
    ///   <pre>
    ///     When a new connection is established, enter a loop that receives from and
    ///     replies to a client.
    ///   </pre>
    /// </summary>
    ///
    private static void HandleConnect(NetworkConnection connection)
    {
        User currentUser = new User(connection);

        // Lock the users while we change the list.
        lock (users)
        {
            users.Add(currentUser);
        }

        try// handle all messages until disconnect.
        {
            while (true)
            {
                string message;

                // Check if this is the first message that the user is sending.
                if (currentUser.lineCount == 0)
                {
                    currentUser.userName = currentUser._connection.ReadLine();
                    message = currentUser.userName + " has joined the chat.";
                }
                else
                {
                    message = currentUser.userName + ": " + currentUser._connection.ReadLine();
                }
                currentUser.lineCount++;

                // Lock for while we iterate through the users loop.
                lock (users)
                {
                    Broadcast(message);
                }
            }
        }
        catch (Exception)
        {
            string name = currentUser.userName;

            // Lock for when we remove the user from the user list and iterate through the list.
            lock (users)
            {
                users.Remove(currentUser);
                if (currentUser.userName != string.Empty)
                {
                    Broadcast(name + " has left the chat.");
                }
            }
            return;
        }
    }

    /// <summary>
    /// Private helper method to broadcast a message to all users.
    /// </summary>
    /// <param name="message">Message to be broadcast.</param>
    private static void Broadcast(string message)
    {
        foreach (User user in users)
        {
            user._connection.Send(message);
        }
    }


}