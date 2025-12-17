using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectSimple : MonoBehaviour
{
    public int characterId;
    public string nextSceneName = "GameSelectionScene";

    public void SelectCharacter()
    {
        PlayerPrefs.SetInt("SelectedCharacter", characterId);
        PlayerPrefs.Save();

        SceneManager.LoadScene(nextSceneName);
    }
}