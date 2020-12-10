using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public AudioSource ShootSound;
    public AudioSource MeleeAttackSound;
    public SpriteRenderer GunSprite;
    public SpriteRenderer WeaponSprite;
    public GameObject Bullet;
    public Transform ShootPoint;
    public Transform AttackPoint;
    public float AttackSize;
    public LayerMask EnemyLayer;
    private int damagePoints;
    private PlayerProperties playerProperties;
    private Animator animator;
    private PlayerController playerController;

    private void Awake()
    {
        playerProperties = GetComponent<PlayerProperties>();
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
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
        AnimationMelee();

        if (!MeleeAttackSound.isPlaying)
            MeleeAttackSound.Play();
    }

    private void AnimationMelee()
    {
        WeaponSprite.sprite = Inventory.instance.GetSpriteCurrentItem();
        if (AttackPoint.localPosition.y < 0)
        {
            if (playerController.Graphics.localScale.x < 0)
                playerController.Flip();
            animator.SetTrigger("AttackDown");
        }
        else if (AttackPoint.localPosition.y > 0)
        {
            if (playerController.Graphics.localScale.x < 0)
                playerController.Flip();
            animator.SetTrigger("AttackUp");
        }
        if (AttackPoint.localPosition.x > 0)
        {
            animator.SetTrigger("AttackRight");
        }
        else if (AttackPoint.localPosition.x < 0)
        {
            if (playerController.Graphics.localScale.x > 0)
                playerController.Flip();
            animator.SetTrigger("AttackLeft");
        }
    }

    private void AnimationShoot()
    {
        GunSprite.sprite = Inventory.instance.GetSpriteCurrentItem();
        if (AttackPoint.localPosition.y < 0)
        {
            if (playerController.Graphics.localScale.x < 0)
                playerController.Flip();
            animator.SetTrigger("ShootDown");
        }
        else if (AttackPoint.localPosition.y > 0)
        {
            if (playerController.Graphics.localScale.x < 0)
                playerController.Flip();
            animator.SetTrigger("ShootUp");
        }
        if (AttackPoint.localPosition.x > 0)
        {
            animator.SetTrigger("ShootRight");
        }
        else if (AttackPoint.localPosition.x < 0)
        {
            if (playerController.Graphics.localScale.x > 0)
                playerController.Flip();
            animator.SetTrigger("ShootLeft");
        }
    }

    public void Shoot(float x, float y, bool saveLastDirection)
    {
        if (!saveLastDirection)
            AttackPoint.localPosition = new Vector3(x, y);


        damagePoints = playerProperties.GetDamagePoints();
     
        var bulletObject = Instantiate(Bullet, ShootPoint.position, AttackPoint.rotation);

        //вращаю объект в сторону точки атаки
        bulletObject.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(AttackPoint.localPosition.y, AttackPoint.localPosition.x) * Mathf.Rad2Deg);
        bulletObject.GetComponent<Bullet>().TargetMaskString = "Enemy";
        bulletObject.GetComponent<Bullet>().DamagePoints = damagePoints;

        AnimationShoot();

        if (!ShootSound.isPlaying)
            ShootSound.Play();
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
