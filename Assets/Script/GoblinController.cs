﻿using UnityEngine;
using System.Collections;

public class GoblinController : MonoBehaviour
{
    public float speed = 2f; // Tốc độ di chuyển của Goblin
    public float leftLimit;  // Giới hạn di chuyển bên trái (so với vị trí ban đầu)
    public float rightLimit; // Giới hạn di chuyển bên phải (so với vị trí ban đầu)
    public float detectionRange = 5f; // Phạm vi phát hiện player
    public float chaseRange = 10f; // Phạm vi mà goblin bắt đầu đuổi theo player
    public float attackRange = 1.5f; // Phạm vi tấn công

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;//
    private float startX; // Vị trí x ban đầu
    private Transform player; // Tham chiếu tới player
    private bool isChasing = false; // Biến đánh dấu liệu Goblin có đuổi theo player hay không
    private bool isAttacking = false; // Biến đánh dấu liệu Goblin đang tấn công hay không
    private bool isTakingHit = false;

    public int maxHealth = 100;
    int currentHealth;

    public Animator animator;
    //public Transform swordTransform;

    private bool attackTriggered = false; // Biến đánh dấu xem tấn công đã được kích hoạt chưa

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startX = transform.position.x; // Lưu vị trí x ban đầu
        player = GameObject.FindGameObjectWithTag("Player").transform; // Tìm và lưu tham chiếu tới player

        currentHealth = maxHealth;
    }

    void Update()
    {
        // Kiểm tra xem Goblin đang trong trạng thái Take hit không
        if (isTakingHit)
        {
            // Nếu đang trong trạng thái Take hit, không thực hiện bất kỳ hành động nào khác
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        //Debug.Log("Distance to player: " + distanceToPlayer);

        // Kiểm tra xem player có nằm trong phạm vi đuổi theo không
        if (distanceToPlayer <= chaseRange)
        {
            // Nếu nằm trong phạm vi đuổi theo, tiếp tục kiểm tra khoảng cách để quyết định đuổi theo hay dừng lại
            if (distanceToPlayer <= detectionRange)
            {
                //Debug.Log("Chasing player");
                isChasing = true;
                ChasePlayer();

                // Kiểm tra khoảng cách để tấn công player
                if (distanceToPlayer <= attackRange)
                {
                    AttackPlayer();
                }
                else
                {
                    // Nếu khoảng cách vượt quá phạm vi tấn công, dừng tấn công
                    StopAttackingPlayer();
                }
            }
            else
            {
                //Debug.Log("Stopping chase");
                isChasing = false;
                StopChasingPlayer();
            }
        }
        else
        {
            // Nếu player nằm ngoài phạm vi đuổi theo, dừng đuổi theo và ở trạng thái idle
            //Debug.Log("Player out of range");
            isChasing = false;
            StopChasingPlayer();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Goblin took damage: " + damage + ", current health: " + currentHealth);

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Goblin died");

        animator.SetBool("isDead", true);
        rb.velocity = Vector2.zero; // Dừng mọi chuyển động
        rb.isKinematic = true; // Biến Rigidbody thành kinematic để ngăn không cho rơi xuống
        this.enabled = false; // Vô hiệu hóa script này để ngăn không cho thực hiện các hành động tiếp theo

        // Hủy đối tượng sau khi hoàn thành animation chết
        Destroy(gameObject);
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // Wait for the "Dead" animation to finish
        Debug.Log("Waiting for dead animation to finish");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log("Destroying goblin game object");
        Destroy(gameObject); // Destroy the game object
    }

    void ChasePlayer()
    {
        // Nếu đang tấn công, dừng di chuyển và không làm gì nữa
        if (isAttacking)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // Di chuyển Goblin theo hướng của player
        if (player.position.x > transform.position.x)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            spriteRenderer.flipX = false; // Lật mặt Goblin sang phải
        }
        else
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            spriteRenderer.flipX = true; // Lật mặt Goblin sang trái
        }

        // Kiểm tra nếu Goblin chạm tới giới hạn bên phải
        if (transform.position.x >= startX + rightLimit)
        {
            //movingRight = false;
        }
        // Kiểm tra nếu Goblin chạm tới giới hạn bên trái
        else if (transform.position.x <= startX - leftLimit)
        {
            // movingRight = true;
        }

        // Chạy animation
        anim.SetBool("isRunning", true);
    }

    void StopChasingPlayer()
    {
        // Nếu không đuổi theo player, dừng di chuyển và chạy animation idle
        if (!isChasing)
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("isRunning", false);
        }
    }

    void AttackPlayer()
    {
        // Dừng di chuyển khi tấn công
        rb.velocity = Vector2.zero;

        // Đặt biến tấn công
        isAttacking = true;

        // Chạy animation tấn công
        anim.SetTrigger("attack");

        // Đặt tốc độ của Goblin về 0
        rb.velocity = Vector2.zero;

        // Đánh dấu rằng tấn công đã được kích hoạt
        attackTriggered = true;
    }

    void StopAttackingPlayer()
    {
        // Nếu đang tấn công player, dừng tấn công và chạy animation idle
        if (isAttacking)
        {
            // Nếu animation tấn công kết thúc, đặt lại biến tấn công và chạy animation idle
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                isAttacking = false;
                anim.SetBool("isRunning", false);

                // Khôi phục tốc độ di chuyển sau khi kết thúc animation tấn công
                rb.velocity = Vector2.zero;

                // Reset biến attackTriggered
                attackTriggered = false;
            }
        }
    }

    void TakeHit()
    {
        // Dừng di chuyển khi bị đánh
        rb.velocity = Vector2.zero;

        // Đặt biến Take hit
        isTakingHit = true;

        // Chạy animation Take hit
        anim.SetTrigger("takeHit");
    }

    void StopTakingHit()
    {
        // Khi kết thúc animation Take hit, đặt lại biến Take hit và cho phép Goblin thực hiện các hành động khác
        isTakingHit = false;
    }

    // Thêm code xử lý khi Goblin bị đánh hoặc va chạm với Trap
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Thực hiện hành động khi Goblin bị đánh
            TakeHit();
        }
        else if (other.CompareTag("Trap") && attackTriggered)
        {
            // Thực hiện hành động khi Goblin va chạm với Trap trong lúc đang tấn công
            AttackPlayer();
        }
    }
}
