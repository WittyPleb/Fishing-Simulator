using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LifeSkills.Fishing
{
    public class FishSlot : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] TMP_Text nameText;
        [SerializeField] TMP_Text quantityText;
    
        Fish _fish;

        public void Setup(Fish fish)
        {
            _fish = fish;
            quantityText.enabled = false;
            icon.sprite = _fish.LockedIcon;
            icon.color = _fish.LockedColor;
            nameText.SetText(_fish.Name);
            quantityText.SetText($"0");
        }

        public Fish GetFish()
        {
            return _fish;
        }

        public void Unlock(Fish fish)
        {
            icon.sprite = fish.UnlockedIcon;
            icon.color = fish.UnlockedColor;
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity > 99)
            {
                quantityText.SetText("99+"); return;
            }
        
            quantityText.SetText($"{newQuantity}");
            quantityText.enabled = newQuantity > 1;
        }
    }
}