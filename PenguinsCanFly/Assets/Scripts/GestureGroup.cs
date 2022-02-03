using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class GestureGroup : MonoBehaviour
{
    public GestureBox box1;

    public GestureBox box2;

    private int _hitCount = 0;
    private bool _prevFrameWasHit = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (box1.IsTouched() && box2.IsTouched() && !_prevFrameWasHit)
        {
            _hitCount += 1;
            _prevFrameWasHit = true;

            // TODO: might want to remove this all together when we make the boxes always invisible
            if (_hitCount == 1)
            {
                box1.GetComponent<Renderer>().enabled = false;
                box2.GetComponent<Renderer>().enabled = false;
            }

        }
        else if (!box1.IsTouched() && !box2.IsTouched())
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
