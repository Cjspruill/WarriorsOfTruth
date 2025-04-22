using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    //Add spawn locations
   [SerializeField] List<Transform> playerSpawnLocations;
   [SerializeField] List<Transform> opponentSpawnLocations;


    // Start is called before the first frame update
    void Start()
    {
        //Call game manager setup player positions and pass in both list of transforms from above
        GameManager.instance.SetupPlayerPositions(playerSpawnLocations, opponentSpawnLocations);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
