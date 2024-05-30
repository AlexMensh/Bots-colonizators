using UnityEngine;

public class Item : MonoBehaviour
{
    public bool IsFound { get; set; } = false;

    public void MarkAsFound()
    {
        IsFound = true;
    }

    public void ResetFoundStatus()
    {
        IsFound = false;
    }
}
