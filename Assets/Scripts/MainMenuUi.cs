using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUi : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        
        playButton.onClick.AddListener(() => { SceneManager.LoadScene(1); });
        
        quitButton.onClick.AddListener(() => { Application.Quit(); });
        
        
    }
}