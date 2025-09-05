using UnityEngine;
using UnityEngine.UI;
public class Energy : MonoBehaviour
{
    [SerializeField] int energy;
    [SerializeField] int maxEnergy;
    [SerializeField] public Slider energySlider;
    [SerializeField] CharacterStats characterStats;
    public int GetEnergy { get => energy; set => energy = value; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterStats = GetComponent<CharacterStats>();
        maxEnergy = characterStats.Energy;

        energySlider.maxValue = maxEnergy;
        GetEnergy = maxEnergy;

    }

    // Update is called once per frame
    void Update()
    {
        energySlider.value = GetEnergy;
    }

    public void GiveEnergy(int value)
    {
        GetEnergy += value;

        if (GetEnergy >= maxEnergy)
        {
            GetEnergy = maxEnergy;
        }

    }

    public void TakeEnergy(int value)
    { 
        GetEnergy -= value;

        if (GetEnergy <= 0)
        {
            GetEnergy = 0;
        }
    }
}
