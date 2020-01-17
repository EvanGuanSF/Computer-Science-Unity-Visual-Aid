using UnityEngine;

public class InsertAnimationHelper : MonoBehaviour
{
    public float baseLerpSpeed = 2.0f;
    private float normalizedLerpSpeed;
    private bool isAnimatingInsertion = false;
    private bool isMovingAwayFromVertical = false, isMovingToFinalHorizontal = false, isMovingToVertical = false;
    private float lerpDistance;
    private float lerpStartTime;
    private GameObject nodeToMove;
    private Vector3 nodeInitialPosition, nodeTempPositionOne, nodeTempPositionTwo, nodeFinalPosition;

    void Update()
    {
        // Handle the lerping of the node being moved if applicable.
        if (isAnimatingInsertion)
        {
            HandleLerpInsertion();
        }
    }

    /// <summary>
    /// Moves the given node from its initial position to the given final position.
    /// </summary>
    /// <param name="nodeToMove"></param>
    /// <param name="finalPosition"></param>
    public void FlagLerpInsertion(GameObject nodeToMove, Vector3 finalPosition)
    {
        if (isAnimatingInsertion)
        {
            return;
        }

        lerpStartTime = Time.time;

        // Set variable data for FixedUpdate calculations.
        this.nodeToMove = nodeToMove;
        nodeFinalPosition = finalPosition;

        nodeInitialPosition = new Vector3(this.nodeToMove.transform.position.x, this.nodeToMove.transform.position.y, this.nodeToMove.transform.position.z);

        // Set the positions of the checkpoints that will be used.
        nodeTempPositionOne = new Vector3(nodeInitialPosition.x, nodeInitialPosition.y + 1.25f, nodeInitialPosition.z);
        nodeTempPositionTwo = new Vector3(nodeFinalPosition.x, nodeTempPositionOne.y, nodeTempPositionOne.z);

        // Calculate the distance between the objects that will be lerped.
        // Update the lerp speed to normalize.
        lerpDistance = (nodeInitialPosition - nodeTempPositionOne).magnitude;
        normalizedLerpSpeed = lerpDistance * baseLerpSpeed;

        isMovingAwayFromVertical = true;
        isAnimatingInsertion = true;
    }

    /// <summary>
    /// Handles the updating of the object being moved.
    /// </summary>
    private void HandleLerpInsertion()
    {
        if (isMovingAwayFromVertical)
        {
            // Distance moved equals elapsed time times speed.
            float distCovered = (Time.time - lerpStartTime) * normalizedLerpSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / lerpDistance;

            if (fractionOfJourney < 1.0f)
            {
                // Set our position as a fraction of the distance between the markers.
                nodeToMove.transform.position = Vector3.Lerp(
                    nodeInitialPosition,
                    nodeTempPositionOne,
                    fractionOfJourney);
            }
            else
            {
                lerpStartTime = Time.time;

                // Calculate the distance between the objects that will be lerped.
                // Update the lerp speed to normalize.
                lerpDistance = (nodeTempPositionOne - nodeTempPositionTwo).magnitude;
                normalizedLerpSpeed = lerpDistance * baseLerpSpeed;

                // Manually move node to final positions to account for any timing deviations.
                nodeToMove.transform.position = nodeTempPositionOne;

                isMovingToFinalHorizontal = true;
                isMovingAwayFromVertical = false;
            }
        }
        else if (isMovingToFinalHorizontal)
        {
            // Distance moved equals elapsed time times speed.
            float distCovered = (Time.time - lerpStartTime) * normalizedLerpSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / lerpDistance;

            if (fractionOfJourney < 1.0f)
            {
                // Set our position as a fraction of the distance between the markers.
                nodeToMove.transform.position = Vector3.Lerp(
                    nodeTempPositionOne,
                    nodeTempPositionTwo,
                    fractionOfJourney);
            }
            else
            {
                lerpStartTime = Time.time;

                // Calculate the distance between the objects that will be lerped.
                // Update the lerp speed to normalize.
                lerpDistance = (nodeTempPositionTwo - nodeFinalPosition).magnitude;
                normalizedLerpSpeed = lerpDistance * baseLerpSpeed;

                // Manually move node to final positions to account for any timing deviations.
                nodeToMove.transform.position = nodeTempPositionTwo;

                isMovingToVertical = true;
                isMovingToFinalHorizontal = false;
            }
        }
        else if (isMovingToVertical)
        {
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - lerpStartTime) * normalizedLerpSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / lerpDistance;

            if (fractionOfJourney < 1.0f)
            {
                // Set our position as a fraction of the distance between the markers.
                // Handle object one.
                nodeToMove.transform.position = Vector3.Lerp(
                    nodeTempPositionTwo,
                    nodeFinalPosition,
                    fractionOfJourney);
            }
            else
            {
                lerpStartTime = Time.time;

                // Manually move node to final positions to account for any timing deviations.
                nodeToMove.transform.position = nodeFinalPosition;

                isMovingToVertical = false;
            }
        }
        else
        {
            //Debug.Log("Done animating lerp.");
            // Flag and reset variables when we finish lerping.
            isAnimatingInsertion = false;
            nodeToMove = null;
            isMovingAwayFromVertical = isMovingToFinalHorizontal = isMovingToVertical = false;
            nodeInitialPosition = nodeTempPositionOne = nodeTempPositionTwo = Vector3.zero;
            lerpDistance = 0.0f;
        }
    }

    /// <summary>
    /// Returns a boolean indicating the state of object position moving.
    /// </summary>
    /// <returns></returns>
    public bool isLerpInserting()
    {
        return isAnimatingInsertion;
    }
}
