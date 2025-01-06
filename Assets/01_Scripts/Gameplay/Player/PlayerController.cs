using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;                 // Karakterin hızı
    public VariableJoystick joystick;       // Joystick referansı
    private Rigidbody rb;
    private Animator animator;

    public float aimRadius = 10f;           // Düşman algılama yarıçapı
    public LayerMask enemyLayer;            // Düşmanların yer aldığı katman
    private Transform currentTarget;        // Hedef alınan düşman
    public float rotationSpeed = 5f;        // Dönüş hızı

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Joystick girdilerini al
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        // Eğer joystick hareketi varsa hareket et
        if (moveDirection.magnitude >= 0.1f)
        {
            // Hareket vektörünü hesapla
            Vector3 targetVelocity = moveDirection * speed;
            rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);

            // En yakın düşmanı bul
            FindClosestEnemy();

            if (currentTarget != null)
            {
                // Hedefe doğru dön
                RotateTowardsTarget(currentTarget.position);
            }
            else
            {
                // Eğer hedef yoksa joystick yönüne dön
                RotateTowardsJoystickDirection(moveDirection);
            }

            // Animasyonu tetikle
            animator.SetFloat("Speed", moveDirection.magnitude);
        }
        else
        {
            // Karakter durduğunda animasyonu sıfırla
            rb.velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
        }
    }

    private void FindClosestEnemy()
    {
        // Algılamak için küre çiz ve içindeki düşmanları bul
        Collider[] enemies = Physics.OverlapSphere(transform.position, aimRadius, enemyLayer);

        float closestDistance = Mathf.Infinity;
        currentTarget = null;

        foreach (var enemyCollider in enemies)
        {
            var enemy = enemyCollider.GetComponent<EnemyController>();

            // Eğer düşman ölü değilse hedef olarak seç
            if (enemy != null && !enemy.isDead)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    currentTarget = enemy.transform; // En yakın düşmanı hedef olarak seç
                }
            }
        }
    }

    private void RotateTowardsTarget(Vector3 targetPosition)
    {
        // Hedefe doğru yönlendirme
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        directionToTarget.y = 0; // Y eksenindeki eğimi sıfırla

        // Yumuşak dönüş
        Quaternion toRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime * 100f);
    }

    private void RotateTowardsJoystickDirection(Vector3 joystickDirection)
    {
        // Joystick yönüne doğru dön
        Quaternion toRotation = Quaternion.LookRotation(joystickDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime * 100f);
    }

    private void OnDrawGizmosSelected()
    {
        // Algılama küresini görselleştirmek için
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aimRadius);
    }
}
