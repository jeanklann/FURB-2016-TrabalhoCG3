using System;

namespace TrabalhoCG3 {
    public class BBox {
        public float MinX { get; set; }
        public float MinY { get; set; }
        public float MinZ { get; set; }
        public float MaxX { get; set; }
        public float MaxY { get; set; }
        public float MaxZ { get; set; }

        public Point4D Center { 
            get { 
                return new Point4D(
                    (MinX + MaxX) / 2,
                    (MinY + MaxY) / 2,
                    (MinZ + MaxZ) / 2);
            }
        }
    }
}

