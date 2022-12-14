using System.Collections;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private PlayerMovementController playerMovementController;
    [SerializeField] private Vector3 quickSavePosition;
    [SerializeField] private float timeToSavePosition = 2f;
    [SerializeField] private float timeToSavePositionCounter;

    private void Awake()
    {
        LoadPersistPos();
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = quickSavePosition;

        playerMovementController = player.GetComponent<PlayerMovementController>();
    }
    private void Update()
    {
        if (timeToSavePositionCounter > 0)
        {
            timeToSavePositionCounter -= Time.deltaTime;
        }
        else
        {
            timeToSavePositionCounter = timeToSavePosition;
            if(playerMovementController.IsGrounded()){
                SaveQuickSavePosition(player.transform.position);
            }
        }   
    }
    private void SaveQuickSavePosition(Vector3 position)
    {
        quickSavePosition = position;
    }

    public void PersistPos()
    {
        //Save position with PlayerPrefs
        PlayerPrefs.SetFloat("quickSavePositionX", quickSavePosition.x);
        PlayerPrefs.SetFloat("quickSavePositionY", quickSavePosition.y);
        PlayerPrefs.SetFloat("quickSavePositionZ", quickSavePosition.z);
        PlayerPrefs.SetInt("hasQuickSavePosition", 1);
    }

    private void LoadPersistPos(){
        //use playerprefs to adquire the last position
        if (PlayerPrefs.HasKey("hasQuickSavePosition"))
        {
            if (PlayerPrefs.GetInt("hasQuickSavePosition")==1)
            {
                quickSavePosition = new Vector3(PlayerPrefs.GetFloat("quickSavePositionX"),
                PlayerPrefs.GetFloat("quickSavePositionY"), PlayerPrefs.GetFloat("quickSavePositionZ"));
            }
            else
            {
                quickSavePosition = player.transform.position;
                PersistPos();
            }
        }
    }

    public void ResetPersistPos()
    {
        PlayerPrefs.SetInt("hasQuickSavePosition", 0);
        PlayerPrefs.DeleteKey("quickSavePositionX");
        PlayerPrefs.DeleteKey("quickSavePositionY");
        PlayerPrefs.DeleteKey("quickSavePositionZ");
    }

}