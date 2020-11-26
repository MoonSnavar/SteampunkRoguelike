using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject ExplosionEffect;
    void Start()
    {
        //должен произойти взрыв, который может всех задеть
        Invoke(nameof(Explosion), 3f);
    }

    private void Explosion()
    {
        print("BOOOOOOOOOOOOOOOOOOOOOM");
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach (Collider2D hitObject in hitObjects)
        {
            //урон бомбы зависит от текущего уровня
            if (hitObject.GetComponent<PlayerProperties>() != null)
                hitObject.GetComponent<PlayerProperties>().TakeDamage(4 * GameManager.instance.LevelNumber);
            else if (hitObject.GetComponent<EnemyProperties>() != null)
                hitObject.GetComponent<EnemyProperties>().TakeDamage(4 * GameManager.instance.LevelNumber);
        }
        Destroy(Instantiate(ExplosionEffect, transform.position, transform.rotation), 2f);
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}
