using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class ControllerGameClient : MonoBehaviour
{
    static ControllerGameClient singleton;

    TcpClient socket = new TcpClient();
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
            DontDestroyOnLoad(gameObject); // don't destrou when loading a new scene
        }
    }

    void Update()
    {
        
    }
}
