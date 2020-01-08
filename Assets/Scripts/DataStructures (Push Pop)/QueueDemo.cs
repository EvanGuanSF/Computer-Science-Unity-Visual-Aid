using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueDemo : MonoBehaviour
{
    public float secondsBetweenInputs = 0.2f;
    private float timeOfLastInput = 0.0f;
    public Queue theQueue;

    private void Start()
    {
        timeOfLastInput = Time.time;
    }

    private void Update()
    {
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetMouseButton(0)) &&
            timeOfLastInput + secondsBetweenInputs < Time.time)
        {
            Debug.Log("Making new node.");
            timeOfLastInput = Time.time;
            Vector3 newNodePosition = new Vector3(theQueue.totalNodesCreated * 2,
                transform.position.y,
                0);
            theQueue.push(newNodePosition);
        }
        else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetMouseButton(1)) &&
            timeOfLastInput + secondsBetweenInputs < Time.time)
        {
            Debug.Log("Removing most recent node.");
            timeOfLastInput = Time.time;


            GameObject toDelete = theQueue.pop();
            if (toDelete != null)
            {
                Destroy(toDelete);
            }
        }
    }
}
