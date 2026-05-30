using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header("Referensi Player")]
    public PlayerController player;

    [Header("UI Nyawa")]
    public Image heartIcon;
    public TMP_Text heartText;

    void Start()
    {
        CariPlayerJikaKosong();
        UpdateHealthUI();
    }

    void Update()
    {
        if (player == null)
        {
            CariPlayerJikaKosong();
        }

        UpdateHealthUI();
    }

    private void CariPlayerJikaKosong()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerController>();
        }
    }

    private void UpdateHealthUI()
    {
        if (player == null) return;

        int darahSekarang = Mathf.Clamp(player.currentHealth, 0, player.maxHealth);

        if (heartText != null)
        {
            heartText.text = "x " + darahSekarang;
        }

        if (heartIcon != null)
        {
            heartIcon.enabled = true;
        }
    }
}