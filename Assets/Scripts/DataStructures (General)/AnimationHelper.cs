using UnityEngine;

public class AnimationHelper : MonoBehaviour
{
    public float lerpSpeed = 3.0f;
    private bool isAnimatingSwap = false;
    private bool isMovingAwayFromVertical = false, isMovingToFinalHorizontal = false, isMovingTowardVertical = false;
    private float lerpDistance;
    private float lerpStartTime;

    GameObject nodeOne, nodeTwo;
    Vector3 nodeOneInitialPosition, nodeTwoInitialPosition, nodeOneTempPositionOne, nodeOneTempPositionTwo, nodeTwoTempPositionOne, nodeTwoTempPositionTwo;

    void FixedUpdate()
    {
        // Handle the lerping of swapping objects.
        HandleLerpSwap();
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
        lerpDistance = (nodeOneInitialPosition - nodeTwoInitialPosition).magnitude;

        isMovingAwayFromVertical = true;
        isAnimatingSwap = true;

        // Set the positions of the checkpoints that will be used.
        nodeOneTempPositionOne = new Vector3(nodeOneInitialPosition.x, nodeOneInitialPosition.y + 2, nodeOneInitialPosition.z);
        nodeTwoTempPositionOne = new Vector3(nodeTwoInitialPosition.x, nodeTwoInitialPosition.y - 2, nodeTwoInitialPosition.z);
        nodeOneTempPositionTwo = new Vector3(nodeTwoInitialPosition.x, nodeOneTempPositionOne.y, nodeOneTempPositionOne.z);
        nodeTwoTempPositionTwo = new Vector3(nodeOneInitialPosition.x, nodeTwoTempPositionOne.y, nodeTwoTempPositionOne.z);
    }

    /// <summary>
    /// Handles the updating of the objects being lerped.
    /// </summary>
    public void HandleLerpSwap()
    {
        if (isAnimatingSwap)
        {
            if (isMovingAwayFromVertical)
            {
                // Distance moved equals elapsed time times speed..
                float distCovered = (Time.time - lerpStartTime) * lerpSpeed;

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
                    isMovingToFinalHorizontal = true;
                    isMovingAwayFromVertical = false;
                    lerpStartTime = Time.time;
                }
            }
            else if (isMovingToFinalHorizontal)
            {
                // Distance moved equals elapsed time times speed..
                float distCovered = (Time.time - lerpStartTime) * lerpSpeed;

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
                    isMovingTowardVertical = true;
                    isMovingToFinalHorizontal = false;
                    lerpStartTime = Time.time;
                }
            }
            else if (isMovingTowardVertical)
            {
                // Distance moved equals elapsed time times speed..
                float distCovered = (Time.time - lerpStartTime) * lerpSpeed;

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
                    isMovingTowardVertical = false;
                    lerpStartTime = Time.time;
                }
            }
            else
            {
                //Debug.Log("Done animating lerp.");
                // Flag and set variables to stop lerping.
                isAnimatingSwap = false;
                nodeOne = nodeTwo = null;
                isMovingAwayFromVertical = isMovingToFinalHorizontal = isMovingTowardVertical = false;
                nodeOneInitialPosition = nodeTwoInitialPosition = Vector3.zero;
                nodeOneTempPositionOne = nodeOneTempPositionTwo = Vector3.zero;
                nodeTwoTempPositionOne = nodeTwoTempPositionTwo = Vector3.zero;
                lerpDistance = 0.0f;
            }
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
