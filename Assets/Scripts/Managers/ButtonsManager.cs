using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour {

	[Header("Panel")]
	public GameObject pausePanel;
	public GameObject bgBlackPanel;

	[Header("HUD Animator Controllers")]
	public Animator continueBtnAnim;
	public Animator gameOverPanelAnim;
	public Animator menuBtnAnim;
	public Animator pauseBtnAnim;
	public Animator playAgainBtnAnim;
	public Animator restartBtnAnim;
	public Animator transitionAnim;

	private CameraController cameraController;
	private GameObject player;
	private Parallax parallax;
	
	void Start(){
		player = GameObject.FindGameObjectWithTag("Player");
		pausePanel.SetActive(false);
		bgBlackPanel.SetActive(false);

		cameraController = FindObjectOfType<CameraController>();
		parallax = FindObjectOfType<Parallax>();
	}

	public void ContinueButton(){
		StartCoroutine(StartPauseExitAnimation());
	}

	public void MenuButton(){
		StartCoroutine(StartMenuAnimation());
	}

	public void PauseButton(){
		bool pauseIsActive = pausePanel.activeInHierarchy;

		if(pauseIsActive){
			StartCoroutine(StartPauseExitAnimation());
		}
		else{
			bgBlackPanel.SetActive(!pauseIsActive);
			pausePanel.SetActive(!pauseIsActive);
			PlayerMovement.Instance.isRunning = false;
			parallax.IsScrolling = false;
            cameraController.IsMoving = false;
		}
	}

	public void RestartButton(){
		StartCoroutine(StartRestartAnimation());
	}
	
	IEnumerator StartMenuAnimation(){
		menuBtnAnim.SetTrigger("Exit");
		pauseBtnAnim.SetTrigger("Exit");
		yield return new WaitForSeconds(.6f);
		transitionAnim.SetTrigger("Exit");
		yield return new WaitForSeconds(.6f);
		SceneManager.LoadScene("MainScene");
	}

	IEnumerator StartPauseExitAnimation(){
		continueBtnAnim.SetTrigger("Exit");
		pauseBtnAnim.SetTrigger("Exit");
		yield return new WaitForSeconds(.6f);
		pausePanel.SetActive(false);
		bgBlackPanel.SetActive(false);
		PlayerMovement.Instance.isRunning = true;
		parallax.IsScrolling = true;
        cameraController.IsMoving = true;
	}

	IEnumerator StartRestartAnimation(){
		if(gameOverPanelAnim.gameObject.activeInHierarchy){
			playAgainBtnAnim.SetTrigger("Exit");
			gameOverPanelAnim.SetTrigger("Exit");
			yield return new WaitForSeconds(.4f);
		}
		else{
			restartBtnAnim.SetTrigger("Exit");
			pauseBtnAnim.SetTrigger("Exit");
			yield return new WaitForSeconds(.6f);
		}
		
		transitionAnim.SetTrigger("Exit");
		yield return new WaitForSeconds(.6f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
