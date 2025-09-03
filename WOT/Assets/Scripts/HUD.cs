using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    //References that will be swapped
    [SerializeField] Image attackImage;
    [SerializeField] Image ability1Image;
    [SerializeField] Image ability2Image;

    //Texts to update
    [SerializeField] TextMeshProUGUI currentFighterText;
    [SerializeField] TextMeshProUGUI currentAbility1Text;
    [SerializeField] TextMeshProUGUI currentAbility2Text;

    [SerializeField] Sprite attackIcon;
    //Abilities 1
    [SerializeField] Sprite boulderSmashIcon;
    [SerializeField] Sprite increaseAccuracyIcon;
    [SerializeField] Sprite LightningBoltIcon;
    [SerializeField] Sprite x2DamageIcon;
    [SerializeField] Sprite shurikenThrowIcon;
    [SerializeField] Sprite waterStreamIcon;
    [SerializeField] Sprite lowerSpeedIcon;
    [SerializeField] Sprite healAllIcon;
    [SerializeField] Sprite fireBallIcon;
    [SerializeField] Sprite extraTurnIcon;
    [SerializeField] Sprite opponentsLoseATurnIcon;
    
    //Abilities 2
    [SerializeField] Sprite increaseCriticalChanceIcon;
    [SerializeField] Sprite confuse2OpponentsIcon;
    [SerializeField] Sprite confuseAnOpponentIcon;
    [SerializeField] Sprite increaseSpeedIcon;
    [SerializeField] Sprite healSelfIcon;
    [SerializeField] Sprite attackTeamIcon;
    [SerializeField] Sprite regainEnergyIcon;
    [SerializeField] Sprite increaseCriticialDamageIcon;
    [SerializeField] Sprite healTeammateIcon;
    [SerializeField] Sprite loseATurnIcon;
    [SerializeField] Sprite vortexIcon;

    [SerializeField] Button attackButton;
    [SerializeField] Button ability1Button;
    [SerializeField] Button ability2Button;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeAbilities()
    {
        
    }

    public void AttackButtonClicked()
    {
        if (CombatManager.instance.GetCurrentTargetedEnemy != null)
        CombatManager.instance.OnPlayerActionChosen(CombatManager.instance.GetCurrentTargetedEnemy);
    }
}
