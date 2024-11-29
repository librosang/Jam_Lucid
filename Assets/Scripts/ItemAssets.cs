using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }
    
    public Sprite flashlight;
    public Sprite battery;
    public Sprite cable;
    public Sprite nightvision;
    public Sprite stungun;
    
    
}
