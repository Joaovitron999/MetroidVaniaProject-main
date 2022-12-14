using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveController : MonoBehaviour
{
    public GameObject Indicator;
    public GameObject objInteractive;
    private bool canInteract = false;

    //message list
    [SerializeField]
    private List<Message> messages;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Indicator.SetActive(true);
            canInteract = true;
        }
        Debug.Log("Player tag", other.gameObject);
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Indicator.SetActive(false);
            canInteract = false;
        }
    }

    private int indice = 0;

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            if(!objInteractive.GetComponent<MessageController>().isWriting){
                objInteractive.GetComponent<Interactive>().Interact(messages[indice]);
                indice++;
                if(indice >= messages.Count){
                    objInteractive.GetComponent<Interactive>().Close();
                }
            }
            
        }
        if(!canInteract)
        {
            objInteractive.GetComponent<Interactive>().Close();
            indice = 0;
        }        
    }

    
}

[System.Serializable]
public class Message
{
    [SerializeField]
    public int id;
    [SerializeField]
    [TextArea(3, 10)]
    [Tooltip("Texto a ser exibido")]
    public string message;

}