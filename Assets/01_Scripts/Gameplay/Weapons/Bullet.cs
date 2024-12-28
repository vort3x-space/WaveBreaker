using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;       // Merminin hızı
    public float damage = 20f;     // Merminin vereceği hasar
    public float lifeTime = 2f;    // Merminin yaşam süresi
    private float lifeTimer;

    private void OnEnable()
    {
        lifeTimer = lifeTime; // Yaşam süresini sıfırla
    }

    private void Update()
    {
        // Mermiyi ileri doğru hareket ettir (transform.forward yönüne göre)
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Yaşam süresi dolduysa havuza geri döndür
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Eğer düşmana çarptıysa hasar ver
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>()?.TakeDamage(damage);
            gameObject.SetActive(false); // Çarptıktan sonra mermiyi devre dışı bırak
        }
    }
}
