using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossGFX : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] EnemyStatus enemyStatus; // enemyStatus reference
    [SerializeField] BossMelee bossMelee; // meleeEnemyAstar reference
    [SerializeField] AIDestinationSetter aiDest;
    [SerializeField] GameObject[] componentToDestroy;
    public GameObject barrier2;
    public GameObject barrier1;
    public AIPath aiPath;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 desiredVelocity = aiPath.desiredVelocity;
        bool isMoving = desiredVelocity.magnitude > 0.01f;

        if (desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        anim.SetBool("run", isMoving);

        PhaseChange();

        if (enemyStatus.currHealth < 1)
        {
            isMoving = false;
            anim.SetTrigger("dead");
            aiPath.maxSpeed = 0;
            anim.SetBool("isDead", true);
            aiDest.enabled = false;
            for (int i = 0; i < componentToDestroy.Length; i++)
            {
                Destroy(componentToDestroy[i]);
            }
        }
    }

    void DeactivateEnemy()
    {
        Destroy(gameObject);

    }

    void DeadChangePos()
    {
        transform.position = new Vector2(transform.localPosition.x, -0.81f);
    }

    void PhaseChange()
    {
        if (enemyStatus.currHealth == 10)
        {
            bossMelee.damage = 1;
            aiPath.maxSpeed = 3;
            bossMelee.canUseMeleeAttack2 = true;
        }
    }
}
