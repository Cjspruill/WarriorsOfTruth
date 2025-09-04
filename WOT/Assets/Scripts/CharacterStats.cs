using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

public class CharacterStats : MonoBehaviour
{
    public CharacterData characterData;

    public bool canSelect = true;
    public GameObject selectionIcon;

    [SerializeField] public string characterName;
    [SerializeField] float health;
    [SerializeField] float attack;
    [SerializeField] int energy;
    [SerializeField] float speed;
    [SerializeField] float defense;
    [SerializeField] float criticalChance;
    [SerializeField] float criticalMultiplier;
    [SerializeField] float accuracy;

    [SerializeField] public Vector3 originalPosition { get; set; }
    [SerializeField] public Quaternion originalRotation { get; set; }
    [SerializeField] public NavMeshAgent navMeshAgent;
    [SerializeField] public Animator animator;
    [SerializeField] Energy energyScript;
  
    //An ability 1 of the above named abilies.
    public Abilities1 ability1;
    public Abilities2 ability2;

    public float Speed { get => speed; set => speed = value; }
    public float Health { get => health; set => health = value; }
    public int Energy { get => energy; set => energy = value; }
    public float Defense { get => defense; set => defense = value; }
    public float CriticalChance { get => criticalChance; set => criticalChance = value; }
    public float CriticalMultiplier { get => criticalMultiplier; set => criticalMultiplier = value; }
    public float Accuracy { get => accuracy; set => accuracy = value; }
    public float Attack { get => attack; set => attack = value; }


    // Start is called before the first frame update
    void Start()
    {
        //Dont destroy this object when loaded in main menu, or else the game wont work.
        DontDestroyOnLoad(gameObject);


        Health = characterData.health;
        Attack = characterData.attack;
        Energy = characterData.energy;
        Speed = characterData.speed;
        Defense = characterData.defense;
        CriticalChance = characterData.criticalChance;
        CriticalMultiplier = characterData.criticalMultiplier;
        Accuracy = characterData.accuracy;

        ability1 = characterData.ability1;
        ability2 = characterData.ability2;

    }

    public void SetupScripts()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
        GetComponent<EnemyClickHandler>().enabled = true;
        animator = GetComponent<Animator>();
        originalPosition = transform.position;
        originalRotation = transform.rotation; // store starting rotation
        GetComponentInChildren<FaceCamera>().StartLookAt();
        energyScript = GetComponent<Energy>();
    }

    // Update is called once per frame
    void Update()
    {

        if (animator != null && navMeshAgent!=null)
            animator.SetBool("IsWalking", navMeshAgent.velocity.magnitude >= .1f);
       
    }


    public void SelectCharacter()
    {
        

    }
}
