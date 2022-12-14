using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//exibe campos de texto



public class MessageController : Interactive
{  
    public GameObject box;
    public bool isOpen = false;  
    public bool isWriting = false;
    //tmp
    public TextMeshProUGUI textMessage; 


    // implementação da interface
    public override void Interact()
    {
    }

    public override void Interact(Message message)
    {
        if(!isWriting)
        {
            
            box.SetActive(true);
            isOpen = true;
            //coroutine
            StartCoroutine(ShowText(message.message));
            
        }
    }

    public override void Close()
    {
        box.SetActive(false);
        isOpen = false;
    }

    // coroutine para exibir o texto
    IEnumerator ShowText(string text)
    {
        isWriting = true;
        textMessage.text = "";
        yield return new WaitForSeconds(0.20f);

        //exibe o texto um caracter por vez
        for (int i = 0; i < text.Length; i++)
        {
            textMessage.text += text[i];
            yield return new WaitForSeconds(0.05f);
        }
        isWriting = false;
    }
    
}
