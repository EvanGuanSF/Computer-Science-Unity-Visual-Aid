using UnityEngine;

public class Node : MonoBehaviour
{
    // Hold references for previous and next nodes.
    [Header("Node References")]
    public GameObject prevNode;
    public GameObject nextNode;

    [Header("Node Properties")]
    public int nodeIndex;
    public int nodeValue;
    private TextMesh valueText = null;

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
        try
        {
            valueText = gameObject.GetComponentInChildren<TextMesh>();
            valueText.text = nodeValue.ToString();
        }
        catch
        {
            valueText = null;
        }
    }
}
