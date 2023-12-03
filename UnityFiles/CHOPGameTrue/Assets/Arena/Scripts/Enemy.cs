using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    public GameObject Player;
    private Rigidbody2D rb;

    public float MoveSpeed = 5f;
    public int attackRange = 15;
    public int attackChase = 600;
    public int turnRange = 100;
    public float AttackRange = 37.49f;
    public Transform attackFloppa;
    public Transform FrontRange;
    public Transform BackRange;
    public LayerMask PlayerLayer;
    public int attackDamage = 10;
    //Health
    public int maxHealth = 100;
    private int currentHealth;
    private bool Dead = false;
    bool facingRight = true;
    bool isAttacking = false;

    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = maxHealth;

        if(Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player"); //Looks for player
        }

        rb = this.GetComponent <Rigidbody2D>();

        if(currentHealth <= 0) //plays kill animation when the health is less than or equal to 0.
        {
            Die();
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        flipping(); //Constantly checks what side the player is on.

        AttackDamage();

        if (Dead == false) //Stops movement if dead.
        {
            Move();
        }
        if (currentHealth <= 0) //Checks if dead.
        {
            currentHealth = 0;
            Die();
            Dead = true;
        }
        if (isAttacking == true) //Stops movement when attacking.
        {
            MoveSpeed = 0;
        }


        Attack();
    }

    void Move()
    {
        if (Vector2.Distance(this.gameObject.transform.position, Player.transform.position) < (attackChase))
        {
            if (Player.transform.position.x > this.transform.position.x) //Moves enemy based on player location (to its left or right).
            {
                rb.velocity = new Vector2(MoveSpeed, rb.velocity.y); 
            }

            else
            {
                rb.velocity = new Vector2(-MoveSpeed, rb.velocity.y);
            }
        }
    }

    void Attack() //Attack function. Calls the animation, and in the animation it calls the function for dealing damage.
    {
        if(Vector2.Distance(this.gameObject.transform.position, Player.transform.position) < (attackRange))
        {
            isAttacking = true;
            animator.SetTrigger("FloppaAttack");
        }
    }
    void isCurrentAttacking() //called by animation to stop the enemy from attacking outside of the animation or attack time.
    {
        isAttacking = false;
    }

    void flipping() //Controls the flip function. same as the player flip function, but instead controlled using the same principles as the attack range function. Made this myself. Go me.
    {
        if (Dead == false || isAttacking == false) // Stops the flip when attacking or if dead. Sometimes while attacking it suddenly flips and catches you which isnt fair, so i added this. It also flipped when dead, and obviously thats not possible.
        {
            Collider2D[] seenPlayers = Physics2D.OverlapCircleAll(FrontRange.position, turnRange, PlayerLayer);

            foreach (Collider2D player in seenPlayers)
            {
                if (isAttacking == false)
                {
                    flip();
                }
            }

            Collider2D[] seenPlayers2 = Physics2D.OverlapCircleAll(BackRange.position, turnRange, PlayerLayer);

            foreach (Collider2D player in seenPlayers2)
            {
                if (isAttacking == false)
                {
                    flip();
                }
            }
        }

    }

    void DealDamage() //Deals damage when in range of the player. Plays the player damage sound based on the character. Uses same principles as the player attack function. Figured I could use the attack on the player on the enemy too.
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackFloppa.position, AttackRange, PlayerLayer);

        foreach (Collider2D player in hitPlayers)
        {
            if (MainMenu.CharacterTwo == 2)
            {
                player.GetComponent<PlayerControl>().CleaveHurt();
            }
            else
            {
                player.GetComponent<PlayerControl>().SliceHurt();
            }
            player.GetComponent<PlayerControl>().TakeDamage(attackDamage);
        }

    }

    void AttackDamage() //Stops damage when dead. Big issue when the enemy is already dead but continued to deal damage.
    {
        if (Dead == true)
        {
            attackDamage = 0;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackFloppa == null)
            return;

        Gizmos.DrawWireSphere(attackFloppa.position, AttackRange);

        if (FrontRange == null)
            return;

        Gizmos.DrawWireSphere(FrontRange.position, turnRange);

        if (BackRange == null)
            return;

        Gizmos.DrawWireSphere(BackRange.position, turnRange);
    }

    public void TakeDamage(int damage) //Plays daamge sound effect. Called enemyDeath, however not used cause it suited the damaged animation better. Too lazy to change the name. Sorry!
    {
        FindObjectOfType<AudioManager>().Play("EnemyDeath");
        currentHealth -= damage;
    }

    void flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void Die() // Death function. Freezes the enemy and adds score to the player by accessing its public variable.
    {
        Debug.Log("Enemy Died!");
        PlayerControl.score += 100;
        //Death Animation
        Dead = true;

        animator.SetBool("IsDead", true);
        this.GetComponent<Collider2D>().enabled = false;
        this.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        this.enabled = false;
    }

    public void Death() //Destroys the object. Called by the animation, which allows the death animation to play first before destroying the object.
    {
        Destroy(gameObject);
    }

    public void Attacking()
    {
        isAttacking = true;
    }

    public void isntAttacking()
    {
        isAttacking = false;
    }

}
