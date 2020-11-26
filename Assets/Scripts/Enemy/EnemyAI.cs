using UnityEngine;

public class EnemyAI : MonoBehaviour
{    
    private enum State
    {
        Roaming,
        ChaseTarget,
        AttackingTarget,
        GoingBackToStart
    }

    private State state;
    private EnemyPathfindingMovement enemyPathfindingMovement;
    private EnemyCombat enemyCombat;
    private Vector3 startingPosition;
    private Vector3 roamPosition;
    private float nextAttackTime = 0f;
    private float attackSpeed;
    private float attackRange;
    private float targetRange;
    private void Awake()
    {
        enemyPathfindingMovement = GetComponent<EnemyPathfindingMovement>();
        enemyCombat = GetComponent<EnemyCombat>();
        state = State.Roaming;
        attackSpeed = GetComponent<EnemyProperties>().AttackSpeed;
        attackRange = GetComponent<EnemyProperties>().AttackRange;
        targetRange = GetComponent<EnemyProperties>().TargetRange;
    }
    private void Start()
    {
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(roamPosition, 1f);
    }
    private void Update()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                enemyPathfindingMovement.StartFollow(roamPosition);

                float reachedPositionDistance = 1f;
                if (Vector3.Distance(transform.position, roamPosition) < reachedPositionDistance)
                {
                    roamPosition = GetRoamingPosition();                    
                }                
                
                FindTarget();
                break;
            case State.ChaseTarget:
                enemyPathfindingMovement.StartFollow(PlayerController.instance.GetPosition().position);
                
                if (Vector3.Distance(transform.position,PlayerController.instance.GetPosition().position) < attackRange)
                {
                    //прекращаем передвижение
                    enemyPathfindingMovement.CanMove = false;
                    if (Time.time > nextAttackTime)
                    {
                        //Атакуем цель                        
                        enemyCombat.Attack();
                        //state = State.AttackingTarget;
                        nextAttackTime = Time.time + attackSpeed;
                    }
                }
                float stopChaseDistance = 21f;
                if(Vector3.Distance(transform.position, PlayerController.instance.GetPosition().position) > stopChaseDistance)
                {
                    //слишком далеко, прекращаем преследование
                    state = State.GoingBackToStart;
                }
                break;
            case State.AttackingTarget:
                break;
            case State.GoingBackToStart:
                enemyPathfindingMovement.StartFollow(startingPosition);

                reachedPositionDistance = 10f;
                if (Vector3.Distance(transform.position, startingPosition) < reachedPositionDistance)
                {
                    //возвращаемся на стартовую точку
                    state = State.Roaming;
                }
                break;
        }       
    }
    private Vector3 GetRoamingPosition()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        return startingPosition + randomDirection * Random.Range(3f, 3f);
    }

    private void FindTarget()
    {        
        if (Vector3.Distance(transform.position, PlayerController.instance.GetPosition().position) < targetRange)
        {
            state = State.ChaseTarget;
        }
    }
}
