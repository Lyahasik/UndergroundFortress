using UnityEngine;

namespace UndergroundFortress.Gameplay.StaticData
{
    [CreateAssetMenu(fileName = "RecipeData", menuName = "Static data/Recipe")]
    public class RecipeStaticData : ScriptableObject
    {
        public int id;
        public int idItem;
    }
}