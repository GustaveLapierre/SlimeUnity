using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSpriteButton : MonoBehaviour
{
    // Reference to the button's image component
    private Image img;

    // References to the two sprites
    public Sprite sprite1;
    public Sprite sprite2;
    private string difficulty;

    private bool isSprite1Active = true;

    void Start()
    {
        // Getting the image component
        img = GetComponent<Image>();
        // Set initial sprite
        img.sprite = sprite1;
        PersistObject.Instance.difficulty = "easy";
    }

    public void ToggleSprite()
    {
        // Toggle the sprites
        if (isSprite1Active)
        {
            img.sprite = sprite2;
            PersistObject.Instance.difficulty = "hard";
        }
        else
        {
            img.sprite = sprite1;
            PersistObject.Instance.difficulty = "easy";
        }

        isSprite1Active = !isSprite1Active;
    }
}