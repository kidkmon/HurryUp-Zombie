using UnityEngine;

public class CameraController : MonoBehaviour{
    
    [Header("Player Transform")]
    private Transform lookAt;
    
    [Header("Camera Configurations")]
    public Vector3 offset;
    public Vector3 rotation;

    //Variable to know if the camera is moving or not.
    public bool IsMoving { set; get; }

    private Vector3 desiredPosition;

    void Start(){
        lookAt = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate(){

        //If not moving just ignore the lines below
        if(!IsMoving){ return; }

        //Get the desired position for the camera
        desiredPosition = lookAt.position + offset;        
        desiredPosition.x = 0;

        //Interpolate between the actual camera position and the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);

        //Interpolate between the initial rotation of the game and start the game
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), 0.25f);
    }
}
