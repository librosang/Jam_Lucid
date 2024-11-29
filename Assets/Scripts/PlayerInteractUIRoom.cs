using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInteractUIRoom : MonoBehaviour
{
    [Header("QuestUI")] 
    [SerializeField] private GameObject _questUI;
    [SerializeField] private Button _questUseButton;
    [SerializeField] private PlayerInteract playerInterct;
    [SerializeField] private GameObject inspectBtn;
    
    private PlayerInputsManager _input;
    private bool _inspecting;
    private bool _openInventory;
    private IInteractable _currentInteractable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _input = playerInterct.GetComponent<PlayerInputsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_inspecting)
        {
            IInteractable interactable = playerInterct.GetInteractable();

            if (interactable != null)
            {
                _currentInteractable = interactable;
                ShowInspect();
            }
            else
            {
                _currentInteractable = null;
                HideInspect();
            }

            // Handle E button (interact)
            if (_input.interact && _currentInteractable != null)
            {
                ShowObjectInspection(_currentInteractable);
            }
            
        }
        else
        {
            _questUseButton.interactable = true;
            if (EventSystem.current.currentSelectedGameObject == null &&
                Cursor.lockState == CursorLockMode.Locked)
            {
                EventSystem.current.SetSelectedGameObject(_questUseButton.gameObject);
            }
        }
        
    }
    
    private void ShowInspect()
    {
        inspectBtn.SetActive(true);
    }

    private void HideInspect()
    {
        inspectBtn.SetActive(false);
    }
    
    private void ShowObjectInspection(IInteractable interactable)
    {
        _questUI.SetActive(true);
        _inspecting = true;
        GameMangerRoom.Instance.GamePause();
        
        _questUseButton.onClick.RemoveAllListeners(); 
        _questUseButton.onClick.AddListener((() =>
                {
                    SceneManager.LoadScene(0);
                    
                }));

        HideInspect();
        EventSystem.current.SetSelectedGameObject(_questUseButton.gameObject);
    }
}
