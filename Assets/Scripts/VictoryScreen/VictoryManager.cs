using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour
{
    // TODO: Añadir el sistema para pasar de nivel hasta el último//
    //------------------------------------------------------------//

    [SerializeField] private Button btnNextLvl;

    public void Start()
    {
        btnNextLvl.enabled = false;
        btnNextLvl.image.color = Color.gray5;
    }


    public void onNextLevel()
    {

        SceneManager.LoadScene("");

    }

    public void onMainMenu()
    {

        SceneManager.LoadScene(0);
    
    }
}
