using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class ClientUDP : MonoBehaviour
{
    public Transform ball;
    UdpClient sock = new UdpClient();
    // Start is called before the first frame update
    void Start()
    {
        ListenForPackets();
        SendPacket(Buffer.From("Hello world!"));
    }

    async void ListenForPackets()
    {
        while (true)
        {
            UdpReceiveResult res;
            try
            {
                res = await sock.ReceiveAsync();
            } 
            catch
            {
                break;
            }
            
            Buffer packet = Buffer.From(res.Buffer);
            ProcessPacket(packet);
        }      
    }

    private void ProcessPacket(Buffer packet)
    {
        if (packet.Length < 4) return;

        string id = packet.ReadString(0, 4);
        switch (id)
        {
            case "BALL":
                if (packet.Length < 16) return;

                float x = packet.ReadSingleLE(4);
                float y = packet.ReadSingleLE(8);
                float z = packet.ReadSingleLE(12);

                ball.position = new Vector3(x, y, z);

                break;
            default:
                break;
        }
    }

    async void SendPacket(Buffer packet)
    {
        await sock.SendAsync(packet.bytes, packet.bytes.Length, "127.0.0.1", 320);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        sock.Close();
    }
}
