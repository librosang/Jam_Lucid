using UnityEngine;

public class ObjectInterctable : MonoBehaviour , IInteractable
{
    [SerializeField] private string objectName;
    [SerializeField] private string objectDescription;
    [SerializeField] private string objectDescriptionAfter;
    [SerializeField] private Sprite objectIcon;
    [SerializeField] private string objectAction;
    
    public void Interact(Transform transform)
    {
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public string GetName()
    {
       return objectName;
    }

    public string GetDescription()
    {
        return objectDescription;
    }

    public string GetDescriptionAfter()
    {
        return objectDescriptionAfter;
    }
    public Sprite GetIcon()
    {
        return objectIcon;
    }

    public string GetAction()
    {
        return objectAction;
    }
}
