namespace LifeSkills
{
    [System.Serializable]
    public class LifeSkill
    {
        public string Name;

        int _currentExperience;

        public LifeSkill(string skillName)
        {
            Name = skillName;
        }

        public void AddExperience(int amountToAdd)
        {
            _currentExperience += amountToAdd;
        }

        public int GetCurrentExperience()
        {
            return _currentExperience;
        }
    }
}