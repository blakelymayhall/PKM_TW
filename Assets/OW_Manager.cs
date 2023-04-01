using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OW_Manager : MonoBehaviour
{
    /* PUBLIC VARS */
    //*************************************************************************
    public GameObject playerPrefab;
    public GameObject player;
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************

    //*************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        CreateInitialPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawns player at 0,0 for now
    public void CreateInitialPlayer()
    {
        player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity,
            GetComponent<Transform>());
    }
}
