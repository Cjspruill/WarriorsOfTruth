using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CharacterSelector : MonoBehaviour
{
    //Text references
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI attackText;
    [SerializeField] TextMeshProUGUI energyText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI defenseText;
    [SerializeField] TextMeshProUGUI criticalChanceText;
    [SerializeField] TextMeshProUGUI criticalMultiplierText;
    [SerializeField] TextMeshProUGUI accuracyText;
    [SerializeField] TextMeshProUGUI ability1Text;
    [SerializeField] TextMeshProUGUI ability2Text;
    [SerializeField] TextMeshProUGUI nameText;

    //Stat variables
    [SerializeField] float health;
    [SerializeField] float attack;
    [SerializeField] float energy;
    [SerializeField] float speed;
    [SerializeField] float defense;
    [SerializeField] float criticalChance;
    [SerializeField] float criticalMultiplier;
    [SerializeField] float accuracy;


    //Enum or list of abilities
    public enum Abilities1
    {
        BoulderSmash,
        IncreaseAccuracy,
        LightningBolt,
        X2Damage,
        ShurikenThrow,
        WaterStream,
        LowerSpeed,
        HealAll,
        Fireball,
        ExtraTurn,
        OpponentsLoseATurn
    }

    //An ability 1 of the above named abilies.
    public Abilities1 ability1;


    //Same logic applies for ablities 2
    public enum Abilities2
    {
        IncreaseCriticalChance,
        Confuse2Opponents,
        ConfuseAnOpponent,
        IncreaseSpeed,
        HealSelf,
        AttackTeam,
        RegainEnergy,
        IncreaseCriticalDamage,
        HealTeammate,
        OpponentLosesATurn,
        Vortex
    }

    public Abilities2 ability2;
    [Header("Character Data List")]
    [SerializeField] List<CharacterData> allCharacters = new List<CharacterData>();

    public CharacterData currentCharacter;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCharacter(string charName)
    {
        // find the character by name
        currentCharacter = allCharacters.Find(c => c.characterName == charName);

        if (currentCharacter == null)
        {
            Debug.LogWarning("No character data found for " + charName);
            return;
        }

        // update UI
        nameText.text = currentCharacter.characterName;
        healthText.text = "Health: " + currentCharacter.health;
        attackText.text = "Attack: " + currentCharacter.attack;
        energyText.text = "Energy: " + currentCharacter.energy;
        speedText.text = "Speed: " + currentCharacter.speed;
        defenseText.text = "Defense: " + currentCharacter.defense;
        criticalChanceText.text = "Critical Chance: " + currentCharacter.criticalChance;
        criticalMultiplierText.text = "Critical Multiplier: " + currentCharacter.criticalMultiplier;
        accuracyText.text = "Accuracy: " + currentCharacter.accuracy;

        ability1Text.text = currentCharacter.ability1.ToString();
        ability2Text.text = currentCharacter.ability2.ToString();

        // store for GameManager
        GameManager.instance.selectedCharacterName = currentCharacter.characterName;
    }
}
