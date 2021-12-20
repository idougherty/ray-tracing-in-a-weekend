using System;
using System.Collections.Generic;

namespace ray_tracing_in_a_weekend {
    class Environments {
        public static Material RandomMaterial() {
            if(Util.RandomDouble() < .5) {
                return new Lambertian(new Vec3(Util.RandomDouble(0, .8), Util.RandomDouble(0, .8), Util.RandomDouble(0, .8)));
            } else {
                return new Metal(new Vec3(Util.RandomDouble(0, .8), Util.RandomDouble(0, .8), Util.RandomDouble(0, .8)), Util.RandomDouble());
            }
        }

        public static (List<Hittable>, Camera, double) BuildRandom(double aspectRatio) {
            List<Hittable> objects = new List<Hittable>();

            double worldRadius = 50;
            
            // Generate random spheres
            for(int r = 1; r < 5; r++) {
                int numBalls = 3 * (r + 1);
                double start = Math.PI * Util.RandomDouble();

                for(double t = start; t < 2 * Math.PI + start - .05; t += 2 * Math.PI / numBalls) {
                    double dist = Util.RandomDouble(1.2 * r, .8 * r);

                    double radius = .01 / (r + 1) * 80;
                    radius = Util.RandomDouble(radius * .9, radius * 1.1);

                    double x = Math.Cos(t) * Math.Sqrt(r) * 3;
                    double z = Math.Sin(t) * Math.Sqrt(r) * 3;
                    double y = radius + Math.Sqrt(worldRadius * worldRadius - x * x - z * z) - worldRadius;

                    Vec3 center = new Vec3(x, y, z);

                    Material mat = RandomMaterial();

                    objects.Add(new Sphere(center, radius, mat));
                }
            }

            // Center mirror ball
            Material mirror = new Metal(new Vec3(.75, .85, .85));
            objects.Add(new Sphere(new Vec3(0, 1, 0), 1, mirror));

            // Ground sphere
            Material groundMaterial = new Lambertian(new Vec3(.2, .15, .35));
            objects.Add(new Sphere(new Vec3(0, -worldRadius, 0), worldRadius, groundMaterial));

            Vec3 origin = new Vec3(4,  4, 20);
            Vec3 lookAt = new Vec3(.16, .75,  .8);
            Camera cam = new Camera(origin, lookAt, aspectRatio, 25, .5);

            return (objects, cam, 1);
        }

        public static (List<Hittable>, Camera, double) Build4Ball(double aspectRatio) {
            List<Hittable> objects = new List<Hittable>();
            
            Material mat1 = new Metal(new Vec3(.9, .3, .3), .25);
            objects.Add(new Sphere(new Vec3(0, .7, 0), .7, mat1));
            
            Material mat2 = new Metal(new Vec3(.8, .8, .3), .5);
            objects.Add(new Sphere(new Vec3(0, .8, 1.5), .8, mat2));
            
            Material mat3 = new Metal(new Vec3(.3, .9, .3), .75);
            objects.Add(new Sphere(new Vec3(0, .85, 3.2), .9, mat3));
            
            Material mat4 = new Metal(new Vec3(.3, .3, .9), 1);
            objects.Add(new Sphere(new Vec3(0,  .9, 5.1), 1, mat4));

            Material mat5 = new Metal(new Vec3(.2, .2, .2), 0);
            objects.Add(new Sphere(new Vec3(0, -200, 0), 200, mat5));

            Vec3 origin = new Vec3(10,  4, -12);
            Vec3 lookAt = new Vec3(1.5, .75,  0);
            Camera cam = new Camera(origin, lookAt, aspectRatio, 25, .5);

            return (objects, cam, 1);
        }

        public static (List<Hittable>, Camera, double) BuildLights(double aspectRatio) {
            List<Hittable> objects = new List<Hittable>();
            
            Material mat1 = new DiffuseLight(new Vec3(6, 6, 3));
            objects.Add(new Sphere(new Vec3(0, .25, 0), .25, mat1));
            
            Material mat2 = new Lambertian(new Vec3(.9, .6, .2));
            objects.Add(new Sphere(new Vec3(-1.2, .8, .5), .8, mat2));
            objects.Add(new Sphere(new Vec3(   0, .8, -1), .8, mat2));
            objects.Add(new Sphere(new Vec3( 1.2, .8, .5), .8, mat2));

            Material mat3 = new Metal(new Vec3(.5, .5, .5), .5);
            objects.Add(new Sphere(new Vec3(0, -200, 0), 200, mat3));

            Vec3 origin = new Vec3(0,  2, 10);
            Vec3 lookAt = new Vec3(0, .75,  0);
            Camera cam = new Camera(origin, lookAt, aspectRatio, 25, .5);

            return (objects, cam, 0.1);
        }
    }
}