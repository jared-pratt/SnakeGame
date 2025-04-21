namespace CS3500.GUI.Client.Controllers
{
    /// <summary>
    /// Class to contain methods for controlling the request for movement sent by client.
    /// Takes in the network connection and sends request to server.
    /// </summary>
    public static class KeyControllers
    {
        /// <summary>
        /// Method to send server request to move up.
        /// </summary>
        /// <param name="network">NetworkConnection used to send data</param>
        public static void Up(NetworkConnection network)
        {
            network.Send("{\"moving\":\"up\"}");
        }

        /// <summary>
        /// Method to send server request to move down.
        /// </summary>
        /// <param name="network">NetworkConnection used to send data</param>
        public static void Down(NetworkConnection network)
        {
            network.Send("{\"moving\":\"down\"}");
        }

        /// <summary>
        /// Method to send server request to move left.
        /// </summary>
        /// <param name="network">NetworkConnection used to send data</param>
        public static void Left(NetworkConnection network)
        {
            network.Send("{\"moving\":\"left\"}");
        }

        /// <summary>
        /// Method to send server request to move right.
        /// </summary>
        /// <param name="network">NetworkConnection used to send data</param>
        public static void Right(NetworkConnection network)
        {
            network.Send("{\"moving\":\"right\"}");
        }

        /// <summary>
        /// Method to send server stating no movement.
        /// </summary>
        /// <param name="network">NetworkConnection used to send data</param>
        public static void None(NetworkConnection network)
        {
            network.Send("{\"moving\":\"none\"}");
        }
    }
}
