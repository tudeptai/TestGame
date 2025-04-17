using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    [SerializeField] public string playerName;

    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float dashDuration = 0.5f;
    public float dashSpeedMultiplier = 2f;
    public float climbSpeed = 3f;
    public Text coinText;
    public Slider healthSlider;
    public GameObject damageTextPrefab;
    public int coinCount = 0;
    private int health = 100;
    private bool isGrounded;
    private bool isClimbing = false;
    private bool nearLadder = false;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private Animator animator;
    private bool isAttacking = false;
    private bool isDashing = false;
    private bool isKnockback = false;

    public GameObject gameOverPanel;
    private bool gameOver = false;
    private Vector3 spawnPoint;

    public Animator animator1;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 0.5f;
    public int attackDamege = 40;

    public int maxHealth = 100;
    private int currentHealth;

    private SoundManager soundManager;

    void Start()
    {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing from this game object");
        }
        if (animator == null)
        {
            Debug.LogError("Animator component is missing from this game object");
        }
        if (coinText == null)
        {
            Debug.LogError("Coin Text UI component is missing");
        }
        if (healthSlider == null)
        {
            Debug.LogError("Health Slider UI component is missing");
        }
        if (damageTextPrefab == null)
        {
            Debug.LogError("Damage Text Prefab is missing");
        }
        
        UpdateCoinText();
        healthSlider.value = health;
        spawnPoint = transform.position;
       // coinCount = 0;  
    }

    void Update()
    {
        if (!gameOver && !isClimbing && !isKnockback)
        {
            if (!isAttacking && !isDashing)
            {
                float move = Input.GetAxis("Horizontal") * moveSpeed;
                rb.velocity = new Vector2(move, rb.velocity.y);

                if (move > 0 && !facingRight)
                {
                    Flip();
                }
                else if (move < 0 && facingRight)
                {
                    Flip();
                }

                animator.SetFloat("Speed", Mathf.Abs(move));
                //
                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
                {
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    animator.SetTrigger("Jump");
                }
            }
            //
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && Mathf.Abs(rb.velocity.x) > 0 && isGrounded)
            {
                StartCoroutine(DashCoroutine());
            }

            if (Input.GetKeyDown(KeyCode.Q) && !isAttacking)
            {
                StartCoroutine(Attack());
            }

            if (Input.GetKeyDown(KeyCode.E) && nearLadder)
            {
                isClimbing = true;
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
            }
        }

        if (isClimbing)
        {

            float climb = Input.GetAxis("Vertical") * climbSpeed;
            rb.velocity = new Vector2(0, climb);
            //
            if (!nearLadder || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                isClimbing = false;
                rb.gravityScale = 1;
            }
        }

        if (isGrounded && !isClimbing)
        {
            //animation
            animator.SetBool("isGrounded", true);
            if (rb.velocity.x == 0 && !isAttacking && !isDashing)
            {
                animator.SetBool("Idle", true);
            }
            else
            {
                animator.SetBool("Idle", false);
            }
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                animator.SetBool("Idle", true);
            }
        }

        if (collision.gameObject.CompareTag("Trap"))
        {
            StartCoroutine(Knockback(collision));
            TakeDamage(10);
            soundManager.PlaySFX(soundManager.attack);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            AddCoin(1);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Coin2"))
        {
            AddCoin(5);
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Coin3"))
        {
            AddCoin(15);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Ladder"))
        {
            nearLadder = true;
        }
        if (other.gameObject.CompareTag("RedZone"))
        {
            Respawn();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            nearLadder = false;
            isClimbing = false;
            rb.gravityScale = 1;
        }
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
       // SavePlayerData();
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + coinCount.ToString();
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isAttacking = false;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            GoblinController goblinController = enemy.GetComponent<GoblinController>();
            if (goblinController != null)
            {
                goblinController.TakeDamage(attackDamege);
            }

            Mushroom mushroom = enemy.GetComponent<Mushroom>();
            if (mushroom != null)
            {
                mushroom.TakeDamage(attackDamege);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        float originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;

        float dashSpeed = facingRight ? moveSpeed * dashSpeedMultiplier : -moveSpeed * dashSpeedMultiplier;
        rb.velocity = new Vector2(dashSpeed, 0);

        animator.SetBool("IsDashing", true);

        yield return new WaitForSeconds(dashDuration);

        animator.SetBool("IsDashing", false);
        rb.gravityScale = originalGravityScale;
        isDashing = false;
    }

    private IEnumerator Knockback(Collision2D collision)
    {
        isKnockback = true;
        animator.SetTrigger("KnockbackTrigger");

        float knockbackDirection = collision.transform.position.x > transform.position.x ? -1 : 1;
        rb.velocity = new Vector2(knockbackDirection * moveSpeed, rb.velocity.y);

        yield return new WaitForSeconds(0.5f);

        isKnockback = false;
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        healthSlider.value = health;
        currentHealth -= damage;

        GameObject damageText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
        damageText.GetComponent<Text>().text = "-" + damage;
        Destroy(damageText, 1f);

        if (health <= 0)
        {
            animator.SetTrigger("Dead");
            soundManager.PlaySFX(soundManager.over);

            StartCoroutine(ShowGameOverPanel());
        }
    }

    public static bool GameIsPaused = false;

    private void Awake()
    {
        soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
    }

    private IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(0f);

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    private void Respawn()
    {
        transform.position = spawnPoint;
        currentHealth = maxHealth;
        health = maxHealth;
        healthSlider.value = health;
        gameOverPanel.SetActive(false);
        gameOver = false;
        Time.timeScale = 1f;
    }

    public void SavePlayerData()
    {
        PlayerPrefs.SetInt("CoinCount", coinCount);
       // PlayerPrefs.Save();
    }

    public void LoadPlayerData()
    {
        //if (PlayerPrefs.HasKey("CoinCount"))
        //{
         
        //}
        coinCount = PlayerPrefs.GetInt("CoinCount");
    }
}
