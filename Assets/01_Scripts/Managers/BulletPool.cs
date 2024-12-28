using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;       // Mermi prefab'i
    public int poolSize = 20;            // Havuz boyutu
    private Queue<GameObject> bulletPool;

    private void Awake()
    {
        bulletPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false); // Başlangıçta tüm mermiler devre dışı
            bulletPool.Enqueue(bullet);
        }
    }

    public GameObject GetBullet()
    {
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            bullet.SetActive(true); // Havuzdan alınan mermiyi etkinleştir
            return bullet;
        }
        else
        {
            // Eğer havuzdaki tüm mermiler kullanılıyorsa yeni bir tane oluştur
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(true);
            return bullet;
        }
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet); // Mermiyi havuza geri döndür
    }
}
