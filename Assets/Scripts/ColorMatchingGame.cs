using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ColorMatchingGame : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI colorNameText;
    public Button[] colorButtons; // 4 buttons
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI finalscoreText;
    public GameObject rewardsPanel;
    public Image[] stars;
    public Image characterImage;

    [Header("Character Sprites")]
    public Sprite[] idleSprites;
    public Sprite[] happySprites;
    public Sprite[] sadSprites;
    public Sprite[] celebrateSprites;

    [Header("Game Settings")]
    public int totalRounds = 10;
    public float roundTime = 5f;
    
    [Header("Colors")]
    private List<ColorData> availableColors = new List<ColorData>();
    
    private int currentScore = 0;
    private int currentRound = 0;
    private string correctColorName;
    private Color correctColor;
    
    [System.Serializable]
    public class ColorData
    {
        public string colorName;
        public Color color;
    }

    private int selectedCharacterIndex;
    void LoadCharacter()
    {
        selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);

        // Safety check
        if (idleSprites.Length > 0)
        {
            selectedCharacterIndex = Mathf.Clamp(selectedCharacterIndex, 0, idleSprites.Length - 1);
            characterImage.sprite = idleSprites[selectedCharacterIndex];
        }
    }
    void Start()
    {
        InitializeColors();
        StartGame();
    }
    
    void InitializeColors()
    {
        // Add basic colors
        availableColors.Add(new ColorData { colorName = "RED", color = Color.red });
        availableColors.Add(new ColorData { colorName = "BLUE", color = Color.blue });
        availableColors.Add(new ColorData { colorName = "GREEN", color = Color.green });
        availableColors.Add(new ColorData { colorName = "YELLOW", color = Color.yellow });
        availableColors.Add(new ColorData { colorName = "ORANGE", color = new Color(1f, 0.5f, 0f) });
        availableColors.Add(new ColorData { colorName = "PURPLE", color = new Color(0.5f, 0f, 0.5f) });
        availableColors.Add(new ColorData { colorName = "PINK", color = new Color(1f, 0.75f, 0.8f) });
        availableColors.Add(new ColorData { colorName = "BROWN", color = new Color(0.6f, 0.3f, 0f) });
    }
    
    public void StartGame()
    {
        currentScore = 0;
        currentRound = 0;
        UpdateScoreUI();
        LoadCharacter();
        SetIdle();
        
        if (rewardsPanel != null)
            rewardsPanel.SetActive(false);
            
        if (feedbackText != null)
            feedbackText.text = "";
            
        StartNewRound();
    }
    
    void StartNewRound()
    {
        SetIdle();
        currentRound++;
        roundText.text = "Round : " + currentRound + " / 10";
        if (currentRound > totalRounds)
        {
            EndGame();
            return;
        }
        
        // Select a random correct color
        int correctIndex = Random.Range(0, availableColors.Count);
        correctColorName = availableColors[correctIndex].colorName;
        correctColor = availableColors[correctIndex].color;
        
        // Display the color name
        colorNameText.text = correctColorName;
        colorNameText.color = availableColors[Random.Range(0, availableColors.Count)].color; // Normal difficulty

        // For harder difficulty, uncomment this:
        // colorNameText.color = availableColors[Random.Range(0, availableColors.Count)].color;

        // Setup buttons with random colors
        List<int> usedIndices = new List<int>();
        usedIndices.Add(correctIndex);
        
        // Randomly assign correct color to one button
        int correctButtonIndex = Random.Range(0, colorButtons.Length);
        
        for (int i = 0; i < colorButtons.Length; i++)
        {
            if (i == correctButtonIndex)
            {
                // This button gets the correct color
                SetButtonColor(colorButtons[i], correctColor, correctColorName);
            }
            else
            {
                // Get a random different color
                int randomIndex;
                do
                {
                    randomIndex = Random.Range(0, availableColors.Count);
                } while (usedIndices.Contains(randomIndex));
                
                usedIndices.Add(randomIndex);
                SetButtonColor(colorButtons[i], availableColors[randomIndex].color, availableColors[randomIndex].colorName);
            }
        }
        
        if (feedbackText != null)
            feedbackText.text = "";
    }
    
    void SetButtonColor(Button button, Color color, string colorName)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
            buttonImage.color = color;
        
        // Remove old listener and add new one
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnColorButtonClicked(colorName));
    }
    
    public void OnColorButtonClicked(string clickedColorName)
    {
        if (clickedColorName == correctColorName)
        {
            SetHappy();
            // Correct answer
            currentScore += 10;
            ShowFeedback("Correct! Great job!", Color.green);
            PlayCorrectSound();
            
            // Wait a bit then move to next round
            Invoke("StartNewRound", 1f);
        }
        else
        {
            // Wrong answer
            SetSad();
            ShowFeedback("Oops! Try again!", Color.red);
            PlayWrongSound();
            Invoke("StartNewRound", 1f);
        }
        
        UpdateScoreUI();
    }
    
    void ShowFeedback(string message, Color color)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.color = color;
        }
    }
    
    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + currentScore;
    }

    void EndGame()
    {
        colorNameText.text = "Game Complete!";
        finalscoreText.text = "Final Score:"+currentScore;
        // Disable all buttons
        foreach (Button btn in colorButtons)
        {
            btn.interactable = false;
        }

        // Show rewards
        if (rewardsPanel != null)
        {
            rewardsPanel.SetActive(true);
            ShowStarsBasedOnScore();
        }
        PlayVictorySound();
    }

    void ShowStarsBasedOnScore()
    {
        int starsEarned = 0;

        if (currentScore >= totalRounds * 8) { starsEarned = 3; Celebrate(); }
        else if (currentScore >= totalRounds * 5) { starsEarned = 2; SetHappy(); }
        else if (currentScore >= totalRounds * 3) { starsEarned = 1; SetSad(); }
        
        if (stars != null && stars.Length > 0)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                if (stars[i] != null)
                    stars[i].enabled = i < starsEarned;
            }
        }
    }

    void SetIdle()
    {
        if (idleSprites.Length > selectedCharacterIndex)
            characterImage.sprite = idleSprites[selectedCharacterIndex];
    }

    void SetHappy()
    {
        if (happySprites.Length > selectedCharacterIndex)
            characterImage.sprite = happySprites[selectedCharacterIndex];
    }

    void SetSad()
    {
        if (sadSprites.Length > selectedCharacterIndex)
            characterImage.sprite = sadSprites[selectedCharacterIndex];
    }

    void Celebrate()
    {
        if (celebrateSprites.Length > selectedCharacterIndex)
            characterImage.sprite = celebrateSprites[selectedCharacterIndex];

        
    }

    void PlayCorrectSound()
    {
        // Add your sound effect here
        // AudioManager.Instance.PlaySound("Correct");
    }
    
    void PlayWrongSound()
    {
        // Add your sound effect here
        // AudioManager.Instance.PlaySound("Wrong");
    }
    
    void PlayVictorySound()
    {
        // Add your sound effect here
        // AudioManager.Instance.PlaySound("Victory");
    }
    
    public void RestartGame()
    {
        // Enable buttons again
        foreach (Button btn in colorButtons)
        {
            btn.interactable = true;
        }
        
        StartGame();
    }
}
