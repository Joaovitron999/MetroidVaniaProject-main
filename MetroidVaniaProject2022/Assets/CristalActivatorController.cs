using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristalActivatorController : MonoBehaviour
{
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private GameObject particles;
    [SerializeField]
    private Transform particlesPosition;

    

    //collision detection
    private void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Attack")
        {
            //chamar funcao de abrir door
            door.GetComponent<DoorController>().OpenDoor();
            //instanciar particulas
            particles = Instantiate(particles, particlesPosition.position, Quaternion.identity);
            Destroy(particles, 5);
            Destroy(gameObject);
        }
    }
}
