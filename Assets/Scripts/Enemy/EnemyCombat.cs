using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public enum EnemyType
    {
        Shooter,
        MeleeFigher,
        Melle
    }
    public EnemyType enemyType;
    public GameObject Bullet;
    public float AttackSize;
    public LayerMask PlayerLayer;
    [Header("Точки атак")]
    public Transform AttackPointL;
    public Transform AttackPointR;
    public Transform AttackPointT;
    public Transform AttackPointD;

    private EnemyPathfindingMovement pathfindingMovement;
    private int damagePoints;
    private Transform attackPoint;

    private void Awake()
    {
        pathfindingMovement = GetComponent<EnemyPathfindingMovement>();
        damagePoints = GetComponent<EnemyProperties>().DamagePoints;        
    }
    public void Attack()
    {
        switch (enemyType)
        {
            case EnemyType.Shooter:
                var bulletObject = Instantiate(Bullet, transform.position, transform.rotation);
                bulletObject.GetComponent<Bullet>().DamagePoints = GetComponent<EnemyProperties>().DamagePoints;
                bulletObject.GetComponent<Bullet>().TargetMaskString = "Player";
                break;
            case EnemyType.MeleeFigher:

                if (Mathf.Abs(pathfindingMovement.Force.x) > Mathf.Abs(pathfindingMovement.Force.y))
                {
                    if (pathfindingMovement.Force.x > 0)
                        attackPoint = AttackPointR;
                    else if (pathfindingMovement.Force.x < 0)
                        attackPoint = AttackPointL;
                }
                else
                {
                    if (pathfindingMovement.Force.y > 0)
                        attackPoint = AttackPointT;
                    else if (pathfindingMovement.Force.y < 0)
                        attackPoint = AttackPointD;
                }              

                Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, AttackSize, PlayerLayer);
                if (hitPlayers.Length > 0)
                    hitPlayers[0].GetComponent<PlayerProperties>().TakeDamage(damagePoints);                

                break;
            case EnemyType.Melle:
                PlayerProperties.instance.TakeDamage(damagePoints);
                break;
        }
    }
}
