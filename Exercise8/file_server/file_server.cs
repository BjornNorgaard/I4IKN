using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
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
			// Initializes a new instance of the <see cref="FileServer"/> class.
			// Sker i main...

			// Opretter en socket
			TcpListener serverSocket = new TcpListener(IPAddress.Parse("10.0.0.1"), Port);
			TcpClient clientSocket = default(TcpClient);

			// Venter på en connect fra en klient
			serverSocket.Start();
			Console.WriteLine("Server running   - Waiting for client.");

			// Connecter en klient
			clientSocket = serverSocket.AcceptTcpClient();
			Console.WriteLine("Client connected - Waiting for filename.");
			
			// Modtager filnavn
			NetworkStream networkStream = clientSocket.GetStream();
			string fileToSend = Lib.ReadTextTcp(networkStream);
			Console.WriteLine("Client trying to retrieve file: " + fileToSend);

			// Finder filstørrelsen
			long fileLenght = Lib.check_File_Exists(fileToSend);

			// Kalder metoden sendFile
			if (fileLenght > 0)
			{
				Console.WriteLine("Size of file: " + fileLenght);

				Lib.WriteTextTcp(networkStream, "1");
				SendFile(fileToSend, fileLenght, networkStream);

				// Lukker socketen og programmet
				Console.WriteLine("Closing connection.");
				clientSocket.Close();
				serverSocket.Stop();
				Console.WriteLine("Connection closed.");
			}
			else
			{
				Lib.WriteTextTcp(networkStream, "0");
				Console.WriteLine("You done goofed!");

				// Lukker socketen og programmet
				clientSocket.Close();
				serverSocket.Stop();
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
			Lib.WriteTextTcp(io, fileSize.ToString());
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
