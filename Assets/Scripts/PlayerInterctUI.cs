using System;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInterctUI : MonoBehaviour
{
    [SerializeField] private GameObject inspectBtn;
    [SerializeField] private PlayerInteract playerInterct;
    [SerializeField] private GameObject inspectPanel;
    [SerializeField] private GameObject inventoryUi;
    [SerializeField] private Volume volume;
    [SerializeField] private TextMeshProUGUI objectName;
    [SerializeField] private TextMeshProUGUI objectDescription;

    [SerializeField] private GameObject closeButton;
    [SerializeField] private GameObject useButton;

    [Header("Inventory UI")] [SerializeField]
    private GameObject _inventoryUI;

    [SerializeField] private TextMeshProUGUI _inventoryObjectName;
    [SerializeField] private TextMeshProUGUI _inventoryObjectDescription;
    [SerializeField] private Button _inventoryUseButton;
    [SerializeField] private Button _inventoryEquipButton;
    [SerializeField] private TextMeshProUGUI _inventoryEquipName;
    [SerializeField] private TextMeshProUGUI _inventoryUseName;
    [SerializeField] private Transform _itemSlotContainer;
    [SerializeField] private Transform _itemSlotTemplate;
    [SerializeField] private GameObject _FlashlightObject;
    

    [Header("Inspect Object UI")] [SerializeField]
    private GameObject _inspectedUI;

    [SerializeField] private TextMeshProUGUI _inspectedName;
    [SerializeField] private TextMeshProUGUI _inspectedDescription;
    [SerializeField] private Image _inspectedIcon;
    [SerializeField] private Button _inspectedActionButton;
    [SerializeField] private Button _inspectedCloseButton;
    [SerializeField] private TextMeshProUGUI _inspectedActionName;


    [Header("Light/Sound")]
    [SerializeField] private AudioSource _soundObject;
    [SerializeField] private GameObject _lightObject;
    
    
    private PlayerInputsManager _input;
    private IInteractable _currentInteractable;

    private bool _inspecting;
    private bool _openInventory;
    private GameObject _currentClone;
    private Inventory _inventory;
    private bool OnUseBtnClicked = false;
    private bool audioPlayed ;
    private void Start()
    {
        _input = playerInterct.GetComponent<PlayerInputsManager>();
        audioPlayed = false;
    }

    private void Update()
    {
        if (audioPlayed && !_soundObject.isPlaying)
        {
            _soundObject.enabled = false;
            SceneManager.LoadScene(3);
        }
       
        if (_input.cancel && (_inspecting || _openInventory))
        {
            _input.cancel = false;
            HideObjectInspection();
            HideInventory();
        }

        
        if (!_inspecting && !_openInventory)
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

            
            if (_input.interact && _currentInteractable != null)
            {
                ShowObjectInspection(_currentInteractable);
            }

          
            if (_input.open)
            {
                ShowInventory();
            }
        }

      
        _input.open = false;
        _input.interact = false;
        _input.cancel = false;
    }
    

    private void RefreshInventoryItems()
    {
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 80f;
       
        float offsetX = 20f;
        float offsetY = 0;
        float margin = 10f;
        
        ClearInventoryItems();
        foreach (Item item in GameManager.Instance.GetInventoryItems())
        {
            RectTransform itemSlotRectTransform =
                Instantiate(_itemSlotTemplate, _itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.anchoredPosition = new Vector2(
                offsetX + x * (itemSlotCellSize + margin),
                offsetY + y * (itemSlotCellSize + margin)
            );

            Image itemImage = itemSlotRectTransform.Find("image").GetComponent<Image>();
            itemImage.sprite = item.GetSprite();

            Button itembButton = itemSlotRectTransform.GetComponent<Button>();
            itembButton.onClick.AddListener((() => OnInventoryItemButtonClick(item)));

            if (x == 0)
            {
                OnInventoryItemButtonClick(item);
                EventSystem.current.SetSelectedGameObject(itemSlotRectTransform.gameObject);
            }

            x += 1;
            if (x > 4)
            {
                x = 0;
                y--;
            }
        }
    }

    private void ClearInventoryItems()
    {
        foreach (Transform child in _itemSlotContainer)
        {
           
            if (child.gameObject == _itemSlotTemplate.gameObject)
                continue;

            Destroy(child.gameObject);
        }
    }

    private void ShowObjectInspection(IInteractable interactable)
    {
        GameManager.Instance.GamePause();

        _inspecting = true;
        _inspectedName.SetText(interactable.GetName());
        _inspectedDescription.SetText(interactable.GetDescription());
        _inspectedIcon.sprite = interactable.GetIcon();
        _inspectedActionName.SetText(interactable.GetAction());

        switch (interactable.GetName())
        {
            
       
            case "Rusty Companion":
                if (GameManager.Instance.IsBatteryTaken())
                {
                    _inspectedDescription.SetText(interactable.GetDescriptionAfter());
                    _inspectedActionButton.interactable = false;
                    _inspectedActionName.SetText("Leave It Behind");
                    break;
                }

                _inspectedActionButton.onClick.RemoveAllListeners(); 
                _inspectedActionButton.onClick.AddListener((() =>
                {
                    
                        AudioSource robotSound = interactable.GetGameObject().GetComponent<AudioSource>();
                        robotSound.enabled = false;

                        GameManager.Instance.GetInventory().AddItem(new Item
                        {
                            itemType = Item.ItemType.Battery, amount = 1,
                            itemName = "Battery",
                            itemDescription =
                                "An old, slightly corroded battery. It still holds some charge—enough to power something small or maybe… something more?"
                        });
                        GameManager.Instance.BatteryTaken();
                        HideObjectInspection();
                    
                }));
                
                break;
            case "Rusty Guardian":
                
                if (GameManager.Instance.IsNightVisionTaken() )
                {
                    _inspectedDescription.SetText(interactable.GetDescriptionAfter());
                    _inspectedActionButton.interactable = false;
                    _inspectedActionName.SetText("Leave It Behind");
                    break;
                }
                
                _inspectedActionButton.onClick.RemoveAllListeners(); 
                _inspectedActionButton.onClick.AddListener((() =>
                {
                    
                    GameManager.Instance.GetInventory().AddItem(new Item
                    {
                        itemType = Item.ItemType.NightVision, amount = 1,
                        itemName = "NightVision",
                        itemDescription =
                            "An old, battered night vision device with scratches on the lens. Despite its worn appearance, it hums with faint energy, promising sight in the darkness where the truth hides."
                    });
                    
                    GameManager.Instance.NightVisionTaken();
                    Debug.Log("Stage 2 Completed");
                    HideObjectInspection();
                    
                }));
                
            break;
            
            
            case "Abandoned Ambulance" :
                if (GameManager.Instance.IsWireTaken() )
                {
                    _inspectedDescription.SetText(interactable.GetDescriptionAfter());
                    _inspectedActionButton.interactable = false;
                    _inspectedActionName.SetText("Leave It Behind");
                    break;
                }
                _inspectedActionButton.onClick.RemoveAllListeners(); 
                _inspectedActionButton.onClick.AddListener((() =>
                {
                    
                    GameManager.Instance.GetInventory().AddItem(new Item
                    {
                        itemType = Item.ItemType.Cable, amount = 1,
                        itemName = "Wire",
                        itemDescription =
                            "A worn, frayed wire pulled from the abandoned ambulance. Though old and weathered, it still holds potential. Its exposed metal ends hint at its purpose—connecting power, perhaps the key to getting something else working again."
                    });
                    
                    GameManager.Instance.WireTaken();

                    HideObjectInspection();
                    
                }));

                break;
            
            case "Failed Escape Tools":
                _inspectedActionButton.onClick.RemoveAllListeners(); 
                _inspectedActionButton.onClick.AddListener((() =>
                {
                    HideObjectInspection();
                    
                }));
                break;
        }

       

        HideInspect();
        _inspectedUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_inspectedCloseButton.gameObject);
    }

    private void OnInventoryItemButtonClick(Item item)
    {
        _inventoryUseButton.onClick.RemoveAllListeners(); 
        _inventoryEquipButton.onClick.RemoveAllListeners(); 
        
        _inventoryObjectName.text = item.itemName;
        _inventoryObjectDescription.text = item.itemDescription;
        _inventoryUseButton.onClick.AddListener((() => OnUseBtnClick(item)));

        if (item.itemName == "Flashlight")
        {
            if (GameManager.Instance.GetInventory().HasItemWithName("Battery"))
            {
                _inventoryUseName.text = "Insert Battery";
                _inventoryUseButton.interactable = true;
                _inventoryEquipButton.interactable = false;
                _FlashlightObject.SetActive(false);
                _inventoryEquipName.text = "Equip";
            }
            else if (!GameManager.Instance.IsBatteryTaken())
            {
                _inventoryUseName.text = "Need Battery";
                _inventoryUseButton.interactable = false;
                _inventoryEquipButton.interactable = false;
            }
            else
            {
                _inventoryUseName.text = "Remove Battery";
                _inventoryUseButton.interactable = true;
                _inventoryEquipButton.interactable = true;
                if (_FlashlightObject.activeSelf)
                {
                    _inventoryEquipName.text = "Unequip";
                }
                else
                {
                    _inventoryEquipName.text = "Equip";
                }
                
                _inventoryEquipButton.onClick.AddListener((() =>
                {
                    if (_FlashlightObject.activeSelf)
                    {
                        _inventoryEquipName.text = "Equip";
                        _FlashlightObject.SetActive(false);
                    }
                    else
                    {
                        _FlashlightObject.SetActive(true);
                        _inventoryEquipName.text = "Unequip";
                    }
                    
                }));
            }
        }
        else if (item.itemName == "Battery")
        {
            _inventoryUseName.text = "Use";
            _inventoryUseButton.interactable = false;
            _inventoryEquipButton.interactable = false;
            
        }else if (item.itemName == "Stun Device")
        {
            _inventoryUseName.text = "Activate";
            _inventoryUseButton.interactable = true;
            _inventoryEquipButton.interactable = false;
            
        }else if (item.itemName == "Wire")
        {
            if (GameManager.Instance.GetInventory().HasItemWithName("Battery"))
            {
                _inventoryUseName.text = "Use Battery";
                _inventoryUseButton.interactable = true;
                _inventoryEquipButton.interactable = false;
                _inventoryEquipName.text = "Equip";
            }
            else if (!GameManager.Instance.IsBatteryTaken())
            {
                _inventoryUseName.text = "Need Battery";
                _inventoryUseButton.interactable = false;
                _inventoryEquipButton.interactable = false;
            }
            else
            {
                _inventoryUseName.text = "Need Battery";
                _inventoryUseButton.interactable = false;
                _inventoryEquipButton.interactable = false; 
            }
            
            
        }
        else if (item.itemName == "NightVision")
        {
            _inventoryUseName.text = "Change Battery";
            _inventoryUseButton.interactable = false;
            _inventoryEquipButton.interactable = true;
            
            if (GameManager.Instance.IsNightvision())
            {
                _inventoryEquipName.text = "Unequip";
            }
            else
            {
                _inventoryEquipName.text = "Equip";
            }
            
            _inventoryEquipButton.onClick.AddListener((() =>
            {
                if (GameManager.Instance.IsNightvision())
                {
                    _inventoryEquipName.text = "Equip";
                    GameManager.Instance.DeactiveNightVision();
                }
                else
                {
                    GameManager.Instance.ActiveNightVision();
                    _inventoryEquipName.text = "Unequip";
                }
                    
            }));
        }
    }

    private void HideObjectInspection()
    {
        GameManager.Instance.GameResume();
        _inspecting = false;
        _inspectedUI.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void ShowInspect()
    {
        inspectBtn.SetActive(true);
    }

    private void HideInspect()
    {
        inspectBtn.SetActive(false);
    }

    public void OnUseBtnClick(Item item)
    {
        
        if (item.itemName == "Flashlight")
        {
            if (GameManager.Instance.GetInventory().HasItemWithName("Battery"))
            {
                GameManager.Instance.GetInventory().RemoveItem("Battery");
                _inventoryUseName.text = "Remove Battery";
                _inventoryUseButton.interactable = true;
                _inventoryEquipButton.interactable = true;
                
                
            }else
            {
                GameManager.Instance.GetInventory().AddItem(new Item
                {
                    itemType = Item.ItemType.Battery, amount = 1,
                    itemName = "Battery",
                    itemDescription =
                        "An old, slightly corroded battery. It still holds some charge—enough to power something small or maybe… something more?"
                });
                
                _inventoryUseName.text = "Insert Battery";
                _inventoryUseButton.interactable = true;
                _inventoryEquipButton.interactable = false;
            }
        }else if (item.itemName == "Wire")
        {
            GameManager.Instance.GetInventory().AddItem(new Item
            {
                itemType = Item.ItemType.StunGun, amount = 1,
                itemName = "Stun Device",
                itemDescription =
                    "A rough, makeshift stun gun made from a wire and a battery. Its design is unstable, but it could be the shock you need to break free."
            });
            GameManager.Instance.GetInventory().RemoveItem("Battery");
            GameManager.Instance.GetInventory().RemoveItem("Wire");
        }else if (item.itemName == "Stun Device")
        {
            OnCloseBtnClick();
            _soundObject.Play();
            audioPlayed = true;
            
            _lightObject.SetActive(true);
            
        }
        RefreshInventoryItems();
    }

    public void OnCloseBtnClick()
    {
        _input.cancel = false;
        HideObjectInspection();
        HideInventory();
    }

    private void HideInventory()
    {
        GameManager.Instance.GameResume();

        _openInventory = false;
        ClearInventoryItems();
        inventoryUi.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        
    }

    private void ShowInventory()
    {
        GameManager.Instance.GamePause();

        _openInventory = true;
        inventoryUi.SetActive(true);

        HideInspect();
        RefreshInventoryItems();
    }
}