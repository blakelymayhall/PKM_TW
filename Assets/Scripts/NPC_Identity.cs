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

public enum NPC_MoveStyle
{
    None,
    Wander,
    Spin
}

public class NPC_Identity : MonoBehaviour
{
    /* PUBLIC VARS */
    //*************************************************************************
    public List<NPC_Type> npc_type;
    public NPC_MoveStyle npc_movestyle;
    //*************************************************************************
}
