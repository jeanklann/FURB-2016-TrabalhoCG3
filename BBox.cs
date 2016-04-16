using System;

namespace TrabalhoCG3 {
    public class BBox {
        public float MinX { get; set; }
        public float MinY { get; set; }
        public float MinZ { get; set; }
        public float MaxX { get; set; }
        public float MaxY { get; set; }
        public float MaxZ { get; set; }

        public Ponto4D Centro { 
            get { 
                return new Ponto4D(
                    (MinX + MaxX) / 2,
                    (MinY + MaxY) / 2,
                    (MinZ + MaxZ) / 2);
            }
        }
    }
}

