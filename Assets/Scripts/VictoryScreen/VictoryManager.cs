using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour
{
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip victory;
    [SerializeField] private AudioClip lastVictory;
    [SerializeField] private Button btnNextLvl;
    [SerializeField] private TextMeshProUGUI txtVictoria;

    public void Start()
    {
        if (GameManager.getLevel() > 6)
        {

            btnNextLvl.enabled = false;

            btnNextLvl.image.color = Color.gray; 

            txtVictoria.text = "Has ganado, oficialmente!";

            audioSource.PlayOneShot(lastVictory);
            
        } 
        else
        {
            
            audioSource.PlayOneShot(victory);

        }
    }


    public void onNextLevel()
    {

        SceneManager.LoadScene(GameManager.getLevel());

    }

    public void onMainMenu()
    {

        SceneManager.LoadScene(0);
    
    }
}
