using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OW_PlayerAnimator : OW_Animator
{
    /* PUBLIC VARS */
    //*************************************************************************

    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    //*************************************************************************

    protected override void Start()
    {
        base.Start();
        mechanics = GetComponent<OW_MovingObject>();
    }
}