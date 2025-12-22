using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMatchUI : MonoBehaviour
{
    public List<GameObject> ImagesObjects;

    public GameObject Congrats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject image in ImagesObjects)
        {
            image.GetComponent<SpriteRenderer>().sprite = GameManager.Get().GetImage();
        }

        GameManager.Get().SetGameMatchUI(this);
    }

    public void Congratulations()
    {
        Congrats.SetActive(true);
    }
}
