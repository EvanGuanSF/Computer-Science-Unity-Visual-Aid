using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSortDemo : MonoBehaviour
{
    public float secondsBetweenInputs = 0.2f;
    private float timeOfLastInput = 0.0f;
    public BubbleSort theSorter;

    private void Start()
    {
        timeOfLastInput = Time.time;
        theSorter.InitializeList();
        theSorter.AnimatedSort();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0) && timeOfLastInput + secondsBetweenInputs < Time.time)
        {
            theSorter.ExecuteSortStep();
        }
    }
}
