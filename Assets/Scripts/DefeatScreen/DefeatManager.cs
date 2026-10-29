using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatManager : MonoBehaviour
{
    public void onTryAgain()
    {

        SceneManager.LoadScene(3);

    }

    public void onMainMenu()
    {

        SceneManager.LoadScene(0);

    }
}