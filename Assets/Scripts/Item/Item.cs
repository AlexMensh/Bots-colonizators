using UnityEngine;

public class Item : MonoBehaviour
{
    public bool IsFound { get; private set; } = false;

    public void MarkAsFound()
    {
        IsFound = true;
    }

    public void ResetFoundStatus()
    {
        IsFound = false;
    }
}
