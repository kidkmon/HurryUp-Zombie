using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour{

    //A static instance of the HudManager
    public static HudManager Instance { set; get; }

    //The value of the coin in the score pontuation
    private const int COIN_SCORE_AMOUNT = 5;

    [Header("Panels")]
    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject pausePanel;

    [Header("Game Panel")]
    public Text scoreText;
    public Text coinText;
    public Text modifierText;

    private float score, coinScore, modifierScore;
    private int lastScore;

    [Header("Game Over Panel")]
    public GameObject gameOverPanel;
    public Text gameOverScoreText, gameOverCoinText;

    void Awake(){
        Instance = this;
        modifierScore = 1;

        scoreText.text = score.ToString("0");
        modifierText.text = "x" + modifierScore.ToString("0.0");
        coinText.text = coinScore.ToString("0");
    }

    public void UpdateScore(){
        score += (Time.deltaTime * modifierScore);
        lastScore = (int) score;
        scoreText.text = lastScore.ToString("0");
    }

    public void GetCoin(){
        GetComponent<AudioSource>().Play();
        coinScore++;
        coinText.text = coinScore.ToString("0");

        score += COIN_SCORE_AMOUNT;
        scoreText.text = score.ToString("0");
    }

    public void UpdateModifier(float modifierAmount){
        modifierScore = 1.0f + modifierAmount;
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

    public void UpdateGameOverPanelValues(){
        gameOverScoreText.text = scoreText.text;
        gameOverCoinText.text = coinText.text;
        gamePanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(true);
    }
}
