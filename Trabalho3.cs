using System;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace TrabalhoCG3 {
    public class Trabalho3 {
        public static void Main(string[] args){
            States.Camera = new Camera();

            Game game = new Game();
            game.ClientSize = new Size (400, 400);
            game.Run (60);
        }
    }
}

