using UnityEngine;

public interface IInteractable 
{
 void Interact(Transform transform);
 
 Transform GetTransform();
 
 GameObject GetGameObject();
 
 string GetName();
 
 string GetDescription();
 
 string GetDescriptionAfter();

 Sprite GetIcon();

 string GetAction();



}
