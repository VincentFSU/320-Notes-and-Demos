using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class ClientUDP : MonoBehaviour
{

    private static ClientUDP _singleton;
    public static ClientUDP singleton
    {
        get { return _singleton; }
        private set { _singleton = value; }
    }

    UdpClient sock = new UdpClient();

    public string Host = "127.0.0.1";
    public ushort Port = 320;

    /// <summary> 
    /// Most recent ball update packet 
    /// that has been received... 
    /// </summary> 
    uint ackBallUpdate = 0;

    void Start()
    {

        if (singleton != null)
        {
            // already have a clientUDP... 
            Destroy(gameObject);

        }
        else
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(Host), Port);
            sock = new UdpClient(ep.AddressFamily);
            sock.Connect(ep);

            //ObjectRegistry.RegisterAll();

            // set up receive loop (async): 
            ListenForPackets();

            // send a packet to the server (async): 
            SendPacket(Buffer.From("JOIN"));
        }

    }

    /// <summary> 
    /// This function listens for incoming UDP packets. 
    /// </summary> 
    async void ListenForPackets()
    {
        while (true)
        {
            UdpReceiveResult res;
            try
            {
                res = await sock.ReceiveAsync();
                Buffer packet = Buffer.From(res.Buffer);
                ProcessPacket(packet);
            }
            catch
            {
                break;
            }
        }
    }
    /// <summary> 
    /// This function processes a packet and decides what to do next. 
    /// </summary> 
    /// <param name="packet">The packet to process</param> 
    private void ProcessPacket(Buffer packet)
    {
        if (packet.Length < 4) return; // do nothing 

        string id = packet.ReadString(0, 4);
        switch (id)
        {
            case "REPL":
                ProcessPacketREPL(packet);
                break;
        }
    }

    private void ProcessPacketREPL(Buffer packet)
    {
        if (packet.Length < 5) return; // do nothing 

        int replType = packet.ReadUInt8(4);

        if (replType != 1 && replType != 2 && replType != 3) return;

        int offset = 5;

        while (offset <= packet.Length)
        {

            int networkID = 0;
            switch (replType)
            {
                case 1: // create:
                    if (packet.Length < offset + 5) return; // do nothing 
                    networkID = packet.ReadUInt8(offset + 4);

                    print("REPL packet CREATE received");
                    string classID = packet.ReadString(offset, 4);

                    NetworkObject obj = ObjectRegistry.SpawnFrom(classID);

                    // check network ID!

                    if (NetworkObject.GetObjectByNetworkID(networkID) != null) return; // ignore if object already exists

                    if (obj == null) return;

                    offset += 4; // trim out ClassID off beginning of packet data

                    Buffer chunk = packet.Slice(offset);
                    offset += obj.Deserialize(chunk);

                    NetworkObject.AddObject(obj); // ERROR: Class ID not found!
                    break;
                case 2: // update:
                    if (packet.Length < offset + 5) return; // do nothing 
                    networkID = packet.ReadUInt8(offset + 4);

                    // lookup the object, using networkID
                    NetworkObject obj2 = NetworkObject.GetObjectByNetworkID(networkID);
                    if (obj2 == null) return;
                    
                    offset += 4;
                    offset += obj2.Deserialize(packet.Slice(offset));
                    
                    break;
                case 3: // delete:
                    if (packet.Length < offset + 1) return; // do nothing 
                    networkID = packet.ReadUInt8(offset);

                    NetworkObject obj3 = NetworkObject.GetObjectByNetworkID(networkID);
                    if (obj3 == null) return;

                    NetworkObject.RemoveObject(networkID);
                    Destroy(obj3.gameObject);
                    offset++;
                    break;
            }

        }
    }

    /// <summary> 
    /// This function sends a packet (current to localhost:320) 
    /// </summary> 
    /// <param name="packet">The packet to send</param> 
    async public void SendPacket(Buffer packet)
    {
        if (sock == null) return;
        if (!sock.Client.Connected) return;
        
        await sock.SendAsync(packet.bytes, packet.bytes.Length);
    }
    /// <summary> 
    /// When destroying, clean up objects: 
    /// </summary> 
    private void OnDestroy()
    {
        sock.Close();
    }
}
