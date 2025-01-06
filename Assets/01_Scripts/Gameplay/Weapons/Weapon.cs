using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;          // Merminin çıkış noktası
    public BulletPool bulletPool;        // BulletPool referansı
    public float fireRate = 0.2f;        // Mermilerin ateşlenme sıklığı (saniyede bir)
    private float fireTimer = 0f;        // Ateş etme zamanlayıcısı

    public float aimRadius = 10f;        // Düşmanları algılamak için yarıçap
    public LayerMask enemyLayer;         // Düşmanların yer aldığı katman

    public AudioSource gunAudioSource;   // Silah sesi için AudioSource
    public AudioClip gunShotClip;        // Silah sesi dosyası

    private Transform currentTarget;     // Şu anda hedef alınan düşman

    private void Update()
    {
        // En yakın düşmanı bul
        FindClosestEnemy();

        // Eğer bir hedef varsa ve zamanlayıcı sıfırlandıysa ateş et
        if (currentTarget != null)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                Fire();
                fireTimer = 0f;
            }
        }
    }

    private void FindClosestEnemy()
    {
        // Algılamak için bir küre çiz ve içindeki düşmanları bul
        Collider[] enemies = Physics.OverlapSphere(transform.position, aimRadius, enemyLayer);

        float closestDistance = Mathf.Infinity; // Başlangıçta sonsuz mesafe
        currentTarget = null; // Başlangıçta hedef yok

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentTarget = enemy.transform; // En yakın düşmanı hedef olarak seç
            }
        }

        // Eğer bir hedef bulunduysa firePoint'i ona çevir
        if (currentTarget != null)
        {
            Vector3 direction = (currentTarget.position - firePoint.position).normalized;
            firePoint.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void Fire()
    {
        if (currentTarget == null) return;

        // Havuzdan bir mermi al ve yönünü ayarla
        GameObject bullet = bulletPool.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;

        // Silah sesini çalF
        if (gunAudioSource != null && gunShotClip != null)
        {
            gunAudioSource.PlayOneShot(gunShotClip);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Algılama küresini görselleştirmek için
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aimRadius);
    }
}
