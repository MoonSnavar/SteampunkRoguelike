using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Speed = 200;
    public int DamagePoints;
    public string TargetMaskString;
    private Rigidbody2D rb;
    private Transform target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        Destroy(gameObject, 10);

        if (TargetMaskString == "Player")
        {
            target = PlayerController.instance.GetPosition();
            var direction = target.position - transform.position;
            transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            rb.AddForce(direction.normalized * Speed); //normalized тут чтобы скорость была постоянна и не уменьшалась, если цель близко к объекту
        }
        else
        {
            rb.AddForce(transform.right * Speed);            
        }
      
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == TargetMaskString && collision.isTrigger)
        {
            if (TargetMaskString == "Player")
                collision.GetComponent<PlayerProperties>().TakeDamage(DamagePoints);
            else if (TargetMaskString == "Enemy")
                collision.GetComponent<EnemyProperties>().TakeDamage(DamagePoints);

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Walls"))
            Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
