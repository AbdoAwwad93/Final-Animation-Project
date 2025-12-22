using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIScript : MonoBehaviour
{
    public int PicesCount = 36;

    public List<Sprite> Images;


    public void StartMatch(int imageId)
    {
        GameManager.Get().StartAGame(PicesCount, Images[imageId]);
    }
}
