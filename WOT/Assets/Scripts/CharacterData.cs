using UnityEngine;

public enum Abilities1
{
    BoulderSmash,
    IncreaseAccuracy,
    LightningBolt,
    X2Damage,
    ShurikenThrow,
    WaterStream,
    LowerSpeed,
    HealAll,
    Fireball,
    ExtraTurn,
    OpponentsLoseATurn

}

public enum Abilities2
{
    IncreaseCriticalChance,
    Confuse2Opponents,
    ConfuseAnOpponent,
    IncreaseSpeed,
    HealSelf,
    AttackTeam,
    RegainEnergy,
    IncreaseCriticalDamage,
    HealTeammate,
    OpponentLosesATurn,
    Vortex
}

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Base Stats")]
    public string characterName;
    public float health;
    public float attack;
    public int energy;
    public float speed;
    public float defense;
    public float criticalChance;
    public float criticalMultiplier;
    public float accuracy;

    [Header("Abilities")]
    public Abilities1 ability1;
    public Abilities2 ability2;
}
