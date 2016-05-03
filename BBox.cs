using System;

namespace TrabalhoCG3 {
    public class BBox {
        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MinZ { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }
        public double MaxZ { get; set; }

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

