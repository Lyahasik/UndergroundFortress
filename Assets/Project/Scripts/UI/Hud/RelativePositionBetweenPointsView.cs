using UnityEngine;

namespace UndergroundFortress.Gameplay.Character
{
    public class RelativePositionBetweenPointsView : MonoBehaviour
    {
        [SerializeField] private Transform marker;
        [SerializeField] private Transform point1;
        [SerializeField] private Transform point2;

        public void UpdatePosition(float t)
        {
            marker.position = Vector3.Lerp(point1.position, point2.position, t);
        }
    }
}