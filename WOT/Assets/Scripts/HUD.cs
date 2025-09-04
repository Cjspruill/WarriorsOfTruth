using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD instance { get; private set; }

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
    [SerializeField] Sprite lightningBoltIcon;
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


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CombatManager.instance != null)
        {
            // Disable if player already acted
            attackButton.interactable = !CombatManager.instance.PlayerHasChosenAction;

            // Do the same for ability buttons if you want:
             ability1Button.interactable = !CombatManager.instance.PlayerHasChosenAction;
             ability2Button.interactable = !CombatManager.instance.PlayerHasChosenAction;

            if(CombatManager.instance.CurrentActivePlayer!=null)
             currentFighterText.text = CombatManager.instance.CurrentActivePlayer.GetComponent<CharacterStats>().characterName + "'s Turn";
        }
    }

    public void ChangeAbilities(string characterName)
    {
        switch (characterName)
        {
            case "Dood":
                ability1Image.sprite = boulderSmashIcon;
                ability2Image.sprite = increaseCriticalChanceIcon;
                currentAbility1Text.text = "Boulder Smash";
                currentAbility2Text.text = "Increase Critical Chance";
                break;
            case "Faedor":
                ability1Image.sprite = increaseAccuracyIcon;
                ability2Image.sprite = confuse2OpponentsIcon;
                currentAbility1Text.text = "Increase Accuracy";
                currentAbility2Text.text = "Confuse 2 Opponents";
                break;
            case "Inirt":
                ability1Image.sprite = lightningBoltIcon;
                ability2Image.sprite = confuseAnOpponentIcon;
                currentAbility1Text.text = "Lightning Bolt";
                currentAbility2Text.text = "Confuse an Opponent";
                break;
            case "Kcaz":
                ability1Image.sprite = x2DamageIcon;
                ability2Image.sprite = increaseSpeedIcon;
                currentAbility1Text.text = "X2 Damage";
                currentAbility2Text.text = "Increase Speed";
                break;
            case "Kim":
                ability1Image.sprite = shurikenThrowIcon;
                ability2Image.sprite = healSelfIcon;
                currentAbility1Text.text = "Shuriken Throw";
                currentAbility2Text.text = "Heal Self";
                break;
            case "Leio":
                ability1Image.sprite = waterStreamIcon;
                ability2Image.sprite = attackTeamIcon;
                currentAbility1Text.text = "Water Stream";
                currentAbility2Text.text = "Attack Team";
                break;
            case "Mikeul":
                ability1Image.sprite = lowerSpeedIcon;
                ability2Image.sprite = regainEnergyIcon;
                currentAbility1Text.text = "Lower Opponent Speed";
                currentAbility2Text.text = "Regain Energy";
                break;
            case "Prince":
                ability1Image.sprite = healAllIcon;
                ability2Image.sprite = increaseCriticialDamageIcon;
                currentAbility1Text.text = "Heal All";
                currentAbility2Text.text = "Increase Critical Damage";
                break;
            case "Sonja":
                ability1Image.sprite = fireBallIcon;
                ability2Image.sprite = healTeammateIcon;
                currentAbility1Text.text = "Fireball";
                currentAbility2Text.text = "Heal Teammate";
                break;
            case "Tomay":
                ability1Image.sprite = extraTurnIcon;
                ability2Image.sprite = loseATurnIcon;
                currentAbility1Text.text = "Extra Turn";
                currentAbility2Text.text = "Opponent Loses A Turn";
                break;
            case "Viewtl":
                ability1Image.sprite = opponentsLoseATurnIcon;
                ability2Image.sprite = vortexIcon;
                currentAbility1Text.text = "Opponents Lose A Turn";
                currentAbility2Text.text = "Vortex";
                break;
        }
    }

    public void AttackButtonClicked()
    {
        if (CombatManager.instance.GetCurrentTargetedEnemy != null)
        CombatManager.instance.OnPlayerActionChosen(CombatManager.instance.GetCurrentTargetedEnemy);
    }

    public void SpeedUpGameTime(int speed)
    {
        switch (speed)
        {
            case 1:
                Time.timeScale = 1;
                break;
            case 2:
                Time.timeScale = 2;
                break;
            case 3:
                Time.timeScale = 3;
                break;
            case 5:
                Time.timeScale = 5;
                break;
        }
    }
}
