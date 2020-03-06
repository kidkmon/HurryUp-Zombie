using UnityEngine;

public class Parallax : MonoBehaviour{

    //A const value to help calculate the distance to respawn to get a better parallax
    private const float DISTANCE_TO_RESPAWN = 10.0f;

    [Header("Parallax Parameters")]
    public float scrollSpeed;
    public float totalLenght;

    //Variable to know if the parallax is scrolling or not.
    public bool IsScrolling { set; get; }

    private float scrollLocation;
    private Transform playerTransform;

    void Start(){
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update(){
        
         //If not scrolling just ignore the lines below
        if(!IsScrolling){ return; }

        scrollLocation += scrollSpeed * Time.deltaTime;
        transform.position = (playerTransform.position.z + scrollLocation) * Vector3.forward;

        //If the distance of the first child of parallax is lower than the difference of player position and distance to respwan
        // than the first child turns the last child of parallax and gets a new position
        if(transform.GetChild(0).transform.position.z < playerTransform.position.z - DISTANCE_TO_RESPAWN){
            transform.GetChild(0).localPosition += Vector3.forward * totalLenght;
            transform.GetChild(0).SetSiblingIndex(transform.childCount);
        }
    }

}
