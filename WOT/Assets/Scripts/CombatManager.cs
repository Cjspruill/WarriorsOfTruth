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
    public bool PlayerHasChosenAction => playerHasChosenAction;
    private bool playerTurn = true; // start with player turn, then alternate

    private int playerIndex = 0;
    private int enemyIndex = 0;

    private GameObject currentActivePlayer;
    public GameObject CurrentActivePlayer => currentActivePlayer; // read-only property

    [SerializeField] GameObject currentTargetedEnemy;

    public bool GetPlayerTurn { get => playerTurn; set => playerTurn = value; }
    public GameObject GetCurrentTargetedEnemy { get => currentTargetedEnemy; set => currentTargetedEnemy = value; }

    private Coroutine currentPlayerActionCoroutine;

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
            characterStats[i].SetupScripts();
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
                        ReplenishEnergyForTeam(playerTeam);
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
                        ReplenishEnergyForTeam(opponentTeam);
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
        HUD.instance.ChangeAbilities(currentActivePlayer.GetComponent<CharacterStats>().characterName);

        var stats = player.GetComponent<CharacterStats>();
        Debug.Log("Player Turn: " + stats.characterName + " (Speed: " + stats.Speed + ")");
        Debug.Log("Waiting for action input from " + stats.characterName + "...");

        // Wait until player chooses an action and it finishes
        while (!playerHasChosenAction)
        {
            yield return null; // wait for button press
        }

        if (currentPlayerActionCoroutine != null)
        {
            yield return currentPlayerActionCoroutine; // wait for attack to finish
            currentPlayerActionCoroutine = null;
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

        attackerStats.animator.SetTrigger("Attack");

        // Wait until attack starts
        yield return new WaitUntil(() =>
            attackerStats.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));

        CalculateDamage(attacker, target);

        // Wait until we are no longer in Attack (and not transitioning back to it)
        yield return new WaitUntil(() =>
        {
            var state = attackerStats.animator.GetCurrentAnimatorStateInfo(0);
            return !state.IsName("Attack");
        });
        // --- Move back ---
        yield return StartCoroutine(MoveWithNavMesh(attackerStats.navMeshAgent, startPos, attackerStats.originalRotation));
    }

    // --- CALLED BY HUD BUTTON ---
    public void OnPlayerActionChosen(GameObject targetEnemy)
    {
        // Prevent spamming
        if (playerHasChosenAction) return;

        playerHasChosenAction = true; // lock action immediately
        currentPlayerActionCoroutine = StartCoroutine(PlayerAttackSequence(currentActivePlayer, targetEnemy));
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

        attackerStats.animator.SetTrigger("Attack");

        // Wait until attack starts
        yield return new WaitUntil(() =>
            attackerStats.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));

        CalculateDamage(attacker, target);

        // Wait until we are no longer in Attack (and not transitioning back to it)
        yield return new WaitUntil(() =>
        {
            var state = attackerStats.animator.GetCurrentAnimatorStateInfo(0);
            return !state.IsName("Attack");
        });

        // --- Move back ---
        yield return StartCoroutine(MoveWithNavMesh(attackerStats.navMeshAgent, startPos, attackerStats.originalRotation));
    }

    private IEnumerator MoveWithNavMesh(NavMeshAgent agent, Vector3 destination, Quaternion? finalRotation = null)
    {
        agent.isStopped = false;
        agent.SetDestination(destination);

        // Wait until agent arrives
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        agent.isStopped = true;


        yield return new WaitForSeconds(1f);
        
        // If a final rotation is specified, rotate smoothly
        if (finalRotation.HasValue)
        {
            Quaternion startRot = agent.transform.rotation;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * 5f; // adjust speed multiplier if needed
                agent.transform.rotation = Quaternion.Slerp(startRot, finalRotation.Value, t);
                yield return null;
            }
            agent.transform.rotation = finalRotation.Value; // snap to final rotation at the end
        }
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


    public void CalculateDamage(GameObject attacker, GameObject target)
    {
        // --- Accuracy Check ---
        float accRoll = Random.Range(0f, 100f);

        if (accRoll < attacker.GetComponent<CharacterStats>().Accuracy)
        {
            Debug.Log(attacker.name + " missed!");
            return;
        }

        float baseDamage = attacker.GetComponent<CharacterStats>().Attack * (attacker.GetComponent<CharacterStats>().Attack / (attacker.GetComponent<CharacterStats>().Attack + Mathf.Pow(target.GetComponent<CharacterStats>().Defense, 1.5f)));

        // --- Critical Roll ---
        float critRoll = Random.Range(0f, 100f);
        bool isCrit = critRoll < attacker.GetComponent<CharacterStats>().CriticalChance;

        if (isCrit)
        {
            baseDamage *= attacker.GetComponent<CharacterStats>().CriticalMultiplier;
            Debug.Log("CRITICAL HIT by " + attacker.name + "!");
        }

        int finalDamage = Mathf.Max(1, Mathf.RoundToInt(baseDamage));

        target.GetComponent<Health>().TakeDamage(finalDamage);
    }

    public void RemoveCharacterFromTeam(GameObject character)
    {
        if (playerTeam.Contains(character))
        {
            int removedIndex = playerTeam.IndexOf(character);
            playerTeam.Remove(character);
            Debug.Log(character.name + " removed from player team!");

            // Adjust playerIndex if necessary
            if (removedIndex <= playerIndex && playerIndex > 0)
                playerIndex--;
        }
        else if (opponentTeam.Contains(character))
        {
            int removedIndex = opponentTeam.IndexOf(character);
            opponentTeam.Remove(character);
            Debug.Log(character.name + " removed from opponent team!");

            // Adjust enemyIndex if necessary
            if (removedIndex <= enemyIndex && enemyIndex > 0)
                enemyIndex--;
        }

        // If the current active player died during their own turn,
        // mark their action as complete so combat doesn’t stall
        if (currentActivePlayer == character)
        {
            playerHasChosenAction = true;
            currentActivePlayer = null;
        }

        if(currentTargetedEnemy == character)
        {
            currentTargetedEnemy = null;
        }

        // Clamp indexes to avoid out-of-range errors
        playerIndex = Mathf.Clamp(playerIndex, 0, Mathf.Max(0, playerTeam.Count - 1));
        enemyIndex = Mathf.Clamp(enemyIndex, 0, Mathf.Max(0, opponentTeam.Count - 1));
    }


  void ReplenishEnergyForTeam(List<GameObject> team)
{
    for (int i = 0; i < team.Count; i++)
    {
        team[i].GetComponent<Energy>().GiveEnergy(2);
    }
}
}
