using UnityEngine;

public class ShiftAnimationHelper : MonoBehaviour
{
    public float baseLerpSpeed = 3.0f;
    public float nodeShiftDistance = 2.0f;
    private float normalizedLerpSpeed;
    private bool isAnimatingShift = false;
    private bool isShiftingNodePosition = false;
    private float lerpDistance;
    private float lerpStartTime;
    private GameObject startNode;
    private Vector3 shiftNodeStartPosition;
    private Vector3 shiftNodeEndPosition;

    void Update()
    {
        // Handle the lerping of shifting objects.
        if (isAnimatingShift)
        {
            HandleLerpShift();
        }
    }

    /// <summary>
    /// Function for shifting NodeList Node(s) by one position to the left or right.
    /// Begins on the given index and shifts every node in the NodeList.
    /// Shifts right if flagShiftRight is true, left if not.
    /// </summary>
    /// <param name="shiftFromIndex"></param>
    /// <param name="flagShiftRight"></param>
    public void FlagNodeShift(int shiftFromIndex, bool flagShiftRight)
    {
        float xOffset = flagShiftRight == true ? 2.0f : -2.0f;

        // Get the starting position and set the ending position.
        startNode = gameObject.GetComponent<NodeList>().NodeAtIndex(shiftFromIndex);
        shiftNodeStartPosition = startNode.transform.position;
        shiftNodeEndPosition = shiftNodeStartPosition + new Vector3(nodeShiftDistance, 0, 0);

        lerpStartTime = Time.time;
        lerpDistance = nodeShiftDistance;
        normalizedLerpSpeed = lerpDistance * baseLerpSpeed;

        isAnimatingShift = true;
        isShiftingNodePosition = true;
    }

    /// <summary>
    /// Handles the updating of the objects being shifted.
    /// </summary>
    private void HandleLerpShift()
    {
        if (startNode.transform.position.x < shiftNodeEndPosition.x)
        {
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - lerpStartTime) * normalizedLerpSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / lerpDistance;

            // Calculate the distance along x to move all of the objects.
            float distanceToMove = Mathf.Lerp(startNode.transform.position.x,
                shiftNodeEndPosition.y,
                shiftNodeEndPosition.z);

            // Move all the nodes by the same amount.
            GameObject workingNode = startNode;
            while (workingNode != null)
            {
                workingNode.transform.position = new Vector3(workingNode.transform.position.x + distanceToMove,
                    workingNode.transform.position.y,
                    workingNode.transform.position.z);

                workingNode = workingNode.GetComponent<Node>().nextNode;
            }
        }
        else
        {
            isAnimatingShift = false;

            // Manually adjust the positions of the nodes for the final step for consistency.
            GameObject workingNode = startNode;
            while(workingNode != null)
            {
                workingNode.transform.position = new Vector3(Mathf.Round(workingNode.transform.position.x),
                    workingNode.transform.position.y,
                    workingNode.transform.position.z);

                workingNode = workingNode.GetComponent<Node>().nextNode;
            }
        }
    }

    /// <summary>
    /// Returns a boolean indicating the state of object position shifting.
    /// </summary>
    /// <returns></returns>
    public bool isLerpShifting()
    {
        return isAnimatingShift;
    }
}
