using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace TrabalhoCG3 {
    public class GraphicObject {
        public BBox BBox { get; set; }
        public List<Point4D> Points { get; set; }
        public Transform Transform { get; set; }
        public List<GraphicObject> Sons { get; set; }
        public PrimitiveType PrimitiveType { get; set; }
        public static PrimitiveType[] PrimitiveTypes = {PrimitiveType.LineStrip, PrimitiveType.LineLoop, PrimitiveType.Lines,  PrimitiveType.Points, PrimitiveType.Polygon, PrimitiveType.Quads, PrimitiveType.TriangleFan, PrimitiveType.Triangles };
        public Color Color { get; set; }
        public float Size { get; set; }

        public GraphicObject(){
            Points = new List<Point4D>();
            Transform = new Transform();
            BBox = new BBox();
            Sons = new List<GraphicObject>();
            PrimitiveType = PrimitiveTypes[0];
            Color = Color.Black;
            Size = 5;
        }

        public void ReadyMatrix(){
            ReadyMatrix(Transform);
        }
        public void ReadyMatrix(Transform transform){
            for(int i = 0; i < Points.Count; i++) {
                Points[i] = transform.TransformPoint(Points[i]);

                if(i == 0) {
                    BBox.MaxX = Points[i].X;
                    BBox.MaxY = Points[i].Y;
                    BBox.MinX = Points[i].X;
                    BBox.MinY = Points[i].Y;
                } else {
                    if(BBox.MaxX < Points[i].X)
                        BBox.MaxX = Points[i].X;
                    if(BBox.MaxY < Points[i].Y)
                        BBox.MaxY = Points[i].Y;
                    if(BBox.MinX > Points[i].X)
                        BBox.MinX = Points[i].X;
                    if(BBox.MinY > Points[i].Y)
                        BBox.MinY = Points[i].Y;
                }
            }
            foreach(GraphicObject obj in Sons) {
                obj.ReadyMatrix(transform);
            }
            Transform.SetIdentity();
        }

        public void AddPoint(Point4D ponto){
            if(Points.Count == 0) {
                BBox.MaxX = ponto.X;
                BBox.MaxY = ponto.Y;
                BBox.MinX = ponto.X;
                BBox.MinY = ponto.Y;
            } else {
                if(BBox.MaxX < ponto.X)
                    BBox.MaxX = ponto.X;
                if(BBox.MaxY < ponto.Y)
                    BBox.MaxY = ponto.Y;
                if(BBox.MinX > ponto.X)
                    BBox.MinX = ponto.X;
                if(BBox.MinY > ponto.Y)
                    BBox.MinY = ponto.Y;
            }


            Points.Add(ponto);
        }

        public void Draw(){
            GL.PushMatrix();
            GL.MultMatrix(Transform.Matrix);
            GL.Begin(PrimitiveType);
            GL.Color3(Color);
            GL.PointSize(Size);
            GL.LineWidth(Size);
            foreach(Point4D point in Points) {
                GL.Vertex3(point.X, point.Y, point.Z);
            }
            GL.End();
            foreach(GraphicObject obj in Sons) {
                obj.Draw();
            }
            GL.PopMatrix();
        }
    }
}

