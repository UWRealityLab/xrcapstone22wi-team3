using System.Collections.Generic;
using UnityEngine;

public class CoinMagnet : MonoBehaviour
{
    public AudioSource audioSource;
    
    private const string FishCoinTag = "FishCoin";
    private const float magnetForce = 100;

    private ISet<Rigidbody> caughtCoins = new HashSet<Rigidbody>();

    // Start is called before the first frame update
    void FixedUpdate()
    {
        Debug.Log("SAVE:caughtCoins:" + caughtCoins.Count);
        ISet<Rigidbody> coinsToDestroy = new HashSet<Rigidbody>();
        
        foreach (Rigidbody r in caughtCoins)
        {
            r.velocity = (transform.position - (r.transform.position + r.centerOfMass)) * magnetForce * Time.deltaTime;
            r.transform.localScale *= 0.97f;

            if (r.transform.localScale == Vector3.zero) // == allows for float imprecision
            {
                coinsToDestroy.Add(r);
            }
        }

        foreach (Rigidbody r in coinsToDestroy)
        {
            caughtCoins.Remove(r);
            Destroy(r.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(FishCoinTag))
        {
            if (caughtCoins.Add(other.attachedRigidbody))
            {
                ScoreCounter.Instance.numCoins++;
                audioSource.Play();
            }
        }
    }
}
