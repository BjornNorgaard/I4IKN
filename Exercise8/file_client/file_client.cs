using System;
using System.IO;
using System.Net.Sockets;
using LIB;

namespace file_client
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
            TcpClient clientSocket = new TcpClient();

            clientSocket.Connect(args[0], Port);    // IP later repalced with args[0]
            Console.WriteLine("Connected to server. Input filename: ");
            string fileToReceive = args[1]; // better than a readLine()

            NetworkStream serverStream = clientSocket.GetStream();

            Lib.WriteTextTcp(serverStream, fileToReceive);

            // checking if the file can be found
            if (Lib.ReadTextTcp(serverStream) == "1")
            {
                ReceiveFile(fileToReceive, serverStream);
                Console.WriteLine("File received. Closing connection.");
                serverStream.Close();
            }
            else
            {
                Console.WriteLine("File not found. Closing connection.");
                serverStream.Close();
            }
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
            long fileSize = long.Parse(Lib.ReadTextTcp(io));
            Console.WriteLine("Size of file: " + fileSize);

            byte[] recData = new byte[Bufsize];
            int recBytes;
            int totalRecBytes = 0;

            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);

            while ((recBytes = io.Read(recData, 0, recData.Length)) > 0)
            {
                fs.Write(recData, 0, recBytes);
                totalRecBytes += recBytes;
            }

            fs.Close();
            io.Close();
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
