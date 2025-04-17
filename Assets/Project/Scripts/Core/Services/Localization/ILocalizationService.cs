using System;
using System.Collections;
using TMPro;

using UndergroundFortress.Core.Services;

namespace UndergroundFortress.Core.Localization
{
    public interface ILocalizationService : IService
    {
        public event Action OnUpdateLocale;
        public void UpdateLocale(int localeId);
        public IEnumerator Initialize();
        public string LocaleMain(string keyValue, TMP_Text textObject = null);
        public string LocaleResource(string keyValue, TMP_Text textObject = null);
        public string LocaleEquipment(string keyValue, TMP_Text textObject = null);
        public string LocaleBonus(string keyValue, TMP_Text textObject = null);
        public string LocalePurchase(string keyValue, TMP_Text textObject = null);
        public string LocaleStat(string keyValue, TMP_Text textObject = null);
        public string LocaleSkill(string keyValue, TMP_Text textObject = null);
        public string LocaleTutorial(string keyValue, TMP_Text textObject = null);
    }
}