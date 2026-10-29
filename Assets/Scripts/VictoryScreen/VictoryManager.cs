using System;
using UnityEditor.Build.Profile;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour
{

    [SerializeField] private Button btnNextLvl;

    public void Start()
    {
        try
        {

            print(SceneManager.GetSceneAt(0));
        }
        catch (Exception)
        {

            btnNextLvl.enabled = false;

            btnNextLvl.image.color = Color.gray5;

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
