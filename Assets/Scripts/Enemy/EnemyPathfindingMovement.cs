using UnityEngine;
using Pathfinding;

public class EnemyPathfindingMovement : MonoBehaviour
{
    public Transform Target;
    public Transform Graphics;
    public float NextWaypointDistance = 3f;

    public Vector2 Force;
    public bool CanMove;
    private float speed;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Seeker seeker;
    private Rigidbody2D rb;
    private float nextFollow;

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
            nextFollow = Time.time + 0.5f;
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
    private void FixedUpdate()
    {
        if (path == null || !CanMove)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
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
        if (Force.x >= 0.01f)
        {
            Graphics.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (Force.x <= -0.01f)
        {
            Graphics.localScale = new Vector3(1f, 1f, 1f);
        }        
    }
}
