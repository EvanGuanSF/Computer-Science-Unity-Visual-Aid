using System.Collections;
using UnityEngine;

public class NodeList : MonoBehaviour
{
    public GameObject nodePrefab;
    public SwapAnimationHelper swapAnimationHelper;
    public ShiftAnimationHelper shiftAnimationHelper;
    public InsertAnimationHelper insertAnimationHelper;

    private int count;
    private int[] nodeValues;
    private float startingYCoord;
    private TextMesh nodeListName = null;

    public GameObject head;
    public GameObject tail;

    void Start()
    {
        swapAnimationHelper = gameObject.GetComponent<SwapAnimationHelper>();
        shiftAnimationHelper = gameObject.GetComponent<ShiftAnimationHelper>();
        insertAnimationHelper = gameObject.GetComponent<InsertAnimationHelper>();
    }

    /// <summary>
    /// Creates a new list of Nodes using values from the given array.
    /// </summary>
    /// <param name="newValuesArray"></param>
    public void InitializeWithArray(int[] newValuesArray)
    {
        nodeValues = newValuesArray;
        startingYCoord = transform.position.y;

        if(nodeValues == null)
        {
            head = tail = null;
            return;
        }

        count = Mathf.Clamp(nodeValues.Length, 0, int.MaxValue);
        Vector3 newHeadPosition;

        // Create the nodes given the array of values.
        for (int nodesCreated = 0; nodesCreated < count; nodesCreated++)
        {
            newHeadPosition = new Vector3(2 * nodesCreated, startingYCoord, 0);
            GameObject newNode = Instantiate(nodePrefab, newHeadPosition, Quaternion.identity);
            newNode.transform.parent = transform;
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

        // Handle the position of the title of the list if needed.
        try
        {
            nodeListName = gameObject.GetComponentInChildren<TextMesh>();
            nodeListName.transform.position = new Vector3(count, startingYCoord + 3f, 0.0f);
        }
        catch
        {
            nodeListName = null;
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
        else if (nodeTwo.GetComponent<Node>().nodeIndex == 0)
        {
            head = nodeTwo;
        }

        // Update tail if needed.
        if (nodeOne.GetComponent<Node>().nodeIndex == count - 1 || count == 1)
        {
            tail = nodeOne;
        }
        else if (nodeTwo.GetComponent<Node>().nodeIndex == count - 1 || count == 1)
        {
            tail = nodeTwo;
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

        swapAnimationHelper.FlagLerpSwap(nodeOne, nodeTwo);
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

    /// <summary>
    /// Insert a new node at the given index with given value.
    /// </summary>
    /// <param name="newNodeIndex"></param>
    /// <param name="newNodeValue"></param>
    public void InsertAtIndex(int newNodeIndex, int newNodeValue)
    {
        // return if the index given is out of range.
        if (newNodeIndex < 0 || newNodeIndex > count)
        {
            Debug.Log("Index out of range for insert.");
            return;
        }

        Vector3 newNodePosition = new Vector3(2 * newNodeIndex, startingYCoord, 0);
        GameObject newNode = Instantiate(nodePrefab, newNodePosition, Quaternion.identity);
        newNode.transform.parent = transform;
        newNode.GetComponent<Node>().UpdatePositions();
        newNode.GetComponent<Node>().nodeIndex = newNodeIndex;
        newNode.GetComponent<Node>().nodeValue = newNodeValue;
        newNode.GetComponent<Node>().UpdateName();

        if (head == null)
        {
            // If we have an empty list.
            head = newNode;
            tail = newNode;
        }

        if (newNodeIndex == 0)
        {
            // If the node is being inserted at the front of the list.
            head.GetComponent<Node>().prevNode = newNode;
            newNode.GetComponent<Node>().nextNode = head;
            head = newNode;
        }
        else if (newNodeIndex == count)
        {
            // If the node being inserted is at the back of the list.
            tail.GetComponent<Node>().nextNode = newNode;
            newNode.GetComponent<Node>().prevNode = tail;
            tail = newNode;
        }
        else
        {
            // General case. Insert between two nodes.
            GameObject leftNode = NodeAtIndex(newNodeIndex - 1);
            GameObject rightNode = NodeAtIndex(newNodeIndex + 1);

            if (leftNode != null)
            {
                leftNode.GetComponent<Node>().nextNode = newNode;
            }
            newNode.GetComponent<Node>().prevNode = leftNode;

            if (rightNode != null)
            {
                rightNode.GetComponent<Node>().prevNode = newNode;
            }
            newNode.GetComponent<Node>().nextNode = rightNode;

        }

        // Update the indices of the remaining nodes.
        ShiftIndices(newNodeIndex + 1, 1);

        count++;

        // Start the animation to reposition and reindex the remaining nodes following the new node in the list.
        shiftAnimationHelper.FlagNodeShift(newNodeIndex + 1, true);
    }

    /// <summary>
    /// Insert an existing node at the index. The node at the exisiting index position and
    /// any subsequent nodes have their indices incremented by one and are shifted one space to the right.
    /// </summary>
    /// <param name="nodeToMove"></param>
    public void InsertAtIndex(int newNodeIndex, GameObject nodeToMove)
    {
        // return if the index given is out of range.
        if (newNodeIndex < 0 || newNodeIndex > count)
        {
            Debug.Log("Index out of range for insert.");
            return;
        }

        nodeToMove.GetComponent<Node>().nodeIndex = newNodeIndex;

        Vector3 newNodeFinalPosition = new Vector3(2 * newNodeIndex, startingYCoord, 0);
        GameObject newNode = nodeToMove;
        newNode.transform.parent = transform;
        newNode.GetComponent<Node>().nodeIndex = newNodeIndex;

        if (head == null)
        {
            // If we have an empty list.
            head = newNode;
            tail = newNode;
        }
        else if (newNodeIndex == 0)
        {
            // If the node is being inserted at the front of the list.
            head.GetComponent<Node>().prevNode = newNode;
            newNode.GetComponent<Node>().nextNode = head;
            head = newNode;
        }
        else if (newNodeIndex == count - 1)
        {
            // If the node being inserted is at the back of the list.
            tail.GetComponent<Node>().nextNode = newNode;
            newNode.GetComponent<Node>().prevNode = tail;
            tail = newNode;
        }
        else
        {
            // General case. Insert between two nodes.
            GameObject leftNode = NodeAtIndex(newNodeIndex - 1);
            GameObject rightNode = NodeAtIndex(newNodeIndex);

            if (leftNode != null)
            {
                leftNode.GetComponent<Node>().nextNode = newNode;
            }
            newNode.GetComponent<Node>().prevNode = leftNode;

            if (rightNode != null)
            {
                rightNode.GetComponent<Node>().prevNode = newNode;
            }
            newNode.GetComponent<Node>().nextNode = rightNode;

        }

        // Update the indices of the remaining nodes.
        ShiftIndices(newNodeIndex + 1, 1);

        count++;

        // Start the animation to move the node to its final position.
        insertAnimationHelper.FlagLerpInsertion(nodeToMove, newNodeFinalPosition);
        // Start the animation to reposition and reindex the remaining nodes following the new node in the list.
        shiftAnimationHelper.FlagNodeShift(newNodeIndex + 1, true);
    }

    /// <summary>
    /// Removes the node at the given index and returns it.
    /// </summary>
    /// <param name="nodeIndex"></param>
    /// <returns></returns>
    public GameObject RemoveAtIndex(int nodeIndex)
    {
        // return if the index given is out of range.
        if (nodeIndex < 0 || nodeIndex >= count)
        {
            Debug.Log("Index out of range for insert.");
            return null;
        }

        GameObject nodeToRemove = NodeAtIndex(nodeIndex);

        if (nodeIndex == 0)
        {
            // If the node is being removed from the front of the list.
            head = nodeToRemove.GetComponent<Node>().nextNode;
            if(head != null)
            {
                head.GetComponent<Node>().prevNode = null;
            }
            else
            {
                // If the head is null, then the list is empty and the tail needs to be set as well.
                tail = null;
            }
        }
        else if (nodeIndex == count - 1)
        {
            // If the node being removed is at the back of the list.
            tail = nodeToRemove.GetComponent<Node>().prevNode;
            tail.GetComponent<Node>().nextNode = null;
        }
        else
        {
            // General case. Remove from between two nodes and "re-stitch" the left and right nodes.
            GameObject leftNode = NodeAtIndex(nodeIndex - 1);
            GameObject rightNode = NodeAtIndex(nodeIndex + 1);

            if (leftNode != null)
            {
                leftNode.GetComponent<Node>().nextNode = rightNode;
            }
            if (rightNode != null)
            {
                rightNode.GetComponent<Node>().prevNode = leftNode;
            }

        }

        // Reset the Node references and indexValue of the node being removed.
        nodeToRemove.GetComponent<Node>().prevNode = null;
        nodeToRemove.GetComponent<Node>().nextNode = null;
        nodeToRemove.GetComponent<Node>().nodeIndex = -1;

        // Update the indices of the remaining nodes.
        ShiftIndices(nodeIndex, -1);

        count--;

        // Animate node shift.
        shiftAnimationHelper.FlagNodeShift(nodeIndex, false);

        return nodeToRemove;
    }

    /// <summary>
    /// Deletes the Node GameObject at the given index from the scene.
    /// </summary>
    public void DeleteAtIndex(int nodeIndex)
    {
        GameObject temp = RemoveAtIndex(nodeIndex);
        if(temp != null)
        {
            Destroy(temp);
        }
    }

    /// <summary>
    /// Increments the index by one for the node at given index and every nextNode in the NodeList.
    /// </summary>
    /// <param name="shiftFromIndex"></param>
    private void ShiftIndices(int shiftFromIndex, int shiftAmount)
    {
        GameObject workingNode = NodeAtIndex(shiftFromIndex);

        if (workingNode == null)
        {
            return;
        }

        while (workingNode != null)
        {
            // Update the index and position of the nodes being shifted.
            workingNode.GetComponent<Node>().nodeIndex += shiftAmount;

            // Continue the loop.
            workingNode = workingNode.GetComponent<Node>().nextNode;
        }
    }
}
