using System.Collections;
using UnityEngine;


public class UICounterData : MonoBehaviour {
    
    private int limit = 20;
    [SerializeField] private int maxQuantity = 5;
    [SerializeField] private int currentQuantity = 3;
    [SerializeField] private Transform initialPosition;
    [SerializeField] private GameObject counterPrefab;
    [SerializeField] private float distanceBetweenCounters = 1f;

    //List of counters
    [SerializeField] private ArrayList counters = new ArrayList();

    private void Start() {
        //instanciate the counters (limit = 20)
        for (int i = 0; i < limit; i++) {
            GameObject counter = Instantiate(counterPrefab, initialPosition.position, Quaternion.identity);
            counter.transform.SetParent(transform);
            counter.transform.position = new Vector3(counter.transform.position.x + (distanceBetweenCounters * i), counter.transform.position.y, counter.transform.position.z);
            counter.SetActive(false);
            counters.Add(counter);
        }

        //deactivate the counters that are not needed
        for (int i = 0; i < limit; i++) {
            if (i < currentQuantity) {
                ((GameObject)counters[i]).SetActive(true);
            } else {
                ((GameObject)counters[i]).SetActive(false);
            }
        }
    }

    public void SetCurrentQuantity(int newQuantity) {
        currentQuantity = newQuantity;
        if(currentQuantity > maxQuantity) {
            currentQuantity = maxQuantity;
            Debug.Log("MAX QUANTITY");
        } else if (currentQuantity < 0) {
            currentQuantity = 0;
            Debug.Log("MENOR QUE 0");
        }

        foreach (GameObject counter in counters) {

            if (counter.transform.GetSiblingIndex() < currentQuantity) {
                counter.SetActive(true);
            } else {
                counter.SetActive(false);
            }
        }
    }

    public int GetCurrentQuantity() {
        return currentQuantity;
    }

    public void SetMaxQuantity(int newMaxQuantity) {
        maxQuantity = newMaxQuantity;
        if (maxQuantity > limit) {
            maxQuantity = limit;
        } else if (maxQuantity < 0) {
            maxQuantity = 0;
        }
    }


    









}
