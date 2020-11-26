using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public GameObject InventoryObject;
    public Transform Graphics;

    private PlayerProperties playerProperties;
    private float currentSpeed;
    private Vector2 movementDirection;
    private float movementSpeed;

    private Room currentRoom;
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private PlayerCombat playerCombat;
    private float nextPickupTime = 0f;
    private float attackSpeed;

    void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();        
        playerCombat = GetComponent<PlayerCombat>();
        playerProperties = GetComponent<PlayerProperties>();
        currentSpeed = playerProperties.StartMovementBaseSpeed;
        attackSpeed = 0;
        currentRoom = RoomPlacer.instance.StartingRoom;
    }

    void Update()
    {
        ProcessInputs();            
        Move();
        CheckFlip();      
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryObject.SetActive(!InventoryObject.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Inventory.instance.DropSelectedItem(transform);
        }

        InputAttackDirection();
        if (Input.GetKey(KeyCode.Space))
            FindAttackDirection();

        if (attackSpeed > 0)
            attackSpeed -= Time.deltaTime;
    }

    private void FindAttackDirection()
    {        
        if (attackSpeed <= 0)
        {
            attackSpeed = playerProperties.AttackSpeed;            

            if (movementDirection.x == 0 && movementDirection.y == 0)
            {
                UseItem(0, 0, true);
                return;
            }

            if (Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y))
            {
                if (movementDirection.x > 0)
                    UseItem(0.6f, 0, false);
                else
                    UseItem(-0.6f, 0, false);
            }
            else
            {
                if (movementDirection.y > 0)
                    UseItem(0, 0.6f, false);
                else
                    UseItem(0, -0.6f, false);
            }           
        }
    }

    private void UseItem(float x, float y, bool saveLastDirection)
    {
        if (Inventory.instance.IsItemInHotBar())
        {
            string selecteditemType = Inventory.instance.GetTypeOfSelectedItem();
            if (selecteditemType == "MeleeWeapon")
                playerCombat.MeleeAttack(x, y, saveLastDirection);
            else if (selecteditemType == "ShootWapon")
                playerCombat.Shoot(x, y, saveLastDirection);
            else if (selecteditemType == "Potion")
            {
                playerProperties.Heal(Inventory.instance.GetSelectedItem().HP);
                Inventory.instance.DestroyCurrentItem(Inventory.instance.SelectedItemIndex);
            }
            else if (selecteditemType == "Bomb")
                playerCombat.CastBomb();
        }
    }

    private void InputAttackDirection()
    {
        if (Input.GetKey(KeyCode.W))
            playerCombat.ChangeAttackDirection(0, 0.6f);
        else if (Input.GetKey(KeyCode.S))
            playerCombat.ChangeAttackDirection(0, -0.6f);
        else if (Input.GetKey(KeyCode.A))
            playerCombat.ChangeAttackDirection(-0.6f, 0);
        else if (Input.GetKey(KeyCode.D))
            playerCombat.ChangeAttackDirection(0.6f, 0);
    }


    public void ProcessInputs()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
        movementDirection.Normalize();        
    }

    void Move()
    {
        rb.velocity = movementDirection * movementSpeed * currentSpeed;
    }

    void CheckFlip()
    {
        //если нажали клавишу для перемещения вправо, а персонаж направлен влево
        if (movementDirection.x > 0 && !isFacingRight)
            //отражаем персонажа вправо
            Flip();
        //обратная ситуация. отражаем персонажа влево
        else if (movementDirection.x < 0 && isFacingRight)
            Flip();
    }

    private void Flip()
    {
        //меняем направление движения персонажа
        isFacingRight = !isFacingRight;
        //получаем размеры персонажа
        Vector3 theScale = Graphics.localScale;
        //зеркально отражаем персонажа по оси Х
        theScale.x *= -1;
        //задаем новый размер персонажа, равный старому, но зеркально отраженный
        Graphics.localScale = theScale;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Items") && Input.GetKeyDown(KeyCode.F))
        {
            //кулдаун подбора
            if (Time.time > nextPickupTime)
            {
                Inventory.instance.AddNewItem(collision.gameObject.GetComponent<Item>());
                nextPickupTime = Time.time + 0.2f;
            }                   
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Room"))
        {
            StartCoroutine(MoveCamera(new Vector3(collision.transform.position.x, collision.transform.position.y, -10), collision.GetComponent<Room>()));
        }
        else if (collision.CompareTag("Exit"))
        {
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator MoveCamera(Vector3 moveTo, Room room)
    {
        while(GameManager.instance.Camera.position != moveTo)
        {
            GameManager.instance.Camera.position = Vector3.MoveTowards(GameManager.instance.Camera.position, moveTo, Time.deltaTime * 12);
            yield return null;
            if (GameManager.instance.Camera.position == moveTo)
            {
                currentRoom.SetActiveEnemies(false);
                currentRoom = room;
                currentRoom.SetActiveEnemies(true);                
            }
        }
    }

    public Transform GetPosition()
    {
        return transform;
    }
}
