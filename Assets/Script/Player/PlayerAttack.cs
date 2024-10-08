using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private AudioClip fireballSound;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    public bool isMoving;

    private Animator anim;
    private PlayerController playerMovement;
    private float cooldownTimer = Mathf.Infinity;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        //bool isGrounded = IsGrounded();
        //anim.SetBool("isGrounded", isGrounded);

        //if (isGrounded)
        //{
        //    if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && Time.timeScale > 0)
        //    {
        //        if (isMoving)
        //        {
        //            MoveAttack();
        //        }
        //        else
        //        {
        //            Attack();
        //        }
        //    }
        //}
        //else
        //{
        //    if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && Time.timeScale > 0 && /*Input.GetKey(KeyCode.Space)*/!IsGrounded())
        //    {
        //        JumpAttack();
        //    }
        //}

        //cooldownTimer += Time.deltaTime;
        //AttackCheck();
    }

    public void AttackCheck()
    {
        bool isGrounded = IsGrounded();
        anim.SetBool("isGrounded", isGrounded);

        if (isGrounded)
        {
            if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && Time.timeScale > 0)
            {
                if (isMoving)
                {
                    MoveAttack();
                }
                else
                {
                    Attack();
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && Time.timeScale > 0 && /*Input.GetKey(KeyCode.Space)*/!IsGrounded())
            {
                JumpAttack();
            }
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        AudioManager.instance.PlaySound(fireballSound);
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.01f, groundLayer);

    }

    private void MoveAttack()
    {
        anim.SetTrigger("moveAttack");
        
        AudioManager.instance.PlaySound(fireballSound);
        cooldownTimer = 0;

        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
    private void JumpAttack()
    {
        anim.SetTrigger("jumpAttack");
        
        AudioManager.instance.PlaySound(fireballSound);
        cooldownTimer = 0;

        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
}