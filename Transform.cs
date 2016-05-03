using System;

namespace TrabalhoCG3 {
    public class Transform {
        public const double DEG_TO_RAD = 0.017453292519943295769236907684886;
        public Transform() {
            
        }
        private double[] matrix =
        {    1, 0, 0, 0 ,
             0, 1, 0, 0 ,
             0, 0, 1, 0 ,
             0, 0, 0, 1     };

        public double[] Matrix {
            set {
                for(int i = 0; i < matrix.Length; i++) {
                    matrix[i] = value[i];
                } }
            get { return matrix; }
        }
            
        public void setMatrix(double valor, int indice){
            matrix[indice] = valor;
        }

        public void SetIdentity() {
            for (int i=0; i<16; ++i) {
                matrix[i] = 0.0;
            }
            matrix[0] = matrix[5] = matrix[10] = matrix[15] = 1.0;
        }
        public void AddIdentity() {
            matrix[0] += 1;
            matrix[5] += 1;
            matrix[10] += 1;
            matrix[15] += 1;
        }

        public void SetTranslation(double tx, double ty, double tz)
        {
            SetIdentity();
            matrix[12] = tx;
            matrix[13] = ty;
            matrix[14] = tz;
        }
        public void AddTranslation(double tx, double ty, double tz)
        {
            //AddIdentity();
            matrix[12] += tx;
            matrix[13] += ty;
            matrix[14] += tz;
        }

        public void SetScale(double sX, double sY, double sZ)
        {
            SetIdentity();
            matrix[0] =  sX;
            matrix[5] =  sY;
            matrix[10] = sZ;
        }
        public void AddScale(double sX, double sY, double sZ)
        {
            AddIdentity();
            matrix[0] +=  sX;
            matrix[5] +=  sY;
            matrix[10] += sZ;
        }

        public void SetRotationX(double radians)
        {
            SetIdentity();
            matrix[5] =   Math.Cos(radians);
            matrix[9] =  -Math.Sin(radians);
            matrix[6] =   Math.Sin(radians);
            matrix[10] =  Math.Cos(radians);
        }

        public void SetRotationY(double radians)
        {
            SetIdentity();
            matrix[0] =   Math.Cos(radians);
            matrix[8] =   Math.Sin(radians);
            matrix[2] =  -Math.Sin(radians);
            matrix[10] =  Math.Cos(radians);
        }

        public void SetRotationZ(double radians)
        {
            SetIdentity();
            matrix[0] =  Math.Cos(radians);
            matrix[4] = -Math.Sin(radians);
            matrix[1] =  Math.Sin(radians);
            matrix[5] =  Math.Cos(radians);
        }
        public void AddRotationZ(double radians)
        {
            AddIdentity();
            matrix[0] +=  Math.Cos(radians);
            matrix[4] += -Math.Sin(radians);
            matrix[1] +=  Math.Sin(radians);
            matrix[5] +=  Math.Cos(radians);
        }

        public Point4D TransformPoint(Point4D point) {
            Point4D pointResult = new Point4D(
                matrix[0]*point.X  + matrix[4]*point.Y + matrix[8]*point.Z + matrix[12]*point.W,
                matrix[1]*point.X  + matrix[5]*point.Y + matrix[9]*point.Z + matrix[13]*point.W,
                matrix[2]*point.X  + matrix[6]*point.Y + matrix[10]*point.Z + matrix[14]*point.W,
                matrix[3]*point.X  + matrix[7]*point.Y + matrix[11]*point.Z + matrix[15]*point.W
            );
            return pointResult;
        }

        public Transform TransformMatrix(Transform t) {
            Transform result = new Transform();
            for (int i=0; i < 16; ++i)
                result.matrix[i] =
                    matrix[i%4]*t.matrix[i/4*4]+matrix[(i%4)+4]*t.matrix[i/4*4+1]
                    + matrix[(i%4)+8]*t.matrix[i/4*4+2]+matrix[(i%4)+12]*t.matrix[i/4*4+3];
            return result;
        }

        public override string ToString() {
            string res = "";
            res += string.Format("|{0},{1},{2},{3}|", Matrix[0],Matrix[1],Matrix[2],Matrix[3]);
            res += string.Format("|{0},{1},{2},{3}|", Matrix[4],Matrix[5],Matrix[6],Matrix[7]);
            res += string.Format("|{0},{1},{2},{3}|", Matrix[8],Matrix[9],Matrix[10],Matrix[11]);
            res += string.Format("|{0},{1},{2},{3}|", Matrix[12],Matrix[13],Matrix[14],Matrix[15]);
            return res;

        }


        


        public void Translacao(){}
        public void Rotacao(){}
        public void Escala(){}

    }
}

