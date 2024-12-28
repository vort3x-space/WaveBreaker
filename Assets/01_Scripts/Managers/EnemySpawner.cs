using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyPool enemyPool;          // EnemyPool referansı
    public Transform[] spawnPoints;     // Spawn pozisyonlarının dizisi
    public float baseSpawnInterval = 2f; // İlk level'da spawn aralığı
    public int baseEnemiesPerWave = 5;  // İlk level'da spawn edilecek düşman sayısı
    public int currentLevel = 1;        // Şu anki level

    private float spawnInterval;
    private int enemiesPerWave;
    private float spawnTimer;

    private readonly List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        LoadLevelData(); // Level'e göre spawn ayarlarını yükle
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnWave();
            spawnTimer = 0f; // Timer'ı sıfırla
        }
    }

    private void SpawnWave()
    {
        // Bir dalga için belirlenen sayıda düşmanı spawn et
        for (int i = 0; i < enemiesPerWave; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Pool'dan bir düşman al ve pozisyonunu ayarla
            GameObject enemy = enemyPool.GetEnemy();
            enemy.transform.position = spawnPoint.position;
            enemy.transform.rotation = Quaternion.identity;

            // Düşmana hedef olarak player'ı ayarla
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.player = GameObject.FindWithTag("Player").transform;
        }
    }

    public void ResetEnemies()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject enemy = enemyPool.GetEnemy();
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true);
            activeEnemies.Add(enemy);
        }
    }

    public void ClearEnemies()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(false); // Havuz sistemine geri gönder
            }
        }
        activeEnemies.Clear();
    }

    private void LoadLevelData()
    {
        // Kaydedilmiş level bilgisini al
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);

        // Zorluk arttırma mekanizması
        spawnInterval = baseSpawnInterval / currentLevel; // Level ilerledikçe spawn aralığı kısalır
        enemiesPerWave = baseEnemiesPerWave * currentLevel; // Level ilerledikçe düşman sayısı artar
    }
}
