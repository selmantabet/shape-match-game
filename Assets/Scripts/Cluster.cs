using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*  Shape Match - Developed by Selman Tabet (@selmantabet - https://selman.io/) for Mezan Studios
 *  
 *  Cluster Script
 *  
 *  This script is responsible for the grouping/clustering behavior of shapes when they collide.
 */

public class Cluster : MonoBehaviour
{
    public int netScore;
    public int multiplier;
    private GameObject shape;
    public List<GameObject> attachedShapes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int CalculateCluster() //Tallies all shapes in a cluster and despawns them all at once.
    { //Returns the total tallied score.
        netScore = 0;
        multiplier = 0;
        for (int i = 0; i < attachedShapes.Count; i++)
        {
            shape = attachedShapes[i];
            if (shape.CompareTag("bonus"))
            {
                multiplier++;
            }

            else
            {
                netScore += 100;
            }

            shape.SetActive(false);
        }

        netScore = netScore * 2 * multiplier;

        return netScore;
    }

    public void Attach(GameObject attachedObject)
    {
        attachedShapes.Add(attachedObject);
        attachedObject.GetComponent<ShapeStatus>().isAttached = true;
    }

    public Cluster(GameObject firstObject) //New cluster constructor.
    {
        attachedShapes = new List<GameObject>();
        attachedShapes.Add(firstObject);
        firstObject.GetComponent<ShapeStatus>().isAttached = true;
    }
}
