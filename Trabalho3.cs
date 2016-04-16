using System;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace TrabalhoCG3 {
    public class Trabalho3 {
        public Trabalho3() {
        }
        public static void Main(string[] args){

        }
        private void DrawSRU()
        {
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Red);
            GL.Vertex2(-200, 0);
            GL.Vertex2(200, 0);
            GL.Color3(Color.Green);
            GL.Vertex2(0, -200);
            GL.Vertex2(0, 200);
            GL.End();
        }

        private void Display(){
            GL.Clear(ClearBufferMask.ColorBufferBit);

        }
    }
}

