using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

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
