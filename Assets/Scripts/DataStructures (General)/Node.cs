using UnityEngine;

public class Node : MonoBehaviour
{
    // Hold references for previous and next nodes.
    public GameObject prevNode;
    public GameObject nextNode;
    public int nodeIndex;
    public int nodeValue;

    // Hold the positional information of the current game object.
    public float xPos = 0.0f;
    public float yPos = 0.0f;
    public float zPos = 0.0f;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public Node()
    {
        prevNode = null;
        nextNode = null;
        nodeValue = 0;
    }

    /// <summary>
    /// Updates the script positional values using the positional information from the parent GameObject.
    /// </summary>
    public void UpdatePositions()
    {
        xPos = transform.position.x;
        yPos = transform.position.y;
        zPos = transform.position.z;
    }

    public Vector3 GetNodePosition()
    {
        return new Vector3(xPos, yPos, zPos);
    }

    /// <summary>
    /// Update the name of the node to be the same as its value.
    /// </summary>
    public void UpdateName()
    {
        transform.name = nodeValue.ToString();
    }
}
