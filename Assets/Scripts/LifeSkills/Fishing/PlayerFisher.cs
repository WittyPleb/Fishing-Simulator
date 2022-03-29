using System.Collections;
using TMPro;
using UnityEngine;

namespace LifeSkills.Fishing
{
    public class PlayerFisher : MonoBehaviour
    {
        [SerializeField] KeyCode interactionKey = KeyCode.F;
        [SerializeField] float maxTime = 4f;
    
        [Header("References")]
        [SerializeField] TMP_Text statusText;
    
        FishLog _fishLog;
        LifeSkillManager _lifeSkillManager;
        FishingSpot _fishingSpot;

        bool _fishing;
        bool _lineCast;
        bool _fishOnHook;
        float _timer;

        Coroutine _startFishingRoutine;

        void Awake()
        {
            _fishLog = FindObjectOfType<FishLog>();
            _lifeSkillManager = GetComponent<LifeSkillManager>();
        }

        void Start()
        {
            statusText.SetText("Status: Idle");
        }

        void Update()
        {
            // There is a fish on the hook
            if (_fishing && _fishOnHook)
            {
                // Count up for fish timer
                _timer += Time.deltaTime;

                // Reached maximum wait time with no input. Fish got away!
                if (_timer >= maxTime)
                {
                    _fishOnHook = false; // Stops timer immediately
                    _lineCast = false; // Set lineCast to false so we cannot cancel fishing
                
                    // Set our status
                    statusText.SetText("Status: Lost fish! Returning to idle...");
                
                    // Reset fishing after waiting a few seconds
                    Invoke(nameof(ResetFishing), 2f);
                }
                else
                {
                    // Pressed interactionKey before our timer reached maxTime
                    if (Input.GetKeyDown(interactionKey))
                    {
                        StartCoroutine(CatchFish());
                    }
                }
            } else if (_fishing && !_fishOnHook && _lineCast)
            {
                // Pressed interactionKey before the fish was able to be hooked, cancel fishing.
                if (Input.GetKeyDown(interactionKey))
                {
                    StartCoroutine(CancelFishing());
                }
            }
        }

        IEnumerator StartFishing()
        {
            // Set fishing to true
            _fishing = true;
        
            // Set our status
            statusText.SetText("Status: Casting line...");
        
            // Wait a little bit before letting the fish be able to bite the hook
            yield return new WaitForSeconds(2f);
        
            // Allow fishing to be cancelled/fish to bite the hook
            _lineCast = true;
        
            // Set our status
            statusText.SetText($"Status: Waiting for fish... Press [{interactionKey}] to cancel");
        
            // Wait a little bit before letting the fish bite
            yield return new WaitForSeconds(5f);
        
            // Make the fish bite
            _fishOnHook = true;
        
            // Set our status
            statusText.SetText($"Status: Fish on hook! Press [{interactionKey}]");
        
            // End this coroutine
            yield return null;
        }

        // This coroutine only gets started if the player presses the interactionKey before maxTime is reached
        IEnumerator CatchFish()
        {
            // Toggle bobber under so the timer stops immediately
            _fishOnHook = false;
        
            // Toggle line cast since our line is no longer casted
            _lineCast = false;
        
            // Pick a random fish from available fish
            int randomIndex = Random.Range(0, _fishingSpot.GetAvailableFish().Count);
        
            // Get a reference to the fish we are catching
            Fish fish = _fishingSpot.GetAvailableFish()[randomIndex];
        
            // Add fish to log
            _fishLog.AddToLog(fish);
        
            // Give the player life skill experience
            _lifeSkillManager.GainLifeSkillExperience("Fishing", fish.ExperienceGiven);
        
            // Tell the player they caught the fish
            statusText.SetText($"Status: Caught {fish.Name}! Returning to idle...");

            // Wait a little bit before resetting everything back
            yield return new WaitForSeconds(2f);
        
            // Reset everything so the player can fish again
            ResetFishing();
        
            // End this coroutine
            yield return null;
        }

        // This coroutine only gets started if the player presses the interactionKey before a fish is on the hook but after the line has been cast
        IEnumerator CancelFishing()
        {
            // Immediately stop the StartFishing coroutine
            StopCoroutine(_startFishingRoutine);
        
            // Toggle fishing off to stop timer (not fishOnHook since it was never true)
            _fishing = false;

            // Set our status
            statusText.SetText("Status: Cancelled fishing! Returning to idle...");
        
            // Wait a little bit before resetting everything back
            yield return new WaitForSeconds(2f);
        
            // Reset everything so the player can fish again
            ResetFishing();
        
            // End this coroutine
            yield return null;
        }

        void ResetFishing()
        {
            _fishingSpot = null;
            _fishOnHook = false;
            _fishing = false;
            _lineCast = false;
            _timer = 0f;
            statusText.SetText("Status: Idle");
        }

        public void SetFishingSpot(FishingSpot spot)
        {
            // Only try to fish if we do not already have a fishing spot
            if (_fishingSpot != null) return;
        
            // Set our current fishing spot
            _fishingSpot = spot;
        
            // Begin fishing
            _startFishingRoutine = StartCoroutine(StartFishing());
        }
    }
}
