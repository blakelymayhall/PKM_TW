using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPC_Type
{
    NonProgress,
    Progress,
    Enemy,
    Monster,
    Recruit
}

public class NPC_Identity : MonoBehaviour
{
    /* PUBLIC VARS */
    //*************************************************************************
    public List<NPC_Type> npc_types;
    //*************************************************************************

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
