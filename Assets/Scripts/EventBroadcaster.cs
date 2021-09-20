using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBroadcaster : MonoBehaviour
{
    public delegate void ShapesCollided();
    public static event ShapesCollided shapesCollidedInfo;

    public delegate void ShapesMatched();
    public static event ShapesMatched shapesMatchedInfo;

    public delegate void ShapeFellOff();
    public static event ShapeFellOff shapeFellOff;

    public delegate void ShapeFossilized();
    public static event ShapeFellOff shapeFossilized;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CollisionEvent()
    {

    }


}
