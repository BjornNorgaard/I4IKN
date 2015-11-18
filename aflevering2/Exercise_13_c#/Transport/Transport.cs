using System;
using Linklaget;

/// <summary>
/// Transport.
/// </summary>
namespace Transportlaget
{
	/// <summary>
	/// Transport.
	/// </summary>
	public class Transport
	{
		/// <summary>
		/// The link.
		/// </summary>
		private Link link;
		/// <summary>
		/// The 1' complements checksum.
		/// </summary>
		private Checksum checksum;
		/// <summary>
		/// The buffer.
		/// </summary>
		private byte[] buffer;
		/// <summary>
		/// The seq no.
		/// </summary>
		private byte seqNo;
		/// <summary>
		/// The old_seq no.
		/// </summary>
		private byte old_seqNo;
		/// <summary>
		/// The error count.
		/// </summary>
		private int errorCount;
		/// <summary>
		/// The DEFAULT_SEQNO.
		/// </summary>
		private const int DEFAULT_SEQNO = 2;

		/// <summary>
		/// Initializes a new instance of the <see cref="Transport"/> class.
		/// </summary>
		public Transport (int BUFSIZE)
		{
			link = new Link(BUFSIZE+(int)TransSize.ACKSIZE);
			checksum = new Checksum();
			buffer = new byte[BUFSIZE+(int)TransSize.ACKSIZE];
			seqNo = 0;
			old_seqNo = DEFAULT_SEQNO;
			errorCount = 0;
		}

		/// <summary>
		/// Receives the ack.
		/// </summary>
		/// <returns>
		/// The ack.
		/// </returns>
		private bool receiveAck()
		{
			byte[] buf = new byte[(int)TransSize.ACKSIZE];
			int size = link.receive(ref buf);
			if (size != (int)TransSize.ACKSIZE) return false;
			if(!checksum.checkChecksum(buf, (int)TransSize.ACKSIZE) ||
					buf[(int)TransCHKSUM.SEQNO] != seqNo ||
					buf[(int)TransCHKSUM.TYPE] != (int)TransType.ACK)
				return false;
			
			seqNo = (byte)((buf[(int)TransCHKSUM.SEQNO] + 1) % 2);
			
			return true;
		}

		/// <summary>
		/// Sends the ack.
		/// </summary>
		/// <param name='ackType'>
		/// Ack type.
		/// </param>
		private void sendAck (bool ackType)
		{
			byte[] ackBuf = new byte[(int)TransSize.ACKSIZE];
			ackBuf [(int)TransCHKSUM.SEQNO] = 
					(ackType ? buffer [(int)TransCHKSUM.SEQNO] : (byte)((buffer[(int)TransCHKSUM.SEQNO] + 1) % 2));
			ackBuf [(int)TransCHKSUM.TYPE] = (byte)(int)TransType.ACK;
			checksum.calcChecksum (ref ackBuf, (int)TransSize.ACKSIZE);

			link.send(ackBuf, (int)TransSize.ACKSIZE);
		}

	    /// <summary>
	    /// Send the specified buffer and size.
	    /// </summary>
	    /// <param name='buf'>
	    /// Buffer.
	    /// </param>
	    /// <param name='size'>
	    /// Size.
	    /// </param>
	    public void send(byte[] buf, int size)
	    {
	        if (size <= 1000)
	        {
	            bool sendFinished;

	            buffer[2] = seqNo;
                buffer[3] = (int)TransType.DATA;
                //Array(array to be copied, startIndex, Array to be copied to, Index to start copy to, length of array to copy)
                Array.Copy(buf, 0, buffer, 4, buf.Length);

                checksum.calcChecksum(ref buffer, size+4);

	            do
	            {
	                link.send(buffer, size + 4);
	                sendFinished = receiveAck();
	            }
                while (sendFinished == false);
	        }
        }

		/// <summary>
		/// Receive the specified buffer.
		/// </summary>
		/// <param name='buffer'>
		/// Buffer.
		/// </param>
		public int receive (ref byte[] buf)
		{
			// TO DO Your own code
		    bool receiveFinished = false;
            byte[] tempBuf = new byte[1004];
		    int size;

            do
		    {
                size = link.receive(ref tempBuf);
		        receiveFinished = checksum.checkChecksum(tempBuf, size);
		        sendAck(receiveFinished);
		    }
            while (receiveFinished == false);

		    buf = tempBuf;
		    return size;
		}
	}
}

