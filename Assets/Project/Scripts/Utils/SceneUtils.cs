using UnityEngine.SceneManagement;

public class SceneUtils
{
    public static string GetScene() => SceneManager.GetActiveScene().name;

    #region Custom Load Scene functions
    public static void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    private static void SceneLoader(string sceneName) => SceneManager.LoadScene(sceneName);
    private static void AsyncSceneLoader(string sceneName) => SceneManager.LoadSceneAsync(sceneName);
    #endregion
}
