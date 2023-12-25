using UnityEngine;

namespace UndergroundFortress.Scripts.Extensions
{
    public static class RectTransformExtensions
    {
        public static bool IsDotInside(this RectTransform rect, Vector3 point)
        {
            Vector3[] vertices = new Vector3[4];
            rect.GetWorldCorners(vertices);

            return PointBetweenTwo(point, LeftDownVertex(vertices), RightUpVertex(vertices));
        }
        
        public static bool IsIntersectingRectangles(this RectTransform rect1, RectTransform rect2)
        {
            Vector3[] vertices1 = new Vector3[4];
            rect1.GetWorldCorners(vertices1);
            
            Vector3[] vertices2 = new Vector3[4];
            rect2.GetWorldCorners(vertices2);

            return PointBetweenTwo(LeftDownVertex(vertices1), LeftDownVertex(vertices2), RightUpVertex(vertices2))
                || PointBetweenTwo(LeftUpVertex(vertices1), LeftDownVertex(vertices2), RightUpVertex(vertices2))
                || PointBetweenTwo(RightUpVertex(vertices1), LeftDownVertex(vertices2), RightUpVertex(vertices2))
                || PointBetweenTwo(RightDownVertex(vertices1), LeftDownVertex(vertices2), RightUpVertex(vertices2));
        }

        private static bool PointBetweenTwo(in Vector3 point, in Vector3 leftDownPoint, in Vector3 rightUpPoint) =>
            FirstHigherSecond(point, leftDownPoint)
            && FirstRightSecond(point, leftDownPoint)
            && FirstHigherSecond(rightUpPoint, point)
            && FirstRightSecond(rightUpPoint, point);

        private static bool FirstHigherSecond(in Vector3 first, in Vector3 second) => 
            first.y >= second.y;

        private static bool FirstRightSecond(in Vector3 first, in Vector3 second) =>
            first.x >= second.x;
        
        private static Vector3 LeftDownVertex(in Vector3[] vectors) => 
            vectors[0];
        
        private static Vector3 LeftUpVertex(in Vector3[] vectors) => 
            vectors[1];
        
        private static Vector3 RightUpVertex(in Vector3[] vectors) => 
            vectors[2];
        
        private static Vector3 RightDownVertex(in Vector3[] vectors) => 
            vectors[3];
    }
}