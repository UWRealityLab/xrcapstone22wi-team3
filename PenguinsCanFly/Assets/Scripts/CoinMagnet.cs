using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMagnet : MonoBehaviour
{
    private const string FishCoinTag = "FishCoin";
    private const float magnetForce = 100;

    private ISet<Rigidbody> caughtCoins = new HashSet<Rigidbody>();
    
    // Start is called before the first frame update
    void FixedUpdate()
    {
        Debug.Log("SAVE:numCoins:" + caughtCoins.Count);
        foreach (Rigidbody r in caughtCoins)
        {
            r.velocity = (transform.position - (r.transform.position + r.centerOfMass)) * magnetForce * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(FishCoinTag))
        {
            caughtCoins.Add(other.attachedRigidbody);
        }
    }
}
