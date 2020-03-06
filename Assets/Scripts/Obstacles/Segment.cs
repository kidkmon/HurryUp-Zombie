using UnityEngine;

public class Segment : MonoBehaviour{

    public int SegId { set; get; }
    
    [Header("Transition")]
    public bool transition;

    [Header("Segment Characteristics")]
    public int lenght;
    public int beginY1, beginY2, beginY3;
    public int endY1, endY2, endY3;

    private PieceSpawner[] pieces;

    private void Awake(){
        pieces = gameObject.GetComponentsInChildren<PieceSpawner>();
    
        //Enabling the mesh renderer of the official pieces
        if(LevelManager.Instance.SHOW_COLLIDER){
            for(int i = 0; i < pieces.Length; i++){
                foreach(MeshRenderer meshRenderer in pieces[i].GetComponentsInChildren<MeshRenderer>())
                    meshRenderer.enabled = LevelManager.Instance.SHOW_COLLIDER;
            }
        }
    }

    public void Spawm(){
        gameObject.SetActive(true);

        for(int i = 0; i < pieces.Length; i++){
            pieces[i].Spawn();
        }
    }

    public void Despawn(){
        gameObject.SetActive(false);
        
        for(int i = 0; i < pieces.Length; i++){
            pieces[i].Despawn();
        }
    }
}
