using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    public GameObject Item
    {
        get
        {
            if (transform.childCount > 0)
                return transform.GetChild(0).gameObject;

            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!Item)
        {            
            if (transform.parent.name == "Equipment")
            {
                //если экипировку помещаем в ячейку экипировки                
                if (transform.tag.ToString() == Inventory.instance.GetItemType())
                {
                    DragHandler.ItemBeingDragged.transform.SetParent(transform);
                    Inventory.instance.ChangeItems(DragHandler.currentIndexItem, GetCurrentIndex(), DragHandler.StartParent, transform);
                    SetSelectedItem();                    
                }
            } 
            else
            {
                DragHandler.ItemBeingDragged.transform.SetParent(transform);
                Inventory.instance.ChangeItems(DragHandler.currentIndexItem, GetCurrentIndex(), DragHandler.StartParent, transform);
                SetSelectedItem();               
            }
        }
    }

    private int GetCurrentIndex()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.name == transform.parent.GetChild(i).name)
            {                
                return i;
            }
        }

        return 0;
    }

    public void SetSelectedItem()
    {
        int selectedIndex = GetCurrentIndex();

        selectedIndex = Inventory.instance.CheckAndChangeIndex(transform.parent, selectedIndex);

        Inventory.instance.SelectedItemIndex = selectedIndex;

        Inventory.instance.ClearAllItemsToWhite();
        GetComponent<Image>().color = Inventory.instance.SelectedColor;

        Inventory.instance.CountingEquipmentBonus();
    }    
}
