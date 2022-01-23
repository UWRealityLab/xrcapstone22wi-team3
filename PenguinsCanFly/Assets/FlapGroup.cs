using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class FlapGroup : MonoBehaviour
{
    public GestureBox left;

    public GestureBox right;

    private int _hitCount = 0;
    private bool _prevFrameWasHit = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (left.isTouched && right.isTouched && !_prevFrameWasHit)
        {
            _hitCount += 1;
            _prevFrameWasHit = true;
        }
        else if (!left.isTouched && !right.isTouched)
        {
            // player has to move both hands out of the collision zone to register the next flap
            _prevFrameWasHit = false;
        }
    }

    public int GetHitCount()
    {
        return _hitCount;
    }
}
