using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //Character prefabs
    public GameObject doodPrefab;
    public GameObject faedorPrefab;
    public GameObject inirtPrefab;
    public GameObject kcazPrefab;
    public GameObject kimPrefab;
    public GameObject leioPrefab;
    public GameObject mikeulPrefab;
    public GameObject princePrefab;
    public GameObject sonjaPrefab;
    public GameObject tomayPrefab;
    public GameObject viewtlPrefab;

    //Teams setup
    [SerializeField] List<GameObject> playerTeam;
    [SerializeField] List<GameObject> opponentTeam;

    //Actual gameobject reference filled when instantiated. Need to move them to the playing field once level is loaded
    public GameObject dood;
    public GameObject faedor;
    public GameObject inirt;
    public GameObject kcaz;
    public GameObject kim;
    public GameObject leio;
    public GameObject mikeul;
    public GameObject prince;
    public GameObject sonja;
    public GameObject tomay;
    public GameObject viewtl;

    //Icon setup, will change colors of them based on if they are in your party or not.
    public Image doodIcon;
    public Image faedorIcon;
    public Image inirtIcon;
    public Image kcazIcon;
    public Image kimIcon;
    public Image leioIcon;
    public Image mikeulIcon;
    public Image princeIcon;
    public Image sonjaIcon;
    public Image tomayIcon;
    public Image viewtlIcon;

    //String for selected character name, will do a switch or if case to check against it
    public string selectedCharacterName;

    //Game manager instance
    public static GameManager instance;

    [SerializeField] Button startButton;
    [SerializeField] Button[] addRemoveButtons;


    [SerializeField] bool oppCanUseDood = true;
    [SerializeField] bool oppCanUseFaedor = true;
    [SerializeField] bool oppCanUseInirt = true;
    [SerializeField] bool oppCanUseKcaz = true;
    [SerializeField] bool oppCanUseKim = true;
    [SerializeField] bool oppCanUseLeio = true;
    [SerializeField] bool oppCanUseMikeul = true;
    [SerializeField] bool oppCanUsePrince = true;
    [SerializeField] bool oppCanUseSonja = true;
    [SerializeField] bool oppCanUseTomay = true;
    [SerializeField] bool oppCanUseViewtl = true;
    
    private void Awake()
    {
        //If instance is null,
        if (instance == null)
        {
            //Set the instance to this
            instance = this;
        }
        //Else if the instance is not this
        else if (instance != this)
        {
            //Destroy this
            Destroy(this);
        }

        //Keep the object living over scene loading
        DontDestroyOnLoad(instance);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Add character to player team function
    public void AddCharacterToPlayerTeam()
    {
        //If we are at 5 players go away, dont add anything
        if (playerTeam.Count == 5) return;

        //Switch for selected character name
        switch (selectedCharacterName)
        {
            //If case is dood
            case "Dood":
                //If dood is null
                if (dood == null)
                {
                    //Set it by spawing a new dood prefab and adding it to the team, and change the icon to blue
                    dood = Instantiate(doodPrefab);
                    playerTeam.Add(dood);
                    doodIcon.color = Color.blue;
                    oppCanUseDood = false;
                }
                //Else if dood is not null and there is no dood in the team already, add him instead of spawning him again
                else if (dood != null && !playerTeam.Contains(dood))
                {
                    //Add dude, change icon to blue
                    playerTeam.Add(dood);
                    doodIcon.color = Color.blue;
                    oppCanUseDood = false;
                }
                break;
            //All other players are set up the same way, so i will not comment the same things.
            case "Faedor":
                if (faedor == null)
                {
                    faedor = Instantiate(faedorPrefab);
                    playerTeam.Add(faedor);
                    faedorIcon.color = Color.blue;
                    oppCanUseFaedor = false;
                }
                else if (faedor != null && !playerTeam.Contains(faedor))
                {
                    playerTeam.Add(dood);
                    faedorIcon.color = Color.blue;
                    oppCanUseFaedor = false;
                }
                break;
            case "Inirt":
                if (inirt == null)
                {
                    inirt = Instantiate(inirtPrefab);
                    playerTeam.Add(inirt);
                    inirtIcon.color = Color.blue;
                    oppCanUseInirt = false;
                }
                else if (inirt != null && !playerTeam.Contains(inirt))
                {
                    playerTeam.Add(inirt);
                    inirtIcon.color = Color.blue;
                    oppCanUseInirt = false;
                }
                break;
            case "Kcaz":
                if (kcaz == null)
                {
                    kcaz = Instantiate(kcazPrefab);
                    playerTeam.Add(kcaz);
                    kcazIcon.color = Color.blue;
                    oppCanUseKcaz = false;
                }
                else if (kcaz != null && !playerTeam.Contains(kcaz))
                {
                    playerTeam.Add(kcaz);
                    kcazIcon.color = Color.blue;
                    oppCanUseKcaz = false;
                }
                break;
            case "Kim":
                if (kim == null)
                {
                    kim = Instantiate(kimPrefab);
                    playerTeam.Add(kim);
                    kimIcon.color = Color.blue;
                    oppCanUseKim = false;
                }
                else if (kim != null && !playerTeam.Contains(kim))
                {
                    playerTeam.Add(kim);
                    kimIcon.color = Color.blue;
                    oppCanUseKim = false;
                }
                break;
            case "Leio":
                if (leio == null)
                {
                    leio = Instantiate(leioPrefab);
                    playerTeam.Add(leio);
                    leioIcon.color = Color.blue;
                    oppCanUseLeio = false;
                }
                else if (leio != null && !playerTeam.Contains(leio))
                {
                    playerTeam.Add(leio);
                    leioIcon.color = Color.blue;
                    oppCanUseLeio = false;
                }
                break;
            case "Mikeul":
                if (mikeul == null)
                {
                    mikeul = Instantiate(mikeulPrefab);
                    playerTeam.Add(mikeul);
                    mikeulIcon.color = Color.blue;
                    oppCanUseMikeul = false;
                }
                else if (mikeul != null && !playerTeam.Contains(mikeul))
                {
                    playerTeam.Add(mikeul);
                    mikeulIcon.color = Color.blue;
                    oppCanUseMikeul = false;
                }
                break;
            case "Prince":
                if (prince == null)
                {
                    prince = Instantiate(princePrefab);
                    playerTeam.Add(prince);
                    princeIcon.color = Color.blue;
                    oppCanUsePrince = false;
                }
                else if (prince != null && !playerTeam.Contains(prince))
                {
                    playerTeam.Add(prince);
                    princeIcon.color = Color.blue;
                    oppCanUsePrince = false;
                }
                break;
            case "Sonja":
                if (sonja == null)
                {
                    sonja = Instantiate(sonjaPrefab);
                    playerTeam.Add(sonja);
                    sonjaIcon.color = Color.blue;
                    oppCanUseSonja = false;
                }
                else if (sonja != null && !playerTeam.Contains(sonja))
                {
                    playerTeam.Add(sonja);
                    sonjaIcon.color = Color.blue;
                    oppCanUseSonja = false;
                }
                break;
            case "Tomay":
                if (tomay == null)
                {
                    tomay = Instantiate(tomayPrefab);
                    playerTeam.Add(tomay);
                    tomayIcon.color = Color.blue;
                    oppCanUseTomay = false;
                }
                else if (tomay != null && !playerTeam.Contains(tomay))
                {
                    playerTeam.Add(tomay);
                    tomayIcon.color = Color.blue;
                    oppCanUseTomay = false;
                }
                break;
            case "Viewtl":
                if (viewtl == null)
                {
                    viewtl = Instantiate(viewtlPrefab);
                    playerTeam.Add(viewtl);
                    viewtlIcon.color = Color.blue;
                    oppCanUseViewtl = false;
                }
                else if (viewtl != null && !playerTeam.Contains(viewtl))
                {
                    playerTeam.Add(viewtl);
                    viewtlIcon.color = Color.blue;
                    oppCanUseViewtl = false;
                }
                break;
        }

        if(playerTeam.Count == 5)
        {
            // startButton.gameObject.SetActive(true);
            for (int i = 0; i < addRemoveButtons.Length; i++)
            {
                addRemoveButtons[i].gameObject.SetActive(false);
            }
         Invoke("AddPlayersToOpponentTeam",2f);
        }

    }

    //Remove character from player team function
    public void RemoveCharacterFromPlayerTeam()
    {
        //If dood if not null
        if (dood != null)
        {
            //If selected character named is dood
            if (selectedCharacterName == "Dood")
            {
                //Destroy that object of dood, remove it from the team and change the icon back to white
                Destroy(dood);
                playerTeam.Remove(dood);
                doodIcon.color = Color.white;
            }
        }
        //All others follow same pattern so reference above for comments in this function
        if (faedor != null)
        {
            if (selectedCharacterName == "Faedor")
            {
                Destroy(faedor);
                playerTeam.Remove(faedor);
                faedorIcon.color = Color.white;
            }
        }
        if (inirt != null)
        {
            if (selectedCharacterName == "Inirt")
            {
                Destroy(inirt);
                playerTeam.Remove(inirt);
                inirtIcon.color = Color.white;
            }
        }
        if (kcaz != null)
        {
            if (selectedCharacterName == "Kcaz")
            {
                Destroy(kcaz);
                playerTeam.Remove(kcaz);
                kcazIcon.color = Color.white;
            }
        }
        if (kim != null)
        {
            if (selectedCharacterName == "Kim")
            {
                Destroy(kim);
                playerTeam.Remove(kim);
                kimIcon.color = Color.white;
            }
        }
        if (leio != null)
        {
            if (selectedCharacterName == "Leio")
            {
                Destroy(leio);
                playerTeam.Remove(leio);
                leioIcon.color = Color.white;
            }
        }
        if (mikeul != null)
        {
            if (selectedCharacterName == "Mikeul")
            {
                Destroy(mikeul);
                playerTeam.Remove(mikeul);
                mikeulIcon.color = Color.white;
            }
        }
        if (prince != null)
        {
            if (selectedCharacterName == "Prince")
            {
                Destroy(prince);
                playerTeam.Remove(prince);
                princeIcon.color = Color.white;
            }
        }
        if (sonja != null)
        {
            if (selectedCharacterName == "Sonja")
            {
                Destroy(sonja);
                playerTeam.Remove(sonja);
                sonjaIcon.color = Color.white;
            }
        }
        if (tomay != null)
        {
            if (selectedCharacterName == "Tomay")
            {
                Destroy(tomay);
                playerTeam.Remove(tomay);
                tomayIcon.color = Color.white;
            }
        }
        if (viewtl != null)
        {
            if (selectedCharacterName == "Viewtl")
            {
                Destroy(viewtl);
                playerTeam.Remove(viewtl);
                viewtlIcon.color = Color.white;
            }
        }

        ////If player team is 
        //if(playerTeam.Count <= 5)
        //{
        //    startButton.gameObject.SetActive(false);
        //}
    }


    //Add opponent players to a team
    public void AddPlayersToOpponentTeam()
    {
        //Create a new list of usable opponents and initialize it.
        List<GameObject> usableOpps = new List<GameObject>();

        //If dood can be used add him to the list of usable opponents, so on and so forth for the remainder of the players
        if (oppCanUseDood)
        {
            usableOpps.Add(doodPrefab);
        }
        if (oppCanUseFaedor)
        {
            usableOpps.Add(faedorPrefab);
        }
        if(oppCanUseInirt)
        {
            usableOpps.Add(inirtPrefab);
        }
        if (oppCanUseKcaz)
        {
            usableOpps.Add(kcazPrefab);
        }
        if (oppCanUseKim)
        {
            usableOpps.Add(kimPrefab);
        }
        if (oppCanUseLeio)
        {
            usableOpps.Add(leioPrefab);
        }
        if (oppCanUseMikeul)
        {
            usableOpps.Add(mikeulPrefab);
        }
        if (oppCanUsePrince)
        {
            usableOpps.Add(princePrefab);
        }
        if (oppCanUseSonja)
        {
            usableOpps.Add (sonjaPrefab);
        }
        if (oppCanUseTomay)
        {
            usableOpps.Add(tomayPrefab);
        }
        if (oppCanUseViewtl)
        {
            usableOpps.Add(viewtlPrefab);
        }


        //While loop. While the opponents team count is less than five, it will do the following.
        while(opponentTeam.Count < 5)
        {
            //Create a new random value variable between 0 and the amount of usable opponents
            int randomVal = Random.Range(0, usableOpps.Count);

            //If usable opponents at the random value equals dood in the list
            if (usableOpps[randomVal].name == "Dood")
            {
                //Spawn a new opponent object from dood prefab, add it to the opponent team, change the icon color to red and remove it from usable opponents now for the next loop. 
                //Same applies for all other players in this section.
                GameObject newOpp = Instantiate(doodPrefab);
                opponentTeam.Add(newOpp);
                doodIcon.color = Color.red;
                usableOpps.Remove(doodPrefab);
            }
            else if (usableOpps[randomVal].name == "Faedor")
            {
                GameObject newOpp = Instantiate(faedorPrefab);
                opponentTeam.Add(newOpp);
                faedorIcon.color = Color.red;
                usableOpps.Remove(faedorPrefab);
            }
            else if (usableOpps[randomVal].name == "Inirt")
            {
                GameObject newOpp = Instantiate(inirtPrefab);
                opponentTeam.Add(newOpp);
                inirtIcon.color = Color.red;
                usableOpps.Remove(inirtPrefab);
            }
            else if (usableOpps[randomVal].name == "Kcaz")
            {
                GameObject newOpp = Instantiate(kcazPrefab);
                opponentTeam.Add(newOpp);
                kcazIcon.color = Color.red;
                usableOpps.Remove(kcazPrefab);
            }
            else if (usableOpps[randomVal].name == "Kim")
            {
                GameObject newOpp = Instantiate(kimPrefab);
                opponentTeam.Add(newOpp);
                kimIcon.color = Color.red;
                usableOpps.Remove(kimPrefab);
            }
            else if (usableOpps[randomVal].name == "Leio")
            {
                GameObject newOpp = Instantiate(leioPrefab);
                opponentTeam.Add(newOpp);
                leioIcon.color = Color.red;
                usableOpps.Remove(leioPrefab);
            }
            else if (usableOpps[randomVal].name == "Mikeul")
            {
                GameObject newOpp = Instantiate(mikeulPrefab);
                opponentTeam.Add(newOpp);
                mikeulIcon.color = Color.red;
                usableOpps.Remove(mikeulPrefab);
            }
            else if (usableOpps[randomVal].name == "Prince")
            {
                GameObject newOpp = Instantiate(princePrefab);
                opponentTeam.Add(newOpp);
                princeIcon.color = Color.red;
                usableOpps.Remove(princePrefab);
            }
            else if (usableOpps[randomVal].name == "Sonja")
            {
                GameObject newOpp = Instantiate(sonjaPrefab);
                opponentTeam.Add(newOpp);
                sonjaIcon.color = Color.red;
                usableOpps.Remove(sonjaPrefab);
            }
            else if (usableOpps[randomVal].name == "Tomay")
            {
                GameObject newOpp = Instantiate(tomayPrefab);
                opponentTeam.Add(newOpp);
                tomayIcon.color = Color.red;
                usableOpps.Remove(tomayPrefab);
            }
            else if (usableOpps[randomVal].name == "Viewtl")
            {
                GameObject newOpp = Instantiate(viewtlPrefab);
                opponentTeam.Add(newOpp);
                viewtlIcon.color = Color.red;
                usableOpps.Remove(viewtlPrefab);
            }
        }

        //After two seconds, we "Invoke", we load a new level. Invoking means we delay it by however many seconds
        Invoke("LoadLevel", 2f);
    }

    //Load level function
    public void LoadLevel()
    {
        //Loads the game scene, called from the invoke above
        SceneManager.LoadScene("Game");
    }

    //Set player positions function
    public void SetupPlayerPositions(List<Transform> playerPositions, List<Transform> opponentPositions)
    {
        //Loop for players team
        for (int i = 0; i < playerTeam.Count; i++) 
        {
            //Set position and rotation on each loop, make them unselectable on player team, since we will only select opponents to attack.
            playerTeam[i].transform.position = playerPositions[i].position;
            playerTeam[i].transform.rotation = playerPositions[i].rotation;
            playerTeam[i].GetComponent<CharacterStats>().canSelect = false;
        }

        //Loop for opponents team
        for (int i = 0; i < opponentTeam.Count; i++)
        {
            //Set position and rotation on each loop
            opponentTeam[i].transform.position = opponentPositions[i].position;
            opponentTeam[i].transform.rotation = opponentPositions[i].rotation;
        }
    }

    public void RearrangePlayerTeamBySpeed()
    {

        float currentHighest = playerTeam[0].GetComponent<CharacterStats>().Speed;

        for (int i = 0; i < playerTeam.Count; i++)
        {
            if (playerTeam[i].GetComponent<CharacterStats>().Speed > currentHighest) 
            {
                currentHighest = playerTeam[i].GetComponent<CharacterStats>().Speed;
            }
        }
    }

    public void RearrangeOpponentTeamBySpeed()
    {

    }
}
