using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

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
    public List<GameObject> OpponentTeam { get => opponentTeam; set => opponentTeam = value; }

    private Coroutine currentPlayerActionCoroutine;

    public GameObject boulderSmashPrefab;
    public GameObject lightningBoltPrefab;
    public GameObject attackTeamPrefab;
    public bool abilityFinished = false;

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

        for (int i = 0; i < characterStats.Length; i++)
        {
            characterStats[i].SetupScripts();
        }

        playerTeam = GameManager.instance.GetPlayerTeam();
        OpponentTeam = GameManager.instance.GetOpponentTeam();

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
                if (OpponentTeam.Count > 0)
                {
                    GameObject activeEnemy = OpponentTeam[enemyIndex];
                    yield return StartCoroutine(HandleOpponentTurn(activeEnemy));

                    // Advance to next fighter
                    enemyIndex++;
                    if (enemyIndex >= OpponentTeam.Count)
                    {
                        enemyIndex = 0; // restart new round
                        RearrangeOpponentTeamBySpeed(); // re-sort for next round
                        ReplenishEnergyForTeam(OpponentTeam);
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
        OpponentTeam.Sort((a, b) => b.GetComponent<CharacterStats>().Speed.CompareTo(a.GetComponent<CharacterStats>().Speed));
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
        else if (OpponentTeam.Contains(character))
        {
            int removedIndex = OpponentTeam.IndexOf(character);
            OpponentTeam.Remove(character);
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

        if (currentTargetedEnemy == character)
        {
            currentTargetedEnemy = null;
        }

        // Clamp indexes to avoid out-of-range errors
        playerIndex = Mathf.Clamp(playerIndex, 0, Mathf.Max(0, playerTeam.Count - 1));
        enemyIndex = Mathf.Clamp(enemyIndex, 0, Mathf.Max(0, OpponentTeam.Count - 1));
    }


    void ReplenishEnergyForTeam(List<GameObject> team)
    {
        for (int i = 0; i < team.Count; i++)
        {
            team[i].GetComponent<Energy>().GiveEnergy(2);
        }
    }

    #region Abilities
    public void BoulderSmash(GameObject attacker, GameObject target)
    {
        if (attacker.GetComponent<Energy>().GetEnergy < 4) return;


        playerHasChosenAction = true; // lock action
        currentPlayerActionCoroutine = StartCoroutine(BoulderSmashSequence(attacker, target));
    }

    private IEnumerator BoulderSmashSequence(GameObject attacker, GameObject target)
    {
        var attackerStats = attacker.GetComponent<CharacterStats>();

        // Spend energy
        attacker.GetComponent<Energy>().TakeEnergy(4);

        abilityFinished = false;

        // Play Boulder Smash animation
        attackerStats.animator.SetTrigger("Ability1");

        // Wait until the animation event sets abilityFinished = true
        yield return new WaitUntil(() => abilityFinished);

        Debug.Log(attackerStats.characterName + " finished Boulder Smash!");

        abilityFinished = false;
    }


    public void IncreaseAccuracy(GameObject attacker) 
    {

    }

    public void LightningBolt(GameObject attacker, GameObject target)
    {
        if (attacker.GetComponent<Energy>().GetEnergy < 6) return;


        playerHasChosenAction = true; // lock action
        currentPlayerActionCoroutine = StartCoroutine(LightningBoltSequence(attacker, target));
    }

    private IEnumerator LightningBoltSequence(GameObject attacker, GameObject target)
    {
        var attackerStats = attacker.GetComponent<CharacterStats>();

        // Spend energy
        attacker.GetComponent<Energy>().TakeEnergy(6);

        abilityFinished = false;

        // Play Boulder Smash animation
        attackerStats.animator.SetTrigger("Ability1");

        // Wait until the animation event sets abilityFinished = true
        yield return new WaitUntil(() => abilityFinished);

        target.GetComponent<Health>().TakeDamage(35);

        Debug.Log(attackerStats.characterName + " finished Lightning Bolt!");

        abilityFinished = false;
    }

    public void X2Damge(GameObject attacker)
    {

    }

    public void ShurikenThrow(GameObject attacker, GameObject target)
    {

    }

    public void WaterStream(GameObject attacker, GameObject target)
    {

    }

    public void LowerSpeed(GameObject attacker, GameObject target)
    {

    }

    public void HealAll(GameObject attacker)
    {

    }

    public void Fireball(GameObject attacker, GameObject target)
    {

    }

    public void ExtraTurn()
    {

    }

    public void OpponentsLoseATurn()
    {

    }


    public void IncreaseCriticalChance(GameObject attacker)
    {

    }

    public void Confuse2Opponents()
    {

    }

    public void ConfuseAnOpponent()
    {

    }

    public void IncreaseSpeed()
    {

    }
    public void HealSelf()
    {
    }

    public void AttackTeam(GameObject attacker)
    {
        if (attacker.GetComponent<Energy>().GetEnergy < 3) return;
        playerHasChosenAction = true; // lock action
        currentPlayerActionCoroutine = StartCoroutine(AttackTeamSequence(attacker));
    }

    private IEnumerator AttackTeamSequence(GameObject attacker)
    {
        var attackerStats = attacker.GetComponent<CharacterStats>();

        // Spend energy
        attacker.GetComponent<Energy>().TakeEnergy(3);

        abilityFinished = false;

        // Play Boulder Smash animation
        attackerStats.animator.SetTrigger("Ability2");

        // Wait until the animation event sets abilityFinished = true
        yield return new WaitUntil(() => abilityFinished);

        Debug.Log(attackerStats.characterName + " finished Attack Team!");

        abilityFinished = false;
    }

    public void RegainEnergy()
    {

    }

    public void IncreaseCriticalDamage()
    {

    }

    public void HealTeammate()
    {

    }

    public void OpponentLosesATurn()
    {

    }

    public void Vortex()
    {

    }

    #endregion
}
