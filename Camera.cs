using System;
using System.Drawing;

namespace TrabalhoCG3 {
    public class Camera {
        public Camera(){
            MinX_ortho = -400;
            MinY_ortho = -400;
            MaxX_ortho = 400;
            MaxY_ortho = 400;
        }
        public void Reset(){
            MinX_ortho = -400;
            MinY_ortho = -400;
            MaxX_ortho = 400;
            MaxY_ortho = 400;
        }
        public Point4D Center { 
            get { 
                return new Point4D(
                    (MinX_ortho + MaxX_ortho) / 2,
                    (MinY_ortho + MaxY_ortho) / 2);
            }
        }
        public Size Size {
            get {
                return new Size(
                    (int)(MaxX_ortho - MinX_ortho),
                    (int)(MaxY_ortho - MinY_ortho));
            }
        }

        public float MinX_ortho { get; set;}
        public float MinY_ortho { get; set;}
        public float MaxX_ortho { get; set;}
        public float MaxY_ortho { get; set;}

        public void Pan(){}
        public void Zoom(){}

    }
}

