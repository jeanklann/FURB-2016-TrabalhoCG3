using System;

namespace TrabalhoCG3 {
    public class Ponto4D {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Ponto4D(float x, float y, float z) {
            X = x;
            Y = y;
            Z = z;
            W = 1;
        }
        public Ponto4D(float x, float y) {
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

