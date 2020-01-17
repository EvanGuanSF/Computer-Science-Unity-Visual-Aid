using UnityEngine;

public class ShiftAnimationHelper : MonoBehaviour
{
    public float baseLerpSpeed = 1.0f;
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
        if(shiftFromIndex >= gameObject.GetComponent<NodeList>().Count())
        {
            return;
        }

        float xOffset = flagShiftRight == true ? nodeShiftDistance : -nodeShiftDistance;

        // Get the starting position and set the ending position.
        startNode = gameObject.GetComponent<NodeList>().NodeAtIndex(shiftFromIndex);
        shiftNodeStartPosition = startNode.transform.position;
        shiftNodeEndPosition = shiftNodeStartPosition + new Vector3(xOffset, 0, 0);
        
        lerpStartTime = Time.time;
        lerpDistance = (shiftNodeStartPosition - shiftNodeEndPosition).magnitude;
        normalizedLerpSpeed = lerpDistance * baseLerpSpeed;

        isAnimatingShift = true;
        isShiftingNodePosition = true;
    }

    /// <summary>
    /// Handles the updating of the objects being shifted.
    /// </summary>
    private void HandleLerpShift()
    {
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - lerpStartTime) * normalizedLerpSpeed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / lerpDistance;

        if (fractionOfJourney < 1.0f)
        {
            //Debug.Log(fractionOfJourney + " || " + distCovered + " || " + lerpDistance);

            // Move all the nodes by the same amount.
            startNode.transform.position = Vector3.Lerp(shiftNodeStartPosition, shiftNodeEndPosition, fractionOfJourney);

            GameObject workingNode = startNode.GetComponent<Node>().nextNode;
            while (workingNode != null)
            {
                workingNode.transform.position = workingNode.GetComponent<Node>().prevNode.transform.position + new Vector3(2.0f, 0, 0);

                workingNode = workingNode.GetComponent<Node>().nextNode;
            }
        }
        else
        {
            isAnimatingShift = false;

            // Manually adjust the positions of the nodes for the final step for consistency.
            GameObject workingNode = startNode;
            while (workingNode != null)
            {
                //Debug.Log(workingNode.GetComponent<Node>().nodeValue + " Pre-Final pos: " + workingNode.transform.position);

                workingNode.transform.position = new Vector3(workingNode.GetComponent<Node>().nodeIndex * 2,
                    startNode.transform.position.y,
                    startNode.transform.position.z);

                //Debug.Log(workingNode.GetComponent<Node>().nodeValue + " Final pos: " + workingNode.transform.position);

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
