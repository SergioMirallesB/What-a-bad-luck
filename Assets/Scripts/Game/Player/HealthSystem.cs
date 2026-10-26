
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 5;
    public int currentHealth = 5;

    [Header("UI")]
    public HeartUI heartUI;

    public void TakeDamage(int damage)
    {
        this.currentHealth = Mathf.Max(0, this.currentHealth - damage);

        if (this.currentHealth == 0)
        {
            // sistema de muerte
            this.die();
        }

        heartUI.UpdateHearts(currentHealth);
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        heartUI.UpdateHearts(currentHealth);
    }

    private void die()
    {
        SceneManager.LoadScene("Defeat");
    }

}
