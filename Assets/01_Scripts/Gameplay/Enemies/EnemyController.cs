using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private LevelManager levelManager;

    public Transform player;          // Oyuncunun Transform referansı
    public float health = 100f;       // Düşmanın canı
    public float attackDistance = 2f; // Saldırı mesafesi
    public float detectionRadius = 10f; // Oyuncuyu fark etme mesafesi
    public float attackCooldown = 1.5f; // Saldırılar arasındaki süre
    public float attackDamage = 10f;     // Oyuncuya vereceği hasar
    private float lastAttackTime;     // Son saldırı zamanı

    private NavMeshAgent agent;       // NavMeshAgent bileşeni
    private Animator animator;        // Animator bileşeni
    private EnemyPool enemyPool;

    public bool isDead = false;      // Düşmanın ölüp ölmediğini kontrol eder
    private bool isAttacking = false; // Saldırıda olup olmadığını kontrol eder
    public GameObject deathParticlePrefab; // Partikül prefab'i referansı

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyPool = FindObjectOfType<EnemyPool>();
    }
    private void ReturnToPool()
    {
        isDead = false;
        health = 100f;
        enemyPool.ReturnEnemy(gameObject);
    }

    private void Update()
    {
        if (isDead) return; // Düşman ölü ise hiçbir şey yapma

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > detectionRadius)
        {
            // Oyuncu algılanmadı, Idle animasyonu çalıştır
            agent.isStopped = true;
            animator.SetBool("Run", false);
            animator.SetBool("Attack", false);
            isAttacking = false;
        }
        else if (distanceToPlayer > attackDistance)
        {
            // Oyuncuya doğru koş
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetBool("Run", true);
            animator.SetBool("Attack", false);
            isAttacking = false;
        }
        else
        {
            // Oyuncuya saldır
            if (!isAttacking)
            {
                agent.isStopped = true;
                animator.SetBool("Run", false);
                animator.SetBool("Attack", true);
                isAttacking = true;
                lastAttackTime = Time.time; // Saldırı zamanını kaydet
            }

            // Saldırı aralığını kontrol et
            if (Time.time - lastAttackTime > attackCooldown)
            {
                // Oyuncuya hasar ver
                player.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);
                lastAttackTime = Time.time;
            }
        }
    }


    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        agent.isStopped = true;
        animator.SetTrigger("Dead");
        levelManager.EnemyKilled(); // Kill count'u arttır
        SpawnDeathParticle();

        // Ölüm animasyonu bittikten sonra düşmanı kaldır
        Invoke(nameof(ReturnToPool), 2f);
    }

    private void SpawnDeathParticle()
    {
        // Ölüm partikül efektini çağır
        if (deathParticlePrefab != null)
        {
            Transform spawnPoint = transform.Find("DeathParticleSpawnPoint");
            if (spawnPoint != null)
            {
                Instantiate(deathParticlePrefab, spawnPoint.position, spawnPoint.rotation);
            }
        }

    }
}
