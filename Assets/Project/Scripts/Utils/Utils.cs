using UnityEngine;
using UnityEngine.SceneManagement;

public class Utils
{
    public static void RestartScene() =>
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public static void QuitGame()
    {
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE) 
    Application.Quit();
#endif
    }

    /// <summary>
    /// Finds the GameObject with the given name in the scene and returns it.
    /// Returns null if no match is found.
    /// </summary>
    /// <param name="objectName">Name of the GameObject to search for</param>
    /// <returns>The found GameObject or null</returns>
    public static GameObject FindGameObjectByName(string objectName)
    {
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allGameObjects)
        {
            if (obj.name.Equals(objectName))
            {
                return obj;
            }
        }
        
        return null;
    }

}

