using System.Collections;
using UnityEngine;

public class SelectionSort : MonoBehaviour
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
    public Material[] materials = new Material[4];

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
        }
    }

    /// <summary>
    /// Sorts the list of Nodes.
    /// </summary>
    public void Sort()
    {
        GameObject runner = null, lowestValueNode = null;
        int currentLowestValue = int.MaxValue;

        // Execute the sorting algorithm. O(n^2)
        for (int i = 0; i < theList.Count(); i++)
        {
            currentLowestValue = theList.GetComponent<NodeList>().NodeAtIndex(i).GetComponent<Node>().nodeValue;
            for (int j = i + 1; j < theList.Count(); j++)
            {
                runner = theList.GetComponent<NodeList>().NodeAtIndex(j);

                if (runner.GetComponent<Node>().nodeValue < currentLowestValue)
                {
                    lowestValueNode = runner;
                    currentLowestValue = runner.GetComponent<Node>().nodeValue;
                }
            }

            // If the temp node was assigned a reference, then we need to swap.
            if (lowestValueNode != null)
            {
                int swapIndex = lowestValueNode.GetComponent<Node>().nodeIndex;
                theList.Swap(i, swapIndex);
                Debug.Log("Selection sort: " + theList.ToString());
            }

            // Reset temp variables.
            lowestValueNode = null;
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
        GameObject runner = null, lowestValueNode = null, initialNode = null;
        int numSwaps = 0;

        // Execute the sorting algorithm. O(n^2)
        for (int i = 0; i < theList.Count() - 1; i++)
        {
            yield return new WaitForSeconds(timeBetweenComparisons);
            initialNode = lowestValueNode = theList.GetComponent<NodeList>().NodeAtIndex(i);
            lowestValueNode.GetComponent<Renderer>().material = materials[1];

            for (int j = i + 1; j < theList.Count(); j++)
            {
                yield return new WaitForSeconds(timeBetweenComparisons);
                runner = theList.GetComponent<NodeList>().NodeAtIndex(j);
                runner.GetComponent<Renderer>().material = materials[2];

                yield return new WaitForSeconds(timeBetweenComparisons);
                if (runner.GetComponent<Node>().nodeValue < lowestValueNode.GetComponent<Node>().nodeValue)
                {
                    lowestValueNode.GetComponent<Renderer>().material = materials[0];
                    initialNode.GetComponent<Renderer>().material = materials[4];
                    lowestValueNode = runner;
                    lowestValueNode.GetComponent<Renderer>().material = materials[1];
                }
                else
                {
                    runner.GetComponent<Renderer>().material = materials[0];
                }
            }

            // If the lowest value node has a different index than the index i, then we need to swap.
            if (lowestValueNode.GetComponent<Node>().nodeIndex != i)
            {
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

                // Send the signal to swap and animate the Nodes.
                numSwaps++;
                int swapIndex = lowestValueNode.GetComponent<Node>().nodeIndex;
                theList.AnimatedSwap(i, swapIndex);
                Debug.Log("Selection sort: " + theList.ToString());

                // Wait until the animation has finished to continue the loop.
                yield return new WaitUntil(() => theList.animationHelper.isLerpSwapping() == false);
                isReadyForInput = true;

                lowestValueNode.GetComponent<Renderer>().material = materials[0];
                initialNode.GetComponent<Renderer>().material = materials[0];
            }
            // Change the material to indicate sorting has finished.
            runner.GetComponent<Renderer>().material = materials[0];
            theList.GetComponent<NodeList>().NodeAtIndex(i).GetComponent<Renderer>().material = materials[3];
        }

        // Change the material to indicate sorting has finished.
        theList.GetComponent<NodeList>().NodeAtIndex(theList.Count() - 1).GetComponent<Renderer>().material = materials[3];

        Debug.Log("Selection sort swaps: " + numSwaps);

        isCoroutineIsActive = false;
    }

    public void ExecuteSortStep()
    {
        if (isUsingManualStepping && !canExecuteStep)
        {
            canExecuteStep = true;
        }
    }
}
