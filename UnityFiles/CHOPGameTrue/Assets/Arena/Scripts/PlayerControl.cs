using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Player Handling
    [SerializeField] private LayerMask PlatformLayerMask;

    private Rigidbody2D rb;
    public bool isGrounded;
    private BoxCollider2D boxCollider2D;
    bool facingRight = true;

    //Character Stats
    public float speed = 8;
    public float jumpHeight = 12f;
    public int MaxHealth = 100;
    private int currentHealth;
    public int attackDamage = 40;
    public float attackRate = 2f;
    float nextAttacktime = 0f;

    public bool music = true;
    public HealthBar healthBar;

    private Animator animator;

    public float AttackRange = 60f;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public bool Dead = false;
    public static int score = 0;
    public GameOver over;
    public int DeathCount;

    // Start is called before the first frame update
    private void Start()
    {
        score = 0;
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody2D>();
        currentHealth = MaxHealth;
        healthBar.SetMaxHealth(MaxHealth);
    }

    private void Awake()
    {
        FindObjectOfType<AudioManager>().Play("BattleTheme");
        FindObjectOfType<AudioManager>().Stop("MenuTheme");
        music = true;
        rb = transform.GetComponent<Rigidbody2D>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    private void Update()
    {
        if (currentHealth <= 0) //Checks if player is dead.
        {
            currentHealth = 0;
            Dead = true;
            DeathCount += 1;
            music = false;
            FindObjectOfType<AudioManager>().Stop("BattleTheme");
        }
        if (DeathCount == 1) //Opens the pause menu when dead. Makes it so that the pause menu is only called once, cause when simply placed in the void update it pauses the game like a million times.
        {
            over.Pause();
        }
        Move();

        Jump();

        if(Time.time >= nextAttacktime) //Attack rate, stops attack spamming.
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
                nextAttacktime = Time.time + 1f / attackRate;
            }
        }
    }

    private void Attack()
    {
        if (IsGrounded())
        {
            //Play ground attack animation if grounded.
            animator.SetTrigger("Attack"); 
            
        }

        else
        {
            //Play aerial attack animation if not grounded. New attack animations can be added in the future. This is added for sake of maintainability ;)
            animator.SetTrigger("AerialAttack");
        }
        
    }
    void DealDamage() 
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, AttackRange, enemyLayers); //Find enemies inside the attack range.

        foreach (Collider2D enemy in hitEnemies) //Deals damage to enemies caught inside the range.
        {
            Debug.Log("Enemy hit.");
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, AttackRange);
    }

    private void Move()
    {
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        //Run animation
        if (IsGrounded() && rb.velocity.magnitude > 0) animator.SetBool("Run", true);
        else animator.SetBool("Run", false);

        // Animation flip.

        if(move<0 && facingRight)
        {
            flip();
        }

        else if (move > 0 && !facingRight)
        {
            flip();
        }
    }

    void flip()
    {
        facingRight = !facingRight;
            transform.Rotate(0f, 180f, 0f);
    }

    private void Jump()
    {
        // Jumping :)
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            float jumpVelocity = jumpHeight;
            rb.velocity = Vector2.up * jumpVelocity;
            animator.SetTrigger("Jump");
        }

    }
     
    private bool IsGrounded() //Grounded Check to stop jump spamming.
    {
        float extraHeightText = 1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, extraHeightText, PlatformLayerMask);

        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        } else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(boxCollider2D.bounds.center + new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, 0), Vector2.down * (boxCollider2D.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2D.bounds.center - new Vector3(boxCollider2D.bounds.extents.x, boxCollider2D.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider2D.bounds.extents.x * 2f), rayColor);

        return raycastHit.collider != null;

    }

    public void TakeDamage(int damage) //Function called by other scripts to recieve damage to player.
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

    }
    void CleaveAttack()
    {
        FindObjectOfType<AudioManager>().Play("cleavehit");
    }
    void CleaveJump()
    {
        FindObjectOfType<AudioManager>().Play("cleavejump");
    }
    public void CleaveHurt()
    {
        FindObjectOfType<AudioManager>().Play("cleavehurt");
    }
    void SliceAttack()
    {
        FindObjectOfType<AudioManager>().Play("slicehit");
    }
    void SliceJump()
    {
        FindObjectOfType<AudioManager>().Play("slicejump");
    }
    public void SliceHurt()
    {
        FindObjectOfType<AudioManager>().Play("slicehurt");
    }
}
