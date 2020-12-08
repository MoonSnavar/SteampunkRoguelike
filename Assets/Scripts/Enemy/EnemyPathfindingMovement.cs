using UnityEngine;
using Pathfinding;

public class EnemyPathfindingMovement : MonoBehaviour
{
    public Animator animator;
    public Transform Target;
    public Transform Graphics;
    public float NextWaypointDistance = 3f;

    public Vector2 Force;
    public bool CanMove;
    private int currentWaypoint = 0;   
    private Path path;
    private Seeker seeker;
    private Rigidbody2D rb;
    private float speed;
    private float nextFollow;
    private float rotationSpeed = 0.5f;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        CanMove = true;
        speed = GetComponent<EnemyProperties>().MovementSpeed;
    }

    public void StartFollow(Vector3 target)
    {
        CanMove = true;
        if (Time.time > nextFollow)
        {
            if (seeker.IsDone())
                seeker.StartPath(rb.position, target, OnPathComplete);           
            nextFollow = Time.time + 0.2f;
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    private void Update()
    {
        if (rotationSpeed > 0)
            rotationSpeed -= Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (path == null || !CanMove)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Force = direction * speed * Time.deltaTime;               
        rb.velocity = Force;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < NextWaypointDistance)
        {
            currentWaypoint++;
        }

        //Flip
        if (rotationSpeed <= 0)
        {
            rotationSpeed = 0.5f;
            if (Mathf.Abs(Force.x) > Mathf.Abs(Force.y))
            {
                if (Force.x > 0.5f)
                    Graphics.localScale = new Vector3(-1f, 1f, 1f);
                else
                    Graphics.localScale = new Vector3(1f, 1f, 1f);

                if (animator != null)
                    animator.SetInteger("State", 5);
            }
            else if (animator != null)
            {
                if (Force.y > 0)
                    animator.SetInteger("State", 4);
                else
                    animator.SetInteger("State", 3);
            }
            
        }
    }
}
