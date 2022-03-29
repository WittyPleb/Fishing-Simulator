using System.Collections.Generic;
using UnityEngine;

namespace LifeSkills.Fishing
{
    public class FishingSpot : MonoBehaviour
    {
        [SerializeField] List<Fish> availableFish;

        PlayerFisher _playerFisher;

        void Awake()
        {
            _playerFisher = FindObjectOfType<PlayerFisher>();
        }

        public void SetFishSpot()
        {
            _playerFisher.SetFishingSpot(this);
        }

        public List<Fish> GetAvailableFish()
        {
            return availableFish;
        }
    }
}
