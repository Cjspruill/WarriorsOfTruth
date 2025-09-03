using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class CharacterContainer
{
    public string characterName;
    public GameObject prefab;
    [HideInInspector] public GameObject instance;
    [HideInInspector] public bool opponentCanUse = true;
}

public class GameManager : MonoBehaviour
{
    [Header("All Characters Setup")]
    [SerializeField] private List<CharacterContainer> allCharacters = new List<CharacterContainer>();

    [Header("Teams")]
    [SerializeField] private List<GameObject> playerTeam = new List<GameObject>();
    [SerializeField] private List<GameObject> opponentTeam = new List<GameObject>();

    [Header("UI")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button[] addRemoveButtons;

    [Header("Character Icons")]
    [SerializeField] private List<Image> characterIcons = new List<Image>(); // Order must match allCharacters

    [Header("Selection")]
    public string selectedCharacterName;

    // Singleton
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);

        DontDestroyOnLoad(instance);
    }

    // --- TEAM MANAGEMENT ---

    public void AddCharacterToPlayerTeam()
    {
        if (playerTeam.Count >= 5) return;

        int index = allCharacters.FindIndex(c => c.characterName == selectedCharacterName);
        if (index == -1) return;

        CharacterContainer container = allCharacters[index];

        // Spawn a new instance if not already in team
        if (container.instance == null)
            container.instance = Instantiate(container.prefab);

        if (!playerTeam.Contains(container.instance))
        {
            playerTeam.Add(container.instance);
            container.opponentCanUse = false;

            // Update UI icon to blue for player
            characterIcons[index].color = Color.blue;
        }

        if (playerTeam.Count == 5)
        {
            foreach (var button in addRemoveButtons)
                button.gameObject.SetActive(false);

            Invoke(nameof(AddPlayersToOpponentTeam), 2f);
        }
    }

    public void RemoveCharacterFromPlayerTeam()
    {
        int index = allCharacters.FindIndex(c => c.characterName == selectedCharacterName);
        if (index == -1) return;

        CharacterContainer container = allCharacters[index];

        if (container.instance != null && playerTeam.Contains(container.instance))
        {
            Destroy(container.instance);
            playerTeam.Remove(container.instance);
            container.instance = null;
            container.opponentCanUse = true;

            // Reset icon color
            characterIcons[index].color = Color.white;
        }
    }

    public void AddPlayersToOpponentTeam()
    {
        // Build list of usable opponents (exclude already in player team)
        List<int> usableIndexes = new List<int>();
        for (int i = 0; i < allCharacters.Count; i++)
        {
            if (allCharacters[i].opponentCanUse)
                usableIndexes.Add(i);
        }

        while (opponentTeam.Count < 5 && usableIndexes.Count > 0)
        {
            int randIndex = Random.Range(0, usableIndexes.Count);
            int charIndex = usableIndexes[randIndex];

            CharacterContainer container = allCharacters[charIndex];

            // Spawn opponent instance
            GameObject newOpp = Instantiate(container.prefab);
            container.instance = newOpp;
            opponentTeam.Add(newOpp);

            // Update UI icon to red for opponent
            characterIcons[charIndex].color = Color.red;

            container.opponentCanUse = false;
            usableIndexes.RemoveAt(randIndex);
        }

        Invoke(nameof(LoadLevel), 2f);
    }

    // --- SCENE MANAGEMENT ---
    public void LoadLevel()
    {
        SceneManager.LoadScene("Game");
    }

    public void SetupPlayerPositions(List<Transform> playerPositions, List<Transform> opponentPositions)
    {
        for (int i = 0; i < playerTeam.Count; i++)
        {
            playerTeam[i].transform.position = playerPositions[i].position;
            playerTeam[i].transform.rotation = playerPositions[i].rotation;
            playerTeam[i].GetComponent<CharacterStats>().canSelect = false;
        }

        for (int i = 0; i < opponentTeam.Count; i++)
        {
            opponentTeam[i].transform.position = opponentPositions[i].position;
            opponentTeam[i].transform.rotation = opponentPositions[i].rotation;
        }
    }

    // --- SPEED SORTING ---
    public void RearrangePlayerTeamBySpeed()
    {
        playerTeam.Sort((a, b) => b.GetComponent<CharacterStats>().Speed.CompareTo(a.GetComponent<CharacterStats>().Speed));
    }

    public void RearrangeOpponentTeamBySpeed()
    {
        opponentTeam.Sort((a, b) => b.GetComponent<CharacterStats>().Speed.CompareTo(a.GetComponent<CharacterStats>().Speed));
    }
}
