using StarterAssets;
using UnityEngine;

public class playerSoundManager : MonoBehaviour
{
    [Header("Footstep Sounds")]
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private float walkStepInterval = 0.5f;
    [SerializeField] private float sprintStepInterval = 0.3f;
    [SerializeField] private float velocityThreshold = 2.0f;
    
    private float nextStepTime;
    private StarterAssetsInputs _input;
    private FirstPersonController _player;
    private int lastPlayedIndex = -1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   private void Start()
    {
        _input = GetComponent<StarterAssetsInputs>(); 
        _player = GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
   private void Update()
   {
       HandleFootsteps();
       
   }

   private bool IsPlayerMoving()
   {
       return (_input.move != Vector2.zero);
   }

   private float GetPlayerMagnitude()
   {
       return _input.move.magnitude;
   }

   private void HandleFootsteps()
   {
       float currentStepInterval = _input.sprint ? sprintStepInterval : walkStepInterval;
       if (_player.Grounded && IsPlayerMoving() && Time.time >= nextStepTime && GetPlayerMagnitude() > velocityThreshold)
       {
           PlayerFootstepSounds();
           nextStepTime = Time.time + currentStepInterval;
       }
   }

   private void PlayerFootstepSounds()
   {
       
       int randomIndex;
       if (footstepSounds.Length == 1)
       {
           randomIndex = 0;
       }
       else
       {
           randomIndex = Random.Range(0, footstepSounds.Length - 1);
           if (randomIndex >= lastPlayedIndex)
           {
               randomIndex++;
           }
           
       }
       lastPlayedIndex = randomIndex;
       footstepSource.clip = footstepSounds[randomIndex];
       footstepSource.Play();
   }
}
