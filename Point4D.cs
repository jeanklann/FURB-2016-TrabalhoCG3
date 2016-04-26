using System;

namespace TrabalhoCG3 {
    public class Point4D {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }

        public Point4D(double x, double y, double z, double w) {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public Point4D(double x, double y, double z) {
            X = x;
            Y = y;
            Z = z;
            W = 1;
        }
        public Point4D(double x, double y) {
            X = x;
            Y = y;
            Z = 0;
            W = 1;
        }
        public Point4D() {
            X = 0;
            Y = 0;
            Z = 0;
            W = 1;
        }

        public Point4D ReverseSignal(){
            return new Point4D(-X, -Y, -Z);
        }

        public override string ToString() {
            return string.Format("[{0},{1},{2},{3}]", X, Y, Z, W);
        }
    }
}

