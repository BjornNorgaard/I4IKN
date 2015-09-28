using System;
using System.Net.Sockets;

namespace tcp
{
    class FileClient
    {
        /// <summary>
        /// The PORT.
        /// </summary>
        const int Port = 9000;
        /// <summary>
        /// The BUFSIZE.
        /// </summary>
        const int Bufsize = 1000;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileClient"/> class.
        /// </summary>
        /// <param name='args'>
        /// The command-line arguments. First ip-adress of the server. Second the filename
        /// </param>
        private FileClient(string[] args)
        {
            // TO DO Your own code
            TcpListener serverSocket = new TcpListener(Port);

            int requestCount = 0;

            TcpClient clientSocket = new TcpClient();
        }

        /// <summary>
        /// Receives the file.
        /// </summary>
        /// <param name='fileName'>
        /// File name.
        /// </param>
        /// <param name='io'>
        /// Network stream for reading from the server
        /// </param>
        private void ReceiveFile(String fileName, NetworkStream io)
        {
            // TO DO Your own code
        }

        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name='args'>
        /// The command-line arguments.
        /// </param>
        public static void Main(string[] args)
        {
            Console.WriteLine("Client starts...");
            new FileClient(args);
        }
    }
}
