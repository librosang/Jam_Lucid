using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameMangerRoom : MonoBehaviour
{
    [SerializeField] private PlayerInteract playerInterct;
    [SerializeField] private Volume volume;
    
    private DepthOfField _depthOfField;
    private ColorAdjustments _colorAdjustments;
    private Vignette _vignette;
    private FirstPersonController _firstPersonController;
    public static GameMangerRoom Instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

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
    }

    void Start()
    {
        _firstPersonController = playerInterct.GetComponent<FirstPersonController>();
        if (volume.profile.TryGet(out _depthOfField))
        {
            _depthOfField.active = false; // Disable Depth of Field initially
        }

        if (volume.profile.TryGet(out _colorAdjustments))
        {
            _colorAdjustments.active = false; // Disable Depth of Field initially
        }

        if (volume.profile.TryGet(out _vignette))
        {
            _vignette.active = false; // Disable Depth of Field initially
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
