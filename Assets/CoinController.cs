using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{

    private static CoinController instance;
    Player player;

    private void Awake()
    {
        player = FindAnyObjectByType<Player>().GetComponent<Player>();
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        cc();
    }
    private void cc()
    {
        
        player.LoadPlayerData();


    }
    
}
