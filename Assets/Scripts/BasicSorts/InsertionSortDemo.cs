using System.Collections;
using UnityEngine;

public class InsertionSortDemo : MonoBehaviour
{
    public float secondsBetweenInputs = 0.2f;
    private float timeOfLastInput = 0.0f;
    public InsertionSort theSorter;
    public GameObject nodePrefab;

    private void Start()
    {
        timeOfLastInput = Time.time;
        //theSorter.InitializeList();

        StartCoroutine(TestOne());
        //StartCoroutine(TestTwo());

        //theSorter.theList.AnimatedInsertAtIndex(0, 10);
        //theSorter.AnimatedSort();
    }

    private IEnumerator TestOne()
    {
        theSorter.InitializeList();

        GameObject newNode = Instantiate(nodePrefab, new Vector3(20, -6, 0), Quaternion.identity);
        newNode.GetComponent<Node>().nodeValue = 10;
        newNode.GetComponent<Node>().UpdateName();
        newNode.GetComponent<Node>().UpdatePositions();

        Debug.Log("Inserting node 10");
        theSorter.theList.AnimatedInsertAtIndex(4, newNode);

        yield return new WaitUntil(() => gameObject.GetComponent<InsertAnimationHelper>().isLerpInserting() == false &&
            gameObject.GetComponent<ShiftAnimationHelper>().isLerpShifting() == false);
        yield return new WaitForSeconds(1.0f);

        Debug.Log("Inserting node 11");
        theSorter.theList.AnimatedInsertAtIndex(0, 11);

        yield return new WaitUntil(() => gameObject.GetComponent<InsertAnimationHelper>().isLerpInserting() == false &&
            gameObject.GetComponent<ShiftAnimationHelper>().isLerpShifting() == false);
        yield return new WaitForSeconds(1.0f);

        Debug.Log("Inserting node 12");
        theSorter.theList.AnimatedInsertAtIndex(12, 12);

        yield return new WaitUntil(() => gameObject.GetComponent<InsertAnimationHelper>().isLerpInserting() == false &&
            gameObject.GetComponent<ShiftAnimationHelper>().isLerpShifting() == false);
        yield return new WaitForSeconds(1.0f);

        Debug.Log("Inserting node 13");
        theSorter.theList.AnimatedInsertAtIndex(14, 13);

        yield return new WaitUntil(() => gameObject.GetComponent<InsertAnimationHelper>().isLerpInserting() == false &&
            gameObject.GetComponent<ShiftAnimationHelper>().isLerpShifting() == false);
        yield return new WaitForSeconds(1.0f);

        Debug.Log("Removing node 12");
        theSorter.theList.DeleteAtIndex(12);

        yield return new WaitUntil(() => gameObject.GetComponent<InsertAnimationHelper>().isLerpInserting() == false &&
            gameObject.GetComponent<ShiftAnimationHelper>().isLerpShifting() == false);
        yield return new WaitForSeconds(1.0f);

        Debug.Log("Removing node 11");
        theSorter.theList.DeleteAtIndex(0);

        yield return new WaitUntil(() => gameObject.GetComponent<InsertAnimationHelper>().isLerpInserting() == false &&
            gameObject.GetComponent<ShiftAnimationHelper>().isLerpShifting() == false);
        yield return new WaitForSeconds(1.0f);

        Debug.Log("Removing node 10");
        theSorter.theList.DeleteAtIndex(4);

        yield return new WaitUntil(() => gameObject.GetComponent<InsertAnimationHelper>().isLerpInserting() == false &&
            gameObject.GetComponent<ShiftAnimationHelper>().isLerpShifting() == false);
        yield return new WaitForSeconds(1.0f);
    }

    private IEnumerator TestTwo()
    {
        theSorter.theArray = null;
        theSorter.InitializeList();

        GameObject newNode = Instantiate(nodePrefab, new Vector3(20, -6, 0), Quaternion.identity);
        newNode.GetComponent<Node>().nodeValue = 10;
        newNode.GetComponent<Node>().UpdateName();
        newNode.GetComponent<Node>().UpdatePositions();

        Debug.Log("Inserting node 10");
        theSorter.theList.AnimatedInsertAtIndex(0, newNode);

        yield return new WaitUntil(() => gameObject.GetComponent<InsertAnimationHelper>().isLerpInserting() == false &&
            gameObject.GetComponent<ShiftAnimationHelper>().isLerpShifting() == false);
        yield return new WaitForSeconds(1.0f);

        Debug.Log("Inserting node 11");
        theSorter.theList.AnimatedInsertAtIndex(1, 11);

        yield return new WaitUntil(() => gameObject.GetComponent<InsertAnimationHelper>().isLerpInserting() == false &&
            gameObject.GetComponent<ShiftAnimationHelper>().isLerpShifting() == false);
        yield return new WaitForSeconds(1.0f);

        Debug.Log("Inserting node 12");
        theSorter.theList.AnimatedInsertAtIndex(2, 12);

        yield return new WaitUntil(() => gameObject.GetComponent<InsertAnimationHelper>().isLerpInserting() == false &&
            gameObject.GetComponent<ShiftAnimationHelper>().isLerpShifting() == false);
        yield return new WaitForSeconds(1.0f);

        Debug.Log("Inserting node 13");
        theSorter.theList.AnimatedInsertAtIndex(4, 13);

        yield return new WaitUntil(() => gameObject.GetComponent<InsertAnimationHelper>().isLerpInserting() == false &&
            gameObject.GetComponent<ShiftAnimationHelper>().isLerpShifting() == false);
        yield return new WaitForSeconds(1.0f);

        Debug.Log("Removing node 12");
        theSorter.theList.DeleteAtIndex(2);

        yield return new WaitUntil(() => gameObject.GetComponent<InsertAnimationHelper>().isLerpInserting() == false &&
            gameObject.GetComponent<ShiftAnimationHelper>().isLerpShifting() == false);
        yield return new WaitForSeconds(1.0f);

        Debug.Log("Removing node 11");
        theSorter.theList.DeleteAtIndex(1);

        yield return new WaitUntil(() => gameObject.GetComponent<InsertAnimationHelper>().isLerpInserting() == false &&
            gameObject.GetComponent<ShiftAnimationHelper>().isLerpShifting() == false);
        yield return new WaitForSeconds(1.0f);

        Debug.Log("Removing node 10");
        theSorter.theList.DeleteAtIndex(0);

        yield return new WaitUntil(() => gameObject.GetComponent<InsertAnimationHelper>().isLerpInserting() == false &&
            gameObject.GetComponent<ShiftAnimationHelper>().isLerpShifting() == false);
        yield return new WaitForSeconds(1.0f);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0) && timeOfLastInput + secondsBetweenInputs < Time.time)
        {
            theSorter.ExecuteSortStep();
        }
    }
}
