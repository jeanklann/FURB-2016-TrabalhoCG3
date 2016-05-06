using System;
using System.Collections.Generic;

namespace TrabalhoCG3 {
    /// <summary>
    /// Classe responsável por gerir o mundo do jogo
    /// </summary>
    public class World {
        public List<GraphicObject> GraphicObjects { get; set; }
        public Camera Camera { get; set; }
        public World() {
            GraphicObjects = new List<GraphicObject>();
            Camera = new Camera();
        }

    }
}

