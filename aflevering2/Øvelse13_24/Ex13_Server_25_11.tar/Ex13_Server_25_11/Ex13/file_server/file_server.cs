using System;
using System.IO;
using System.Text;
using Transportlaget;

namespace serial
{
	class file_server
	{
		private const int BUFSIZE = 1000;
		const int PORT = 9000;



		/// <summary>
		/// Initializes a new instance of the <see cref="file_server"/> class.
		/// </summary>
		private file_server (Transport transport)
		{
			try
			{
				Console.WriteLine("Server started - Listening for client");

				//Get filename

				string fileToSend = transport.readText();

				Console.WriteLine("Client trying to retrieve file: " + fileToSend);

				long fileSize = LIB.check_File_Exists (fileToSend);

				if (fileSize != 0) 
				{
					transport.sendText("File found");

					sendFile (fileToSend, fileSize, transport);
				} 
				else 
				{
					transport.sendText("File not found");

					Console.WriteLine ("404 - File not found");
				}
			}
			catch (Exception ex) 
			{
				Console.WriteLine (ex.Message);
			}
			finally
			{
				Console.WriteLine ("Exit");
			}

		
		}

		/// <summary>
		/// Sends the file.
		/// </summary>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		/// <param name='fileSize'>
		/// File size.
		/// </param>
		/// <param name='tl'>
		/// Tl.
		/// </param>
		private void sendFile(String fileName, long fileSize, Transport transport)
		{
			FileStream fileStream = null;

			try
			{
				Console.WriteLine ("Size of file " + fileSize);

				transport.sendText(fileSize.ToString());

				byte[] SendingBuffer = null;

				fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
				int NoOfPackets = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(fileStream.Length) / Convert.ToDouble(BUFSIZE)));

				int TotalLength = (int)fileStream.Length;
				int CurrentPacketLength, bytesSent = 0;

				for (int i = 1; i < NoOfPackets+1; i++)
				{
					if (TotalLength > BUFSIZE) 
					{
						CurrentPacketLength = BUFSIZE;
						TotalLength = TotalLength - CurrentPacketLength;
						bytesSent += BUFSIZE;
					} 
					else 
					{
						CurrentPacketLength = TotalLength;
						bytesSent += CurrentPacketLength;
					}

					SendingBuffer = new byte[CurrentPacketLength];
					fileStream.Read(SendingBuffer, 0, CurrentPacketLength);
					transport.send(SendingBuffer, (int)SendingBuffer.Length);

					Console.Write("\rSent " + i + " of " + NoOfPackets + " packets to client. Total of " + bytesSent + " bytes sent");
				}
				Console.WriteLine ("\nFile sent - closing connection");
			}
			catch (Exception ex) 
			{
				Console.WriteLine (ex.Message);
			}
			finally 
			{
				if (fileStream != null)
					fileStream.Close ();
			}
		}

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main (string[] args)
		{

			Transport trans = new Transport (BUFSIZE);
			/*
			string myString = "abccbaabbcc";
			Byte[] dataToSend = new byte[myString.Length * sizeof(char)];
			System.Buffer.BlockCopy (myString.ToCharArray (), 0, dataToSend, 0, dataToSend.Length);
*/
			/*
			byte[] bytesToSend = new byte[BUFSIZE];
			bytesToSend [0] = 65;
			bytesToSend [1] = 66;
			bytesToSend [2] = 67;
			bytesToSend [3] = 10;
			bytesToSend [4] = 13;
			for (int i = 5; i < BUFSIZE; i++) {
				bytesToSend [i] = 66;
			}

			Console.WriteLine (bytesToSend.Length);
			trans.send (bytesToSend,bytesToSend.Length);



			string test = "hello this is a test message";
			byte[] bytes = new byte[test.Length * sizeof(char)];
			System.Buffer.BlockCopy (test.ToCharArray (), 0, bytes, 0, bytes.Length);
			trans.send (bytes, bytes.Length);
*/
			new file_server(trans);
			//trans.sendText ("billede.jpgHELLO THIS IS A b TEST MESSAGE");

			//trans.sendText ("Test");

			//Console.WriteLine(trans.readText());
			//Console.WriteLine(trans.readText());

		}
	}
}
