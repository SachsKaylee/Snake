using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private Text _highscoreText;
    [SerializeField] private GameObject _interfaceParent;
    [SerializeField] private Text _scoreText;

    private void Start()
    {
        ShowGameOverScreen();
    }

    public void OnClick_NewGame()
    {
        HideGameOverScreen();
        _gameController.StartNewGame();
    }

    public void OnClick_QuitGame()
    {
        Application.Quit();
    }

    public void ShowGameOverScreen(int score = 0)
    {
        GameController.IsPaused = true;
        _interfaceParent.SetActive(true);
        _highscoreText.text = PlayerPrefs.GetInt("Highscore", 0).ToString();
        _scoreText.text = score.ToString();
    }

    public void HideGameOverScreen()
    {
        GameController.IsPaused = false;
        _interfaceParent.SetActive(false);
    }
}