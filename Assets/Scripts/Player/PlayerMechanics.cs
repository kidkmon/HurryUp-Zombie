using UnityEngine;

public class PlayerMechanics : MonoBehaviour{

    public static PlayerMechanics Instance { set; get; }

    private Animator playerAnimator;
    private CharacterController controller;

    void Awake(){
        Instance = this;
    }

    void Start(){
        controller = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
    }

    public void StartRunning(){
        PlayerMovement.Instance.isRunning = true;
        playerAnimator.SetTrigger("StartGame");
    }

    public void StartSliding(){
        playerAnimator.SetBool("Roll", true);
        controller.height /= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y / 2, controller.center.z);
    }

    public void Crash(){
        playerAnimator.SetTrigger("Death");
        PlayerMovement.Instance.isRunning = false;
        GameManager.Instance.OnDeath();
    }
}
