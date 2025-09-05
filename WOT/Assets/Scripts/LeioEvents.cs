using UnityEngine;

public class LeioEvents : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackTeam()
    {
        for (int i = 0; i < CombatManager.instance.OpponentTeam.Count; i++) 
        {
            Instantiate(CombatManager.instance.attackTeamPrefab, CombatManager.instance.OpponentTeam[i].transform.position, Quaternion.identity);
            float damageToTake = CombatManager.instance.OpponentTeam[i].GetComponent<Health>().GetHealth * .20f;
            CombatManager.instance.OpponentTeam[i].GetComponent<Health>().TakeDamage(damageToTake);
        }
    }

    public void AbilityFinished()
    {
        CombatManager.instance.abilityFinished = true;
    }
}
