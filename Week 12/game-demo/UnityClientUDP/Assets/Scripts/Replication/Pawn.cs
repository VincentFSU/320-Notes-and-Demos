﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : NetworkObject
{
    new public static string classID = "PAWN"; // override parent value

    public override void Serialize()
    {
        // TODO: turn object into a byte array
    }

    public override void Deserialize()
    {
        // TODO: turn object into a byte array
    }
}
