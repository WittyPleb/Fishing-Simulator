using UnityEngine;

namespace LifeSkills.Fishing
{
    [CreateAssetMenu(fileName = "New Fish", menuName = "Life Skills/Fishing/New Fish", order = 0)]
    public class Fish : ScriptableObject
    {
        public string Name;
        public Sprite LockedIcon;
        public Sprite UnlockedIcon;
        public int ExperienceGiven;
        public Color UnlockedColor;
        public Color LockedColor;
    }
}