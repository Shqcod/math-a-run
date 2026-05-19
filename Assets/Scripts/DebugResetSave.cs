using UnityEngine;

public class DebugResetSave : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.DeleteAll();

        Debug.Log("SAVE RESET");
    }
}