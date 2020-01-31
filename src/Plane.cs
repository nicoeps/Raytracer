using  System.Numerics;

namespace Raytracer
{
    public class Plane : Shape
    {
        public Vector3 Point;
        public Vector3 Normal;
        
        public Plane(Vector3 point, Vector3 normal)
        {
            Point = point;
            Normal = normal;
            Color = normal;
        }
        
        public override (bool, Vector3, Vector3) Intersect(Ray ray)
        {
            float denom = Vector3.Dot(Normal, ray.Direction);
            if (denom > 1e-6) {
                float t = Vector3.Dot(Point - ray.Origin, Normal) / denom;
                return (t >= 0, ray.Origin + ray.Direction * t, Normal);
            } 

            return (false, Vector3.Zero, Vector3.Zero);
        }
    }
}
