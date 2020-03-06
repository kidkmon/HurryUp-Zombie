using UnityEngine;


//To get a better knowledge of the types of the pieces on the segments
public enum PieceType{
    none = -1,
    ramp = 0,
    longblock = 1,
    jump = 2,
    slide = 3
}

public class Piece : MonoBehaviour{
    public PieceType type;
    public int visualIndex;
}
