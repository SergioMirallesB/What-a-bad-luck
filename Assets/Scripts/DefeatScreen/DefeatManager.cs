using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatManager : MonoBehaviour
{
        
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip defeat;


    void Start()
    {
        audioSource.PlayOneShot(defeat);
    }

    public void onTryAgain()
    {

        GameManager.setLevel(3);
        SceneManager.LoadScene(GameManager.getLevel());

    }

    public void onMainMenu()
    {

        SceneManager.LoadScene(0);

    }
}