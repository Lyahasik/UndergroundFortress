using UnityEditor;
using UnityEngine;

using UndergroundFortress.Gameplay.StaticData;
using UndergroundFortress.Helpers.GeneratorId;

namespace UndergroundFortress.Helpers.CreateItemWindowEditor.Editor
{
    public class CreateItemsDataWindow : EditorWindow
    {
        private const string BaseName = "Item";

        private const string PathGeneratedIdsData = "StaticData/GeneratedIdsData";
        private const string PathMain = "Assets/Project/Resources/StaticData/";
        private const string PathResource = "Items/Resource/";
        private const string PathEquipment = "Items/Equipment/";
        private const string PathRecipe = "Recipes/Recipe";

        private string _itemName = BaseName;
        private ItemStaticDataType _itemType;

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
            
            ItemStaticData itemAsset = CreateItemAsset();
            TryCreateRecipeAsset(itemAsset);

            AssetDatabase.SaveAssets();


            EditorUtility.FocusProjectWindow();

            Selection.activeObject = itemAsset;
        }

        private ItemStaticData CreateItemAsset()
        {
            switch (_itemType)
            {
                case ItemStaticDataType.Resource:
                    return CreateItemAsset<ResourceStaticData>(PathResource);
                case ItemStaticDataType.Consumable:
                    return CreateItemAsset<ConsumableStaticData>(PathResource);
                case ItemStaticDataType.Equipment:
                    return CreateItemAsset<EquipmentStaticData>(PathEquipment);
            }

            return null;
        }

        private ItemStaticData CreateItemAsset<T>(string pathItem) where T : ItemStaticData
        {
            var generatedIdsStaticData = Resources.Load<GeneratedIdsStaticData>(PathGeneratedIdsData);
            
            string fullName = $"{PathMain}{pathItem}{_itemName}_Data.asset";
            T itemData = CreateInstance<T>();
            itemData.id = UndergroundFortress.Helpers.GeneratorId.GeneratorId.GenerateUnique(generatedIdsStaticData, _itemType);
            
            AssetDatabase.CreateAsset(itemData, fullName);

            return itemData;
        }

        private void TryCreateRecipeAsset(ItemStaticData itemAsset)
        {
            if (_itemType != ItemStaticDataType.Equipment
                && _itemType != ItemStaticDataType.Consumable)
                return;
            
            string fullName = $"{PathMain}{PathRecipe}{_itemName}_Data.asset";
            RecipeStaticData recipeData = CreateInstance<RecipeStaticData>();
            recipeData.itemData = itemAsset;
            
            AssetDatabase.CreateAsset(recipeData, fullName);
        }
    }
}