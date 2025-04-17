using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

using UndergroundFortress.Constants;

namespace UndergroundFortress.Core.Localization
{
    public class LocalizationService : ILocalizationService
    {
        public event Action OnUpdateLocale;

        public IEnumerator Initialize()
        {
            yield return LocalizationSettings.InitializationOperation;
            
            Debug.Log($"[{ GetType() }] initialize");
        }

        public void UpdateLocale(int localeId)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
            
            OnUpdateLocale?.Invoke();
        }

        public string LocaleMain(string keyValue, TMP_Text textObject) => 
            LocaleResourceAsync(ConstantValues.LOCALE_MAIN_TABLE, keyValue, textObject);

        public string LocaleResource(string keyValue, TMP_Text textObject) => 
            LocaleResourceAsync(ConstantValues.LOCALE_RESOURCES_TABLE, keyValue, textObject);
        
        public string LocaleEquipment(string keyValue, TMP_Text textObject) => 
            LocaleResourceAsync(ConstantValues.LOCALE_EQUIPMENTS_TABLE, keyValue, textObject);
        
        public string LocaleBonus(string keyValue, TMP_Text textObject) => 
            LocaleResourceAsync(ConstantValues.LOCALE_BONUSES_TABLE, keyValue, textObject);
        
        public string LocalePurchase(string keyValue, TMP_Text textObject) => 
            LocaleResourceAsync(ConstantValues.LOCALE_PURCHASES_TABLE, keyValue, textObject);
        
        public string LocaleStat(string keyValue, TMP_Text textObject) => 
            LocaleResourceAsync(ConstantValues.LOCALE_STATS_TABLE, keyValue, textObject);
        
        public string LocaleSkill(string keyValue, TMP_Text textObject) => 
            LocaleResourceAsync(ConstantValues.LOCALE_SKILLS_TABLE, keyValue, textObject);
        
        public string LocaleTutorial(string keyValue, TMP_Text textObject) => 
            LocaleResourceAsync(ConstantValues.LOCALE_TUTORIAL_TABLE, keyValue, textObject);
        
        private string LocaleResourceAsync(string tableName, string keyValue, TMP_Text textObject = null)
        {
            if (textObject != null)
            {
                var op = LocalizationSettings
                    .StringDatabase
                    .GetLocalizedStringAsync(tableName, keyValue);
                if (op.IsDone)
                    textObject.text = op.Result;
                else
                    op.Completed += data => textObject.text = data.Result;
            }
            else
            {
                return LocalizationSettings.StringDatabase.GetLocalizedString(tableName, keyValue);
            }
            
            return null;
        }
    }
}