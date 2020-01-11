using UnityEngine;

public class SelectionSortDemo : MonoBehaviour
{
    public float secondsBetweenInputs = 0.2f;
    private float timeOfLastInput = 0.0f;
    public SelectionSort theSorter;

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
