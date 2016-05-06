using System;
using System.Drawing;

namespace TrabalhoCG3 {
    /// <summary>
    /// Classe responsável pelos pontos na tela
    /// </summary>
    public class Point4D {
        #region Fields
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }
        #endregion
        #region Methods
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

        /// <summary>
        /// Calcula a distância entre 2 pontos
        /// </summary>
        /// <param name="p1">Ponto 1.</param>
        /// <param name="p2">Ponto 2.</param>
        public static double Distance(Point4D p1, Point4D p2){
            return Math.Sqrt(
                Math.Pow((p2.X - p1.X), 2)
                +
                Math.Pow((p2.Y - p1.Y), 2)
            );
        }
        /// <summary>
        /// Calcula a distância entre outro pontos
        /// </summary>
        /// <param name="p2">Ponto.</param>
        public double Distance(Point4D p2){
            return Math.Sqrt(
                Math.Pow((p2.X - X), 2)
                +
                Math.Pow((p2.Y - Y), 2)
            );
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

        public static Point4D operator +(Point4D n1,  Point n2){
            return new Point4D(n1.X+n2.X, n1.Y + n2.Y, n1.Z, n1.W);
        }
        public static Point4D operator -(Point4D n1,  Point n2){
            return new Point4D(n1.X-n2.X, n1.Y - n2.Y, n1.Z, n1.W);
        }
        public static Point4D operator *(Point4D n1,  Point n2){
            return new Point4D(n1.X*n2.X, n1.Y*n2.Y, n1.Z, n1.W);
        }
        public static Point4D operator /(Point4D n1,  Point n2){
            return new Point4D(n1.X/n2.X, n1.Y/n2.Y, n1.Z, n1.W);
        }
        #endregion
    }
}

