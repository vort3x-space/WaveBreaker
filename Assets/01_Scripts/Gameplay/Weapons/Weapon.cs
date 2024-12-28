using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;         // Merminin çıkış noktası
    public BulletPool bulletPool;       // BulletPool referansı
    public float fireRate = 0.2f;       // Mermilerin ateşlenme sıklığı (saniyede bir)
    private float fireTimer = 0f;       // Ateş etme zamanlayıcısı

    public VariableJoystick joystick;  // Joystick referansı

    public AudioSource gunAudioSource; // Silah sesi için AudioSource
    public AudioClip gunShotClip;      // Silah sesi dosyası

    private void Update()
    {
        // Joystick hareket kontrolü
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                Fire();
                fireTimer = 0f;
            }
        }
    }

    private void Fire()
    {
        // Havuzdan bir mermi al ve yönünü ayarla
        GameObject bullet = bulletPool.GetBullet();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;

        // Silah sesini çal
        if (gunAudioSource != null && gunShotClip != null)
        {
            gunAudioSource.PlayOneShot(gunShotClip);
        }
    }
}
