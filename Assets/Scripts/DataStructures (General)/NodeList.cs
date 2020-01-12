using UnityEngine;

public class NodeList : MonoBehaviour
{
    private int count;
    public GameObject head;
    private GameObject tail;
    public GameObject prefab;
    private int[] nodeValues;
    private float startingYCoord;
    public AnimationHelper animationHelper;

    void Start()
    {
        animationHelper = gameObject.GetComponent<AnimationHelper>();
    }

    /// <summary>
    /// Creates a new list of Nodes using values from the given array.
    /// </summary>
    /// <param name="newValuesArray"></param>
    public void InitializeWithArray(int[] newValuesArray)
    {
        nodeValues = newValuesArray;
        startingYCoord = transform.position.y;

        count = Mathf.Clamp(nodeValues.Length, 0, int.MaxValue);
        Vector3 newHeadPosition;

        // Create the nodes given the array of values.
        for (int nodesCreated = 0; nodesCreated < count; nodesCreated++)
        {
            newHeadPosition = new Vector3(2 * nodesCreated, startingYCoord, 0);
            GameObject newNode = Instantiate(prefab, newHeadPosition, Quaternion.identity);
            newNode.transform.parent = this.transform;
            newNode.GetComponent<Node>().UpdatePositions();
            newNode.GetComponent<Node>().nodeIndex = nodesCreated;
            newNode.GetComponent<Node>().nodeValue = nodeValues[nodesCreated];
            newNode.GetComponent<Node>().UpdateName();

            // Create the head if needed.
            if (nodesCreated == 0)
            {
                head = newNode;
                tail = head;
                continue;
            }

            // General case. Move the tail.
            tail.GetComponent<Node>().nextNode = newNode;
            newNode.GetComponent<Node>().prevNode = tail;
            tail = newNode;
        }
    }

    /// <summary>
    /// Returns the number of elements in the list.
    /// </summary>
    /// <returns></returns>
    public int Count()
    {
        return count;
    }

    public void Print()
    {
        Debug.Log(this.ToString());
    }

    public override string ToString()
    {
        string output = "";

        if (head == null)
        {
            Debug.Log("No elements in NodeList.");
            return "";
        }

        for (int i = 0; i < count; i++)
        {
            output += ValueAtIndex(i) + " ";
        }

        output.Trim();

        return output;
    }

    /// <summary>
    /// Returns a reference to the node at the given index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameObject NodeAtIndex(int index)
    {
        //Debug.Log("NodeAtIndex: " + index);
        if (index < 0 || index >= count)
        {
            return null;
        }

        GameObject runner = head;

        while (index > 0)
        {
            runner = runner.GetComponent<Node>().nextNode;
            index--;
        }

        return runner;
    }

    /// <summary>
    /// Returns the value of the node at the given index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int ValueAtIndex(int index)
    {
        //Debug.Log("ValueAtIndex: " + index);
        return NodeAtIndex(index).GetComponent<Node>().nodeValue;
    }

    /// <summary>
    /// Swaps the position of two nodes given their indices.
    /// </summary>
    /// <param name="nodeOneIndex"></param>
    /// <param name="nodeTwoIndex"></param>
    /// <returns>Returns success or failure of swap.</returns>
    public bool Swap(int nodeOneIndex, int nodeTwoIndex)
    {
        if (nodeOneIndex < 0 || nodeOneIndex >= count ||
            nodeTwoIndex < 0 || nodeTwoIndex >= count ||
            nodeOneIndex == nodeTwoIndex)
        {
            return false;
        }

        // Assign temp nodes so we don't lose referenences which can lead to memory leaks.
        GameObject nodeOne = NodeAtIndex(Mathf.Min(NodeAtIndex(nodeOneIndex).GetComponent<Node>().nodeIndex, NodeAtIndex(nodeTwoIndex).GetComponent<Node>().nodeIndex));
        GameObject nodeTwo = NodeAtIndex(Mathf.Max(NodeAtIndex(nodeOneIndex).GetComponent<Node>().nodeIndex, NodeAtIndex(nodeTwoIndex).GetComponent<Node>().nodeIndex));
        GameObject leftNodeOne, rightNodeOne, leftNodeTwo, rightNodeTwo;

        leftNodeOne = rightNodeOne = leftNodeTwo = rightNodeTwo = null;
        if (nodeOne.GetComponent<Node>().prevNode != null)
        {
            leftNodeOne = nodeOne.GetComponent<Node>().prevNode;
        }
        if (nodeOne.GetComponent<Node>().nextNode != null)
        {
            rightNodeOne = nodeOne.GetComponent<Node>().nextNode;
        }
        if (nodeTwo.GetComponent<Node>().prevNode != null)
        {
            leftNodeTwo = nodeTwo.GetComponent<Node>().prevNode;
        }
        if (nodeTwo.GetComponent<Node>().nextNode != null)
        {
            rightNodeTwo = nodeTwo.GetComponent<Node>().nextNode;
        }

        // Handle references that point away from the two nodes.
        nodeTwo.GetComponent<Node>().prevNode = leftNodeOne;
        nodeOne.GetComponent<Node>().nextNode = rightNodeTwo;


        // Handle references that point between and toward the two nodes.
        if (rightNodeOne == leftNodeTwo)
        {
            // Middle nodes are the same.
            //Debug.Log("r==l");
            rightNodeOne.GetComponent<Node>().nextNode = nodeOne;
            rightNodeOne.GetComponent<Node>().prevNode = nodeTwo;
            nodeTwo.GetComponent<Node>().nextNode = rightNodeOne;
            nodeOne.GetComponent<Node>().prevNode = leftNodeTwo;
        }
        else if (Mathf.Abs(nodeOneIndex - nodeTwoIndex) == 1)
        {
            // Nodes are adjacent.
            //Debug.Log("adjacent");
            nodeTwo.GetComponent<Node>().nextNode = nodeOne;
            nodeOne.GetComponent<Node>().prevNode = nodeTwo;
        }
        else
        {
            // General case.
            //Debug.Log("base");
            nodeTwo.GetComponent<Node>().nextNode = rightNodeOne;
            nodeOne.GetComponent<Node>().prevNode = leftNodeTwo;
            if (rightNodeOne != null)
            {
                rightNodeOne.GetComponent<Node>().prevNode = nodeTwo;
            }
            if (leftNodeTwo != null)
            {
                leftNodeTwo.GetComponent<Node>().nextNode = nodeOne;
            }
        }
        if (leftNodeOne != null)
        {
            leftNodeOne.GetComponent<Node>().nextNode = nodeTwo;
        }
        if (rightNodeTwo != null)
        {
            rightNodeTwo.GetComponent<Node>().prevNode = nodeOne;
        }

        // Swap indices.
        int tempIndex = nodeOne.GetComponent<Node>().nodeIndex;
        nodeOne.GetComponent<Node>().nodeIndex = nodeTwo.GetComponent<Node>().nodeIndex;
        nodeTwo.GetComponent<Node>().nodeIndex = tempIndex;

        // Update head if needed.
        if (nodeOne.GetComponent<Node>().nodeIndex == 0)
        {
            head = nodeOne;
        }
        if (nodeTwo.GetComponent<Node>().nodeIndex == 0)
        {
            head = nodeTwo;
        }

        return true;
    }

    /// <summary>
    /// Executes an animated swap.
    /// </summary>
    /// <param name="nodeOneIndex"></param>
    /// <param name="nodeTwoIndex"></param>
    public void AnimatedSwap(int nodeOneIndex, int nodeTwoIndex)
    {
        // Do the swap first since it has checks against bad input.
        if (!Swap(nodeOneIndex, nodeTwoIndex))
        {
            return;
        }

        // Get the nodes and call the animationHelper to start the animation.
        GameObject nodeOne = NodeAtIndex(Mathf.Max(NodeAtIndex(nodeOneIndex).GetComponent<Node>().nodeIndex, NodeAtIndex(nodeTwoIndex).GetComponent<Node>().nodeIndex));
        GameObject nodeTwo = NodeAtIndex(Mathf.Min(NodeAtIndex(nodeOneIndex).GetComponent<Node>().nodeIndex, NodeAtIndex(nodeTwoIndex).GetComponent<Node>().nodeIndex));

        animationHelper.FlagLerpSwap(nodeOne, nodeTwo);
    }

    /// <summary>
    /// Swaps the values of two nodes.
    /// </summary>
    /// <param name="nodeOneIndex"></param>
    /// <param name="nodeTwoIndex"></param>
    public void SwapValues(int nodeOneIndex, int nodeTwoIndex)
    {
        if (nodeOneIndex < 0 || nodeOneIndex >= count ||
            nodeTwoIndex < 0 || nodeTwoIndex >= count ||
            nodeOneIndex == nodeTwoIndex)
        {
            return;
        }

        GameObject nodeOne = NodeAtIndex(nodeOneIndex);
        GameObject nodeTwo = NodeAtIndex(nodeTwoIndex);
        int tempVal;

        tempVal = nodeOne.GetComponent<Node>().nodeValue;
        nodeOne.GetComponent<Node>().nodeValue = nodeTwo.GetComponent<Node>().nodeValue;
        nodeTwo.GetComponent<Node>().nodeValue = tempVal;

        // Update node names.
        nodeOne.GetComponent<Node>().UpdateName();
        nodeTwo.GetComponent<Node>().UpdateName();
    }
}
