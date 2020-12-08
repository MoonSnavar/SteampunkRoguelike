using UnityEngine;

public class Item : MonoBehaviour
{
    public string NameItem;
    public int Id;
    public int Rarity;
    public Sprite Icon;
    public string PrefabPath;
    public string AdditionalObjectPath;

    public enum ItemTypes
    {
        EquipmentHelmet,
        EquipmentArmor,
        Equipment,
        MeleeWeapon,
        ShootWapon,
        Potion,
        Bomb
    };
    public ItemTypes itemType;

    [Header("Улучшает эти характеристики")]
    public int HP;
    public int DP;
    public float MBS;
    public float AS;
    [Range(0, 1)]
    public float CDC;

    private void Start()
    {        
        ParticleSystem.MainModule psMain = GetComponentInChildren<ParticleSystem>().main;
        psMain.startColor = Inventory.instance.GetColorByRarity(Rarity);
    }
}
