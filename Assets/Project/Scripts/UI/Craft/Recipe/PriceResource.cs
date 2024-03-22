using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UndergroundFortress.UI.Craft.Recipe
{
    public class PriceResource : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text textInStock;
        [SerializeField] private TMP_Text textRequired;

        private int _itemId;
        private int _inStock;
        private int _required;

        private bool _isInit;
        private bool _isEnough;

        public int ItemId => _itemId;
        public int InStock => _inStock;
        public int Required => _required;

        public bool IsEnough => _isEnough;
        public bool IsInit => _isInit;

        public void Init(in int itemId, Sprite iconSprite, in int required)
        {
            icon.sprite = iconSprite;

            _itemId = itemId;
            _required = required;
            textRequired.text = _required.ToString();

            _isInit = true;
        }

        public void UpdateInStock(in int inStock)
        {
            _inStock = inStock;
            _isEnough = _inStock >= _required;
            
            textInStock.color = _isEnough ? Color.white : Color.red;
            textInStock.text = _inStock.ToString();
        }
    }
}
