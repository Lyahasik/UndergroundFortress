using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UndergroundFortress.Gameplay.Stats;
using UndergroundFortress.Extensions;
using UndergroundFortress.Gameplay.Inventory.Services;

namespace UndergroundFortress.UI.Inventory
{
    [RequireComponent(typeof(RectTransform))]
    public class CellBagView : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image quality;
        [SerializeField] private TMP_Text numberText;
        
        private int _id;
        private IMovingItemService _movingItemService;

        private RectTransform _rect;

        public Sprite Icon => icon.sprite;
        public Sprite Quality => quality.sprite;
        public string Number => numberText.text;

        public int Id => _id;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

        public void Construct(in int cellId, IMovingItemService movingItemService)
        {
            _id = cellId;
            _movingItemService = movingItemService;
        }

        public void Initialize()
        {
            gameObject.name = nameof(CellBagView) + _id;
            
            Reset();
        }

        public void Subscribe(ActiveArea activeArea)
        {
            activeArea.OnDown += Hit;
            activeArea.OnUp += Hit;
        }

        public void SetValues(Sprite icon,
            in QualityType qualityType,
            in int number)
        {
            this.icon.sprite = icon;
            quality.sprite = null;
            numberText.text = number.ToString();
        }

        public void SetValues(Sprite icon,
            Sprite quality,
            string number)
        {
            this.icon.sprite = icon;
            this.quality.sprite = quality;
            numberText.text = number;
        }

        public void Show()
        {
            icon.gameObject.SetActive(true);
            quality.gameObject.SetActive(true);
            numberText.gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            icon.gameObject.SetActive(false);
            quality.gameObject.SetActive(false);
            numberText.gameObject.SetActive(false);
        }

        public void Reset()
        {
            icon.sprite = null;
            quality.sprite = null;
            numberText.text = string.Empty;
        }

        public void Hit(Vector3 position)
        {
            if (!_rect.IsDotInside(position))
                return;
            
            _movingItemService.AddItem(this, position);
        }

        public static bool operator ==(CellBagView value1, CellBagView value2)
        {
            if (value1 is null)
                return value2 is null;
                
            return  value2 is not null && value1._id == value2._id;
        }

        public static bool operator !=(CellBagView value1, CellBagView value2) => 
            !(value1 == value2);
    }
}