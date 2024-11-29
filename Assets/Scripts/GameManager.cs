using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerInteract playerInterct;
    [SerializeField] private Volume volume;
    [SerializeField] private GameObject _questPanel;
    [SerializeField] private GameObject _inputPanel;
    [SerializeField] private Button _questBtn;
    [SerializeField] private Button _inputBtn;
    [SerializeField] private GameObject _GraffitiObject;
    
    [Header("Cheat Mode")]
    [SerializeField] private GameObject _cheatPanel;
    [SerializeField] private Toggle _lightingToggle;
    [SerializeField] private Toggle _NightVisionToggle;
    [SerializeField] private Toggle _FlashLightToggle;
    [SerializeField] private GameObject _PlayerFlashingLight;
    [SerializeField] private GameObject _WorldLight;
    
    private DepthOfField _depthOfField;
    private ColorAdjustments _colorAdjustments;
    private Vignette _vignette;

    private FirstPersonController _firstPersonController;
    private Inventory _inventory;
    private bool _initialized;
    private bool _nightvision;

    private bool _isNightVisionTaken;
    private bool _isBatteryTaken;
    private bool _isWireTaken;
    private PlayerInputsManager _input;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Check if an instance already exists
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate instance of SingletonExample found. Destroying new instance.");
            Destroy(gameObject); // Destroy the duplicate instance
            return;
        }

        // Set the instance
        Instance = this;
        _inventory = new Inventory();
        // Optional: Prevent this object from being destroyed on scene load
        // DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _input = playerInterct.GetComponent<PlayerInputsManager>();
        _firstPersonController = playerInterct.GetComponent<FirstPersonController>();

        if (volume.profile.TryGet(out _depthOfField))
        {
            _depthOfField.active = false; 
        }

        if (volume.profile.TryGet(out _colorAdjustments))
        {
            _colorAdjustments.active = false; 
        }

        if (volume.profile.TryGet(out _vignette))
        {
            _vignette.active = false;
        }

        _isWireTaken = false;
        _isNightVisionTaken = false;
        _isBatteryTaken = false;
        _nightvision = false;
        _initialized = false;
        _GraffitiObject.SetActive(false);
        _inventory.AddItem(new Item
        {
            itemType = Item.ItemType.Flashlight, amount = 1,
            itemName = "Flashlight",
            itemDescription =
                "A worn-out flashlight with scratches and dents. It doesn’t seem reliable, but it’s better than wandering in the dark. If only it had some power..."
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_initialized == false)
        {
            if (_questPanel.activeSelf == false)
            {
                GamePause();
                _questPanel.SetActive(true);
                EventSystem.current.SetSelectedGameObject(_questBtn.gameObject);
            }
            else
            {
                if (EventSystem.current.currentSelectedGameObject == null &&
                    Cursor.lockState == CursorLockMode.Locked)
                {
                    EventSystem.current.SetSelectedGameObject(_questBtn.gameObject);
                }
            }
            
            _initialized = true;
        }

        if (_questPanel.activeSelf )
        {
            if (EventSystem.current.currentSelectedGameObject == null &&
                Cursor.lockState == CursorLockMode.Locked)
            {
                EventSystem.current.SetSelectedGameObject(_questBtn.gameObject);
            }
        }

        if (_inputPanel.activeSelf )
        {
            if (EventSystem.current.currentSelectedGameObject == null &&
                Cursor.lockState == CursorLockMode.Locked)
            {
                EventSystem.current.SetSelectedGameObject(_inputBtn.gameObject);
            }
        }

        if (_input.cheat)
        {
            if (_cheatPanel.activeSelf)
            {
                _cheatPanel.SetActive(false);
                EventSystem.current.SetSelectedGameObject(null);
            }
            else
            {
                _cheatPanel.SetActive(true);
                EventSystem.current.SetSelectedGameObject(_lightingToggle.gameObject);
                
            }
        }
        _input.cheat = false;
    }

    public List<Item> GetInventoryItems()
    {
        return _inventory.GetItemList();
    }

    public Inventory GetInventory()
    {
        return _inventory;
    }
    
    public void OnInputButtonClicked()
    {

        _inputPanel.SetActive(false);
        GameResume();
    }
    public void OnQuestButtonClicked()
    {

        _questPanel.SetActive(false);
        _inputPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_inputBtn.gameObject);
    }

    public void GamePause()
    {
        Time.timeScale = 0;
        _firstPersonController.enabled = false;
        _depthOfField.active = true;
       
    }

    public void GameResume()
    {
        Time.timeScale = 1;
        _firstPersonController.enabled = true;
        _depthOfField.active = false;
        EventSystem.current.SetSelectedGameObject(null);
    
    }

    public bool IsNightvision()
    {
        return _nightvision;
    }

    public void ActiveNightVision()
    {
        _colorAdjustments.active = true;
        _vignette.active = true;
        _nightvision = true;
        _GraffitiObject.SetActive(true);

    }

    public void DeactiveNightVision()
    {
        _colorAdjustments.active = false;
        _vignette.active = false;
        _nightvision = false;
        _GraffitiObject.SetActive(false);

    }

    public bool IsBatteryTaken()
    {
        return _isBatteryTaken;
    }

    public void BatteryTaken()
    {
        _isBatteryTaken = true;
    }

    public bool IsNightVisionTaken()
    {
        return _isNightVisionTaken;
    }

    public void NightVisionTaken()
    {
        _isNightVisionTaken = true;
    }

    public bool IsWireTaken()
    {
        return _isWireTaken;
    }

    public void WireTaken()
    {
        _isWireTaken = true;
    }

    public void OnLightingToggle()
    {
        if (_lightingToggle.isOn)
        {
            _WorldLight.SetActive(true);
        }
        else
        {
            _WorldLight.SetActive(false);
        }
    }

    public void OnNightVisionToggle()
    {
        if (_NightVisionToggle.isOn)
        {
            ActiveNightVision();
        }
        else
        {
            DeactiveNightVision();
        }
    }
    public void OnFlashLightToggle()
    {
        if (_FlashLightToggle.isOn)
        {
            _PlayerFlashingLight.SetActive(true);
        }
        else
        {
            _PlayerFlashingLight.SetActive(false);
        }
    }
    
}