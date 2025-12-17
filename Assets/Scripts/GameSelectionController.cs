using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSelectButton : MonoBehaviour
{
    public string gameSceneName;

    public void LoadGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
