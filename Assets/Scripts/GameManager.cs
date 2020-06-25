using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    GameOver,
    InGame,
    OnMenu
}

public class GameManager : MonoBehaviour
{
    public GameState state { get; private set; }
    public static GameManager instance;
    
    [SerializeField] private Text inGameScoreText;
    [SerializeField] private Text gameOverScreenScoreText;

    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject storeScreen;

    public Action OnRestart;
    public Action OnStateChange;

    public int score { get; private set; } = 0;
    public int totalScore { get; private set; } = 0;
    bool isInShop = false;


    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;        
    }

    void Start()
    {
        state = GameState.OnMenu;
        OnStateChange?.Invoke();
        mainMenu.SetActive(true);
    }


    void Update()
    {
        if (state == GameState.InGame)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                OpenShop();
            }

        }
        else if (state == GameState.OnMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !isInShop || Input.GetKeyDown(KeyCode.Tab) && isInShop)
            {
                ResumeGame();                
            }

        }
    }

    public void GameOver()
    {
        state = GameState.GameOver;
        OnStateChange?.Invoke();
        gameOverScreenScoreText.text = "Score: \n  " + totalScore;
        ChangeScore(-score, true);
        totalScore = 0;
        inGameUI.SetActive(false);
        gameOverScreen.SetActive(true);
    }

    public void PauseGame()
    {
        //Time.timeScale = 0;
        inGameUI.SetActive(false);
        pauseScreen.SetActive(true);
        state = GameState.OnMenu;
        OnStateChange?.Invoke();
    }

    public void ResumeGame()
    {
        //Time.timeScale = 1;
        pauseScreen.SetActive(false);
        inGameUI.SetActive(true);
        state = GameState.InGame;
        OnStateChange?.Invoke();
        storeScreen.SetActive(false);
        if (isInShop)
        {
            isInShop = false;
        }
    }

    public void PlayGame()
    {        
        if (!TutorialManager.instance.dontShowTutorial && !TutorialManager.instance.isInTuto)
        {
            TutorialManager.instance.ShowTutorial();            
        }
        else
        {
            TutorialManager.instance.HideTutorial();
            inGameUI.SetActive(true);
            state = GameState.InGame;
            OnStateChange?.Invoke();
            totalScore = 0;
            mainMenu.SetActive(false);
        }

    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OpenShop()
    {
        foreach (var item in Store.instance.items)
        {
            Button upgradeButton = item.whenOwnedUI.GetComponentsInChildren<Button>()[1];
            Button buyButton = item.whenNotOwnedUI.GetComponentInChildren<Button>();            
            if (score < item.projectilePrefab.GetComponent<Projectile>().price)
            {
                upgradeButton.interactable = false;
                buyButton.interactable = false;
            }
            else
            {
                upgradeButton.interactable = true;
                buyButton.interactable = true;
            }
        }
        storeScreen.SetActive(true);
        inGameUI.SetActive(false);        
        state = GameState.OnMenu;
        OnStateChange?.Invoke();
        isInShop = true;
    }

    public void RestartGame()
    {
        gameOverScreen.SetActive(false);
        OnRestart?.Invoke();
        PlayGame();
    }

    public void ChangeScore(int points, bool addTotalScore)
    {
        score += points;
        
        if (score > 999)
            score = 999;

        if (points > 0 && addTotalScore)
            totalScore += points;
        
        inGameScoreText.text = score.ToString();
    }
}
