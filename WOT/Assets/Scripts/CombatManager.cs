using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance { get; private set; }

    [Header("Teams")]
    [SerializeField] private List<GameObject> playerTeam = new List<GameObject>();
    [SerializeField] private List<GameObject> opponentTeam = new List<GameObject>();

    private bool playerHasChosenAction = false;
    private bool playerTurn = true; // start with player turn, then alternate

    private int playerIndex = 0;
    private int enemyIndex = 0;

    private GameObject currentActivePlayer;
    public GameObject CurrentActivePlayer => currentActivePlayer; // read-only property

    [SerializeField] GameObject currentTargetedEnemy;

    public bool GetPlayerTurn { get => playerTurn; set => playerTurn = value; }
    public GameObject GetCurrentTargetedEnemy { get => currentTargetedEnemy; set => currentTargetedEnemy = value; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {

        CharacterStats[] characterStats = FindObjectsByType<CharacterStats>(FindObjectsSortMode.None);

        for(int i = 0; i < characterStats.Length; i++)           
        {
            characterStats[i].StartNavMeshAgent();
        }

        playerTeam = GameManager.instance.GetPlayerTeam();
        opponentTeam = GameManager.instance.GetOpponentTeam();

        // Sort once at the beginning
        RearrangePlayerTeamBySpeed();
        RearrangeOpponentTeamBySpeed();

        StartCoroutine(RunCombatLoop());
    }

    // --- MAIN COMBAT LOOP ---
    private IEnumerator RunCombatLoop()
    {
        while (true) // add victory/defeat condition later
        {
            if (GetPlayerTurn)
            {
                if (playerTeam.Count > 0)
                {
                    GameObject activePlayer = playerTeam[playerIndex];
                    yield return StartCoroutine(HandlePlayerTurn(activePlayer));

                    // Advance to next fighter
                    playerIndex++;
                    if (playerIndex >= playerTeam.Count)
                    {
                        playerIndex = 0; // restart new round
                        RearrangePlayerTeamBySpeed(); // re-sort for next round
                    }
                }
            }
            else
            {
                if (opponentTeam.Count > 0)
                {
                    GameObject activeEnemy = opponentTeam[enemyIndex];
                    yield return StartCoroutine(HandleOpponentTurn(activeEnemy));

                    // Advance to next fighter
                    enemyIndex++;
                    if (enemyIndex >= opponentTeam.Count)
                    {
                        enemyIndex = 0; // restart new round
                        RearrangeOpponentTeamBySpeed(); // re-sort for next round
                    }
                }
            }

            // Alternate teams each turn
            GetPlayerTurn = !GetPlayerTurn;
        }
    }

    // --- PLAYER TURN ---
    private IEnumerator HandlePlayerTurn(GameObject player)
    {
        playerHasChosenAction = false;
        currentActivePlayer = player;

        var stats = player.GetComponent<CharacterStats>();
        Debug.Log("Player Turn: " + stats.characterName + " (Speed: " + stats.Speed + ")");
        Debug.Log("Waiting for action input from " + stats.characterName + "...");

        while (!playerHasChosenAction)
        {
            yield return null;
        }

        Debug.Log(stats.characterName + " has finished their turn!");
    }

    // --- ENEMY TURN ---
    private IEnumerator HandleOpponentTurn(GameObject enemy)
    {
        var stats = enemy.GetComponent<CharacterStats>();

        Debug.Log("Opponent Turn: " + stats.characterName + " (Speed: " + stats.Speed + ")");

        // Pick a random target from the player team
        if (playerTeam.Count == 0)
        {
            Debug.LogWarning("No players left to target!");
            yield break;
        }

        GameObject randomTarget = playerTeam[Random.Range(0, playerTeam.Count)];

        // Run the enemy attack sequence (just like player)
        yield return StartCoroutine(EnemyAttackSequence(enemy, randomTarget));

        Debug.Log(stats.characterName + " finished their turn!");
    }

    private IEnumerator EnemyAttackSequence(GameObject attacker, GameObject target)
    {
        CharacterStats attackerStats = attacker.GetComponent<CharacterStats>();
        Vector3 startPos = attackerStats.originalPosition;
        Vector3 targetPos = target.transform.position;

        // --- Move to target ---
        yield return StartCoroutine(MoveWithNavMesh(attackerStats.navMeshAgent, targetPos));

        // --- Perform attack ---
        Debug.Log(attackerStats.characterName + " attacks " + target.GetComponent<CharacterStats>().characterName + "!");
        yield return new WaitForSeconds(0.5f); // simulate attack animation

        // --- Move back ---
        yield return StartCoroutine(MoveWithNavMesh(attackerStats.navMeshAgent, startPos));
    }

    // --- CALLED BY HUD BUTTON ---
    public void OnPlayerActionChosen(GameObject targetEnemy)
    {
        StartCoroutine(PlayerAttackSequence(currentActivePlayer, targetEnemy));
    }

    private IEnumerator PlayerAttackSequence(GameObject attacker, GameObject target)
    {
        CharacterStats attackerStats = attacker.GetComponent<CharacterStats>();
        Vector3 startPos = attackerStats.originalPosition;
        Vector3 targetPos = target.transform.position;

        // --- Move to enemy ---
        yield return StartCoroutine(MoveWithNavMesh(attackerStats.navMeshAgent, targetPos));

        // --- Perform attack ---
        Debug.Log(attackerStats.characterName + " attacks " + target.GetComponent<CharacterStats>().characterName + "!");
        yield return new WaitForSeconds(0.5f); // simulate attack animation

        // --- Move back ---
        yield return StartCoroutine(MoveWithNavMesh(attackerStats.navMeshAgent, startPos));

        // End action
        playerHasChosenAction = true;
    }

    private IEnumerator MoveWithNavMesh(NavMeshAgent agent, Vector3 destination)
    {
        agent.isStopped = false;
        agent.SetDestination(destination);

        // Wait until agent arrives
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        agent.isStopped = true;
    }

    // --- SPEED SORTING ---
    private void RearrangePlayerTeamBySpeed()
    {
        playerTeam.Sort((a, b) => b.GetComponent<CharacterStats>().Speed.CompareTo(a.GetComponent<CharacterStats>().Speed));
    }

    private void RearrangeOpponentTeamBySpeed()
    {
        opponentTeam.Sort((a, b) => b.GetComponent<CharacterStats>().Speed.CompareTo(a.GetComponent<CharacterStats>().Speed));
    }
}
