using System.Collections;
using UnityEngine;

public class BubbleSort : MonoBehaviour
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
        GameObject runner = null, leftNode = null;

        if(theList.Count() < 2)
        {
            return;
        }

        // Execute the sorting algorithm. O(n^2)
        for (int i = theList.Count() - 1; i > 0; i--)
        {
            leftNode = theList.GetComponent<NodeList>().NodeAtIndex(0);

            for (int j = 1; j <= i; j++)
            {
                runner = theList.GetComponent<NodeList>().NodeAtIndex(j);
                
                // Bubble sort swaps on every instance of i+1 > i.
                if (leftNode.GetComponent<Node>().nodeValue > runner.GetComponent<Node>().nodeValue)
                {
                    theList.Swap(leftNode.GetComponent<Node>().nodeIndex, runner.GetComponent<Node>().nodeIndex);
                    Debug.Log("Bubble sort: " + theList.ToString());
                }

                leftNode = runner;
            }

            // Reset temp variables.
            leftNode = null;
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
        GameObject runner = null, leftNode = null;
        int numSwaps = 0;

        if (theList.Count() < 2)
        {
            yield return null;
        }

        // Execute the sorting algorithm. O(n^2)
        for (int i = theList.Count() - 1; i > 0; i--)
        {
            yield return new WaitForSeconds(timeBetweenComparisons);
            leftNode = theList.GetComponent<NodeList>().NodeAtIndex(0);

            for (int j = 1; j <= i; j++)
            {
                runner = theList.GetComponent<NodeList>().NodeAtIndex(j);
                leftNode.GetComponent<Renderer>().material = materials[1];
                runner.GetComponent<Renderer>().material = materials[2];

                // Bubble sort swaps on every instance of i+1 > i.
                if (leftNode.GetComponent<Node>().nodeValue > runner.GetComponent<Node>().nodeValue)
                {
                    yield return new WaitForSeconds(timeBetweenComparisons);
                    runner.GetComponent<Renderer>().material = materials[4];

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
                    theList.AnimatedSwap(leftNode.GetComponent<Node>().nodeIndex, runner.GetComponent<Node>().nodeIndex);
                    Debug.Log("Bubble sort: " + theList.ToString());

                    // Wait until the animation has finished to continue the loop.
                    yield return new WaitUntil(() => theList.swapAnimationHelper.isLerpSwapping() == false);
                    isReadyForInput = true;

                    leftNode.GetComponent<Renderer>().material = materials[0];
                    runner.GetComponent<Renderer>().material = materials[0];
                }
                
                yield return new WaitForSeconds(timeBetweenComparisons);
                leftNode.GetComponent<Renderer>().material = materials[0];
                leftNode = theList.GetComponent<NodeList>().NodeAtIndex(j);
            }

            // Retexture Nodes.
            runner.GetComponent<Renderer>().material = materials[0];
            leftNode.GetComponent<Renderer>().material = materials[0];
            theList.GetComponent<NodeList>().NodeAtIndex(i).GetComponent<Renderer>().material = materials[3];
            // Reset temp variables.
            leftNode = null;
        }

        // Change the material to indicate sorting has finished.
        theList.GetComponent<NodeList>().NodeAtIndex(0).GetComponent<Renderer>().material = materials[3];

        Debug.Log("Bubble sort swaps: " + numSwaps);

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
