using System.Collections;
using UnityEngine;

public class InsertionSort : MonoBehaviour
{
    [Header("Node References")]
    public int[] theArray;
    public NodeList theList;

    [Header("Operation Booleans")]
    public bool isUsingManualStepping = true;
    public bool isCoroutineIsActive = false;
    public bool isReadyForInput = false;
    public bool canExecuteStep = false;

    [Header("Array of Node Materials")]
    public Material[] materials = new Material[5];

    [Header("Sorting Options")]
    public float timeBetweenComparisons = 0.25f;

    /// <summary>
    /// Initialize the list with the values provided in the editor.
    /// </summary>
    public void InitializeList()
    {
        if (theList != null)
        {
            theList.InitializeWithArray(theArray);
            isReadyForInput = true;
        }
    }

    /// <summary>
    /// Sorts the list of Nodes.
    /// </summary>
    public void Sort()
    {
        GameObject runner = null, currentNode = null;
        int currentNodeValue = int.MaxValue;

        // Execute the sorting algorithm. O(n^2)
        for (int i = 0; i < theList.Count(); i++)
        {
            // Current node = theList[i]
            currentNode = theList.GetComponent<NodeList>().NodeAtIndex(i);
            currentNodeValue = currentNode.GetComponent<Node>().nodeValue;
            // Runner node = theList[i - 1]
            runner = currentNode.GetComponent<Node>().prevNode;

            while (runner != null)
            {
                if (currentNodeValue < runner.GetComponent<Node>().nodeValue)
                {
                    // If the current node's value is less than the runner;s, then we keep going.
                    // runnerNode = theList[runnerNode.Index - 1]
                    runner = runner.GetComponent<Node>().prevNode;
                }
                else
                {
                    // If the current node's value is greater than the runner's,
                    // remove the node and place it at the index after the runner's.
                    theList.RemoveAtIndex(i);
                    theList.InsertAtIndex(runner.GetComponent<Node>().nodeIndex + 1, currentNode);

                    break;
                }

            }

            // If we get here, then that means the current value is the smallest in the sorted list.
            // Insert at the head.
            if (runner == null)
            {
                currentNode = theList.RemoveAtIndex(i);
                theList.InsertAtIndex(0, currentNode);
            }

            // Reset temp variables.
            currentNode = runner = null;
            currentNodeValue = int.MaxValue;

            Debug.Log("Insertion sort: " + theList.ToString());
        }
    }

    /// <summary>
    /// Handler for the AnimatedSortCoroutine.
    /// </summary>
    public void AnimatedSort()
    {
        if (!isCoroutineIsActive)
        {
            isCoroutineIsActive = true;
            StartCoroutine(AnimatedSortCoroutine());
        }
    }

    /// <summary>
    /// Sorts the list of Nodes with animations asynchrnously with yields.
    /// </summary>
    /// <returns></returns>
    private IEnumerator AnimatedSortCoroutine()
    {
        GameObject runner = null, currentNode = null;
        int currentNodeValue = int.MaxValue;
        int comparisonCount = 0;

        // Execute the sorting algorithm. O(n^2)
        for (int i = 0; i < theList.Count(); i++)
        {
            // Current node = theList[i]
            currentNode = theList.GetComponent<NodeList>().NodeAtIndex(i);
            currentNodeValue = currentNode.GetComponent<Node>().nodeValue;
            // Runner node = theList[i - 1]
            runner = currentNode.GetComponent<Node>().prevNode;

            yield return new WaitForSeconds(timeBetweenComparisons);
            currentNode.GetComponent<Renderer>().material = materials[1];

            while (runner != null)
            {
                yield return new WaitForSeconds(timeBetweenComparisons);
                runner.GetComponent<Renderer>().material = materials[2];

                if (currentNodeValue < runner.GetComponent<Node>().nodeValue)
                {
                    yield return new WaitForSeconds(timeBetweenComparisons);
                    runner.GetComponent<Renderer>().material = materials[3];

                    // If the current node's value is less than the runner;s, then we keep going.
                    // runnerNode = theList[runnerNode.Index - 1]
                    runner = runner.GetComponent<Node>().prevNode;
                }
                else
                {
                    runner.GetComponent<Renderer>().material = materials[4];
                    yield return new WaitForSeconds(timeBetweenComparisons);

                    // If on manual mode, flag so that we wait until the animation is done before we accept input for the next step.
                    canExecuteStep = false;

                    // If we are using manual stepping, wait until we get input to execute before executing swaps.
                    if (isUsingManualStepping && !canExecuteStep)
                    {
                        yield return new WaitUntil(() => canExecuteStep == true);
                        canExecuteStep = false;
                    }
                    isReadyForInput = false;

                    // If the current node's value is greater than the runner's,
                    // remove the node and place it at the index after the runner's.
                    if (currentNode.GetComponent<Node>().nodeIndex - 1 != runner.GetComponent<Node>().nodeIndex)
                    {
                        // Only perform the animated removal and insert if the element being inserted needs to be moved.
                        theList.AnimatedRemoveAtIndex(i);
                        theList.AnimatedInsertAtIndex(runner.GetComponent<Node>().nodeIndex + 1, currentNode);
                    }
                    else
                    {
                        theList.RemoveAtIndex(i);
                        theList.InsertAtIndex(runner.GetComponent<Node>().nodeIndex + 1, currentNode);
                    }

                    // Wait for any active animations to finish.
                    yield return new WaitUntil(() => theList.shiftAnimationHelper.isLerpShifting() == false);
                    yield return new WaitUntil(() => theList.insertAnimationHelper.isLerpInserting() == false);
                    isReadyForInput = true;

                    runner.GetComponent<Renderer>().material = materials[3];

                    break;
                }

                comparisonCount++;
            }

            // If we get here, then that means the current value is the smallest in the sorted list.
            // Insert at the head.
            if (runner == null)
            {
                GameObject tempNode = theList.GetComponent<NodeList>().NodeAtIndex(0);
                tempNode.GetComponent<Renderer>().material = materials[4];
                yield return new WaitForSeconds(timeBetweenComparisons);

                // If on manual mode, flag so that we wait until the animation is done before we accept input for the next step.
                canExecuteStep = false;

                // If we are using manual stepping, wait until we get input to execute before executing swaps.
                if (isUsingManualStepping && !canExecuteStep)
                {
                    yield return new WaitUntil(() => canExecuteStep == true);
                    canExecuteStep = false;
                }
                isReadyForInput = false;

                if (i != 0)
                {
                    // Only perform the animated removal and insert if the element being inserted isn't the first element being sorted.
                    currentNode = theList.AnimatedRemoveAtIndex(i);
                    theList.AnimatedInsertAtIndex(0, currentNode);
                }
                else
                {
                    currentNode = theList.RemoveAtIndex(i);
                    theList.InsertAtIndex(0, currentNode);
                }

                // Wait for any active animations to finish.
                yield return new WaitUntil(() => theList.insertAnimationHelper.isLerpInserting() == false);
                yield return new WaitUntil(() => theList.shiftAnimationHelper.isLerpShifting() == false);
                isReadyForInput = true;

                tempNode.GetComponent<Renderer>().material = materials[3];
                currentNode.GetComponent<Renderer>().material = materials[3];
            }
            else
            {
                currentNode.GetComponent<Renderer>().material = materials[3];
            }

            // Reset temp variables.
            currentNode = runner = null;
            currentNodeValue = int.MaxValue;

            //Debug.Log("Insertion sort: " + theList.ToString());
        }

        // Change the material to indicate sorting has finished.
        theList.GetComponent<NodeList>().NodeAtIndex(theList.Count() - 1).GetComponent<Renderer>().material = materials[3];

        Debug.Log("Insertion sort comparisons: " + comparisonCount);

        isCoroutineIsActive = false;
    }

    public void ExecuteSortStep()
    {
        if (isUsingManualStepping && isReadyForInput)
        {
            canExecuteStep = true;
        }
    }
}
