using UnityEngine;
using UnityEngine.SceneManagement;
public class RemenuAndExit : MonoBehaviour
{
    public string sceneName;
    public string ExitSceneName;
    public void reGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void reMenu()
    {
        SceneManager.LoadScene(ExitSceneName);
    }
}
