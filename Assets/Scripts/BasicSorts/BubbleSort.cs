using System.Collections;
using UnityEngine;

public class BubbleSort : MonoBehaviour
{
    public int[] theArray;
    public NodeList theList;
    public bool isUsingManualStepping = true;
    public bool isCoroutineIsActive = false;
    public bool isReadyForInput = false;
    public bool canExecuteStep = false;

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
                    int swapIndex = leftNode.GetComponent<Node>().nodeIndex;
                    theList.Swap(leftNode.GetComponent<Node>().nodeIndex, runner.GetComponent<Node>().nodeIndex);
                    theList.Print();
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

        if (theList.Count() < 2)
        {
            yield return null;
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
                    int swapIndex = leftNode.GetComponent<Node>().nodeIndex;
                    theList.AnimatedSwap(leftNode.GetComponent<Node>().nodeIndex, runner.GetComponent<Node>().nodeIndex);
                    theList.Print();

                    // Wait until the animation has finished to continue the loop.
                    yield return new WaitUntil(() => theList.animationHelper.isLerpSwapping() == false);
                    isReadyForInput = true;
                }

                leftNode = runner;
            }

            // Reset temp variables.
            leftNode = null;
        }

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
