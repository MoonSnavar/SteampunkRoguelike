using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour
{    
    public int HealthPoints = 5;
    public int DamagePoints = 1;
    public float MovementSpeed = 150f;
    public float AttackSpeed = 1f;
    public float AttackRange = 10f;
    public float TargetRange = 10f;
    [Range(0,1)]
    public float ChanceOfDefaultDrop = 0.8f;    
    public List<Item> DropList;
    private float chanceOfDrop = 0.4f;
    private Room currentRoom;

    public void TakeDamage(int damage)
    {        
        HealthPoints -= damage;

        if (HealthPoints <= 0)
        {
            if (DropList.Count > 0 && Random.value < chanceOfDrop)
            {
                int randomIndex = Random.Range(0, DropList.Count);

                if (Random.value > ChanceOfDefaultDrop)
                    DropList[randomIndex].Rarity = GameManager.instance.LevelNumber + 1;
                else
                    DropList[randomIndex].Rarity = GameManager.instance.LevelNumber;

                Instantiate(Resources.Load<GameObject>(DropList[randomIndex].PrefabPath), transform.position, transform.rotation);                
            }
            Destroy(gameObject);                    
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            currentRoom = collision.GetComponent<Room>();            
        }
    }
    private void OnDestroy()
    {        
        currentRoom?.OpenRoom();
    }    
}
