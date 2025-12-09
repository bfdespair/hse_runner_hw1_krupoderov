using UnityEngine;
using TMPro;

public class UIHealth : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private TextMeshProUGUI hpText;

    private void Start()
    {
        if (player == null)
            player = FindObjectOfType<PlayerController>();

        if (hpText == null)
            hpText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (player == null || hpText == null) return;

        hpText.text = "HP: " + player.CurrentHealth.ToString();
    }
}
