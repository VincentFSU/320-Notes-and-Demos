using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using TMPro;
using System;
using System.Threading.Tasks;

public enum Panel
{
    Host,
    Username,
    Gameplay
}
public class ControllerGameClient : MonoBehaviour
{
    static public ControllerGameClient singleton;

    TcpClient socket = new TcpClient();

    Buffer buffer = Buffer.Alloc(0);

    public TMP_InputField inputHost;
    public TMP_InputField inputPort;
    public TMP_InputField inputUsername;

    public Transform panelHostDetails;
    public Transform panelUsername;
    public ControllerGameplay panelGameplay;

    void Start()
    {
        if (singleton)
        {
            // already set...
            Destroy(gameObject); //there's already one out there...
        }
        else
        {
            singleton = this;
            DontDestroyOnLoad(gameObject); // don't destroy when loading a new scene

            // show connection screen:
            panelHostDetails.gameObject.SetActive(true);
            panelUsername.gameObject.SetActive(false);
            panelGameplay.gameObject.SetActive(false);
        }

        Buffer buff = Buffer.Alloc(4);

        buff.Concat(new byte[] { 1, 2, 3, 4 }, 0);
        buff.Consume(10);
        print(buff);
    }

    public void ChangeToPanel(Panel panel)
    {
        switch (panel)
        {
            case Panel.Host:
                break;
            case Panel.Username:
                break;
            case Panel.Gameplay:
                break;
            default:
                break;
        }
    }

    public void OnButtonConnect()
    {
        string host = inputHost.text;
        UInt16.TryParse(inputPort.text, out ushort port);

        TryToConnect(host, port);
    }

    public void OnButtonUsername()
    {
        string name = inputUsername.text;
        Buffer packet = PacketBuilder.Join(name);
        SendPacketToServer(packet);
    }

    public async void TryToConnect(string host, int port)
    {
        if (socket.Connected) return; // already connected to a server, cancel...

        try
        {
            await socket.ConnectAsync(host, port);

            panelHostDetails.gameObject.SetActive(false);
            panelUsername.gameObject.SetActive(true);
            panelGameplay.gameObject.SetActive(false);

            StartReceivingPackets();
        }
        catch (Exception e)
        {
            print("Failed to Connect...");

            panelHostDetails.gameObject.SetActive(true);
            panelUsername.gameObject.SetActive(false);
            panelGameplay.gameObject.SetActive(false);
        }
    }

    public async void StartReceivingPackets()
    {
        int maxPacketSize = 4096;
        while (socket.Connected)
        {
            byte[] data = new byte[maxPacketSize];

            try
            {
                int bytesRead = await socket.GetStream().ReadAsync(data, 0, maxPacketSize);

                buffer.Concat(data, bytesRead);

                ProcessPackets();
            }
            catch (Exception e)
            {
                                
            }
        }   
    }

    void ProcessPackets()
    {
        if (buffer.Length < 4) return; // not enough data in the buffer
        print(buffer);

        string packetIdentifier = buffer.ReadString(0, 4);
        if (packetIdentifier.Contains("PDT"))
        {
            packetIdentifier = "UPDT";
        }

        switch (packetIdentifier)
        {
            case "JOIN":
                if (buffer.Length < 5) return; // not enough data for a JOIN packet
                byte joinResponse = buffer.ReadUInt8(4);
                if (joinResponse == 1 || joinResponse == 2 || joinResponse == 3)
                {
                    //Username accepted
                    panelHostDetails.gameObject.SetActive(false);
                    panelUsername.gameObject.SetActive(false);
                    panelGameplay.gameObject.SetActive(true);
                }
                else if (joinResponse == 9)
                {
                    //Server full
                    panelHostDetails.gameObject.SetActive(true);
                    panelUsername.gameObject.SetActive(false);
                    panelGameplay.gameObject.SetActive(false);
                }
                else
                {
                    // TODO: show error message to user
                    panelHostDetails.gameObject.SetActive(false);
                    panelUsername.gameObject.SetActive(true);
                    panelGameplay.gameObject.SetActive(false);
                    inputUsername.text = string.Empty;

                    print(joinResponse);
                }
                // TODO: change which screen we're looking at...

                // TODO: consume data from buffer

                buffer.Consume(5);
                break;
            case "UPDT":
                if (buffer.Length < 15) return; // not enough data for an UPDT packet

                print(buffer);

                byte whoseTurn = buffer.ReadUInt8(4);
                byte gameStatus = buffer.ReadUInt8(5);

                byte[] spaces = new byte[9];
                for (int i = 0; i < 9; i++)
                {
                    spaces[i] = buffer.ReadUInt8(6 + i);
                }
                // TODO: switch to gameplay screen...
                panelHostDetails.gameObject.SetActive(false);
                panelUsername.gameObject.SetActive(false);
                panelGameplay.gameObject.SetActive(true);
                panelGameplay.UpdateFromServer(gameStatus, whoseTurn, spaces);

                // TODO: consume data from buffer
                buffer.Consume(15);
                break;
            case "CHAT":

                // TODO: switch to gameplay screen...
                panelHostDetails.gameObject.SetActive(false);
                panelUsername.gameObject.SetActive(false);
                panelGameplay.gameObject.SetActive(true);
                // TODO: update chat view

                byte usernameLength = buffer.ReadByte(4);
                ushort messageLength = buffer.ReadUInt16BE(5);

                if (buffer.Length < 7 + usernameLength + messageLength) return;

                string username = buffer.ReadString(7, usernameLength);
                string message = buffer.ReadString(7 + usernameLength, messageLength);

                // TODO: consume data from buffer
                buffer.Consume(7 + usernameLength + messageLength);
                break;
            default:
                print("unknown packet identifier...");

                // TODO: clear buffer?
                buffer.Clear();
                break;
        }
    }

    public async void SendPacketToServer(Buffer packet)
    {
        if (!socket.Connected)
            return;
        await socket.GetStream().WriteAsync(packet.bytes, 0, packet.bytes.Length);
    }

    public void SendPlayPacket(int x, int y)
    {
        Buffer packet = PacketBuilder.Play(x, y);
        SendPacketToServer(packet);
    }
}
