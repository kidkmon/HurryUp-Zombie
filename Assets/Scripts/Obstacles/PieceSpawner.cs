using UnityEngine;

public class PieceSpawner : MonoBehaviour{

    [Header("Type of the Piece")]
    public PieceType type;

    private Piece currentPiece;

    public void Spawn(){
        int amountObj = 0;

        amountObj = (type.Equals(PieceType.jump)) ? LevelManager.Instance.jumps.Count : 
                    (type.Equals(PieceType.slide)) ? LevelManager.Instance.slides.Count :
                    (type.Equals(PieceType.longblock)) ? LevelManager.Instance.longblocks.Count :
                    (type.Equals(PieceType.ramp)) ? LevelManager.Instance.ramps.Count : 0;

        
        currentPiece = LevelManager.Instance.GetPiece(type, Random.Range(0, amountObj));
        currentPiece.gameObject.SetActive(true);
        currentPiece.transform.SetParent(transform, false);
    }

    public void Despawn(){
        currentPiece.gameObject.SetActive(false);
    }
}
