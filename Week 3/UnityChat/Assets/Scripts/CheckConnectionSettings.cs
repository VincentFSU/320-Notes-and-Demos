using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CheckConnectionSettings : MonoBehaviour
{

    public TMP_InputField inputHost;
    public TMP_InputField inputPort;
    public TMP_InputField inputUsername;
    public GameObject Button;
    public TextMeshProUGUI errorLabel;


    TcpClient socketToServer = new TcpClient();

    static string host;
    static string port;
    static string username;


    private void Start()
    {
        inputHost.text = PlayerPrefs.GetString("UnityChatHost");
        inputPort.text = PlayerPrefs.GetString("UnityChatPort");
        inputUsername.text = PlayerPrefs.GetString("UnityChatUsername");

        CheckInputFields();
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void OnButtonDown()
    {
        PlayerPrefs.SetString("UnityChatHost", host);
        PlayerPrefs.SetString("UnityChatPort", port);
        PlayerPrefs.SetString("UnityChatUsername", username);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CheckInputFields()
    {
        if (inputUsername.text == string.Empty || inputPort.text == string.Empty || inputUsername.text == string.Empty)
        {
            Button.SetActive(false);
            return;
        }

        host = inputHost.text;
        port = inputPort.text;
        username = inputUsername.text;
        Button.SetActive(true);        
    }

    public async void DoConnect()
    {
        try
        {
            await socketToServer.ConnectAsync(host, ushort.Parse(host));
            SendPacketToServer(Packet.BuildName(username));
        }
        catch (Exception e)
        {
            return;
        }
    }

    public void SetResponseLabel(string txt)
    {

    }

    static class Packet
    {
        public static string BuildName(string newName)
        {
            return $"NAME\t{newName}\n";
        }
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
