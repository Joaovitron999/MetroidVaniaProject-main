using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Interactive : MonoBehaviour
{
    abstract public void Interact();
    abstract public void Interact(Message message);
    abstract public void Close();
    
}
