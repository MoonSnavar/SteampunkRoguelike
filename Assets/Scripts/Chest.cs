using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public bool isOpen = false;
    public Sprite OpenSprite;
    [Range(0, 1)]
    public float ChanceOfDefaultDrop = 0.6f;
    public List<Item> DropList;

    public Room currentRoom;

    public void OpenChest()
    {
        if (transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == OpenSprite)
            return;
        else
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = OpenSprite;

        int randomIndex = Random.Range(0, DropList.Count);

        if (Random.value > ChanceOfDefaultDrop)
            DropList[randomIndex].Rarity = GameManager.instance.LevelNumber + 1;
        else
            DropList[randomIndex].Rarity = GameManager.instance.LevelNumber;

        Instantiate(Resources.Load<GameObject>(DropList[randomIndex].PrefabPath), new Vector3(transform.position.x, transform.position.y - 0.5f), transform.rotation);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (currentRoom.isSecretRoom || isOpen)
                OpenChest();
        }
    }
}
