using System.Text.Json.Serialization;
using OpenTK.Mathematics;

namespace OpenTKCubo3D
{
    public class Transformaciones{
        [JsonIgnore]
        public Vector3 Posicion { get; set; } = Vector3.Zero;
        [JsonIgnore]
        public Vector3 Rotacion { get; set; } = Vector3.Zero; 
        [JsonIgnore]
        public Vector3 Escala { get; set; } = Vector3.One;

        public Matrix4 GetMatrix(Vector3 centro) => Matrix4.CreateTranslation(-centro) *
        Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotacion.X)) *
        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotacion.Y)) *
        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotacion.Z)) *
        Matrix4.CreateScale(Escala) *
        Matrix4.CreateTranslation(centro + Posicion);
        public void Transladate(float x, float y, float z) {
            Posicion += new Vector3(x, y, z);
        }

        public void Rotate(float xDeg, float yDeg, float zDeg) {
            Rotacion += new Vector3(xDeg, yDeg, zDeg);
        }

        public void RotateA(Vector3 centro, float xDeg, float yDeg, float zDeg)
        {
            Rotate(xDeg, yDeg, zDeg);
        }

        public void Escalate(float n) {
            if(n != 0){
            Escala *= new Vector3(n);
            }
        }
    }

}