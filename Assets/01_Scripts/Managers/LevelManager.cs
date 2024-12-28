using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    public WorldManager worldManager;       // Ortam yönetimi
    public EnemySpawner enemySpawner;      // Düşman yönetimi
    public Transform player;               // Oyuncunun transform referansı
    public Transform playerStartPoint;     // Oyuncunun başlangıç pozisyonu

    private int currentLevel;              // Şu anki level
    public float[] levelDurations;        // Her level için ayrı süre
    private float timer;
    private int enemiesKilled = 0;
    private int totalEnemiesKilled = 0;
    private float totalTime = 0;

    public GameObject levelEndPanel;       // Level tamamlandığında gösterilecek panel
    public GameObject losePanel;           // Oyuncu öldüğünde gösterilecek panel
    public GameObject winPanel;            // Tüm level'lar tamamlandığında gösterilecek panel
    public Text timerText;                 // Zaman UI
    public Text levelEndEnemiesKilledText; // LevelEndPanel'deki düşman sayısı
    public Text levelText;                 // Level tamamlandığında gösterilen text
    public Text loseEnemiesKilledText;     // Lose ekranındaki düşman sayısı
    public Text loseRemainingTimeText;     // Lose ekranındaki kalan süre
    public Text winTotalEnemiesKilledText; // Win ekranındaki toplam düşman sayısı
    public Text winTotalTimeText;          // Win ekranındaki toplam süre

    private bool levelEnded = false;

    private void Start()
    {
        LoadProgress(); // Oyun durumunu yükle
        InitializeLevel(); // Sahneyi yüklenen duruma göre başlat
    }

    private void InitializeLevel()
    {
        timer = levelDurations[currentLevel - 1]; // Şu anki level'in süresini ayarla
        UpdateTimerUI();
        ResetPlayerAndEnemies();
        worldManager.LoadWorld(currentLevel - 1); // Kaydedilen world'ü yükle
    }

    private void Update()
    {
        if (levelEnded) return;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            EndLevel();
        }
    }

    private void UpdateTimerUI()
    {
        timerText.text = Mathf.Ceil(timer) + "s";
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
        totalEnemiesKilled++;
    }

    public void PlayerDied()
    {
        if (levelEnded) return;

        levelEnded = true;
        losePanel.SetActive(true);
        loseEnemiesKilledText.text = "" + enemiesKilled;
        loseRemainingTimeText.text = Mathf.Ceil(timer) + "s";

        RectTransform panelTransform = losePanel.GetComponent<RectTransform>();
        panelTransform.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutBounce);
    }

    private void EndLevel()
    {
        levelEnded = true;
        totalTime += (levelDurations[currentLevel - 1] - timer); // Bu level'in geçen süresini toplam süreye ekle

        levelEndPanel.SetActive(true);
        levelEndEnemiesKilledText.text = "" + enemiesKilled;
        levelText.text = "" + currentLevel;

        RectTransform panelTransform = levelEndPanel.GetComponent<RectTransform>();
        panelTransform.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutBounce);

        SaveProgress(); // Oyun durumunu kaydet
    }

    public void NextLevel()
    {
        RectTransform panelTransform = levelEndPanel.GetComponent<RectTransform>();
        panelTransform.DOAnchorPos(new Vector2(0, 1000), 1f).SetEase(Ease.InBack).OnComplete(() =>
        {
            levelEndPanel.SetActive(false);

            if (currentLevel >= worldManager.worldPrefabs.Length)
            {
                ShowWinScreen();
                return;
            }

            currentLevel++;
            timer = levelDurations[currentLevel - 1];
            enemiesKilled = 0;
            levelEnded = false;
            Time.timeScale = 1f;

            enemySpawner.ClearEnemies(); // Önceki level'daki düşmanları temizle

            UpdateTimerUI();
            ResetPlayerAndEnemies();
            worldManager.LoadWorld(currentLevel - 1);
            SaveProgress(); // Yeni level'e geçildiğinde kaydet
        });
    }

    private void ResetPlayerAndEnemies()
    {
        player.position = playerStartPoint.position;
        player.rotation = playerStartPoint.rotation;

        enemySpawner.ResetEnemies();
    }

    private void ShowWinScreen()
    {
        winPanel.SetActive(true);
        winTotalEnemiesKilledText.text = "" + totalEnemiesKilled;
        winTotalTimeText.text = Mathf.Ceil(totalTime) + "s";

        RectTransform panelTransform = winPanel.GetComponent<RectTransform>();
        panelTransform.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.OutBounce);
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        PlayerPrefs.SetInt("TotalEnemiesKilled", totalEnemiesKilled);
        PlayerPrefs.SetFloat("TotalTime", totalTime);
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        totalEnemiesKilled = PlayerPrefs.GetInt("TotalEnemiesKilled", 0);
        totalTime = PlayerPrefs.GetFloat("TotalTime", 0);
    }
}
