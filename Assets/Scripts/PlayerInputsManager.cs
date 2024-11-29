using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputsManager : MonoBehaviour
{
    public bool interact;
    public bool cancel;
    public bool open;
    public bool cheat;
    
#if ENABLE_INPUT_SYSTEM
    
    public void OnInteract(InputValue value)
    {
        InteractInput(value.isPressed);
    }

    public void OnCancel(InputValue value)
    {
        CancelInput(value.isPressed);
    }
    
    public void OnOpen(InputValue value)
    {
        OpenInput(value.isPressed);
    }
    
    public void OnCheat(InputValue value)
    {
        CheatInput(value.isPressed);
    }
#endif
    
    public void InteractInput(bool newInteractState)
    {
        interact = newInteractState;
    }

    public void CancelInput(bool newCancelState)
    {
        cancel = newCancelState;
    }
    
    public void OpenInput(bool newOpenState)
    {
        open = newOpenState;
    }
    
    public void CheatInput(bool newCheatState)
    {
        cheat = newCheatState;
    }
}
