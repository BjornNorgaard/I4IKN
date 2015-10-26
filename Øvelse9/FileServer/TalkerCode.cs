using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program 
{

	static void Main(string[] args)
	{
		//Setup UDP client
		int listenport = 9000;
		UdpClient listener = new UdpClient (listenport);
		Socket s = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

		//Initialize ip and command from args
		IPAddress sendIP = IPAddress.Parse (args[0]);
		string sendCommand = args[1];

		//Send command
		byte[] sendbuf = Encoding.ASCII.GetBytes (sendCommand);
		IPEndPoint sendEP = new IPEndPoint (sendIP, listenport);
		s.SendTo (sendbuf, sendEP);

		//Wait for answer and output to console
		byte[] bytes = listener.Receive (ref sendEP);
		Console.WriteLine (Encoding.ASCII.GetString (bytes, 0, bytes.Length));
	}
}
