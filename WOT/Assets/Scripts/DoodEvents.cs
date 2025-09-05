using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DoodEvents : MonoBehaviour
{

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BoulderSmash()
    {
        Instantiate(CombatManager.instance.boulderSmashPrefab, CombatManager.instance.GetCurrentTargetedEnemy.transform.position, Quaternion.identity);
        CombatManager.instance.CurrentActivePlayer.GetComponent<Energy>().TakeEnergy(4);
        CombatManager.instance.GetCurrentTargetedEnemy.GetComponent<Health>().TakeDamage(25);
    }

    public void AbilityFinished()
    {
        CombatManager.instance.abilityFinished = true;
    }
}
