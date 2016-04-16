using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace TrabalhoCG3 {
    public class ObjetoGrafico {
        public BBox BBox { get; set; }
        public List<Ponto4D> Pontos { get; set; }
        public Transformacao Transformacao { get; set; }
        public List<ObjetoGrafico> Filhos { get; set; }
        public PrimitiveType Tipo { get; set; }
        public Color Cor { get; set; }

        public void AddPonto(Ponto4D ponto){
            Pontos.Add(ponto);
        }

        public void Desenha(){
            GL.PushMatrix();
            ///////////////////////GL.MultMatrix(
            GL.Begin(Tipo);
            GL.Color3(Cor);
            foreach(Ponto4D ponto in Pontos) {
                GL.Vertex3(ponto.X, ponto.Y, ponto.Z);
            }
            GL.End();
            foreach(ObjetoGrafico objeto in Filhos) {
                objeto.Desenha();
            }
            GL.PopMatrix();
        }
    }
}

