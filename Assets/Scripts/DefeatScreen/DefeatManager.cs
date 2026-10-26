using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatManager : MonoBehaviour
{
    public void onTryAgain()
    {

        SceneManager.LoadScene(1);

    }

    public void onMainMenu()
    {

        SceneManager.LoadScene(0);

    }
}