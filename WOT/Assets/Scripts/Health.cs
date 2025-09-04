using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float maxHealth;
    [SerializeField] Slider healthSlider;
    [SerializeField] CharacterStats characterStats;
    [SerializeField] Animator animator;
    [SerializeField] EnemyClickHandler enemyClickHandler;
    public float GetHealth { get => health; set => health = value; }


    // Start is called before the first frame update
    void Start()
    {
        enemyClickHandler = GetComponent<EnemyClickHandler>();
        characterStats = GetComponent<CharacterStats>();
        maxHealth = characterStats.Health;
        GetHealth = maxHealth;
        healthSlider.maxValue = health;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = GetHealth;
    }

    public void TakeDamage(float value)
    {
        GetHealth -= value;

        if(GetHealth <= 0)
        {
            healthSlider.gameObject.SetActive(false);
            GetComponent<Energy>().energySlider.gameObject.SetActive(false);
            animator.SetTrigger("Death");
            enemyClickHandler.TurnOffSelector();
            CombatManager.instance.RemoveCharacterFromTeam(gameObject);
        }    
    }

    public void GiveHealth(float value)
    {
        GetHealth += value;

        if (GetHealth >= maxHealth)
        {
            GetHealth = maxHealth;
        }
    }
}
