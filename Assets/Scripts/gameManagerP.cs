using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager Singleton;

    public static GameManager Get()
    {
        return Singleton;
    }


    private int NumberOfPiecesInGame = 0;
    private int NumberOfPlacedPieces = 0;
    private Sprite PuzzleImage; 
    private GameMatchUI MatchUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Singleton = this;
    }

    public Sprite GetImage()
    {
        return PuzzleImage;
    }

    public void SetPiecesInGame(int piecesNum)
    {
        NumberOfPiecesInGame = piecesNum;
    }

    public void AddPiecePlaced()
    {
        NumberOfPlacedPieces++;
    }

    public void StartAGame(int PiecesNumber, Sprite _image)
    {
        PuzzleImage = _image;
        NumberOfPiecesInGame = PiecesNumber;
        NumberOfPlacedPieces = 0;
        SceneManager.LoadSceneAsync("PuzzleStage-36Pieces");
    }

    public void SetGameMatchUI(GameMatchUI _ui)
    {
        MatchUI = _ui;
    }

    public void TryEndMatch()
    {
        if(NumberOfPlacedPieces == NumberOfPiecesInGame)
        {
            MatchUI.Congratulations();
            StartCoroutine(DelayAction());            
        }
    }

    IEnumerator DelayAction()
    {
        yield return new WaitForSeconds(5f);
        EndMatch();
    }

    public void EndMatch()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    
}
