using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

public class UDPListener 
{
	private const int ListenPort = 9000;

	private static void StartListener() 
	{
		//Initiate UDP server
		Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		UdpClient listener = new UdpClient(ListenPort);
		IPEndPoint groupEp = new IPEndPoint(IPAddress.Any,ListenPort);
		IPEndPoint responseEp;
		string receivedCommand;

		try 
		{
			while (true) 
			{
				//Wait for incoming command
				Console.WriteLine("Waiting for command");
				byte[] bytes = listener.Receive( ref groupEp);
				receivedCommand = Encoding.ASCII.GetString(bytes,0,bytes.Length);
				Console.WriteLine("Received command: " + receivedCommand + " from " + groupEp.Address);

				//Send matching response
				responseEp = new IPEndPoint(groupEp.Address, ListenPort);
				if (receivedCommand == "U" || receivedCommand ==  "u")
				{
					using (StreamReader sr = new StreamReader ("/proc/uptime"))
					{
						String line = sr.ReadToEnd();
						Console.WriteLine("Sending uptime: " + line);
					
						byte[] sendbuf = Encoding.ASCII.GetBytes(line);
						s.SendTo(sendbuf, responseEp);
					}	
				}
				else if(receivedCommand == "L" || receivedCommand ==  "l") 
				{
					using (StreamReader sr = new StreamReader ("/proc/loadavg"))
					{
						String line = sr.ReadToEnd();
						Console.WriteLine("Sending load average: " + line);

						byte[] sendbuf = Encoding.ASCII.GetBytes(line);
						s.SendTo(sendbuf, responseEp);
					}
				}
				else
				{
					Console.WriteLine("Command " + receivedCommand + " not found\n");
					byte[] sendbuf = Encoding.ASCII.GetBytes("Input not recognized, please try again!");
					s.SendTo(sendbuf, responseEp);
				}
			}
		} 
		catch (Exception e) 
		{
			Console.WriteLine(e.ToString());
		}
		finally
		{
			listener.Close();
		}
	}

	public static int Main() 
	{
		StartListener();

		return 0;
	}
}