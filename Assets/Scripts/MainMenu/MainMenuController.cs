using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip menuMusic;

    private void Start()
    {
        
        audioSource.volume = 0.3f;
        
        audioSource.PlayOneShot(menuMusic);

        audioSource.loop = true;

        GameManager.setLevel(3);

        if (PlayerController.card)
        {
            GameObject card = Instantiate(prefab);

            card.transform.position = new Vector3(385.2f, 89.9f, 50f);

            card.transform.localScale = Vector3.one * 30;
        }
    }

    // In case we press the "Play" button.
    public void onBtnPlay()
    {
        // We just want to load the game scene and change it.
        SceneManager.LoadScene(GameManager.getLevel());
    }
    
    // In case we press the "Exit" button.
    public void onBtnExit()
    {
        // Exit the game.
        Application.Quit();
    }

}
