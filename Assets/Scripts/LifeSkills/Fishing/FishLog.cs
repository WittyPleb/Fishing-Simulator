using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace LifeSkills.Fishing
{
    public class FishLog : MonoBehaviour
    {
        [SerializeField] List<Fish> availableFish;
    
        [Header("References")]
        [SerializeField] TMP_Text caughtText;
        [SerializeField] GameObject fishSlotPrefab;
        [SerializeField] RectTransform slotParent;

        [Header("Events")]
        public UnityEvent<Fish> onFishUnlocked;
        public UnityEvent<Fish> onFishAdded;

        readonly List<FishSlot> _fishSlots = new List<FishSlot>();
        readonly List<FishStore> _fishStorage = new List<FishStore>();
    
        void Awake()
        {
            Setup();
        }

        void Setup()
        {
            // Clear everything
            foreach (Transform child in slotParent.transform)
            {
                Destroy(child.gameObject);
            }
        
            _fishSlots.Clear();
            _fishStorage.Clear();
        
            foreach (Fish fish in availableFish)
            {
                // Add all available fish to the storage so we can keep track of them
                _fishStorage.Add(new FishStore(fish));
            
                // Create the fish slot in the UI
                FishSlot newFishSlot = Instantiate(fishSlotPrefab, slotParent).GetComponent<FishSlot>();
            
                // Setup the newly made fish slot with fish information
                newFishSlot.Setup(fish);
            
                // Add the new fish slot to our list of fish slots
                _fishSlots.Add(newFishSlot);
            }
        }

        int GetTotalCaught()
        {
            return _fishStorage.Sum(fish => fish.GetQuantity());
        }

        public void AddToLog(Fish caughtFish)
        {
            // Get the fish from our storage of fish
            FishStore fishStore = _fishStorage.FirstOrDefault(fishStore => fishStore.GetFish() == caughtFish);

            // If we could not find the correct fish in our storage, stop running.
            if (fishStore == null) return;
        
            // Get the fishing slot that corresponds to the fish we want to unlock
            FishSlot fishSlot = _fishSlots.FirstOrDefault(slot => slot.GetFish() == caughtFish);

            // If we could not find the fish slot, stop running.
            if (fishSlot == null) return;

            // Unlock fish if it isn't already unlocked
            if (!fishStore.IsUnlocked())
            {
                fishStore.Unlock();
                fishSlot.Unlock(caughtFish);
                onFishUnlocked?.Invoke(caughtFish);
            }
        
            // Increase the quantity of this fish
            fishStore.IncreaseQuantity();

            // Invoke onFishAdded event and pass the fish we caught
            onFishAdded?.Invoke(caughtFish);
        
            // Update the slot's UI quantity
            fishSlot.UpdateQuantity(fishStore.GetQuantity());
        
            // Update our caught count text
            caughtText.SetText($"Fish Caught: {GetTotalCaught()}");
        }

        [System.Serializable]
        class FishStore
        {
            Fish _fish;
        
            bool _unlocked;
            int _quantity;

            public FishStore(Fish fish)
            {
                _fish = fish;
                _unlocked = false;
                _quantity = 0;
            }

            public void IncreaseQuantity(int amountToAdd = 1)
            {
                _quantity += amountToAdd;
            }

            public int GetQuantity()
            {
                return _quantity;
            }

            public Fish GetFish()
            {
                return _fish;
            }

            public bool IsUnlocked()
            {
                return _unlocked;
            }

            public void Unlock()
            {
                _unlocked = true;
            }
        }
    }
}