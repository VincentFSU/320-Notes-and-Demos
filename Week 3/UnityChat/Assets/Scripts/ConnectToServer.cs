using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using TMPro;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

public class ConnectToServer : MonoBehaviour
{
    public string host = "127.0.0.1";
    public ushort port = 320; // 0 - 65535

    public TextMeshProUGUI chatDisplay;
    public TextMeshProUGUI usersDisplay;
    public TMP_InputField inputDisplay;


    TcpClient socketToServer = new TcpClient();

    string buffer = "";

    static class Packet
    {
        public static string BuildChat(string message)
        {
            return $"CHAT\t{message}\n";
        }
        public static string BuildDM(string recipient, string message)
        {
            return $"DMSG\t{recipient}\t{message}\n";
        }
        public static string BuildName(string newName)
        {
            return $"NAME\t{newName}\n";
        }
        public static string BuildListRequest()
        {
            return $"LIST\n";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        host = PlayerPrefs.GetString("UnityChatHost");
        ushort.TryParse(PlayerPrefs.GetString("UnityChatPort"), out port);
        DoConnect();
    }

    async void DoConnect()
    {
        try
        {
            await socketToServer.ConnectAsync(host, port);
            SendPacketToServer(Packet.BuildName(PlayerPrefs.GetString("UnityChatUsername")));
            AddMessageToChatDisplay("Successfully connected to server...");
        }
        catch (Exception e)
        {
            AddMessageToChatDisplay($"ERROR: {e.Message}");
            return;
        }

        while (true)
        {
            byte[] data = new byte[4096];
            int bytesRead = await socketToServer.GetStream().ReadAsync(data, 0, data.Length); // returns how many bytes were read

            // add data to the buffer:
            buffer += Encoding.ASCII.GetString(data).Substring(0, bytesRead);

            // split the buffer into packets:
            string[] packets = buffer.Split('\n');

            // set the buffer to the last (incomplete) packet:
            buffer = packets[packets.Length - 1];

            // process all packets (except the last one)
            for (int i = 0; i < packets.Length - 1; i++)
            {
                HandlePacket(packets[i]);
            }
        }
    }

    private void HandlePacket(string packet)
    {
        string[] parts = packet.Split('\t');

        switch (parts[0])
        {
            case "CHAT":
                string user = parts[1];
                string message = parts[2];

                AddMessageToChatDisplay($"{user}: {message}");
                break;
            case "LIST":
                string users = "Users on server:\n";
                for (int i = 1; i < parts.Length; i++)
                {
                    users += parts[i];
                    users += "\n";

                    //if (i > 1)
                    //{
                    //    users += "\n";
                    //    users += parts[i];
                    //    users += "\n";
                    //}
                }

                AddMessageToUsersDisplay(users);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    public void AddMessageToChatDisplay(string txt)
    {
        chatDisplay.text += $"{txt}\n";
    }

    public void AddMessageToUsersDisplay(string txt)
    {
        usersDisplay.text = $"{txt}\n";
    }

    public void UserDoneEditingMessage(string txt)
    {
        if (new Regex(@"^\\name ", RegexOptions.IgnoreCase).IsMatch(txt))
        {
            string name = txt.Substring(6);

            SendPacketToServer(Packet.BuildName(name));
            inputDisplay.text = "";
        }
        else if(new Regex(@"^\\list\s*$",RegexOptions.IgnoreCase).IsMatch(txt))
        {
            SendPacketToServer(Packet.BuildListRequest());
            inputDisplay.text = "";
        }
        else if (!new Regex(@"^(\s|\t)*$").IsMatch(txt))
        {
            SendPacketToServer(Packet.BuildChat(txt));
            inputDisplay.text = "";
        }

        inputDisplay.Select();
        inputDisplay.ActivateInputField();
    }

    public async void SendPacketToServer(string packet)
    {
        if (socketToServer.Connected)
        {
            byte[] data = Encoding.ASCII.GetBytes(packet);
            await socketToServer.GetStream().WriteAsync(data, 0, data.Length);
        }
    }
}
