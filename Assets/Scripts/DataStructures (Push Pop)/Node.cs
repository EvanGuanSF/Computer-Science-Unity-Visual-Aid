using UnityEngine;

public class Node : MonoBehaviour
{
    // Hold references for previous and next nodes.
    public GameObject prevNode { set; get; }
    public GameObject nextNode { set; get; }

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
}
