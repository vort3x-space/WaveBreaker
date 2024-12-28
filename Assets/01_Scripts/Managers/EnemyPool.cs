using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public GameObject enemyPrefab;    // Düşman prefab'i
    public int poolSize = 10;         // Havuzdaki maksimum düşman sayısı
    private Queue<GameObject> enemyPool;

    private void Awake()
    {
        enemyPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false); // Başlangıçta tüm düşmanlar devre dışı
            enemyPool.Enqueue(enemy);
        }
    }

    public GameObject GetEnemy()
    {
        if (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool.Dequeue();
            enemy.SetActive(true);
            return enemy;
        }
        else
        {
            // Eğer havuzdaki tüm düşmanlar kullanılıyorsa yeni bir tane oluştur
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(true);
            return enemy;
        }
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
    }
}
