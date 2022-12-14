using System.Collections;
using UnityEngine;

public class MainManager : MonoBehaviour  
{
    public static MainManager  Instance;

    public int health;
    public int maxHealth;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}