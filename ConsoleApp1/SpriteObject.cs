using Raylib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Raylib.Raylib;

namespace TankGame
{
    public class SpriteObject : SceneObject
    {
        Texture2D texture = new Texture2D();
        Image image = new Image();
        public float Width
        {
            get { return texture.width; }
        }
        public float Height
        {
            get { return texture.height; }
        }
        public SpriteObject()
        {
        }
        public void Load(string filename)
        {
            Image img = LoadImage(filename);
            texture = LoadTextureFromImage(img);
        }
        public override void OnDraw()
        {
            float rotation = (float)Math.Atan2(
            globalTransform.y1, globalTransform.x1);
            DrawTextureEx(
            texture,
            new Vector2(globalTransform.x3, globalTransform.y3),
            rotation * (float)(180.0f / Math.PI),
            1, Color.WHITE);
        }
    }
}
