using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace TrabalhoCG3 {
    public class GraphicObject {
        public BBox BBox { get; set; }
        public List<Point4D> Points { get; set; }
        public Transform Transform { get; set; }
        public List<GraphicObject> Filhos { get; set; }
        public PrimitiveType PrimitiveType { get; set; }
        public Color Color { get; set; }
        public float Size { get; set; }

        public void AddPoint(Point4D ponto){
            Points.Add(ponto);
        }

        public void Desenha(){
            GL.PushMatrix();
            //GL.MultMatrix(
            GL.Begin(PrimitiveType);
            GL.Color3(Color);
            GL.PointSize(Size);
            GL.LineWidth(Size);
            foreach(Point4D point in Points) {
                GL.Vertex3(point.X, point.Y, point.Z);
            }
            GL.End();
            foreach(GraphicObject objeto in Filhos) {
                objeto.Desenha();
            }
            GL.PopMatrix();
        }
    }
}

