using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpRotate : MonoBehaviour
{
    public float startZ = 0.0f;
    public float endZ = 0.0f;
    public static float lerpSpeed = 3.0f;

    private long rotationCount = 1;
    private long rotMod = 0;
    private float lerpDistance;
    private float lerpStartTime;

    // Use this for initialization
    void Start()
    {
        lerpStartTime = Time.time;

        lerpDistance = Mathf.Abs(startZ - endZ);

        startZ = startZ != 0 ? 90.0f : startZ;
    }

    void FixedUpdate()
    {

        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - lerpStartTime) * lerpSpeed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / lerpDistance;

        if (endZ - transform.eulerAngles.z > 0.001f)
        {
            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                Quaternion.Euler(new Vector3(0, 0, endZ)),
                fractionOfJourney * lerpSpeed);
        }
        else
        {
            lerpStartTime = Time.time;

            rotMod = rotationCount % 4;

            //transform.eulerAngles = new Vector3(0, 0, 0);

            switch (rotMod)
            {
                case 0:
                    {
                        transform.eulerAngles = new Vector3(0, 90f, 0);
                        break;
                    }
                case 1:
                    {
                        transform.eulerAngles = new Vector3(90f, 0, 0);
                        break;
                    }
                case 2:
                    {
                        transform.eulerAngles = new Vector3(0, -90f, 0);
                        break;
                    }
                case 3:
                    {
                        transform.eulerAngles = new Vector3(-90f, 0, 0);
                        break;
                    }

                default:
                    {
                        return;
                    }
            }

            //if(rotationCount % 2 == 0)
            //{
            //    transform.eulerAngles = new Vector3(0, startZ, 0);
            //}
            //else
            //{
            //    transform.eulerAngles = new Vector3(0, 0, 0);
            //}

            rotationCount++;
        }
    }
}
