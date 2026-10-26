
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public void UpdateHearts(int health)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            int heartHealth = health - i;

            hearts[i].sprite = heartHealth >= 1 ? fullHeart : emptyHeart;

        }
    }
}
