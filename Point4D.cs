using System;

namespace TrabalhoCG3 {
    /// <summary>
    /// Classe responsável pelos pontos na tela
    /// </summary>
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
        /// <summary>
        /// Inverte o sinal deste ponto
        /// </summary>
        /// <returns>O ponto com o sinal invertido</returns>
        public Point4D ReverseSignal(){
            return new Point4D(-X, -Y, -Z);
        }

        public override string ToString() {
            return string.Format("[{0},{1},{2},{3}]", X, Y, Z, W);
        }

        public static Point4D operator +(Point4D n1,  Point4D n2){
            return new Point4D(n1.X+n2.X, n1.Y + n2.Y, n1.Z+n2.Z, n1.W);
        }
        public static Point4D operator -(Point4D n1,  Point4D n2){
            return new Point4D(n1.X-n2.X, n1.Y - n2.Y, n1.Z-n2.Z, n1.W);
        }
        public static Point4D operator *(Point4D n1,  Point4D n2){
            return new Point4D(n1.X*n2.X, n1.Y*n2.Y, n1.Z*n2.Z, n1.W);
        }
        public static Point4D operator /(Point4D n1,  Point4D n2){
            return new Point4D(n1.X/n2.X, n1.Y/n2.Y, n1.Z/n2.Z, n1.W);
        }
    }
}

