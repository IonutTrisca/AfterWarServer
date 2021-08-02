using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class TcpConnection
{
    public TcpClient socket;
    public int id;

    private Packet receivedData;
    private byte[] receiveBuffer;
    private NetworkStream stream;
    private int bufferSize;

    public TcpConnection(int id, int bufferSize)
    {
        this.bufferSize = bufferSize;
        this.id = id;
    }

    public void Connect(TcpClient socket)
    {
        this.socket = socket;
        socket.ReceiveBufferSize = bufferSize;
        socket.SendBufferSize = bufferSize;

        stream = socket.GetStream();

        receivedData = new Packet();
        receiveBuffer = new byte[bufferSize];

        stream.BeginRead(receiveBuffer, 0, bufferSize, ReceiveCallback, null);
        ServerSend.Welcome(id);
    }

    public void SendData(Packet packet)
    {
        try
        {
            if (socket != null)
            {
                stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null); // Send data to appropriate client
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error sending data {ex}");
        }
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            int byteLength = stream.EndRead(result);
            if (byteLength <= 0)
            {
                Server.clients[id].Disconnect();
                return;
            }

            byte[] data = new byte[byteLength];
            Array.Copy(receiveBuffer, data, byteLength);

            receivedData.Reset(HandleData(data));
            stream.BeginRead(receiveBuffer, 0, bufferSize, ReceiveCallback, null);
        }
        catch (Exception ex)
        {
            Debug.Log($"Error receiving TCP data: {ex}");
            Server.clients[id].Disconnect();
        }
    }

    private bool HandleData(byte[] data)
    {
        int packetLength = 0;

        receivedData.SetBytes(data);

        if (receivedData.UnreadLength() >= 4)
        {
            // If client's received data contains a packet
            packetLength = receivedData.ReadInt();
            if (packetLength <= 0)
            {
                // If packet contains no data
                return true; // Reset receivedData instance to allow it to be reused
            }
        }

        while (packetLength > 0 && packetLength <= receivedData.UnreadLength())
        {
            // While packet contains data AND packet data length doesn't exceed the length of the packet we're reading
            byte[] packetBytes = receivedData.ReadBytes(packetLength);
            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet packet = new Packet(packetBytes))
                {
                    int packetId = packet.ReadInt();
                    Server.packetHandlers[packetId](id, packet); // Call appropriate method to handle the packet
                }
            });

            packetLength = 0; // Reset packet length
            if (receivedData.UnreadLength() >= 4)
            {
                // If client's received data contains another packet
                packetLength = receivedData.ReadInt();
                if (packetLength <= 0)
                {
                    // If packet contains no data
                    return true; // Reset receivedData instance to allow it to be reused
                }
            }
        }

        if (packetLength <= 1)
        {
            return true; // Reset receivedData instance to allow it to be reused
        }

        return false;
    }

    public void Disconnect()
    {
        socket.Close();
        stream = null;
        receivedData = null;
        receiveBuffer = null;
        socket = null;
    }
}
