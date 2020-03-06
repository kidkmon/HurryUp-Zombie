using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour{

    //A static instance of the LevelManager
    public static LevelManager Instance { set; get; }
    
    private const float DISTANCE_BEFORE_SPAWN = 100.0f;
    private const int INITIAL_SEGMENTS = 12;
    private const int INITIAL_TRANSITION_SEGMENTS = 2;
    private const int MAX_SEGMENTs_ON_SCREEN = 17;
    
    //Variable to know the state of the collider
    [HideInInspector] public bool SHOW_COLLIDER = true;

    private Transform mainCameraPosition;

    private int amountOfActiveSegments;
    private int continuouSegments;
    private int currentSpawnZ;
    //private int currentLevel = 0;
    private int y1, y2, y3;

    [Header("List of Obstacles")]
    public List<Piece> ramps = new List<Piece>();
    public List<Piece> longblocks = new List<Piece>();
    public List<Piece> jumps = new List<Piece>();
    public List<Piece> slides = new List<Piece>();
    [HideInInspector] public List<Piece> pieces = new List<Piece>();

    [Header("List of available segments")]
    [SerializeField] private List<Segment> availableSegments = new List<Segment>();
    
    [Header("List of available transitions")]
    [SerializeField] private List<Segment> availableTransitions = new List<Segment>();
    [HideInInspector] public List<Segment> segments = new List<Segment>();


    void Awake(){
        Instance = this;
        mainCameraPosition = Camera.main.transform;
        currentSpawnZ = 0;
        // currentLevel = 0;
    }

    void Start(){
        for(int i = 0; i < INITIAL_SEGMENTS; i++){
            //Give a initial space when the game starts
            if(i < INITIAL_TRANSITION_SEGMENTS){
                SpawnTransition();
            }
            else{
                GenerateSegment();
            }
        }
    }

    void Update(){
        //If the difference of the lenght of the segments and the camera forward position is lower than the distance to spawn then start to generate more segments
        if(currentSpawnZ - mainCameraPosition.position.z < DISTANCE_BEFORE_SPAWN){
            GenerateSegment();
        }

        //If the list of segments reach the max segments on screen then deactivate the last element
        if(amountOfActiveSegments >= MAX_SEGMENTs_ON_SCREEN){
            segments[amountOfActiveSegments - 1].Despawn();
            amountOfActiveSegments--;
        }
    }

    void GenerateSegment(){
        SpawnSegment();

        //Check if the segments needs a transition or not
        if(Random.Range(0f, 1f) < (continuouSegments*0.25f)){
            continuouSegments = 0;
            SpawnTransition();
        }
        else{
            continuouSegments++;
        }
    }

    void SpawnSegment(){
        
        //Search the possible segments that satisfy the begin condition 
        List<Segment> possibleSeg = availableSegments.FindAll(x => x.beginY1.Equals(y1) || x.beginY2.Equals(y2) || x.beginY3.Equals(y3));
        
        //Get a random segment of the possible segments list
        int id = Random.Range(0, possibleSeg.Count);
        
        Segment segment = GetSegment(id, false);
        y1 = segment.endY1;
        y2 = segment.endY2;
        y3 = segment.endY3;

        segment.transform.SetParent(transform);
        segment.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += segment.lenght;
        amountOfActiveSegments++;
        segment.Spawm();
    }

    void SpawnTransition(){
        
        //Search the possible transition that satisfy the begin condition
        List<Segment> possibleTransition = availableTransitions.FindAll(x => x.beginY1.Equals(y1) || x.beginY2.Equals(y2) || x.beginY3.Equals(y3));
        
        //Get a random transition of the possible transitions list
        int id = Random.Range(0, possibleTransition.Count);
        
        Segment segment = GetSegment(id, true);
        y1 = segment.endY1;
        y2 = segment.endY2;
        y3 = segment.endY3;

        segment.transform.SetParent(transform);
        segment.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += segment.lenght;
        amountOfActiveSegments++;
        segment.Spawm();
    }

    public Segment GetSegment(int id, bool transition){
        Segment segment = null;
        segment = segments.Find(x => x.SegId.Equals(id) && x.transition.Equals(transition) && !x.gameObject.activeSelf);

        if(segment == null){
            GameObject go = Instantiate((transition) ? availableTransitions[id].gameObject : availableSegments[id].gameObject) as GameObject;
            segment = go.GetComponent<Segment>();
            segment.SegId = id;
            segment.transition = transition;
            segments.Insert(0, segment);
        }
        else{
            segments.Remove(segment);
            segments.Insert(0, segment);
        }

        return segment;
    }

    public Piece GetPiece(PieceType pieceType, int visualIndex){
        Piece piece = pieces.Find(x => x.type.Equals(pieceType) && x.visualIndex.Equals(visualIndex) && !x.gameObject.activeSelf);
        
        if(piece == null){
            GameObject go = null;

            if(pieceType.Equals(PieceType.ramp)){ go = ramps[visualIndex].gameObject; }
            else if(pieceType.Equals(PieceType.longblock)){ go = longblocks[visualIndex].gameObject; }
            else if(pieceType.Equals(PieceType.jump)){ go = jumps[visualIndex].gameObject; }
            else if(pieceType.Equals(PieceType.slide)){ go = slides[visualIndex].gameObject; }

            go = Instantiate(go);
            piece = go.GetComponent<Piece>();
        }
        
        return piece;
    }
}
