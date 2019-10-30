using System;
using System.Collections.Generic;
using System.Text;

namespace MathClass
{
    public class Matrix3
    {
        public float m1, m4, m7, m2, m5, m8, m3, m6, m9;
        public Matrix3(float one, float two, float three, float four, float five, float six, float seven, float eight, float nine)
        {
            m1 = one; m4 = four; m7 = seven;
            m2 = two; m5 = five; m8 = eight;
            m3 = three; m6 = six; m9 = nine;
        }
        public Matrix3()
        {
            m1 = 1; m4 = 0; m7 = 0;
            m2 = 0; m5 = 1; m8 = 0;
            m3 = 0; m6 = 0; m9 = 1;
        }
        public static Matrix3 operator *(Matrix3 m1, Matrix3 m2)
        {
            Matrix3 m3 = new Matrix3();
            m3.m1 = (m1.m1 * m2.m1) + (m1.m4 * m2.m2) + (m1.m7 * m2.m3);
            m3.m4 = (m1.m1 * m2.m4) + (m1.m4 * m2.m5) + (m1.m7 * m2.m6);
            m3.m7 = (m1.m1 * m2.m7) + (m1.m4 * m2.m8) + (m1.m7 * m2.m9);
            m3.m2 = (m1.m2 * m2.m1) + (m1.m5 * m2.m2) + (m1.m8 * m2.m3);
            m3.m5 = (m1.m2 * m2.m4) + (m1.m5 * m2.m5) + (m1.m8 * m2.m6);
            m3.m8 = (m1.m2 * m2.m7) + (m1.m5 * m2.m8) + (m1.m8 * m2.m9);
            m3.m3 = (m1.m3 * m2.m1) + (m1.m6 * m2.m2) + (m1.m9 * m2.m3);
            m3.m6 = (m1.m3 * m2.m4) + (m1.m6 * m2.m5) + (m1.m9 * m2.m6);
            m3.m9 = (m1.m3 * m2.m7) + (m1.m6 * m2.m8) + (m1.m9 * m2.m9);
            return m3;
        }
        public static Matrix3 operator +(Matrix3 m1, Matrix3 m2)
        {
            Matrix3 x3 = new Matrix3();
            x3.m1 = (m1.m1 + m2.m1);
            x3.m4 = (m1.m4 + m2.m4);
            x3.m7 = (m1.m7 + m2.m7);
            x3.m2 = (m1.m2 + m2.m2);
            x3.m5 = (m1.m5 + m2.m5);
            x3.m8 = (m1.m8 + m2.m8);
            x3.m3 = (m1.m3 + m2.m3);
            x3.m6 = (m1.m6 + m2.m6);
            x3.m9 = (m1.m9 + m2.m9);
            return x3;
        }
        public static Matrix3 operator -(Matrix3 m1, Matrix3 m2)
        {
            Matrix3 m3 = new Matrix3();
            m3.m1 = (m1.m1 - m2.m1);
            m3.m4 = (m1.m4 - m2.m4);
            m3.m7 = (m1.m7 - m2.m7);
            m3.m2 = (m1.m2 - m2.m2);
            m3.m5 = (m1.m5 - m2.m5);
            m3.m8 = (m1.m8 - m2.m8);
            m3.m3 = (m1.m3 - m2.m3);
            m3.m6 = (m1.m6 - m2.m6);
            m3.m9 = (m1.m9 - m2.m9);
            return m3;
        }
        public static Matrix3 Matrix3Transpose(Matrix3 m1)
        {
            Matrix3 m2 = new Matrix3();
            m2.m1 = m1.m1;
            m2.m4 = m1.m2;
            m2.m7 = m1.m3;
            m2.m2 = m1.m4;
            m2.m5 = m1.m5;
            m2.m8 = m1.m6;
            m2.m3 = m1.m7;
            m2.m6 = m1.m8;
            m2.m9 = m1.m9;
            return m2;
        }
        public void SetTranslation(float x, float y)
        {
            //x1 = (x1 * x) + (y1 * y) + t1;
            //y1 = (x2 * x) + (y2 * y) + t2;
            m1 = x;
            m4 = y;
        }
        public void Translate(float x, float y)
        {
            //x1 = (x1 * x) + (y1 * y) + t1;
            //y1 = (x2 * x) + (y2 * y) + t2;
            m1 = x;
            m4 = y;
        }
        public void SetScaled(Vector3 v)
        {
            m1 = v.x; m4 = 0; m7 = 0;
            m2 = 0; m5 = v.y; m8 = 0;
            m3 = 0; m6 = 0; m9 = v.z;
        }
        public void SetScaled(float x, float y, float z)
        {
            m1 = x; m4 = 0; m7 = 0;
            m2 = 0; m5 = y; m8 = 0;
            m3 = 0; m6 = 0; m9 = z;
        }
        public void Set(Matrix3 m)
        {
            m1 = m.m1;
            m4 = m.m4;
            m7 = m.m7;
            m2 = m.m2;
            m5 = m.m5;
            m8 = m.m8;
            m3 = m.m3;
            m6 = m.m6;
            m9 = m.m9;
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
            Set(new Matrix3(1, 0, 0,
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
            Set(new Matrix3((float)Math.Cos(radians), (float)Math.Sin(radians), 0,
                            (float)-Math.Sin(radians), (float)Math.Cos(radians), 0,
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
            Set(new Matrix3((float)Math.Cos(radians), 0, (float)-Math.Sin(radians),
            0, 1, 0,
            (float)Math.Sin(radians), 0, (float)Math.Cos(radians)));
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
    public class Matrix4
    {
        public float m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12, m13, m14, m15, m16;
        public Matrix4(float one, float two, float three, float four, float five, float six, float seven, float eight, float nine, float ten, float eleven, float twelve, float thirteen, float fourteen, float fivteen, float sixteen)
        {
            m1 = one; m5 = five; m9 = nine; m13 = thirteen;
            m2 = two; m6 = six; m10 = ten; m14 = fourteen;
            m3 = three; m7 = seven; m11 = eleven; m15 = fivteen;
            m4 = four; m8 = eight; m12 = twelve; m16 = sixteen;
        }
        public Matrix4()
        {
            m1 = 1; m5 = 0; m9 = 0; m13 = 0;
            m2 = 0; m6 = 1; m10 = 0; m14 = 0;
            m3 = 0; m7 = 0; m11 = 1; m15 = 0;
            m4 = 0; m8 = 0; m12 = 0; m16 = 1;
        }
        public static Matrix4 operator *(Matrix4 m1, Matrix4 m2)
        {
            Matrix4 m3 = new Matrix4();
            m3.m1 = (m1.m1 * m2.m1) + (m1.m5 * m2.m2) + (m1.m9 * m2.m3) + (m1.m13 * m2.m4);
            m3.m5 = (m1.m1 * m2.m5) + (m1.m5 * m2.m6) + (m1.m9 * m2.m7) + (m1.m13 * m2.m8);
            m3.m9 = (m1.m1 * m2.m9) + (m1.m5 * m2.m10) + (m1.m9 * m2.m11) + (m1.m13 * m2.m12);
            m3.m13 = (m1.m1 * m2.m13) + (m1.m5 * m2.m14) + (m1.m9 * m2.m15) + (m1.m13 * m2.m16);

            m3.m2 = (m1.m2 * m2.m1) + (m1.m6 * m2.m2) + (m1.m10 * m2.m3) + (m1.m14 * m2.m4);
            m3.m6 = (m1.m2 * m2.m5) + (m1.m6 * m2.m6) + (m1.m10 * m2.m7) + (m1.m14 * m2.m8);
            m3.m10 = (m1.m2 * m2.m9) + (m1.m6 * m2.m10) + (m1.m10 * m2.m11) + (m1.m14 * m2.m12);
            m3.m14 = (m1.m2 * m2.m13) + (m1.m6 * m2.m14) + (m1.m10 * m2.m15) + (m1.m14 * m2.m16);

            m3.m3 = (m1.m3 * m2.m1) + (m1.m7 * m2.m2) + (m1.m11 * m2.m3) + (m1.m15 * m2.m4);
            m3.m7 = (m1.m3 * m2.m5) + (m1.m7 * m2.m6) + (m1.m11 * m2.m7) + (m1.m15 * m2.m8);
            m3.m11 = (m1.m3 * m2.m9) + (m1.m7 * m2.m10) + (m1.m11 * m2.m11) + (m1.m15 * m2.m12);
            m3.m15 = (m1.m3 * m2.m13) + (m1.m7 * m2.m14) + (m1.m11 * m2.m15) + (m1.m15 * m2.m16);

            m3.m4 = (m1.m4 * m2.m1) + (m1.m8 * m2.m2) + (m1.m12 * m2.m3) + (m1.m16 * m2.m4);
            m3.m8 = (m1.m4 * m2.m5) + (m1.m8 * m2.m6) + (m1.m12 * m2.m7) + (m1.m16 * m2.m8);
            m3.m12 = (m1.m4 * m2.m9) + (m1.m8 * m2.m10) + (m1.m12 * m2.m11) + (m1.m16 * m2.m12);
            m3.m16 = (m1.m4 * m2.m13) + (m1.m8 * m2.m14) + (m1.m12 * m2.m15) + (m1.m16 * m2.m16);
            return m3;
        }
        public static Matrix4 operator +(Matrix4 m1, Matrix4 m2)
        {
            Matrix4 x3 = new Matrix4();
            x3.m1 = (m1.m1 + m2.m1);
            x3.m5 = (m1.m5 + m2.m5);
            x3.m9 = (m1.m9 + m2.m9);
            x3.m13 = (m1.m13 + m2.m13);
            x3.m2 = (m1.m2 + m2.m5);
            x3.m6 = (m1.m6 + m2.m6);
            x3.m10 = (m1.m10 + m2.m10);
            x3.m14 = (m1.m14 + m2.m14);
            x3.m3 = (m1.m3 + m2.m3);
            x3.m7 = (m1.m7 + m2.m7);
            x3.m11 = (m1.m11 + m2.m11);
            x3.m15 = (m1.m15 + m2.m15);
            x3.m4 = (m1.m4 + m2.m4);
            x3.m8 = (m1.m8 + m2.m8);
            x3.m12 = (m1.m12 + m2.m12);
            x3.m16 = (m1.m16 + m2.m16);
            return x3;
        }
        public static Matrix4 operator -(Matrix4 m1, Matrix4 m2)
        {
            Matrix4 m3 = new Matrix4();
            m3.m1 = (m1.m1 - m2.m1);
            m3.m5 = (m1.m5 - m2.m5);
            m3.m9 = (m1.m9 - m2.m9);
            m3.m13 = (m1.m13 - m2.m13);
            m3.m2 = (m1.m2 - m2.m2);
            m3.m6 = (m1.m6 - m2.m6);
            m3.m10 = (m1.m10 - m2.m10);
            m3.m14 = (m1.m14 - m2.m14);
            m3.m3 = (m1.m3 - m2.m3);
            m3.m7 = (m1.m7 - m2.m7);
            m3.m11 = (m1.m11 - m2.m11);
            m3.m15 = (m1.m15 - m2.m15);
            m3.m4 = (m1.m4 - m2.m4);
            m3.m8 = (m1.m8 - m2.m8);
            m3.m12 = (m1.m12 - m2.m12);
            m3.m16 = (m1.m16 - m2.m16);
            return m3;
        }
        public static Matrix4 Matrix4Transpose(Matrix4 m1)
        {
            Matrix4 m2 = new Matrix4();
            m2.m1 = m1.m1;
            m2.m5 = m1.m2;
            m2.m9 = m1.m3;
            m2.m13 = m1.m4;
            m2.m2 = m1.m5;
            m2.m6 = m1.m6;
            m2.m10 = m1.m7;
            m2.m14 = m1.m8;
            m2.m3 = m1.m9;
            m2.m7 = m1.m10;
            m2.m11 = m1.m11;
            m2.m15 = m1.m12;
            m2.m4 = m1.m13;
            m2.m8 = m1.m14;
            m2.m12 = m1.m15;
            m2.m16 = m1.m16;
            return m2;
        }
        public void SetScaled(Vector4 v)
        {
            m1 = v.x; m5 = 0; m9 = 0;
            m13 = 0; m2 = v.y; m6 = 0;
            m10 = 0; m14 = 0; m3 = v.z;
        }
        public void SetScaled(float x, float y, float z)
        {
            m1 = x; m5 = 0; m9 = 0;
            m13 = 0; m2 = y; m6 = 0;
            m10 = 0; m14 = 0; m3 = z;
        }
        public void Set(Matrix4 m)
        {
            m1 = m.m1;
            m2 = m.m2;
            m3 = m.m3;
            m4 = m.m4;
            m5 = m.m5;
            m6 = m.m6;
            m7 = m.m7;
            m8 = m.m8;
            m9 = m.m9;
            m10 = m.m10;
            m11 = m.m11;
            m12 = m.m12;
            m13 = m.m13;
            m14 = m.m14;
            m15 = m.m15;
            m16 = m.m16;
        }

        public void Scale(Vector4 v)
        {
            Matrix4 m = new Matrix4();
            m.SetScaled(v);
            Set(this * m);
        }
        public void Scale(float x, float y, float z)
        {
            Matrix4 m = new Matrix4();
            m.SetScaled(x, y, z);
            Set(this * m);
        }
        public void SetRotateX(double radians)
        {
            Set(new Matrix4(1, 0, 0, 0,
                            0, (float)Math.Cos(radians), (float)Math.Sin(radians), 0,
                            0, (float)-Math.Sin(radians), (float)Math.Cos(radians), 0,
                            0, 0, 0, 1));
        }
        public void RotateX(double radians)
        {
            Matrix4 m = new Matrix4();
            m.SetRotateX(radians);
            Set(this * m);
        }
        public void SetRotateZ(double radians)
        {
            Set(new Matrix4((float)Math.Cos(radians), (float)Math.Sin(radians), 0, 0,
                            (float)-Math.Sin(radians), (float)Math.Cos(radians), 0, 0,
                             0, 0, 1, 0,
                             0, 0, 0, 1));
        }
        public void RotateZ(double radians)
        {
            Matrix4 m = new Matrix4();
            m.SetRotateZ(radians);
            Set(this * m);
        }
        public void SetRotateY(double radians)
        {
            Set(new Matrix4((float)Math.Cos(radians), 0, (float)-Math.Sin(radians), 0,
                             0, 1, 0, 0,
                            (float)Math.Sin(radians), 0, (float)Math.Cos(radians), 0,
                             0, 0, 0, 1));
        }
        public void RotateY(double radians)
        {
            Matrix4 m = new Matrix4();
            m.SetRotateY(radians);
            Set(this * m);
        }
        public void SetEuler(float pitch, float yaw, float roll)
        {
            Matrix4 x = new Matrix4();
            Matrix4 y = new Matrix4();
            Matrix4 z = new Matrix4();
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
        public Vector3(float x1, float y1, float t1)
        {
            x = x1;
            y = y1;
            z = t1;
        }
        public Vector3(Vector3 Vector3)
        {

        }
        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
            v1.x * v2.x,
            v1.y * v2.y,
            v1.z * v2.z);
        }
        public static Vector3 operator *(Matrix3 m, Vector3 v)
        {
            return new Vector3((v.x * m.m1) + (v.y * m.m4) + (v.z * m.m7),
                               (v.x * m.m2) + (v.y * m.m5) + (v.z * m.m8),
                               (v.x * m.m3) + (v.y * m.m6) + (v.z * m.m9));
        }
        public static Vector3 operator *(float x, Vector3 v)
        {
            return new Vector3(v.x * x, v.y * x, v.z * x);
        }
        public static Vector3 operator *(Vector3 v, float x)
        {
            return new Vector3(v.x * x, v.y * x, v.z * x);
        }
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
            v1.x + v2.x,
            v1.y + v2.y,
            v1.z + v2.z);
        }
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
            v1.x - v2.x,
            v1.y - v2.y,
            v1.z - v2.z);
        }
        public float Magnitude()
        {
            return (float)Math.Sqrt((x * x) + (y * y) + (z * z));
        }
        public float Dot(Vector3 v)
        {
            return x * v.x + y * v.y + z * v.z;
        }
        public Vector3 cross(Vector3 v)
        {
            return new Vector3((y * v.z) - (z * v.y), (z * v.x) - (x * v.z), (x * v.y) - (y * v.x));
        }
        public void Normalize()
        {
            float m = Magnitude();
            x /= m;
            y /= m;
            z /= m;
        }
    }
    public class Vector4
    {
        public float x, y, z, w;
        public Vector4(float x, float y, float z, float w)
        {
            this.x = x; this.y = y; this.z = z; this.w = w;
        }
        public Vector4(Vector4 vector4)
        {

        }
        public static Vector4 operator *(Vector4 v1, Vector4 v2)
        {
            return new Vector4(
            v1.x * v2.x,
            v1.y * v2.y,
            v1.z * v2.z,
            v1.w * v2.w);
        }
        public static Vector4 operator *(float x, Vector4 v)
        {
            return new Vector4(v.x * x, v.y * x, v.z * x, v.w * x);
        }
        public static Vector4 operator *(Vector4 v, float x)
        {
            return new Vector4(v.x * x, v.y * x, v.z * x, v.w * x);
        }
        public static Vector4 operator *(Matrix4 m, Vector4 v)
        {
            return new Vector4((v.x * m.m1) + (v.y * m.m5) + (v.z * m.m9) + (v.w * m.m13),
                               (v.x * m.m2) + (v.y * m.m6) + (v.z * m.m10) + (v.w * m.m14),
                               (v.x * m.m3) + (v.y * m.m7) + (v.z * m.m11) + (v.w * m.m15),
                               (v.x * m.m4) + (v.y * m.m8) + (v.z * m.m12) + (v.w * m.m16));
        }
        public static Vector4 operator +(Vector4 v1, Vector4 v2)
        {
            return new Vector4(
            v1.x + v2.x,
            v1.y + v2.y,
            v1.z + v2.z,
            v1.w + v2.w);
        }
        public static Vector4 operator -(Vector4 v1, Vector4 v2)
        {
            return new Vector4(
            v1.x - v2.x,
            v1.y - v2.y,
            v1.z - v2.z,
            v1.w - v2.w);
        }
        public float Magnitude()
        {
            return (float)Math.Sqrt((x * x) + (y * y) + (z * z) + (w * w));
        }
        public float Dot(Vector4 v)
        {
            return x * v.x + y * v.y + z * v.z + w * v.w;
        }
        public Vector4 Cross(Vector4 v)
        {
            return new Vector4((y * v.z) - (z * v.y), (z * v.x) - (x * v.z), (x * v.y) - (y * v.x), (w * v.z) - (v.z * w));
        }
        public void Normalize()
        {
            float m = Magnitude();
            x /= m;
            y /= m;
            z /= m;
            w /= m;
        }
    }
    public class Colour
    {
        public UInt32 colour;

        public Colour()
        {
            colour = 0x00000000;
        }
        public Colour(UInt32 r, UInt32 g, UInt32 b, UInt32 a)
        {
            r = (r >> 32);
            g = (g >> 32);
            b = (b >> 32);
            colour = (r << 24) + (g << 16) + (b << 8) + (a >> 32);
        }
        public void SetRed(UInt32 r)
        {
            colour = (r >> 32);
            colour = (colour << 24);
        }
        public void SetGreen(UInt32 g)
        {
            colour = (g >> 32);
            colour = (colour << 16);
        }
        public void SetBlue(UInt32 b)
        {
            colour = (b >> 32);
            colour = (colour << 8);
        }
        public void SetAlpha(UInt32 a)
        {
            colour = (a >> 32);
        }
        public byte GetAlpha()
        {
            UInt32 other = (colour << 24);
            return Convert.ToByte(other >> 24);
        }
        public byte GetBlue()
        {
            UInt32 other = (colour << 16);
            return Convert.ToByte(other >> 24);
        }
        public byte GetGreen()
        {
            UInt32 other = (colour << 8);
            return Convert.ToByte(other >> 24);
        }
        public byte GetRed()
        {
            return Convert.ToByte(colour >> 24);
        }
    }
}
