using System;
using System.Collections.Generic;
using System.Numerics;

namespace Raytracer
{
    public class Functions
    {
        public static Random Rand = new Random();
        
        public static float Deg2Rad(float angle)
        {
            return (float) Math.PI * angle / 180.0f;
        }
        
        public static uint Vec2Col(Vector3 col)
        {
            byte r = (byte) ((col.X / 2 + 0.5) * 255);
            byte g = (byte) ((col.Y / 2 + 0.5) * 255);
            byte b = (byte) ((col.Z / 2 + 0.5) * 255);
            return (uint) (255 << 24 | r << 16 | g << 8 | b);
        }

        public static (float, float) QuadraticSolver(float a, float b, float c)
        {
            float t0, t1;
            float d = b * b - 4 * a * c;

            if (d < 0)
            {
                return (-2, -1);
            }
            else if (d == 0)
            {
                t0 = -b / (2 * a);
                t1 = t0;
            }
            else
            {
                t0 = -b + (float) Math.Sqrt(d) / (2 * a);
                t1 = -b - (float) Math.Sqrt(d) / (2 * a);
            }
            return (t0, t1);
        }
        
        static void MultVecMatrix(Matrix4x4 matrix, Vector3 src, ref Vector3 dst)
        {
            float a = src.X * matrix.M11 + src.Y * matrix.M21 + src.Z * matrix.M31 + matrix.M41;
            float b = src.X * matrix.M12 + src.Y * matrix.M22 + src.Z * matrix.M32 + matrix.M42;
            float c = src.X * matrix.M13 + src.Y * matrix.M23 + src.Z * matrix.M33 + matrix.M43;
            float w = src.X * matrix.M14 + src.Y * matrix.M24 + src.Z * matrix.M34 + matrix.M44;
            
            dst.X = a / w;
            dst.Y = b / w;
            dst.Z = c / w;
        }
        
        static void MultDirMatrix(Matrix4x4 matrix, Vector3 src, ref Vector3 dst) 
        {
            float a = src.X * matrix.M11 + src.Y * matrix.M21 + src.Z * matrix.M31;
            float b = src.X * matrix.M12 + src.Y * matrix.M22 + src.Z * matrix.M32;
            float c = src.X * matrix.M13 + src.Y * matrix.M23 + src.Z * matrix.M33;
            
            dst.X = a; 
            dst.Y = b; 
            dst.Z = c;
        }
        
        public static void Render(
            ref uint[] framebuffer,
            int width, int height,
            Matrix4x4 cameraToWorld, float fov,
            ref List<Shape> objects)
        {
            float scale = (float) Math.Tan(Deg2Rad(fov * 0.5f));
            float ratio = width / (float) height;
//            int rayAmount = 20;
            
            Vector3 origin = new Vector3(0, 0, 0);
            MultVecMatrix(cameraToWorld, Vector3.Zero, ref origin);
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
//                    Vector3 acc = Vector3.Zero;
//                    for (int k = 0; k < rayAmount; k++)
                    {
                        float x = (2 * (i + 0.5f/* + (float) Rand.NextDouble()*/) / width - 1) * ratio * scale;
                        float y = (1 - 2 * (j + 0.5f/* + (float) Rand.NextDouble()*/) / height) * scale;
                        float z = -1.0f;

                        Vector3 dir = new Vector3(x, y, z);
                        MultDirMatrix(cameraToWorld, new Vector3(x, y, z), ref dir);
                        Ray ray = new Ray(origin, Vector3.Normalize(dir));
                        foreach (Shape shape in objects)
                        {
                            var (hit, hitp, normal) = shape.Intersect(ray);
                            if (hit)
                            {
//                                acc += normal;
//                                acc += (dir + Vector3.One) * 0.5f;
                                switch (shape)
                                {
                                    case Triangle t:
                                        framebuffer[j * width + i] = t.Vec2Col(normal);
                                        break;
                                    
                                    default:
                                        framebuffer[j * width + i] = Vec2Col(normal);
//                                        framebuffer[j * width + i] = Vec2Col((dir + Vector3.One) * 0.5f);
                                        break;
                                }
                            }
                        } 
                    }
//                    framebuffer[j * width + i] = Vec2Col(acc / rayAmount);
                }
            }
        }
    }
}
