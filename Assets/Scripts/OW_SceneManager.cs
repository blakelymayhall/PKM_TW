using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class OW_SceneManager : MonoBehaviour
{
    /* PUBLIC VARS */
    //*************************************************************************
    public bool loaded = false;
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************

    //*************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // TODO replace with on impact of an exit
        // TODO Could I add DontDestroyOnLoad to my scenes rather than program it
        if(Input.GetKeyDown(KeyCode.Escape) && !loaded)
        {
            loaded = true;
            SceneManager.LoadScene("OverworldSandbox1");
        }
    }
}

/* LoadNPCs ()
 * This method loads the non-player characters that will be used in the 
 * sandbox scene testing. 
 * 
 * In a real map section, these NPCs will be loaded at pre-determined 
 * locations and rotations

void LoadNPCs()
{
    // Bound X/Y positions NPCs can be loaded at
        // TODO these will be hardcoded based on map design 
    float minX = -4f;
    float maxX = 4f;
    float minY = -4f;
    float maxY = 4f;

    // Define the four possible facing directions
        // TODO these will be hardcoded based on map design 
    Vector3[] directions =
        { Vector3.up, Vector3.down, Vector3.left, Vector3.right };

    Vector2 currentDirection = Vector3.up;
    Vector2 targetDirection =
        directions[Random.Range(0, directions.Length)];
    float angle =
        Vector2.SignedAngle(currentDirection, targetDirection);

    npcs.Add(Instantiate(npcPrefab,
        new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0),
        Quaternion.Euler(new Vector3(0f, 0f, angle)),
        GetComponent<Transform>()));
    npcs[0].name = "npc_0";

    targetDirection = directions[Random.Range(0, directions.Length)];
    angle = Vector2.SignedAngle(currentDirection, targetDirection);

    npcs.Add(Instantiate(npcPrefab,
        new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0),
        Quaternion.Euler(new Vector3(0f, 0f, angle)),
        GetComponent<Transform>()));
    npcs[1].name = "npc_1";
}
*/

/* OLD CODE
 * 
 *     // Spawns player at 0,0 for now
    public void CreateInitialPlayer()
    {
        player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity,
            GetComponent<Transform>());
    }
 * 
 */