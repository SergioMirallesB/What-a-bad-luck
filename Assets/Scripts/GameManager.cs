using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    static private int level = 3;

    static public void setLevel (int level)
    {
        GameManager.level = level;
    }

    static public int getLevel ()
    {
        return GameManager.level;
    }
    


}
