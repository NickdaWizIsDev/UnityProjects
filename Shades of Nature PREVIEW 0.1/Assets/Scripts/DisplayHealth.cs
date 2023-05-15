using UnityEngine;
using TMPro;

public class DisplayHealth : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TMP_Text healthDisplay;

    private void Start()
    {
        UpdateHealthDisplay();
    }

    private void Update()
    {
        UpdateHealthDisplay();
    }

    private void UpdateHealthDisplay()
    {
        healthDisplay.SetText("Health: " + playerHealth.GetCurrentHealth() + "/" + playerHealth.GetMaxHealth());
    }

}
