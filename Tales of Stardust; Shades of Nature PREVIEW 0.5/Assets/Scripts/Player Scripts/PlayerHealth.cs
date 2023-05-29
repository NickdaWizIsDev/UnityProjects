using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public TMP_Text healthDisplay;
    public Damageable hp;
    private void Awake()
    {
        healthDisplay = GetComponent<TMP_Text>();
        hp = GetComponent<Damageable>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthDisplay();
    }

    private void FixedUpdate()
    {
        currentHealth = hp.Health;
    }

    private void Update()
    {
        if (!hp.IsAlive)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    void UpdateHealthDisplay()
    {
        healthDisplay.SetText("Health: " + currentHealth + "/" + maxHealth);
    }
}

