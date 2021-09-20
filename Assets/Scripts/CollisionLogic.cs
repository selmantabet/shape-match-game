using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*  Shape Match - Developed by Selman Tabet (@selmantabet - https://selman.io/) for Mezan Studios
 *  
 *  Collision Logic Script
 *  
 *  This script is responsible for handling clustering and all collisions between world objects.
 */

public class CollisionLogic : MonoBehaviour
{
    public List<Cluster> clusters;
    public Cluster tempCluster;
    public int clusterScoreTotal;

    public AudioSource audioData;

    public Renderer shapeRendererCollider; 
    public Renderer shapeRendererCollidee; //I know it is not a real word. Please ignore it :)
    public Renderer fossilizer;
    public Renderer fossilizee; //Same here.

    public delegate void ShapesMatched();
    public static event ShapesMatched shapesMatchedInfo; //Increments score.

    public delegate void ShapeFellOff();
    public static event ShapeFellOff shapeFellOffInfo; //Lose health.

    public delegate void ShapeFossilized();
    public static event ShapeFellOff shapeFossilizedInfo; //Lose health.

    private void OnCollisionEnter(Collision other)
    {
        //shapeRendererCollider = gameObject.transform.GetChild(0).GetComponent<Renderer>();
        //shapeRendererCollidee = other.gameObject.transform.GetChild(0).GetComponent<Renderer>();

        //Debug.Log("Collider: " + gameObject.name);
        //Debug.Log("Collidee: " + other.gameObject.name);

        if (other.gameObject.CompareTag("Ground")) //Check if the shape just landed.
        { //First contact with arena.
            Debug.Log("Grounded");
            audioData = other.gameObject.GetComponent<AudioSource>();
            audioData.Play(0);
            gameObject.GetComponent<ShapeStatus>().isGrounded = true;
            
        }

        else if (other.gameObject.CompareTag("Despawn")) //Check if the shape fell off.
        {
            audioData = other.gameObject.GetComponent<AudioSource>();
            audioData.Play(0);
            shapeFellOffInfo?.Invoke();
            gameObject.SetActive(false);
        }

        else if (gameObject.CompareTag("Despawn"))
        {
            audioData = gameObject.GetComponent<AudioSource>();
            audioData.Play(0);
            shapeFellOffInfo?.Invoke();
            other.gameObject.SetActive(false);
        }

        else if (other.gameObject.CompareTag("bomb") || gameObject.CompareTag("bomb")) // && gameObject.GetComponent<ShapeStatus>().isGrounded == true
        { //An object touched the bomb, despawn all objects attached but do not invoke any event.
            Debug.Log("BOOM.");
            audioData = gameObject.GetComponent<AudioSource>();
            audioData.Play(0);
            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
        }

        else if (other.gameObject.CompareTag("bonus") || gameObject.CompareTag("bonus"))
        { //An object touched the bonus shape, despawn all objects attached and invoke shapeMatched event.
            shapesMatchedInfo?.Invoke();
            audioData = other.gameObject.GetComponent<AudioSource>();
            audioData.Play(0);
            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
        }

        else if (gameObject.CompareTag(other.gameObject.tag)) //Matching shape scenario
        {
            Debug.Log("Tag matched.");
            audioData = other.gameObject.GetComponent<AudioSource>();
            audioData.Play(0);
            StartCoroutine(Matched());

            //if (shapeRendererCollider.material == shapeRendererCollidee.material
            //    && shapeRendererCollider.material.mainTexture == shapeRendererCollidee.material.mainTexture)
            //{//Perfect match scenario (Material + Texture match)
            //    Debug.Log("Perfect match.");
            //    audioData = other.gameObject.GetComponent<AudioSource>();
            //    audioData.Play(0);
            //    StartCoroutine(Matched(gameObject, other.gameObject));
            //}
            //Debug.Log("Failed matching texture check.");
        }
        //else if (gameObject.CompareTag("Despawn"))
        //{
        //    audioData = gameObject.GetComponent<AudioSource>();
        //    audioData.Play(0);
        //    shapeFellOffInfo?.Invoke();
        //    other.gameObject.SetActive(false);
        //}

        
        else
        {
            Debug.Log("Fossilization started.");
            shapeFossilizedInfo?.Invoke();
            var renderer = other.gameObject.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", Color.grey); //Supposed to turn the shape grey.
        }



        //else if ( (other.gameObject.name != "arena") && (other.gameObject.name != gameObject.name))
        //{
        //    shapeFossilizedInfo?.Invoke();
        //    var renderer = other.gameObject.GetComponent<Renderer>();
        //    renderer.material.SetColor("_Color", Color.grey);

        //}
    }

    IEnumerator Matched()
    {

        //if (!collidee.GetComponent<ShapeStatus>().isAttached)
        //{
        //    Debug.Log("Cluster created.");
        //    tempCluster = new Cluster(collider);
        //    clusters.Add(tempCluster);
        //    collider.GetComponent<ShapeStatus>().clusterID = clusters.IndexOf(tempCluster);
        //}

        //else
        //{
        //    Debug.Log("Appending to cluster.");
        //    clusters[collidee.GetComponent<ShapeStatus>().clusterID].Attach(collider);
        //}


        //Matching scenario

        shapesMatchedInfo?.Invoke();
        //Matching scenario
        Debug.Log("It's a match!");
        gameObject.SetActive(false);
        //Rigidbody rigidBody = GetComponent<Rigidbody>();
        //rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(3);
        //Debug.Log("Waited 3 seconds!");

        

        //clusterScoreTotal = clusters[collidee.GetComponent<ShapeStatus>().clusterID].CalculateCluster();
        //shapesMatchedInfo?.Invoke();
    }

    IEnumerator Fossilized()
    {
        shapeFossilizedInfo?.Invoke();
        fossilizer = GameObject.Find("Fossilizer").GetComponent<Renderer>();
        fossilizee = gameObject.transform.GetChild(0).GetComponent<Renderer>();
        fossilizee.material.mainTexture = null;
        fossilizee.material = fossilizer.material;

        Rigidbody rigidBody = GetComponent<Rigidbody>();
        rigidBody.constraints = RigidbodyConstraints.FreezeAll; //Freeze until blown up.

        yield return new WaitForSeconds(3);
    }

}
