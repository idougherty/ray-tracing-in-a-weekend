using System;
using System.Collections.Generic;

namespace ray_tracing_in_a_weekend {
    struct HitRecord {
        public Vec3 p;
        public Vec3 normal;
        public Material mat;
        public double t;
        public bool frontFace;

        public void SetFaceNormal(Ray r, Vec3 outNormal) {
            frontFace = r.dir.Dot(outNormal) < 0;
            normal = frontFace ? outNormal : -1 * outNormal;
        }
    };

    class Ray {
        public Vec3 origin;
        public Vec3 dir;

        public Ray(Vec3 origin, Vec3 dir) {
            this.origin = origin;
            this.dir = dir;
        }

        public Vec3 At(double t) {
            return origin + dir * t;
        }

        public Vec3 Trace(List<Hittable> objects, double envBrightness, int depth) {
            HitRecord closest = new HitRecord();
            closest.t = Double.PositiveInfinity;

            if(depth <= 0)
                return new Vec3(0, 0, 0);

            foreach(Hittable obj in objects) {
                HitRecord rec = new HitRecord();
                
                if(obj.Hit(this, 0.001, 100, ref rec)) {
                    if(rec.t < closest.t)
                        closest = rec;
                }
            }

            if(Double.IsFinite(closest.t)) {
                Ray scattered;
                Vec3 attenuation;

                if(closest.mat.Scatter(this, closest, out attenuation, out scattered))
                    return attenuation * scattered.Trace(objects, envBrightness, depth - 1);
                
                return attenuation;
            }

            return BgColor(envBrightness);
        }

        public Vec3 BgColor(double brightness) {
            Vec3 unit_dir = this.dir.Normalize();
            double t = .5 * (unit_dir.y + 1);

            Vec3 color = new Vec3(.5, .7, 1) * brightness;

            return (1 - t) * color * 1.5 + t * color;
        }
    }
}