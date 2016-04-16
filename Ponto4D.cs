using System;

namespace TrabalhoCG3 {
    public class Ponto4D {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }

        public Ponto4D(double x, double y, double z, double w) {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public Ponto4D(double x, double y, double z) {
            X = x;
            Y = y;
            Z = z;
            W = 1;
        }
        public Ponto4D(double x, double y) {
            X = x;
            Y = y;
            Z = 0;
            W = 1;
        }
        public Ponto4D() {
            X = 0;
            Y = 0;
            Z = 0;
            W = 1;
        }

        public Ponto4D InverteSinal(){
            X = -X;
            Y = -Y;
            Z = -Z;
        }

        public override string ToString() {
            return string.Format("[{0},{1},{2},{3}]", X, Y, Z, W);
        }
    }
}

