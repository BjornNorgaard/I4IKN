using LIB;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace FileServer
{
	class FileServer
	{
		private const int Port = 9000;
		private const int BufferSize = 1000;

		public FileServer()
		{
			#region Setting up server and connecting client

			IPAddress serverIP = IPAddress.Parse("10.0.0.1");
			TcpListener serverSocket = new TcpListener(serverIP, Port);
			TcpClient clientSocket;
			NetworkStream networkStream;
			string requestedFile;
			long fileSize = 0;

			serverSocket.Start();
			Console.WriteLine("Waiting for client...");

			clientSocket = serverSocket.AcceptTcpClient();
			networkStream = clientSocket.GetStream();
			Console.WriteLine("Client connected - waiting for filename.");

			#endregion

			#region Getting filename and calculating lenght

			requestedFile = Lib.ReadTextTcp(networkStream); // 1: server waits here for filename

			fileSize = Lib.check_File_Exists(requestedFile);
			Console.WriteLine("Requested file: " + requestedFile + " of lenght: " + fileSize);

			#endregion

			#region Sending file

			if (fileSize > 0)
			{
				Lib.WriteTextTcp(networkStream, "Sending file");        // 2: sending OK to client, sending file
				Console.WriteLine("Sending file");
				SendFile(requestedFile, fileSize, networkStream);
			}
			else
			{
				Lib.WriteTextTcp(networkStream, "Error sending file");  // 2: sending FUCK to client, cannot send file
				Console.WriteLine("Error sending file");
			}

			#endregion

			#region Closing connection

			serverSocket.Stop();
			clientSocket.Close();

			#endregion

		}

		public void SendFile(string filename, long filesize, NetworkStream stream)
		{
			#region Local variables

			FileStream fileStream;
			int numberOfPackets = 0;
			int totalLenght = 0;

			#endregion

			#region Assigning variables

			fileStream = new FileStream(path: filename, mode: FileMode.Open, access: FileAccess.Read);
			numberOfPackets = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(fileStream.Length)/Convert.ToDouble(BufferSize)));
			totalLenght = (int)fileStream.Length;

			#endregion

			#region Sending file

			Lib.WriteTextTcp(stream, filesize.ToString()); // 3: sending filesize

			for (int i = 0; i < numberOfPackets; i++)
			{
				int currentPacketsLenght = 0;

				#region Checking if filesize is less than buffersize

				if (totalLenght > BufferSize)
				{
					currentPacketsLenght = BufferSize;
					totalLenght -= currentPacketsLenght;
				}
				else
				{
					currentPacketsLenght = totalLenght;
				}

				#endregion

				byte[] sendingBuffer = new byte[currentPacketsLenght];      // nedded for filestream operation
				fileStream.Read(sendingBuffer, 0, currentPacketsLenght);    // placing currentpacket stuff in the sending buffer
				stream.Write(sendingBuffer, 0, currentPacketsLenght);       // writes the sending buffer to filestream

				Console.Write("\rSent " + (i + 1) + " of " + numberOfPackets + " packets to client");
				if (i == (numberOfPackets - 1)) Console.WriteLine("");
			}

			#endregion

			#region Closing connection

			fileStream.Close();
			stream.Close();

			#endregion
		}

		static void Main(string[] args)
		{
			while (true) 
			{
				Console.WriteLine("Starting server...");
				new FileServer();
				Console.WriteLine ("Restarting server...\n");
			}
		}
	}
}
