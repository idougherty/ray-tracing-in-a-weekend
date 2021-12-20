using System;

namespace ray_tracing_in_a_weekend {
    class Vec3 {

        public double x;
        public double y;
        public double z;

        public Vec3(double x, double y, double z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vec3 operator +(Vec3 A, Vec3 B) {
            return new Vec3(A.x + B.x, A.y + B.y, A.z + B.z);
        }

        public static Vec3 operator -(Vec3 A, Vec3 B) {
            return new Vec3(A.x - B.x, A.y - B.y, A.z - B.z);
        }
        
        public static Vec3 operator *(Vec3 A, Vec3 B) {
            return new Vec3(A.x * B.x, A.y * B.y, A.z * B.z);
        }

        public static Vec3 operator *(Vec3 A, double B) {
            return new Vec3(A.x * B, A.y * B, A.z * B);
        }

        public static Vec3 operator *(double B, Vec3 A) {
            return new Vec3(A.x * B, A.y * B, A.z * B);
        }

        public static Vec3 operator /(Vec3 A, double B) {
            return new Vec3(A.x / B, A.y / B, A.z / B);
        }

        public static Vec3 Random() {
            return new Vec3(Util.RandomDouble() * 2 - 1, Util.RandomDouble() * 2 - 1, Util.RandomDouble() * 2 - 1);
        }

        public static Vec3 RandomInUnitSphere() {
            while(true) {
                Vec3 p = Vec3.Random();
                if(p.Length2() > 1) continue;
                return p;
            }
        }

        public static Vec3 RandomInUnitDisk() {
            while(true) {
                Vec3 p = new Vec3(Util.RandomDouble(-1, 1), Util.RandomDouble(-1, 1), 0);
                if(p.Length2() > 1) continue;
                return p;
            }
        }

        public static Vec3 RandomUnitVec() {
            Vec3 p = Vec3.RandomInUnitSphere();
            return p.Normalize();
        }

        public static Vec3 RandomInHemisphere(Vec3 normal) {
            Vec3 p = Vec3.RandomInUnitSphere();
            if(p.Dot(normal) > 0) {
                return p;
            } else {
                return -1 * p;
            }
        }
        
        public static Vec3 Reflect(Vec3 v, Vec3 N) {
            return v - 2 * v.Dot(N) * N;
        }

        public double Length() {
            return Math.Sqrt(this.Length2());
        }

        public double Length2() {
            return this.x * this.x + this.y * this.y + this.z * this.z;
        }

        public double Dot(Vec3 B) {
            return this.x * B.x + this.y * B.y + this.z * B.z;
        }

        public Vec3 Cross(Vec3 B) {
            return new Vec3(this.y * B.z - this.z * B.y, this.z * B.x - this.x * B.z, this.x * B.y - this.y * B.x);
        }

        public Vec3 Normalize() {
            return new Vec3(this.x, this.y, this.z) / this.Length();
        }

        public bool NearZero() {
            double tolerance = .000001;
            return Math.Abs(this.x) < tolerance && Math.Abs(this.y) < tolerance && Math.Abs(this.z) < tolerance;
        }
    }
}