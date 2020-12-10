using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerProperties : MonoBehaviour
{
    public static PlayerProperties instance;
    public GameObject LoseMenu;

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

    public Text HPText;
    public Text DPText;
    public Text MBSText;
    public Text ASText;
    public Text CDCText;

    private int maxHp;
    private AudioSource hitSound;

    private void Awake()
    {
        instance = this;
        hitSound = GetComponent<AudioSource>();
    }
    private void Start()
    {
        CountStats();
        //регенерация здоровья до максимального
        InvokeRepeating(nameof(RegenerationHP), 1f, 1f);
    }
    public void CountStats(int HP, int DP,float MBS, float AS, float CDC)
    {
        maxHp = StartHealthPoints + HP;
        DamagePoints = StartDamagePoints + DP;
        MovementBaseSpeed = StartMovementBaseSpeed + MBS;
        CriticalDamageChance = StartCriticalDamageChance + CDC;
        if (StartAttackSpeed - AS > 0)
            AttackSpeed = StartAttackSpeed - AS;
        else
            AttackSpeed = 0.2f;

        DisplayProperties();
        CheckHealPoints();
    }
    private void CountStats()
    {
        CountStats(0, 0, 0, 0, 0);
    }

    public void TakeDamage(int damage)
    {
        int takedDamage = HealthPoints - damage;
        if (takedDamage < 0)
            HealthPoints = 0;
        else
            HealthPoints = takedDamage;

        CheckHealPoints();
        DisplayProperties();

        if (!hitSound.isPlaying)
            hitSound.Play();
    }

    public void CheckHealPoints()
    {
        if (HealthPoints <= 0)
        {
            Time.timeScale = 0;
            Inventory.instance.SaveItems();
            LoseMenu.SetActive(true);
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

        DisplayProperties();
    }
    private void RegenerationHP()
    {
        int heal = HealthPoints + 1;
        if (heal > maxHp)
            HealthPoints = maxHp;
        else
            HealthPoints = heal;

        DisplayProperties();
    }

    public void DisplayProperties()
    {
        HPText.text = "ОЗ: " + HealthPoints + "/" + maxHp;
        DPText.text = "Урон: " + DamagePoints;
        MBSText.text = "Скорость передвижения: " + MovementBaseSpeed;
        ASText.text = "Скорость атаки: " + AttackSpeed;
        CDCText.text = "Шанс критического урона: " + CriticalDamageChance;
    }
}
