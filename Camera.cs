using System;
using System.Drawing;

namespace TrabalhoCG3 {
    /// <summary>
    /// Classe responsável por controlar a câmera
    /// </summary>
    public class Camera {
        #region Fields
        public double MinX_ortho { get; set;}
        public double MinY_ortho { get; set;}
        public double MaxX_ortho { get; set;}
        public double MaxY_ortho { get; set;}
        /// <summary>
        /// Pega o centro da câmera
        /// </summary>
        /// <value>O centro</value>
        public Point4D Center { 
            get { 
                return new Point4D(
                    (MinX_ortho + MaxX_ortho) / 2,
                    (MinY_ortho + MaxY_ortho) / 2);
            }
            set {
                double W = MaxX_ortho - MinX_ortho;
                double H = MaxY_ortho - MinY_ortho;
                MinX_ortho = value.X - W / 2;
                MaxX_ortho = value.X + W / 2;
                MinY_ortho = value.Y - H / 2;
                MaxY_ortho = value.Y + H / 2;
            }
        }
        /// <summary>
        /// Pega o tamanho que está renderizando a câmera
        /// </summary>
        /// <value>O tamanho da câmera</value>
        public Size Size {
            get {
                return new Size(
                    (int)(MaxX_ortho - MinX_ortho),
                    (int)(MaxY_ortho - MinY_ortho));
            }
            set {
                Point4D c = Center;
                MaxX_ortho = c.X + value.Width/2;
                MinX_ortho = c.X - value.Width/2;
                MaxY_ortho = c.Y + value.Height/2;
                MinY_ortho = c.Y - value.Height/2;
            }
        }
        #endregion
        #region Methods
        public Camera(){
            MinX_ortho = -400;
            MinY_ortho = -400;
            MaxX_ortho = 400;
            MaxY_ortho = 400;
        }
        /// <summary>
        /// Reseta a câmera
        /// </summary>
        public void Reset(){
            MinX_ortho = -400;
            MinY_ortho = -400;
            MaxX_ortho = 400;
            MaxY_ortho = 400;
        }


        #endregion

    }
}

