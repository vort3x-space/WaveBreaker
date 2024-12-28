using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;         // Karakterin hızı
    public VariableJoystick joystick; // Joystick referansı
    private Rigidbody rb;
    private Animator animator;

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

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // Hareketi kontrol et
        if (direction.magnitude >= 0.1f)
        {
            // Rigidbody'nin hızını doğrudan ayarla (kaymayı önlemek için)
            Vector3 targetVelocity = direction * speed;
            rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);

            // Karakterin yönünü joystick yönüne döndür
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.deltaTime);

            // Animasyonu tetikle
            animator.SetFloat("Speed", direction.magnitude);
        }
        else
        {
            // Karakter durduğunda hızı sıfırla
            rb.velocity = Vector3.zero;
            animator.SetFloat("Speed", 0f);
        }
    }
}
