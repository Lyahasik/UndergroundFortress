using System.Collections;
using TMPro;

using UndergroundFortress.Core.Services;

namespace UndergroundFortress.Core.Localization
{
    public interface ILocalizationService : IService
    {
        public IEnumerator Initialize();
        public string LocaleMain(string keyValue);
        public string LocaleResource(string keyValue);
        public string LocaleEquipment(string keyValue);
        public string LocaleBonus(string keyValue);
        public string LocalePurchase(string keyValue);
        public string LocaleStat(string keyValue);
        public string LocaleSkill(string keyValue);
        public string LocaleTutorial(string keyValue);
        public void LocaleResourceAsync(TMP_Text textObject, string keyValue);
    }
}