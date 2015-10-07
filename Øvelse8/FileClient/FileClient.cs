using LIB;
using System;
using System.IO;
using System.Net.Sockets;

namespace FileClient
{
    class FileClient
    {
        const int Port = 9000;
        const int BufferSize = 1000;

        public FileClient(string[] args)
        {
            #region Declaring variables

            TcpClient clientSocket = new TcpClient();
            NetworkStream networkStream;
            string fileToRequest;
            string hostname = "10.0.0.1";
            string serverResponse = "Server response never received";

            #endregion

            #region Assigning variables

            hostname = args[0];
            fileToRequest = args[1];

            clientSocket.Connect(hostname: hostname, port: Port);   // connecting to server
            networkStream = clientSocket.GetStream();               // connecting networkstream, to communicate

            #endregion

            #region Requesting and receiving file

            Lib.WriteTextTcp(networkStream, fileToRequest);     // 1: client sends filename
            serverResponse = Lib.ReadTextTcp(networkStream);    // 2: waiting for server to check for file

            switch (serverResponse)
            {
                case "Sending file":
                    ReceiceFile(fileToRequest, networkStream);
                    Console.WriteLine("File received.");
                    break;
                case "Error sending file":
                    Console.WriteLine(serverResponse);
                    break;
                default:
                    Console.WriteLine("I don't know how you fucked this up...");
                    break;
            }

            #endregion

            #region Closing connection

            networkStream.Close();
            clientSocket.Close();

            #endregion
        }

        public void ReceiceFile(string filename, NetworkStream stream)
        {
            #region Declaring variables

            string filesize;
            byte[] receivingBuffer = new byte[BufferSize];
            int numberOfBytesToBeReceived = 0;
            int totalNumberOfReceivedBytes = 0;
            FileStream fileStream;

            #endregion

            #region Assigning variables and setting up fileStream

            filesize = Lib.ReadTextTcp(stream); // 3: receiving filesize
            Console.WriteLine("Size of file: " + filesize);
            fileStream = new FileStream(path: filename, mode: FileMode.Create, access: FileAccess.Write);

            #endregion

            #region Receiving data

            while ((numberOfBytesToBeReceived = stream.Read(receivingBuffer, 0, receivingBuffer.Length)) > 0)
            {
                fileStream.Write(receivingBuffer, 0, numberOfBytesToBeReceived);
                totalNumberOfReceivedBytes += numberOfBytesToBeReceived;
                int i = 0;
                Console.Write("\rReceived " + ++i + " of " + numberOfBytesToBeReceived + " packets to client");
            }

            #endregion

            #region Closing connection

            fileStream.Close();
            stream.Close();

            #endregion
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Starting client...");
            FileClient myClient = new FileClient(args);
        }
    }
}
