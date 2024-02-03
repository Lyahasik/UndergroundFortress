using UnityEditor;
using UnityEngine;

using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Helpers.GeneratorId;

namespace UndergroundFortress.Helpers.CreateItemWindowEditor.Editor
{
    public class CreateItemsDataWindow : EditorWindow
    {
        private const string BaseName = "ItemData";

        private const string PathGeneratedIdsData = "StaticData/GeneratedIdsData";
        private const string PathMain = "Assets/Project/Resources/StaticData/Items/";
        private const string PathResource = "Resource/";
        private const string PathEquipment = "Equipment/";

        private string _itemName = BaseName;
        private ItemStaticDataType _itemType;

        private string _fullName;

        [MenuItem("Window/Create Items")]
        public static void ShowWindow()
        {
            GetWindow<CreateItemsDataWindow>("Create items");
        }

        public void OnGUI()
        {
            GUILayout.Label("Item static data");
            
            _itemName = EditorGUILayout.TextField("Item name", _itemName);
            _itemType = (ItemStaticDataType) EditorGUILayout.EnumPopup("Item type", _itemType);

            if (GUILayout.Button("Create"))
            {
                if (_itemType != ItemStaticDataType.Empty) 
                    CreateItem();
                else
                    Debug.LogWarning("Required fields are not filled in.");
            }
        }

        private void CreateItem()
        {
            if (_itemName == string.Empty)
                _itemName = BaseName;
            
            ScriptableObject asset = CreateAsset();

            AssetDatabase.CreateAsset(asset, _fullName);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }

        private ScriptableObject CreateAsset()
        {
            switch (_itemType)
            {
                case ItemStaticDataType.Resource:
                    return CreateAsset<ResourceStaticData>(PathResource);
                case ItemStaticDataType.Equipment:
                    return CreateAsset<EquipmentStaticData>(PathEquipment);
            }

            return null;
        }

        private ScriptableObject CreateAsset<T>(string pathItem) where T : ItemStaticData
        {
            var generatedIdsStaticData = Resources.Load<GeneratedIdsStaticData>(PathGeneratedIdsData);
            
            _fullName = $"{PathMain}{pathItem}{_itemName}.asset";
            T resourceData = CreateInstance<T>();
            resourceData.id = UndergroundFortress.Helpers.GeneratorId.GeneratorId.GenerateUnique(generatedIdsStaticData, _itemType);
            
            return resourceData;
        }
    }
}