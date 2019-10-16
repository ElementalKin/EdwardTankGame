using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculations
{

    public class Matrix3
    {
        public float x1, x2, x3, y1, y2, y3, t1, t2, t3;
        public Matrix3(float one, float two, float three, float four, float five, float six, float seven, float eight, float nine)
        {
            x1 = one; x2 = two; x3 = three;
            y1 = four; y2 = five; y3 = six;
            t1 = seven; t2 = eight; t3 = nine;
        }
        public Matrix3()
        {
            x1 = 1; x2 = 0; x3 = 0;
            y1 = 0; y2 = 1; y3 = 0;
            t1 = 0; t2 = 0; t3 = 1;
        }
        public static Matrix3 operator *(Matrix3 m1, Matrix3 m2)
        {
            Matrix3 m3 = new Matrix3();
            m3.x1 = (m1.x1 * m2.x1) + (m1.x2 * m2.y1) + (m1.x3 * m2.t1);
            m3.x2 = (m1.x1 * m2.x2) + (m1.x2 * m2.y2) + (m1.x3 * m2.t2);
            m3.x3 = (m1.x1 * m2.x3) + (m1.x2 * m2.y3) + (m1.x3 * m2.t3);
            m3.y1 = (m1.y1 * m2.x1) + (m1.y2 * m2.y1) + (m1.y3 * m2.t1);
            m3.y2 = (m1.y1 * m2.x2) + (m1.y2 * m2.y2) + (m1.y3 * m2.t2);
            m3.y3 = (m1.y1 * m2.x3) + (m1.y2 * m2.y3) + (m1.y3 * m2.t3);
            m3.t1 = (m1.t1 * m2.x1) + (m1.t2 * m2.y1) + (m1.t3 * m2.t1);
            m3.t2 = (m1.t1 * m2.x2) + (m1.t2 * m2.y2) + (m1.t3 * m2.t2);
            m3.t3 = (m1.t1 * m2.x3) + (m1.t2 * m2.y3) + (m1.t3 * m2.t3);
            return m3;
        }
        public static Matrix3 operator +(Matrix3 m1, Matrix3 m2)
        {
            Matrix3 x3 = new Matrix3();
            x3.x1 = (m1.x1 + m2.x1);
            x3.x2 = (m1.x2 + m2.x2);
            x3.x3 = (m1.x3 + m2.x3);
            x3.y1 = (m1.y1 + m2.y1);
            x3.y2 = (m1.y2 + m2.y2);
            x3.y3 = (m1.y3 + m2.y3);
            x3.t1 = (m1.t1 + m2.t1);
            x3.t2 = (m1.t2 + m2.t2);
            x3.t3 = (m1.t3 + m2.t3);
            return x3;
        }
        public static Matrix3 operator -(Matrix3 m1, Matrix3 m2)
        {
            Matrix3 m3 = new Matrix3();
            m3.x1 = (m1.x1 - m2.x1);
            m3.x2 = (m1.x2 - m2.x2);
            m3.x3 = (m1.x3 - m2.x3);
            m3.y1 = (m1.y1 - m2.y1);
            m3.y2 = (m1.y2 - m2.y2);
            m3.y3 = (m1.y3 - m2.y3);
            m3.t1 = (m1.t1 - m2.t1);
            m3.t2 = (m1.t2 - m2.t2);
            m3.t3 = (m1.t3 - m2.t3);
            return m3;
        }
        public static Matrix3 Matrix3Transpose(Matrix3 m1)
        {
            Matrix3 m2 = new Matrix3();
            m2.x1 = m1.x1;
            m2.x2 = m1.y1;
            m2.x3 = m1.t1;
            m2.y1 = m1.x2;
            m2.y2 = m1.y2;
            m2.y3 = m1.t2;
            m2.t1 = m1.x3;
            m2.t2 = m1.y3;
            m2.t3 = m1.t3;
            return m2;
        }
        public void SetTranslation(float x, float y)
        {
            //x1 = (x1 * x) + (y1 * y) + t1;
            //y1 = (x2 * x) + (y2 * y) + t2;
            x3 = x;
            y3 = y;
        }
        public void Translate(float x, float y)
        {
            x3 +=x;
            y3 +=y;
            //x1 = x;
            //x2 = y;
        }
        public void SetScaled(Vector3 v)
        {
            x1 = v.x; x2 = 0; x3 = 0;
            y1 = 0; y2 = v.y; y3 = 0;
            t1 = 0; t2 = 0; t3 = v.z;
        }
        public void SetScaled(float x, float y, float z)
        {
            x1 = x; x2 = 0; x3 = 0;
            y1 = 0; y2 = y; y3 = 0;
            t1 = 0; t2 = 0; t3 = z;
        }
        public void Set(Matrix3 m)
        {
            x1 = m.x1;
            x2 = m.x2;
            x3 = m.x3;
            y1 = m.y1;
            y2 = m.y2;
            y3 = m.y3;
            t1 = m.t1;
            t2 = m.t2;
            t3 = m.t3;
        }

        public void Scale(Vector3 v)
        {
            Matrix3 m = new Matrix3();
            m.SetScaled(v);
            Set(this * m);
        }
        public void Scale(float x, float y, float z)
        {
            Matrix3 m = new Matrix3();
            m.SetScaled(x, y, z);
            Set(this * m);
        }
        public void SetRotateX(double radians)
        {
            Set (new Matrix3(1, 0, 0,
                             0, (float)Math.Cos(radians), (float)Math.Sin(radians),
                             0, (float)-Math.Sin(radians), (float)Math.Cos(radians)));
        }
        public void RotateX(double radians)
        {
            Matrix3 m = new Matrix3();
            m.SetRotateX(radians);
            Set(this * m);
        }
        public void SetRotateZ(double radians)
        {
            Set(new Matrix3((float)Math.Cos(radians), (float)-Math.Sin(radians),0,
                            (float)Math.Sin(radians), (float)Math.Cos(radians), 0,
                             0, 0, 1));
        }
        public void RotateZ(double radians)
        {
            Matrix3 m = new Matrix3();
            m.SetRotateZ(radians);
            Set(this * m);
        }
        public void SetRotateY(double radians)
        {
            Set(new Matrix3((float)Math.Cos(radians),0, (float)-Math.Sin(radians),
                             0, 1, 0,
                            (float)Math.Sin(radians), 0,(float)Math.Cos(radians)));
        }
        public void RotateY(double radians)
        {
            Matrix3 m = new Matrix3();
            m.SetRotateY(radians);
            Set(this * m);
        }
        public void SetEuler(float pitch, float yaw, float roll)
        {
            Matrix3 x = new Matrix3();
            Matrix3 y = new Matrix3();
            Matrix3 z = new Matrix3();
            x.SetRotateX(pitch);
            y.SetRotateY(yaw);
            z.SetRotateZ(roll);
            // combine rotations in a specific order
            Set(z * y * x);
        }
    }
    public class Vector3
    {
        public float x, y, z;
        public Vector3(float x1, float y1, float z1)
        {
            x = x1; y = y1; z = z1;
        }
        public Vector3(Vector3 Vector3)
        {

        }

    }

}
