using DigitalRuby.LightningBolt;
using UnityEngine;

public class InirtEvents : MonoBehaviour
{
    [SerializeField] Transform lightningBoltStartPosition;
    bool isLightning;

    GameObject currentLightningBolt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LightningBolt()
    {
        currentLightningBolt = Instantiate(CombatManager.instance.lightningBoltPrefab, lightningBoltStartPosition.position, Quaternion.identity);
        //Place second line element
        LightningBoltScript lightningBoltScript = currentLightningBolt.GetComponent<LightningBoltScript>();
        lightningBoltScript.StartObject = lightningBoltStartPosition.gameObject;
        lightningBoltScript.EndObject = CombatManager.instance.GetCurrentTargetedEnemy.gameObject;
        CombatManager.instance.CurrentActivePlayer.GetComponent<Energy>().TakeEnergy(6);
        isLightning = true;
    }

    public void EndLightning()
    {
        if (isLightning)
        {
            Destroy(currentLightningBolt);

            isLightning = false;
        }
    }

    public void AbilityFinished()
    {
        CombatManager.instance.abilityFinished = true;
    }
}
