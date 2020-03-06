using UnityEngine;

public class Coin : MonoBehaviour{
    
    private Animator coinAnimator;


    void Awake(){ coinAnimator = GetComponent<Animator>(); }

    void OnEnable(){ coinAnimator.SetTrigger("Spawn"); }

    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.tag.Equals("Player")){
            HudManager.Instance.GetCoin();
            coinAnimator.SetTrigger("Collect");
        }
    }
}
