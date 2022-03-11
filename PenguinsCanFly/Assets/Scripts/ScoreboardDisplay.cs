using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreboardDisplay : MonoBehaviour
{
    public TextMeshProUGUI textbox;
    public Transform fishSpawnPoint;
    public GameObject fishCoinPrefab;
    public AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        textbox.text = SaveManager.Instance.GetHiScore() + "\n" + SaveManager.Instance.GetPrevScore()
                       + "\n" + SaveManager.Instance.GetTotalCoins();
        StartCoroutine(SpawnFish());
    }
    
    IEnumerator SpawnFish()
    {
        int prevNumCoins = SaveManager.Instance.GetPrevNumCoins();
        if (prevNumCoins > 0)
        {
            audioSource.Play();
        }
        for (int i = 0; i < prevNumCoins; i++)
        {
            Instantiate(fishCoinPrefab,
                fishSpawnPoint.position,
                Random.rotation);
            yield return new WaitForSeconds(.2f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
