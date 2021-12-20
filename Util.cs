using System;

namespace ray_tracing_in_a_weekend {
    class Util {
        public static double RandomDouble() {
            Random rand = new Random();
            return rand.NextDouble();
        }

        public static double RandomDouble(double min, double max) {
            return min + (max - min) * RandomDouble();
        }
    }
}