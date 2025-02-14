﻿using UnityEngine;

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
    private Item[] items;
    private float chanceOfDrop = 0.4f;
    private Room currentRoom;
    private AudioSource enemyHit;

    private void Awake()
    {
        items = GameManager.instance.items;
        enemyHit = GetComponent<AudioSource>();
        HealthPoints *= GameManager.instance.LevelNumber;
    }

    public void TakeDamage(int damage)
    {        
        if (!enemyHit.isPlaying)
            enemyHit.Play();

        HealthPoints -= damage;

        if (HealthPoints <= 0)
        {
            if (items.Length > 0 && Random.value < chanceOfDrop)
            {
                int randomIndex = Random.Range(0, items.Length);

                if (Random.value > ChanceOfDefaultDrop)
                    items[randomIndex].Rarity = GameManager.instance.LevelNumber + 1;
                else
                    items[randomIndex].Rarity = GameManager.instance.LevelNumber;

                Instantiate(Resources.Load<GameObject>(items[randomIndex].PrefabPath), transform.position, transform.rotation);                
            }
            Destroy(gameObject, 0.15f);                    
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
        if (currentRoom != null)
            currentRoom.OpenRoomAndChest();
    }    
}
