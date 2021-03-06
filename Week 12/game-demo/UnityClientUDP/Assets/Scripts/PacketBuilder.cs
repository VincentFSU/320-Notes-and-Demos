﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PacketBuilder
{
    static int previousInputH = 0;

    public static Buffer CurrentInput()
    {
        int h = (int)Input.GetAxisRaw("Horizontal"); // (-1 | 0 | 1)

        if (h == previousInputH) return null;

        previousInputH = h;
        
        Buffer b = Buffer.Alloc(5); // 5 bytes for an INPT packet

        b.WriteString("INPT", 0);
        b.WriteInt8((sbyte)h, 4); // write the horizontal input 4 bytes in to the packet

        return b;
    }
}
