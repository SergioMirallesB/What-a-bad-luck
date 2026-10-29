using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private void Start()
    {
        GameManager.setLevel(3);
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
