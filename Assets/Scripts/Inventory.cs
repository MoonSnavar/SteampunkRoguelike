using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    [Header("Цвет ячейки, при её активации")]
    public Color SelectedColor;
    public GameObject IconPrefab;
    public GameObject CellContainer;
    public GameObject EquipmentContainer;
    public GameObject HotBarContainer;
    public Item EmptyItem;
    public int SelectedItemIndex;
    private List<Item> items;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {        
        items = new List<Item>();
        //заполняю список пустыми предметами
        for (int i = 0; i < CellContainer.transform.childCount; i++)
        {
            items.Add(EmptyItem);
        }

        for (int i = 0; i < EquipmentContainer.transform.childCount; i++)
        {
            items.Add(EmptyItem);
        }

        for (int i = 0; i < HotBarContainer.transform.childCount; i++)
        {
            items.Add(EmptyItem);
        }

        DisplayItems();
        CountingEquipmentBonus();
    }

    //отображаем все иконки предметов
    private void DisplayItems() 
    {
        int nextIndex = 0;
        for (int i = 0; i < items.Count; i++)
        {
            Transform cell;
            if (i < CellContainer.transform.childCount)
                cell = CellContainer.transform.GetChild(i);   //получаем доступ к слоту и иконке в нем
            else
            {                
                if (i - CellContainer.transform.childCount < EquipmentContainer.transform.childCount)
                {
                    nextIndex = i - CellContainer.transform.childCount;
                    cell = EquipmentContainer.transform.GetChild(nextIndex);
                }
                else
                {
                    cell = HotBarContainer.transform.GetChild(i - (CellContainer.transform.childCount + nextIndex + 1));
                }
            }
            
            if (items[i].Id != 0)
            {
                Transform icon;
                if (cell.childCount == 0)
                    icon = Instantiate(IconPrefab, cell).transform;
                else
                    icon = cell.GetChild(0).transform;


                Image imageRarity = icon.GetComponent<Image>();

                Transform itemIcon = icon.GetChild(0);
                Image imageIcon = itemIcon.GetComponent<Image>();

                imageIcon.sprite = items[i].Icon;
                imageRarity.color = GetColorByRarity(items[i].Rarity);

                icon.GetComponent<DragHandler>().canDrag = true;
            }
            else
            {
                if (cell.childCount > 0)
                    Destroy(cell.GetChild(0).gameObject);
            }
        }
    }

    private Color GetColorByRarity(int rarityId)
    {
        Color color;

        switch (rarityId)
        {
            default:
                color = new Color32(193, 188, 94, 255);
                break;
            case 1:
                color = new Color32(222,64,66, 255);
                break;
            case 2:
                color = new Color32(255, 154, 73, 255);
                break;
            case 3:
                color = new Color32(255, 255, 74, 255);
                break;
            case 4:
                color = new Color32(74, 255, 127, 255);
                break;
            case 5:
                color = new Color32(74, 255, 204, 255);
                break;
        }

        return color;
    }

    //добавление предмета в первую пустую ячейку
    public void AddNewItem(Item pickedItem)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == 0) //если ячейка пустая
            {
                items[i] = pickedItem.GetComponent<Item>();
                Destroy(pickedItem.gameObject);
                DisplayItems();
                break;
            }
        }
    }

    public void ChangeItems(int lastIndex, int nextIndex, Transform lastParent, Transform nextParent)
    {
        lastIndex = CheckAndChangeIndex(lastParent.parent, lastIndex);

        nextIndex = CheckAndChangeIndex(nextParent.parent, nextIndex);

        Item lastItem = items[lastIndex];
        items[lastIndex] = items[nextIndex];
        items[nextIndex] = lastItem;

    }

    public int CheckAndChangeIndex(Transform parent, int index)
    {
        if (parent != CellContainer.transform)
        {
            if (parent == EquipmentContainer.transform)
                index += 25;
            else
                index += 25 + 5;
        }
        return index;
    }

    public void ClearAllItemsToWhite()
    {
        for (int i = 0; i < CellContainer.transform.childCount; i++)
        {
            CellContainer.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        for (int i = 0; i < EquipmentContainer.transform.childCount; i++)
        {
            EquipmentContainer.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        for (int i = 0; i < HotBarContainer.transform.childCount; i++)
        {
            HotBarContainer.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }

    private void SelectItemByButton()
    {        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectedItemIndex = 30;
            ClearAllItemsToWhite();
            HotBarContainer.transform.GetChild(0).GetComponent<Image>().color = SelectedColor;
            CountingEquipmentBonus();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectedItemIndex = 31;
            ClearAllItemsToWhite();
            HotBarContainer.transform.GetChild(1).GetComponent<Image>().color = SelectedColor;
            CountingEquipmentBonus();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectedItemIndex = 32;
            ClearAllItemsToWhite();
            HotBarContainer.transform.GetChild(2).GetComponent<Image>().color = SelectedColor;
            CountingEquipmentBonus();
        }
    }

    private void Update()
    {
        SelectItemByButton();       
    }

    public void DropSelectedItem(Transform player)
    {
        if (items[SelectedItemIndex].Id != 0)
        {
            //заспавнить его префаб возле персонажа
            var dropItem = Instantiate(Resources.Load<GameObject>(items[SelectedItemIndex].PrefabPath), player.position, player.rotation);
            //установить ему редкость оригинала
            dropItem.GetComponent<Item>().Rarity = items[SelectedItemIndex].Rarity;
            //заменить на пустой предмет в списке
            items[SelectedItemIndex] = EmptyItem;
            //убрать его конку из инвентаря
            DisplayItems();
        }        
    }

    public void DestroyCurrentItem(int itemIndex)
    {
        //заменяем на пустой предмет в списке
        items[itemIndex] = EmptyItem;
        //убраем его иконку из инвентаря
        DisplayItems();
    }

    public string GetItemType()
    {
        int index = CheckAndChangeIndex(DragHandler.StartParent.parent, DragHandler.currentIndexItem);
        return items[index].itemType.ToString();
    }

    public void CountingEquipmentBonus()
    {
        int HealthPoints = 0;
        int DamagePoints = 0;
        float MovementBaseSpeed = 0f;
        float AttackSpeed = 0f;
        float CriticalDamageChance = 0f;

        int countItemsWithoutHotBar = CellContainer.transform.childCount + EquipmentContainer.transform.childCount;
        for (int i = CellContainer.transform.childCount; i < countItemsWithoutHotBar; i++)
        {
            HealthPoints += items[i].HP * items[i].Rarity;
            DamagePoints += items[i].DP * items[i].Rarity;
            MovementBaseSpeed += items[i].MBS * items[i].Rarity;
            AttackSpeed += items[i].AS * items[i].Rarity;
            CriticalDamageChance += items[i].CDC * items[i].Rarity;
        }

        //проверяем и считаем выбранный предмет
        string selecteditemType = GetTypeOfSelectedItem();
        if (selecteditemType == "MeleeWeapon" || selecteditemType == "ShootWapon")
        {
            HealthPoints += items[SelectedItemIndex].HP * items[SelectedItemIndex].Rarity;
            DamagePoints += items[SelectedItemIndex].DP * items[SelectedItemIndex].Rarity;
            MovementBaseSpeed += items[SelectedItemIndex].MBS * items[SelectedItemIndex].Rarity;
            AttackSpeed += items[SelectedItemIndex].AS * items[SelectedItemIndex].Rarity;
            CriticalDamageChance += items[SelectedItemIndex].CDC * items[SelectedItemIndex].Rarity;
        }

        PlayerProperties.instance.CountStats(HealthPoints, DamagePoints, MovementBaseSpeed, AttackSpeed, CriticalDamageChance);
    }

    public string GetTypeOfSelectedItem()
    {
        return items[SelectedItemIndex].itemType.ToString();
    }
    public bool IsItemInHotBar()
    {
        if (SelectedItemIndex >= 30 && SelectedItemIndex <= 32)
            return true;
        else
            return false;
    }
    public Item GetSelectedItem()
    {
        return items[SelectedItemIndex];
    }
}
