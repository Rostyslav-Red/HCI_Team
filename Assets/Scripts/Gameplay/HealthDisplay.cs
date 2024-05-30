using System.Collections.Generic;
using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public PlayerController player;
    private Health playerHealth;
    public float usedHeartAlpha = 0.3f;  // Alpha value for used hearts

    private List<Image> heartImages = new List<Image>();

    void Start()
    {
        playerHealth = player.GetComponent<Health>();

        // Initialize the heart images
        foreach (Transform child in transform)
        {
            Image heartImage = child.GetComponent<Image>();
            if (heartImage != null)
            {
                heartImages.Add(heartImage);
            }
        }

        UpdateHearts();
    }

    void Update()
    {
        UpdateHearts();
    }

    void UpdateHearts()
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < playerHealth.currentHP)
            {
                SetHeartAlpha(heartImages[i], 1f);
            }
            else
            {
                SetHeartAlpha(heartImages[i], usedHeartAlpha);
            }
        }
    }

    void SetHeartAlpha(Image heartImage, float alpha)
    {
        Color color = heartImage.color;
        color.a = alpha;
        heartImage.color = color;
    }
}
