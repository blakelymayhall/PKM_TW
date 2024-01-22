using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OW_PlayerModes {
    STANDBY,
    MOVE_DELAY,
    MOVING,
    SPOTTED,
    ENGAGED
}

public class OW_Player : MonoBehaviour
{
    /* PUBLIC VARS */
    //*************************************************************************
    public OW_PlayerModes playerMode;
    //*************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        playerMode = OW_PlayerModes.STANDBY;
    }
}
