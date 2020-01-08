using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    public int count { get; set; }
    public int totalNodesCreated { get; set; }
    public GameObject head { get; set; }
    public GameObject tail { get; set; }
    public GameObject prefab;


    public Queue()
    {
        count = 0;
        head = null;
    }

    public Queue(GameObject newHead)
    {
        head = newHead;
    }

    /// <summary>
    /// Push a new node to the end of the queue given its coordinates.
    /// </summary>
    /// <param name="newHeadPosition"></param>
    public void push(Vector3 newHeadPosition)
    {
        GameObject newHead = Instantiate(prefab, newHeadPosition, Quaternion.identity);
        newHead.GetComponent<Node>().UpdatePositions();

        if (head != null)
        {
            GameObject nodeReference = head;

            tail.GetComponent<Node>().nextNode = newHead;
            newHead.GetComponent<Node>().prevNode = tail;
            tail = newHead;
        }
        else
        {
            head = newHead;
            tail = newHead;
        }

        totalNodesCreated++;
        count++;
    }

    /// <summary>
    /// Pop the oldest node from the front of the queue.
    /// </summary>
    /// <returns>Reference to the GameObject (node) that needs to be (and is only able to be) destroyed via Update, FixedUpdate, or other main Unity loop.</returns>
    public GameObject pop()
    {
        GameObject temp = null;

        // Do the variable shuffle and adjust the references in the Node scripts.
        if (head != null && head.GetComponent<Node>().nextNode != null)
        {
            temp = head.gameObject;
            head = head.GetComponent<Node>().nextNode;
            head.GetComponent<Node>().prevNode = null;
            count--;
        }
        else if (head != null)
        {

            temp = head.gameObject;
            head = null;
            count--;
        }

        if (temp != null)
        {
            Debug.Log("Removing node at xPos: " +
                temp.GetComponent<Node>().xPos);
        }

        return temp;
    }
}
