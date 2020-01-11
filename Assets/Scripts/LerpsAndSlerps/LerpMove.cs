using UnityEngine;

public class LerpMove : MonoBehaviour
{
    public float startX = 0.0f;
    public float startY = 0.0f;
    public float endX = 0.0f;
    public static float lerpSpeed = 5.0f;

    private float lerpDistance;
    private float lerpStartTime;

    // Use this for initialization
    void Start()
    {
        lerpStartTime = Time.time;

        lerpDistance = Mathf.Abs(startX - endX);

        startX = startX != 0.0f ? startX : transform.position.x;
        startY = startY != 0.0f ? startY : transform.position.y;
    }

    void FixedUpdate()
    {
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - lerpStartTime) * lerpSpeed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / lerpDistance;

        if (fractionOfJourney < 1.0f)
        {
            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(new Vector3(startX, startY, 0), new Vector3(endX, startY, 0), fractionOfJourney);
        }
        else
        {
            transform.position.Set(startX, startY, 0.0f);
            lerpStartTime = Time.time;
        }
    }
}
