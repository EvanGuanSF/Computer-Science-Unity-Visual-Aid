using System.Collections;
using UnityEngine;

public class SelectionSort : MonoBehaviour
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
                theList.Print();
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
                int swapIndex = lowestValueNode.GetComponent<Node>().nodeIndex;
                theList.AnimatedSwap(i, swapIndex);
                theList.Print();

                // Wait until the animation has finished to continue the loop.
                yield return new WaitUntil(() => theList.animationHelper.isLerpSwapping() == false);
                isReadyForInput = true;
            }

            // Reset temp variables.
            lowestValueNode = null;
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
