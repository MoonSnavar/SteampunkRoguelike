using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public GameObject Bullet;
    public Transform AttackPoint;
    public float AttackSize;
    public LayerMask EnemyLayer;
    private int damagePoints;
    private PlayerProperties playerProperties;

    private void Awake()
    {
        playerProperties = GetComponent<PlayerProperties>();
    }
    public void MeleeAttack(float x, float y, bool saveLastDirection)
    {
        if (!saveLastDirection)
            AttackPoint.localPosition = new Vector3(x,y);

        damagePoints = playerProperties.GetDamagePoints();

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackSize, EnemyLayer);
        foreach (Collider2D hitEnemy in hitEnemies)
        {            
            if (hitEnemy.isTrigger)
                hitEnemy.GetComponent<EnemyProperties>().TakeDamage(damagePoints);
        }        
    }

    int lastX, lastY;
    public void Shoot(float x, float y, bool saveLastDirection)
    {
        if (!saveLastDirection)
            AttackPoint.localPosition = new Vector3(x, y);


        damagePoints = playerProperties.GetDamagePoints();
     
        var bulletObject = Instantiate(Bullet, AttackPoint.position, AttackPoint.rotation);

        //вращаю объект в сторону точки атаки
        bulletObject.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(AttackPoint.localPosition.y, AttackPoint.localPosition.x) * Mathf.Rad2Deg);
        bulletObject.GetComponent<Bullet>().TargetMaskString = "Enemy";
        bulletObject.GetComponent<Bullet>().DamagePoints = damagePoints;
    }

    public void ChangeAttackDirection(float x, float y)
    {
        AttackPoint.localPosition = new Vector3(x, y);
    }

    public void CastBomb()
    {
        Item item = Inventory.instance.GetSelectedItem();
        //заспавнить префаб
        var bomb = Instantiate(Resources.Load<GameObject>(item.AdditionalObjectPath), AttackPoint.position, AttackPoint.rotation);
        //выкинуть вперед                        
        bomb.GetComponent<Rigidbody2D>().AddForce(AttackPoint.localPosition * 350);        
        //удалить предмет из инвентаря
        Inventory.instance.DestroyCurrentItem(Inventory.instance.SelectedItemIndex);        
    }    

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackPoint.position, AttackSize);
    }
}
