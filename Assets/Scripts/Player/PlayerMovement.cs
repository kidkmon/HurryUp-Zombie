using UnityEngine;

public class PlayerMovement : MonoBehaviour{

    public static PlayerMovement Instance { set; get; }

    private const float LANE_DISTANCE = 2.0f;
    private const float TURN_SPEED = 0.05f;

    [HideInInspector] public bool isRunning;

    private Animator playerAnimator;
    private CharacterController controller;
    
    [Range(0, 2), HideInInspector] public int desiredLane = 1;
    
    private float jumpForce = 4.5f;
    private float gravity = 12.0f;
    private float verticalVelocity;

    private float originalSpeed = 7.0f;
    private float speed;
    private float speedIncreaseLastTick;
    private float speedIncreaseTime = 2.5f;
    private float speedIncreaseAmount = 0.1f;

    private Vector3 targetPosition;

    void Awake(){
        Instance = this;
        isRunning = false;
    }

    void Start(){
        speed = originalSpeed;
        controller = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
    }

    
    void Update(){

        if(!isRunning){ return; }
        
        if(Time.time - speedIncreaseLastTick > speedIncreaseTime){
            speedIncreaseLastTick = Time.time;
            speed += speedIncreaseAmount;
            HudManager.Instance.UpdateModifier(speed - originalSpeed);
        }

        if(MobileInput.Instance.SwipeLeft){
            MoveLane(false);
        }
        if(MobileInput.Instance.SwipeRight){
            MoveLane(true);
        }

        MoveToSide();

        CheckMechanics();
        
        MoveRotation();
    }


    void OnControllerColliderHit(ControllerColliderHit hit){
        if(hit.gameObject.tag.Equals("Obstacle")){
            PlayerMechanics.Instance.Crash();
        }
    }

    void MoveLane(bool goingRight){
        PlayerMovement.Instance.desiredLane += (goingRight) ? 1 : -1;
        PlayerMovement.Instance.desiredLane = Mathf.Clamp(PlayerMovement.Instance.desiredLane, 0, 2);
    }

    bool IsGrounded(){
        Ray groundRay = new Ray(new Vector3(PlayerMovement.Instance.controller.bounds.center.x, (PlayerMovement.Instance.controller.bounds.center.y - PlayerMovement.Instance.controller.bounds.extents.y) + 0.2f, PlayerMovement.Instance.controller.bounds.center.z), Vector3.down);
        return Physics.Raycast(groundRay, 0.2f + .1f);
    }

    void MoveToSide(){
        targetPosition = transform.position.z * Vector3.forward;

        if(desiredLane == 0){
            targetPosition += Vector3.left * LANE_DISTANCE;
        }
        else if(desiredLane == 2){
            targetPosition += Vector3.right * LANE_DISTANCE;
        }
    }

    void CheckMechanics(){
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).x * speed;
        
        bool isGrounded = IsGrounded();
        playerAnimator.SetBool("Grounded", isGrounded);

        if(isGrounded){
            verticalVelocity = -0.1f;

            if(MobileInput.Instance.SwipeUp){
                playerAnimator.SetTrigger("Jump");
                verticalVelocity = jumpForce;
            }
            else if(MobileInput.Instance.SwipeDown){
                PlayerMechanics.Instance.StartSliding();
                Invoke("StopSliding", 1.0f);
            }
        }
        else{
            verticalVelocity -= (gravity * Time.deltaTime);
            if(MobileInput.Instance.SwipeDown){
                verticalVelocity = -jumpForce;
            }
        }

        moveVector.y = verticalVelocity;
        moveVector.z = speed;

        controller.Move(moveVector * Time.deltaTime);
    }

    void MoveRotation(){
        Vector3 dir = controller.velocity;
        dir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, dir, TURN_SPEED);
    }

    public void StopSliding(){
        playerAnimator.SetBool("Roll", false);
        controller.height *= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y * 2, controller.center.z);
    }
}
