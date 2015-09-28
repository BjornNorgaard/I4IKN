using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace tcp
{
	class FileServer
	{
		/// <summary>
		/// The PORT
		/// </summary>
		const int Port = 9000;

		/// <summary>
		/// The BUFSIZE
		/// </summary>
		const int Bufsize = 1000;

		/// <summary>
		/// Initializes a new instance of the <see cref="FileServer"/> class.
		/// Opretter en socket.
		/// Venter på en connect fra en klient.
		/// Modtager filnavn
		/// Finder filstørrelsen
		/// Kalder metoden sendFile
		/// Lukker socketen og programmet
		/// </summary>
		private FileServer ()
		{
			// TO DO Your own code
            TcpListener serverSocket = new TcpListener(Port);
            TcpClient clientSocket = new TcpClient();
            int requestCount = 0;

            serverSocket.Start();
            Console.WriteLine(" >> Server Started.");

            clientSocket = serverSocket.AcceptTcpClient();  // client found
            Console.WriteLine(" >> Accept connection from client");

            requestCount = 0;

            while (true)
            {
                try
                {
                    requestCount++;

                    NetworkStream networkStream = clientSocket.GetStream(); // finder stream fra client
                    string received = Lib.ReadTextTcp(networkStream);   // henter stream til string

                }
                catch (Exception)
                {

                    throw;
                }
            }
		}

		/// <summary>
		/// Sends the file.
		/// </summary>
		/// <param name='fileName'>
		/// The filename.
		/// </param>
		/// <param name='fileSize'>
		/// The filesize.
		/// </param>
		/// <param name='io'>
		/// Network stream for writing to the client.
		/// </param>
		private void SendFile (String fileName, long fileSize, NetworkStream io)
		{
			// TO DO Your own code
		}

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main (string[] args)
		{
			Console.WriteLine ("Server starts...");
			new FileServer();
		}
	}
}
