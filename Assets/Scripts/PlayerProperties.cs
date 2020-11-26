using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    public static PlayerProperties instance;
    [Header("Стартовые характеристики:")]
    public int StartHealthPoints = 10;
    public int StartDamagePoints = 10;
    public float StartMovementBaseSpeed = 4.0f;
    public float StartAttackSpeed = 1f;
    [Range(0,1)]
    public float StartCriticalDamageChance = 0.1f;

    [Header("Текущие характеристики:")]
    public int HealthPoints;
    public int DamagePoints;
    public float MovementBaseSpeed;
    public float AttackSpeed;
    public float CriticalDamageChance;

    private int maxHp;

    private void Awake()
    {
        instance = this;           
    }
    private void Start()
    {
        CountStats();
        //регенерация здоровья до максимального
        Invoke(nameof(RegenerationHP), 1f);
    }
    public void CountStats(int HP, int DP,float MBS, float AS, float CDC)
    {
        maxHp = StartHealthPoints + HP;
        DamagePoints = StartDamagePoints + DP;
        MovementBaseSpeed = StartMovementBaseSpeed + MBS;
        AttackSpeed = StartAttackSpeed - AS;
        CriticalDamageChance = StartCriticalDamageChance + CDC;

        CheckHealPoints();        
    }
    private void CountStats()
    {
        CountStats(0, 0, 0, 0, 0);
    }

    public void TakeDamage(int damage)
    {
        HealthPoints -= damage;        
    }

    public void CheckHealPoints()
    {
        if (HealthPoints <= 0)
        {
            //print("You lose");
        }
    }
    public int GetDamagePoints()
    {
        int DP = DamagePoints;
        if (Random.value <= CriticalDamageChance)
            DP *= 2;
           
        return DP;
    }
    public void Heal(int healPoint)
    {
        //ограничение на хил
        int heal = HealthPoints + healPoint;
        if (heal > maxHp)
            HealthPoints = maxHp;
        else
            HealthPoints = heal;
    }
    private void RegenerationHP()
    {
        int heal = HealthPoints + 1;
        if (heal > maxHp)
            HealthPoints = maxHp;
        else
            HealthPoints = heal;
    }
}
