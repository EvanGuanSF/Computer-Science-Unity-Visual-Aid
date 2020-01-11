using UnityEngine;

public class Stack : MonoBehaviour
{
    public int count;
    public GameObject head;
    public GameObject prefab;


    public Stack()
    {
        count = 0;
        head = null;
    }

    public Stack(GameObject newHead)
    {
        head = newHead;
    }

    /// <summary>
    /// Push a new node onto the stack given its coordinates.
    /// </summary>
    /// <param name="newHeadPosition"></param>
    public void push(Vector3 newHeadPosition)
    {
        GameObject newHead = Instantiate(prefab, newHeadPosition, Quaternion.identity);
        newHead.transform.parent = this.transform;
        newHead.GetComponent<Node>().UpdatePositions();

        if (head != null)
        {
            head.GetComponent<Node>().nextNode = newHead;
            newHead.GetComponent<Node>().prevNode = head;
            head = newHead;
        }
        else
        {
            head = newHead;
        }

        count++;
    }

    /// <summary>
    /// Pop the most recently added node off the top of the list.
    /// </summary>
    /// <returns>Reference to the GameObject (node) that needs to be (and is only able to be) destroyed via Update, FixedUpdate, or other main Unity loop.</returns>
    public GameObject pop()
    {
        GameObject temp = null;

        // Do the variable shuffle and adjust the references in the Node scripts.
        if (head != null && head.GetComponent<Node>().prevNode != null)
        {
            temp = head.gameObject;
            head = head.GetComponent<Node>().prevNode;
            head.GetComponent<Node>().nextNode = null;
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
