using UnityEngine;

public class GameManager : MonoBehaviour{

    //A static instance of the GameManager
    public static GameManager Instance { set; get; }
    
    //Variable to check if the player is dead or alive
    public bool IsDead { set; get; }

    private bool isGameStarted = false;

    void Awake(){
        Instance = this;
    }

    void Update(){
        if(MobileInput.Instance.Tap && !isGameStarted){
            isGameStarted = true;
            GetComponents<AudioSource>()[0].Play();
            HudManager.Instance.startPanel.SetActive(false);
            HudManager.Instance.gamePanel.SetActive(true);
            PlayerMechanics.Instance.StartRunning();
            FindObjectOfType<Parallax>().IsScrolling = true;
            FindObjectOfType<CameraController>().IsMoving = true;
        }

        if(isGameStarted && !IsDead){
            HudManager.Instance.UpdateScore();
        }
    }

    public void OnDeath(){
        IsDead = true;
        GetComponents<AudioSource>()[0].Stop();
        GetComponents<AudioSource>()[1].Play();
        FindObjectOfType<Parallax>().IsScrolling = false;
        HudManager.Instance.UpdateGameOverPanelValues();
    }
}
