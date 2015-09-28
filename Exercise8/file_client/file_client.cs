using System;
using System.IO;
using System.Net.Sockets;
using LIB;

namespace file_client
{
    class FileClient
    {
        const int Port = 9000;
        const int Bufsize = 1000;

        private FileClient(string[] args)
        {
            TcpClient clientSocket = new TcpClient();

            clientSocket.Connect(args[0], Port);
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

        public static void Main(string[] args)
        {
            Console.WriteLine("Client starts...");
            new FileClient(args);
        }
    }
}
