using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace LifeSkills
{
    public class LifeSkillManager : MonoBehaviour
    {
        [SerializeField] List<LifeSkill> lifeSkills;
    
        [Header("References")]
        [SerializeField] TMP_Text experienceText;
        [SerializeField] UnityEvent onLifeSkillExperienceGained;

        public void GainLifeSkillExperience(string lifeSkillName, int amountToGain)
        {
            LifeSkill lifeSkill = lifeSkills.FirstOrDefault(ls => ls.Name == lifeSkillName);

            if (lifeSkill != null)
            {
                lifeSkill.AddExperience(amountToGain);
                onLifeSkillExperienceGained?.Invoke();
                experienceText.SetText($"Experience: {lifeSkill.GetCurrentExperience():N0}");
            }
        }
    }
}
