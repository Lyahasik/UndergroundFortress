using System.Runtime.InteropServices;

using UndergroundFortress.Helpers;

#if UNITY_WEBGL
namespace UndergroundFortress.Core.Publish.Web.Yandex
{
    public class YandexDataModule : DataModule
    {
        [DllImport("__Internal")]
        private static extern void LoadDataExtern();
        [DllImport("__Internal")]
        private static extern void SaveDataExtern(string data);
    
        [DllImport("__Internal")]
        private static extern void SetLeaderBoardExtern(int value);
    
        public override void StartLoadData()
        {
            LoadDataExtern();
        }

        public override void SaveData(string data)
        {
            if (OSManager.IsEditor())
                return;
            
            SaveDataExtern(data);
        }

        public override void SetLeaderBoard(int value)
        {
            if (OSManager.IsEditor())
                return;
        
            SetLeaderBoardExtern(value);
        }
    }
}
#endif