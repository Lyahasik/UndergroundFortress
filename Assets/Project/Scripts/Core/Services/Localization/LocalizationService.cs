using System.Collections;
using TMPro;
using UnityEngine.Localization.Settings;

using UndergroundFortress.Constants;

namespace UndergroundFortress.Core.Localization
{
    public class LocalizationService : ILocalizationService
    {
        public IEnumerator Initialize()
        {
            yield return LocalizationSettings.InitializationOperation;
        }
        
        public string LocaleMain(string keyValue) =>
            LocalizationSettings
                .StringDatabase
                .GetLocalizedString(ConstantValues.LOCALE_MAIN_TABLE, keyValue);
        
        public string LocaleResource(string keyValue) =>
            LocalizationSettings
                .StringDatabase
                .GetLocalizedString(ConstantValues.LOCALE_RESOURCES_TABLE, keyValue);
        
        public string LocaleEquipment(string keyValue) =>
            LocalizationSettings
                .StringDatabase
                .GetLocalizedString(ConstantValues.LOCALE_EQUIPMENTS_TABLE, keyValue);
        
        public string LocaleBonus(string keyValue) =>
            LocalizationSettings
                .StringDatabase
                .GetLocalizedString(ConstantValues.LOCALE_BONUSES_TABLE, keyValue);
        
        public string LocalePurchase(string keyValue) =>
            LocalizationSettings
                .StringDatabase
                .GetLocalizedString(ConstantValues.LOCALE_PURCHASES_TABLE, keyValue);
        
        public string LocaleStat(string keyValue) =>
            LocalizationSettings
                .StringDatabase
                .GetLocalizedString(ConstantValues.LOCALE_STATS_TABLE, keyValue);
        
        public string LocaleSkill(string keyValue) =>
            LocalizationSettings
                .StringDatabase
                .GetLocalizedString(ConstantValues.LOCALE_SKILLS_TABLE, keyValue);
        
        public string LocaleTutorial(string keyValue) =>
            LocalizationSettings
                .StringDatabase
                .GetLocalizedString(ConstantValues.LOCALE_TUTORIAL_TABLE, keyValue);
        
        public void LocaleResourceAsync(TMP_Text textObject, string keyValue)
        {
            var op = LocalizationSettings
                .StringDatabase
                .GetLocalizedStringAsync(ConstantValues.LOCALE_RESOURCES_TABLE, keyValue);
            if (op.IsDone)
                textObject.text = op.Result;
            else
                op.Completed += data => textObject.text = data.Result;
        }
    }
}