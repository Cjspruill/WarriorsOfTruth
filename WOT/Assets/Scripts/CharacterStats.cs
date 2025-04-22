using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CharacterStats : MonoBehaviour
{
    public bool canSelect = true;
    public GameObject selectionIcon;

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

    public float Speed { get => speed; set => speed = value; }
    public float Health { get => health; set => health = value; }
    public float Energy { get => energy; set => energy = value; }
    public float Defense { get => defense; set => defense = value; }
    public float CriticalChance { get => criticalChance; set => criticalChance = value; }
    public float CriticalMultiplier { get => criticalMultiplier; set => criticalMultiplier = value; }
    public float Accuracy { get => accuracy; set => accuracy = value; }


    // Start is called before the first frame update
    void Start()
    {
        //Dont destroy this object when loaded in main menu, or else the game wont work.
        DontDestroyOnLoad(gameObject);

        if (gameObject.name.Contains("Dood"))
        {
            Health = 75;
            attack = 80;
            Energy = 8;
            Speed = 45;
            Defense = 85;
            CriticalChance = 50;
            CriticalMultiplier = 1;
            Accuracy = 45;

            ability1 = Abilities1.BoulderSmash;
            ability2 = Abilities2.IncreaseCriticalChance;
        }
        else if (gameObject.name.Contains("Faedor"))
        {
            Health = 50;
            attack = 55;
            Energy = 6;
            Speed = 60;
            Defense = 70;
            CriticalChance = 45;
            CriticalMultiplier = 1.25f;
            Accuracy = 60;

            ability1 = Abilities1.IncreaseAccuracy;
            ability2 = Abilities2.Confuse2Opponents;
        }
        else if (gameObject.name.Contains("Inirt"))
        {
            Health = 65;
            attack = 50;
            Energy = 6;
            Speed = 55;
            Defense = 45;
            CriticalChance = 40;
            CriticalMultiplier = 1.25f;
            Accuracy = 60;

            ability1 = Abilities1.LightningBolt;
            ability2 = Abilities2.ConfuseAnOpponent;
        }
        else if (gameObject.name.Contains("Kcaz"))
        {
            Health = 80;
            attack = 60;
            Energy = 5;
            Speed = 90;
            Defense = 45;
            CriticalChance = 25;
            CriticalMultiplier = 1.5f;
            Accuracy = 75;

            ability1 = Abilities1.X2Damage;
            ability2 = Abilities2.IncreaseSpeed;
        }
        else if (gameObject.name.Contains("Kim"))
        {
            Health = 95;
            attack = 75;
            Energy = 9;
            Speed = 80;
            Defense = 65;
            CriticalChance = 20;
            CriticalMultiplier = 2;
            Accuracy = 85;

            ability1 = Abilities1.ShurikenThrow;
            ability2 = Abilities2.HealSelf;
        }
        else if (gameObject.name.Contains("Leio"))
        {
            Health = 60;
            attack = 60;
            Energy = 6;
            Speed = 65;
            Defense = 70;
            CriticalChance = 70;
            CriticalMultiplier = .75f;
            Accuracy = 60;

            ability1 = Abilities1.WaterStream;
            ability2 = Abilities2.AttackTeam;
        }
        else if (gameObject.name.Contains("Mikeul"))
        {
            Health = 50;
            attack = 55;
            Energy = 7;
            Speed = 45;
            Defense = 65;
            CriticalChance = 30;
            CriticalMultiplier = 1.25f;
            Accuracy = 80;

            ability1 = Abilities1.LowerSpeed;
            ability2 = Abilities2.RegainEnergy;
        }
        else if (gameObject.name.Contains("Prince"))
        {
            Health = 70;
            attack = 85;
            Energy = 8;
            Speed = 55;
            Defense = 80;
            CriticalChance = 10;
            CriticalMultiplier = 2;
            Accuracy = 35;

            ability1 = Abilities1.HealAll;
            ability2 = Abilities2.IncreaseCriticalDamage;
        }
        else if (gameObject.name.Contains("Sonja"))
        {
            Health = 90;
            attack = 65;
            Energy = 7;
            Speed = 40;
            Defense = 50;
            CriticalChance = 50;
            CriticalMultiplier = 1.15f;
            Accuracy = 50;

            ability1 = Abilities1.Fireball;
            ability2 = Abilities2.HealTeammate;
        }
        else if (gameObject.name.Contains("Tomay"))
        {
            Health = 85;
            attack = 70;
            Energy = 5;
            Speed = 60;
            Defense = 60;
            CriticalChance = 20;
            CriticalMultiplier = 1.75f;
            Accuracy = 65;

            ability1 = Abilities1.ExtraTurn;
            ability2 = Abilities2.OpponentLosesATurn;
        }
        else if (gameObject.name.Contains("Viewtl"))
        {
            Health = 45;
            attack = 35;
            Energy = 7;
            Speed = 71;
            Defense = 100;
            CriticalChance = 85;
            CriticalMultiplier = 1.75f;
            Accuracy = 65;

            ability1 = Abilities1.OpponentsLoseATurn;
            ability2 = Abilities2.Vortex;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        CharacterStats[] charStats = FindObjectsOfType<CharacterStats>();

        for (int i = 0; i < charStats.Length; i++)
        {
            if (charStats[i].selectionIcon != null)
            {
                charStats[i].selectionIcon.SetActive(false);
            }
        }


        if (selectionIcon != null)
        {
            selectionIcon.SetActive(true);
        }
    }
}
