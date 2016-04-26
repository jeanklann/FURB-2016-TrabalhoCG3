using System;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace TrabalhoCG3 {
    public class Trabalho3 {
        public static void Main(string[] args){
            States.Camera = new Camera();

            Game game = new Game();
            game.Bounds = new Rectangle (0, 0, 800, 800);
            game.Run (60);
        }
    }
}

