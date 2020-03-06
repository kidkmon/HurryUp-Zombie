using UnityEngine;

public class CoinSpawner : MonoBehaviour{

    [Header("Coin Configuration")]
    [Tooltip("The max number of coins in a line")]
    [SerializeField] private int maxCoin = 5;

    [Tooltip("The chance of the coin to spawn, value between 0 and 1")]
    [Range(0,1)]
    [SerializeField] private float chanceToSpawn = 0.5f;
    
    private GameObject[] coins;

    void Awake(){
        
        //Instatiating the array of coins
        coins = new GameObject[transform.childCount];

        for(int i = 0; i < transform.childCount; i++){
            coins[i] = transform.GetChild(i).gameObject;
        }

        OnDisable();
    }

    void OnEnable(){
        
        //If the random number isnt't bigger than the chance to spawn variable then just ignore the lines below
        if(Random.Range(0.0f, 1.0f) > chanceToSpawn){ return; }

        int randomNumber = Random.Range(0, maxCoin);

        //Active a certain number of coins based on the random number
        for(int i = 0; i < randomNumber; i++){
            coins[i].SetActive(true);
        }
    }

    void OnDisable(){
        foreach(GameObject gameObject in coins){
            gameObject.SetActive(false);
        }
    }
}
