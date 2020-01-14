using UnityEngine;

public class SwapAnimationHelper : MonoBehaviour
{
    public float baseLerpSpeed = 3.0f;
    private float normalizedLerpSpeed;
    private bool isAnimatingSwap = false;
    private bool isMovingAwayFromVertical = false, isMovingToFinalHorizontal = false, isMovingTowardVertical = false;
    private float lerpDistance;
    private float lerpStartTime;
    GameObject nodeOne, nodeTwo;
    Vector3 nodeOneInitialPosition, nodeTwoInitialPosition, nodeOneTempPositionOne, nodeOneTempPositionTwo, nodeTwoTempPositionOne, nodeTwoTempPositionTwo;

    void Update()
    {
        // Handle the lerping of swapping objects.
        if (isAnimatingSwap)
        {
            HandleLerpSwap();
        }
    }

    /// <summary>
    /// Swaps the positions of two game objects in a straight line.
    /// </summary>
    /// <param name="objectOne"></param>
    /// <param name="objectTwo"></param>
    public void FlagLerpSwap(GameObject objectOne, GameObject objectTwo)
    {
        if (isAnimatingSwap)
        {
            return;
        }

        lerpStartTime = Time.time;

        // Set variable data for FixedUpdate calculations.
        nodeOne = objectOne;
        nodeTwo = objectTwo;

        nodeOneInitialPosition = new Vector3(nodeOne.transform.position.x, nodeOne.transform.position.y, nodeOne.transform.position.z);
        nodeTwoInitialPosition = new Vector3(nodeTwo.transform.position.x, nodeTwo.transform.position.y, nodeTwo.transform.position.z);

        // Calculate the distance between the objects that will be lerped.
        // Update the lerp speed to normalize.
        lerpDistance = (nodeOneInitialPosition - nodeOneTempPositionOne).magnitude;
        normalizedLerpSpeed = lerpDistance * baseLerpSpeed;

        // Set the positions of the checkpoints that will be used.
        nodeOneTempPositionOne = new Vector3(nodeOneInitialPosition.x, nodeOneInitialPosition.y + 1.25f, nodeOneInitialPosition.z);
        nodeTwoTempPositionOne = new Vector3(nodeTwoInitialPosition.x, nodeTwoInitialPosition.y - 1.25f, nodeTwoInitialPosition.z);
        nodeOneTempPositionTwo = new Vector3(nodeTwoInitialPosition.x, nodeOneTempPositionOne.y, nodeOneTempPositionOne.z);
        nodeTwoTempPositionTwo = new Vector3(nodeOneInitialPosition.x, nodeTwoTempPositionOne.y, nodeTwoTempPositionOne.z);

        isMovingAwayFromVertical = true;
        isAnimatingSwap = true;
    }

    /// <summary>
    /// Handles the updating of the objects being swapped.
    /// </summary>
    private void HandleLerpSwap()
    {
        if (isMovingAwayFromVertical)
        {
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - lerpStartTime) * normalizedLerpSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / lerpDistance;

            if (fractionOfJourney < 1.0f)
            {
                // Set our position as a fraction of the distance between the markers.
                // Handle object one.
                nodeOne.transform.position = Vector3.Lerp(
                    nodeOneInitialPosition,
                    nodeOneTempPositionOne,
                    fractionOfJourney);
                // Handle object two.
                nodeTwo.transform.position = Vector3.Lerp(
                    nodeTwoInitialPosition,
                    nodeTwoTempPositionOne,
                    fractionOfJourney);
            }
            else
            {
                lerpStartTime = Time.time;

                // Calculate the distance between the objects that will be lerped.
                // Update the lerp speed to normalize.
                lerpDistance = (nodeOneTempPositionOne - nodeOneTempPositionTwo).magnitude;
                normalizedLerpSpeed = lerpDistance * baseLerpSpeed;

                // Manually move nodes to final positions to account for any timing deviations.
                nodeOne.transform.position = nodeOneTempPositionOne;
                nodeTwo.transform.position = nodeTwoTempPositionOne;

                isMovingToFinalHorizontal = true;
                isMovingAwayFromVertical = false;
            }
        }
        else if (isMovingToFinalHorizontal)
        {
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - lerpStartTime) * normalizedLerpSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / lerpDistance;

            if (fractionOfJourney < 1.0f)
            {
                // Set our position as a fraction of the distance between the markers.
                // Handle object one.
                nodeOne.transform.position = Vector3.Lerp(
                    nodeOneTempPositionOne,
                    nodeOneTempPositionTwo,
                    fractionOfJourney);
                // Handle object two.
                nodeTwo.transform.position = Vector3.Lerp(
                    nodeTwoTempPositionOne,
                    nodeTwoTempPositionTwo,
                    fractionOfJourney);
            }
            else
            {
                lerpStartTime = Time.time;

                // Calculate the distance between the objects that will be lerped.
                // Update the lerp speed to normalize.
                lerpDistance = (nodeOneTempPositionTwo - nodeTwoInitialPosition).magnitude;
                normalizedLerpSpeed = lerpDistance * baseLerpSpeed;

                // Manually move nodes to final positions to account for any timing deviations.
                nodeOne.transform.position = nodeOneTempPositionTwo;
                nodeTwo.transform.position = nodeTwoTempPositionTwo;

                isMovingTowardVertical = true;
                isMovingToFinalHorizontal = false;
            }
        }
        else if (isMovingTowardVertical)
        {
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - lerpStartTime) * normalizedLerpSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / lerpDistance;

            if (fractionOfJourney < 1.0f)
            {
                // Set our position as a fraction of the distance between the markers.
                // Handle object one.
                nodeOne.transform.position = Vector3.Lerp(
                    nodeOneTempPositionTwo,
                    nodeTwoInitialPosition,
                    fractionOfJourney);
                // Handle object two.
                nodeTwo.transform.position = Vector3.Lerp(
                    nodeTwoTempPositionTwo,
                    nodeOneInitialPosition,
                    fractionOfJourney);
            }
            else
            {
                lerpStartTime = Time.time;

                // Manually move nodes to final positions to account for any timing deviations.
                nodeOne.transform.position = nodeTwoInitialPosition;
                nodeTwo.transform.position = nodeOneInitialPosition;

                isMovingTowardVertical = false;
            }
        }
        else
        {
            //Debug.Log("Done animating lerp.");
            // Flag and reset variables when we finish lerping.
            isAnimatingSwap = false;
            nodeOne = nodeTwo = null;
            isMovingAwayFromVertical = isMovingToFinalHorizontal = isMovingTowardVertical = false;
            nodeOneInitialPosition = nodeTwoInitialPosition = Vector3.zero;
            nodeOneTempPositionOne = nodeOneTempPositionTwo = Vector3.zero;
            nodeTwoTempPositionOne = nodeTwoTempPositionTwo = Vector3.zero;
            lerpDistance = 0.0f;
        }
    }

    /// <summary>
    /// Returns a boolean indicating the state of object position swapping.
    /// </summary>
    /// <returns></returns>
    public bool isLerpSwapping()
    {
        return isAnimatingSwap;
    }
}
