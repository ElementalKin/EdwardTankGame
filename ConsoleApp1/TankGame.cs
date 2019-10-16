using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib;
using static Raylib.Raylib;

namespace TankGame
{
    struct Bullet
    {
        public Calculations.Matrix3 Transforms;
        public float bulletLifeTime;
    }
    struct EnemyTank
    {
        public SceneObject EnemyBody;
        public SceneObject EnemyTurret;
        public SceneObject EnemyBulletSpawn;
        public SpriteObject EnemyBodyText;
        public SpriteObject EnemyTurretText;
        
    }
    struct AmmoPack
    {
        public Vector2 location;
    }


    class Game
    {
        public static SceneObject tankObject = new SceneObject();
        public static SceneObject turretObject = new SceneObject();
        public static SceneObject BulletSpawn = new SceneObject();
        public static SceneObject BulletSpawn2 = new SceneObject();
        public static SpriteObject tankSprite = new SpriteObject();
        public static SpriteObject turretSprite = new SpriteObject();
        public static Texture2D AmmoPack = new Texture2D();
        public static Texture2D bulletText = new Texture2D();
        public static Camera2D camera = new Camera2D();


        Stopwatch stopwatch = new Stopwatch();

        //Initlizing all of the variables that will be used in the game.
        private long currentTime = 0;
        private long lastTime = 0;
        private float timer = 0;
        private int fps = 1;
        private int frames;
        private float deltaTime = 0.005f;
        float reloadSpeed = .1f;
        float reload = 0;
        int barrel = 0;
        int ammo = 100;
        float AmmoPackSpawn = 60;
        float AmmoPackTimer = 0;
        //Number of player bullets in the air.
        public int BulletCount = 0;
        //Number of enemy bullets in the air.
        public int EnemyBulletCount = 0;
        //Number of ammopacks in the game.
        public int AmmoPackCounter = 0;
        //number of enemys alive.
        public int NumberOfEnemys = 0;
        Bullet[] bullet = new Bullet[10000];
        Bullet[] EnemyBullets = new Bullet[10000];
        AmmoPack[] ammopack = new AmmoPack[1000];
        AABB aabb = new AABB();

        public void Init()
        {
            stopwatch.Start();
            lastTime = stopwatch.ElapsedMilliseconds;
            tankSprite.Load("resources/tankBody_green.png");
            // sprite is facing the wrong way... fix that here
            tankSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            // sets an offset for the base, so it rotates around the centre
            tankSprite.SetPosition(-tankSprite.Width / 2.0f, tankSprite.Height /
           2.0f);
            turretSprite.Load("resources/tankGreen_Double1.png");
            turretSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            // set the turret offset from the tank base
            // set up the scene object hierarchy - parent the turret to the base,
            // then the base to the tank sceneObject
            turretObject.AddChild(turretSprite);
            tankObject.AddChild(tankSprite);
            tankObject.AddChild(turretObject);
            turretObject.SetPosition(-10, 0);
            turretSprite.SetPosition(-10, turretSprite.Width / 2.3f);
            turretObject.AddChild(BulletSpawn);
            turretObject.AddChild(BulletSpawn2);
            // having an empty object for the tank parent means we can set the
            // position/rotation of the tank without
            // affecting the offset of the base sprite
            tankObject.SetPosition(GetScreenWidth() / 2.0f, GetScreenHeight() / 2.0f);
            bulletText = LoadTextureFromImage(LoadImage("resources/bulletDark1.png"));
            AmmoPack = LoadTextureFromImage(LoadImage("resources/AmmoPack.png"));
            camera.target = new Vector2(tankObject.GlobalTransform.x3 + 20, tankObject.GlobalTransform.y3 + 20);
            camera.offset = new Vector2( GetScreenWidth() / 2, GetScreenHeight() / 2 );
            camera.zoom = 1.0f;
        }

        public void Shutdown()
        { }
        public void Update()
        {
            BulletSpawn.SetPosition(turretSprite.Height, -10);
            BulletSpawn2.SetPosition(turretSprite.Height, 0);
            currentTime = stopwatch.ElapsedMilliseconds;
            deltaTime = (currentTime - lastTime) / 1000.0f;
            timer += deltaTime;
            reload -= deltaTime;
            if (timer >= 1)
            {
                fps = frames;
                frames = 0;
                timer -= 1;
            }
            frames++;
            if (IsKeyDown(KeyboardKey.KEY_A))
            {
                tankObject.Rotate(-deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_D))
            {
                tankObject.Rotate(deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_W))
            {
               Vector3 facing = new Vector3(
               tankObject.LocalTransform.x1,
               tankObject.LocalTransform.y1, 1) * deltaTime * 100;
                tankObject.Translate(facing.x, facing.y);
            }
            if (IsKeyDown(KeyboardKey.KEY_S))
            {
                Vector3 facing = new Vector3(
               tankObject.LocalTransform.x1,
               tankObject.LocalTransform.y1, 1) * deltaTime * -100;
                tankObject.Translate(facing.x, facing.y);
            }
            if (IsKeyDown(KeyboardKey.KEY_Q))
            {
                turretObject.Rotate(-deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_E))
            {
                turretObject.Rotate(deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_SPACE))
            {
                if (ammo > 0) {

                    if (reload <= 0)
                    {
                        if (barrel == 0)
                        {
                            bullet[BulletCount].Transforms = BulletSpawn.GlobalTransform;
                            bullet[BulletCount].bulletLifeTime = 10;
                            BulletCount++;
                            reload = reloadSpeed;
                            barrel += 1;
                            ammo--;
                        }
                        else
                        {
                            bullet[BulletCount].Transforms = BulletSpawn2.GlobalTransform;
                            bullet[BulletCount].bulletLifeTime = 10;
                            BulletCount++;
                            reload = reloadSpeed;
                            barrel -= 1;
                            ammo--;
                        }
                    }
                }
            }
            camera.target = new Vector2(tankObject.GlobalTransform.x3 + 20, tankObject.GlobalTransform.y3 + 20);
            camera.offset = new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2);
            camera.zoom = 1.0f;
            if (AmmoPackTimer <= 0)
            {
                ammopack[AmmoPackCounter].location = new Vector2(GetRandomValue(0, GetScreenWidth()), GetRandomValue(0, GetScreenHeight()));
                AmmoPackCounter++;
                AmmoPackTimer = AmmoPackSpawn;
            }
            if (tankObject.GlobalTransform.x3 > GetScreenWidth())
            {
                tankObject.GlobalTransform.x3 = 0;
            }
            if (tankObject.GlobalTransform.x3 < -tankSprite.Width/2.5)
            {
                tankObject.GlobalTransform.x3 = GetScreenWidth();
            }
            if (tankObject.GlobalTransform.y3 > GetScreenHeight())
            {
                tankObject.GlobalTransform.y3 = 0;
            }
            if (tankObject.GlobalTransform.y3 < -tankSprite.Height/2.5)
            {
                tankObject.GlobalTransform.y3 = GetScreenHeight();
            }
            AmmoPackTimer -= deltaTime;
            tankObject.Update(deltaTime);
            lastTime = currentTime;
        }
        public void delete(int x)
           {
            for (int i = 0; i <= BulletCount; i++)
            {
                Bullet CloneBullet = bullet[x];
                Bullet CloneBullet2 = bullet[x + 1];
                CloneBullet.Transforms = CloneBullet2.Transforms;
                CloneBullet.bulletLifeTime = CloneBullet2.bulletLifeTime;
                bullet[x] = CloneBullet2;
                x++;
            }
            BulletCount--;
        }
        public void Draw()
        {
            BeginDrawing();
            ClearBackground(Color.WHITE);
            DrawText(fps.ToString(), 10, 10, 12, Color.RED);
            DrawText(ammo.ToString(), 50, GetScreenHeight()-100, 100, Color.GREEN);
            for (int i = 0; i < BulletCount; i++)
            {
                AABB hitbox = new AABB(new Calculations.Vector3(bullet[i].Transforms.x1, bullet[i].Transforms.x2, bullet[i].Transforms.x3), 
                                       new Calculations.Vector3(bullet[i].Transforms.y1, bullet[i].Transforms.y2, bullet[i].Transforms.y3));
                float rotation = (float)Math.Atan2(bullet[i].Transforms.y1, bullet[i].Transforms.x1);
                DrawTextureEx(bulletText, new Vector2(bullet[i].Transforms.x3, bullet[i].Transforms.y3), (rotation * (float)(180.0f / Math.PI)) + 90, .5f, Color.GRAY);
                bullet[i].Transforms.x3 += 400 * bullet[i].Transforms.x1 * deltaTime;
                bullet[i].Transforms.y3 += 400 * bullet[i].Transforms.y1 * deltaTime;
                aabb.SetToTransformedBox(hitbox, bullet[i].Transforms);
                List<Calculations.Vector3> HitBoxCorners = new List<Calculations.Vector3>(4);
                 HitBoxCorners = hitbox.Corners();
                for (int x = 0; x < HitBoxCorners.Count; x++)
                {
                    int count = 0;
                    if (count != 3)
                    {
                        count = x;
                    }
                    else
                    {
                        count = 0;
                    }
                    DrawLineEx(new Vector2(HitBoxCorners[x].x, HitBoxCorners[x].y), new Vector2(HitBoxCorners[count].x, HitBoxCorners[count].y), 5, Color.RED);
                }
                if (bullet[i].Transforms.x3 > GetScreenWidth())
                {
                    bullet[i].Transforms.x3 = 0;
                }
                if (bullet[i].Transforms.x3 < -tankSprite.Width / 2.5)
                {
                    bullet[i].Transforms.x3 = GetScreenWidth();
                }
                if (bullet[i].Transforms.y3 > GetScreenHeight())
                {
                    bullet[i].Transforms.y3 = 0;
                }
                if (bullet[i].Transforms.y3 < -tankSprite.Height / 2.5)
                {
                    bullet[i].Transforms.y3 = GetScreenHeight();
                }
                if (bullet[i].bulletLifeTime < 0)
                {
                    delete(i);
                }
                bullet[i].bulletLifeTime -= deltaTime;
            }
            for (int i = 0; i < AmmoPackCounter; i++)
            {
                DrawTextureEx(AmmoPack,ammopack[i].location, 0, 1f, Color.WHITE);
            }
            tankObject.Draw();
            EndDrawing();
        }


    }
    public class Program
    {

        public static void Main()
        {
            Game game = new Game();
            InitWindow(1900, 1000, "Tanks for Everything!");
            game.Init();
            while (!WindowShouldClose())
            {
                game.Update();
                game.Draw();
                
            }
            game.Shutdown();

            CloseWindow();
        }
    }
}

