using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public LevelManager levelManager;
    public float maxHealth = 100f; // Oyuncunun maksimum can değeri
    private float currentHealth; // Oyuncunun mevcut can değeri (gizli, yalnızca bu script içinde erişilebilir)
    public Animator animator; // Oyuncunun animasyonlarını kontrol etmek için Animator referansı
    private bool isDead = false; // Oyuncunun ölüp ölmediğini kontrol etmek için bir bayrak

    private void Start()
    {
        currentHealth = maxHealth;
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; // Eğer oyuncu zaten öldüyse daha fazla işlem yapma

        currentHealth -= damage; // Hasar al ve mevcut canı düşür

        if (currentHealth <= 0)
        {
            Die(); // Eğer can 0 veya altına düştüyse oyuncuyu öldür
        }
    }

    private void Die()
    {
        if (isDead) return;  // Eğer oyuncu zaten ölü ise işlemleri tekrar yapma

        isDead = true; // Ölüm bayrağını kaldır

        animator.SetTrigger("Dead");  // Ölüm animasyonunu tetikle

        levelManager.PlayerDied(); // LevelManager'a oyuncunun öldüğünü bildir

        Debug.Log("Player has died.");
    }
}
