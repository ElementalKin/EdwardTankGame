using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib;
using static Raylib.Raylib;
using AABB;

namespace TankGame
{
    struct Bullet
    {
        public Calculations.Matrix3 Transforms;
        public float bulletSize;
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
        public Calculations.Matrix3 location;
    }
    struct Target
    {
        public Calculations.Matrix3 location;
    }

    class Game
    {
        public static SceneObject tankObject = new SceneObject();
        public static SceneObject tankAABB = new SceneObject();
        public static SceneObject tankAABB2 = new SceneObject();
        public static SceneObject turretObject = new SceneObject();
        public static SceneObject BulletSpawn = new SceneObject();
        public static SceneObject BulletSpawn2 = new SceneObject();
        public static SpriteObject tankSprite = new SpriteObject();
        public static SpriteObject turretSprite = new SpriteObject();
        public static Texture2D AmmoPack = new Texture2D();
        public static Texture2D ShotExplosion = new Texture2D();
        public static Texture2D BulletExplosion = new Texture2D();
        public static Texture2D bulletText = new Texture2D();
        public static Texture2D TargetText = new Texture2D();
        public static Camera2D camera = new Camera2D();


        Stopwatch stopwatch = new Stopwatch();

        //Initlizing all of the variables that will be used in the game.
        private long currentTime = 0;
        private long lastTime = 0;
        private float timer = 0;
        private int fps = 1;
        private int frames;
        private float deltaTime = 0.005f;
        float reloadSpeed = .2f;
        float reload = 0;
        public float bulletSpeed = 400;
        float bulletSpeedSaved = 0;
        float RotateSpeed = 1;
        int SpeedChange = 100;
        int barrel = 0;
        int CurrentTurret = 0;
        int ammo = 1000;
        float AmmoPackSpawn = 15;
        float AmmoPackTimer = 0;
        float TargetPackSpawn = 3;
        float TargetTimer = 0;
        public static float BulletWidth = 0.5f;
        //Number of player bullets in the air.
        public int BulletCount = 0;
        //Number of enemy bullets in the air.
        public int EnemyBulletCount = 0;
        //Number of ammopacks in the game.
        public int AmmoPackCounter = 0;
        //number of enemys alive.
        public int NumberOfEnemys = 0;
        public string[] Turrets = new string[5] { "resources/tankGreen_Double1.png", "resources/specialBarrel4Green.png", "resources/tankGreen_barrel2.png", "resources/tankGreen_barrel2_outline.png", "resources/tankGreen_barrel3.png" };
        Bullet[] bullet = new Bullet[10000];
        Bullet[] EnemyBullets = new Bullet[10000];
        AmmoPack[] ammopack = new AmmoPack[100000];
        Target[] target = new Target[1000];
        public Calculations.Vector3 BulletAABBHitBoxMin;
        public Calculations.Vector3 BulletAABBHitBoxMax;
        public Calculations.Vector3 TankAABBHitBoxMin;
        public Calculations.Vector3 TankAABBHitBoxMax;
        public Calculations.Vector3 AmmoAABBHitBoxMin;
        public Calculations.Vector3 AmmoAABBHitBoxMax;
        float AmmoPackCenter = 18;
        float TankCenter = 18;
        int Targetcount = 0;
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
            turretSprite.Load(Turrets[0]);
            turretSprite.SetRotate(-90 * (float)(Math.PI / 180.0f));
            // set the turret offset from the tank base
            // set up the scene object hierarchy - parent the turret to the base,
            // then the base to the tank sceneObject
            turretObject.AddChild(turretSprite);
            tankObject.AddChild(tankSprite);
            tankObject.AddChild(turretObject);
            tankObject.AddChild(tankAABB);
            tankObject.AddChild(tankAABB2);
            tankAABB.SetPosition((tankSprite.Width / 2) - 5, tankSprite.Height / 2);
            tankAABB2.SetPosition((-tankSprite.Width / 2), (-tankSprite.Height / 2) - 5);
            turretObject.SetPosition(-10, -2);
            turretSprite.SetPosition(-10, turretSprite.Width / 2.3f);
            turretObject.AddChild(BulletSpawn);
            turretObject.AddChild(BulletSpawn2);
            // having an empty object for the tank parent means we can set the
            // position/rotation of the tank withoutl
            // affecting the offset of the base sprite
            tankObject.SetPosition(GetScreenWidth() / 2.0f, GetScreenHeight() / 2.0f);
            bulletText = LoadTextureFromImage(LoadImage("resources/bulletDark1.png"));
            AmmoPack = LoadTextureFromImage(LoadImage("resources/AmmoPack.png"));
            BulletExplosion = LoadTextureFromImage(LoadImage("resources/explosion2.png"));
            TargetText = LoadTextureFromImage(LoadImage("resources/Target.png"));
            camera.target = new Vector2(tankObject.GlobalTransform.z1 + 20, tankObject.GlobalTransform.z2 + 20);
            camera.offset = new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2);
            camera.zoom = 10000f;

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
            AmmoPackTimer -= deltaTime;
            TargetTimer -= deltaTime;
            reload -= deltaTime;
            if (timer >= 1)
            {
                fps = frames;
                frames = 0;
                timer -= 1;
            }
            frames++;
            if (IsKeyPressed(KeyboardKey.KEY_F))
            {
                if (CurrentTurret == 0)
                {
                    turretSprite.Load(Turrets[1]);
                    reloadSpeed = .4f;
                    BulletWidth = .75f;
                    CurrentTurret++;
                    return;
                }
                else if (CurrentTurret == 1)
                {
                    turretSprite.Load(Turrets[2]);
                    turretSprite.SetPosition(-10, turretSprite.Width / 2.3f);
                    reloadSpeed = .4f;
                    BulletWidth = 1f;
                    CurrentTurret++;
                    return;
                }
                else if (CurrentTurret == 2)
                {
                    turretSprite.Load(Turrets[3]);
                    turretSprite.SetPosition(-10, turretSprite.Width / 2.3f);
                    reloadSpeed = .6f;
                    BulletWidth = 1.25f;
                    CurrentTurret++;
                    return;
                }
                else if (CurrentTurret == 3)
                {
                    turretSprite.Load(Turrets[4]);
                    turretSprite.SetPosition(-10, turretSprite.Width / 2.3f);
                    reloadSpeed = .4f;
                    BulletWidth = 1f;
                    CurrentTurret++;
                    return;
                }
                else if (CurrentTurret == 4)
                {
                    turretSprite.Load(Turrets[0]);
                    turretSprite.SetPosition(-10, turretSprite.Width / 2.3f);
                    reloadSpeed = .2f;
                    BulletWidth = 0.5f;
                    CurrentTurret = 0;
                    return;
                }

            }
            if (IsKeyDown(KeyboardKey.KEY_A))
            {
                tankObject.Rotate(RotateSpeed * -deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_D))
            {
                tankObject.Rotate(RotateSpeed * deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_W))
            {
                Vector3 facing = new Vector3(
                tankObject.LocalTransform.x1,
                tankObject.LocalTransform.x2, 1) * RotateSpeed * deltaTime * 100;
                tankObject.Translate(facing.x, facing.y);
            }
            if (IsKeyDown(KeyboardKey.KEY_S))
            {
                Vector3 facing = new Vector3(
               tankObject.LocalTransform.x1,
               tankObject.LocalTransform.x2, 1) * RotateSpeed * deltaTime * -100;
                tankObject.Translate(facing.x, facing.y);
            }
            if (IsKeyDown(KeyboardKey.KEY_Q))
            {
                turretObject.Rotate(RotateSpeed * -deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_E))
            {
                turretObject.Rotate(RotateSpeed * deltaTime);
            }
            if (IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL))
            {
                RotateSpeed = 2;
            }
            else
            {
                RotateSpeed = 1;
            }
            if (IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT))
            {
                SpeedChange = 10;
            }
            else
            {
                SpeedChange = 100;
            }
            if (IsKeyDown(KeyboardKey.KEY_SPACE))
            {
                if (ammo > 0)
                {

                    if (reload <= 0)
                    {
                        if (CurrentTurret == 0)
                        {
                            if (barrel == 0)
                            {
                                bullet[BulletCount].Transforms = (BulletSpawn.GlobalTransform);
                                bullet[BulletCount].bulletLifeTime = 1000;
                                bullet[BulletCount].bulletSize = BulletWidth;
                                BulletCount++;
                                reload = reloadSpeed;
                                barrel += 1;
                                ammo--;
                            }
                            else
                            {
                                bullet[BulletCount].Transforms = BulletSpawn2.GlobalTransform;
                                bullet[BulletCount].bulletLifeTime = 1000;
                                bullet[BulletCount].bulletSize = BulletWidth;
                                BulletCount++;
                                reload = reloadSpeed;
                                barrel -= 1;
                                ammo--;
                            }
                        }
                        else
                        {
                            bullet[BulletCount].Transforms = (BulletSpawn2.GlobalTransform);
                            bullet[BulletCount].bulletLifeTime = 1000;
                            bullet[BulletCount].bulletSize = BulletWidth;
                            BulletCount++;
                            reload = reloadSpeed;
                            ammo--;
                        }
                    }
                }
            }
            if (IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
            {
                bulletSpeed += SpeedChange;
            }
            if (IsMouseButtonPressed(MouseButton.MOUSE_RIGHT_BUTTON))
            {
                bulletSpeed -= SpeedChange;
            }
            if (IsMouseButtonPressed(MouseButton.MOUSE_MIDDLE_BUTTON))
            {
                bulletSpeedSaved = bulletSpeed;
                bulletSpeed = 0;
            }
            else if (IsMouseButtonReleased(MouseButton.MOUSE_MIDDLE_BUTTON))
            {
                bulletSpeed = bulletSpeedSaved;

            }
            camera.target = new Vector2(tankObject.GlobalTransform.z1 + 20, tankObject.GlobalTransform.z2 + 20);
            camera.offset = new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2);
            camera.zoom = 1.0f;
            if (AmmoPackTimer <= 0)
            {
                ammopack[AmmoPackCounter].location = new Calculations.Matrix3(0, 0, GetRandomValue(0, GetScreenWidth() - 30), 0, 0, GetRandomValue(0, GetScreenHeight()- 30), 0, 0, 0);
                AmmoPackCounter++;
                AmmoPackTimer = AmmoPackSpawn;
            }
            if (TargetTimer <= 0)
            {
                target[Targetcount].location = new Calculations.Matrix3(0, 0, GetRandomValue(0, GetScreenWidth() - 30), 0, 0, GetRandomValue(0, GetScreenHeight() - 30), 0, 0, 0);
                Targetcount++;
                TargetTimer = TargetPackSpawn;
            }
            if (tankObject.GlobalTransform.z1 > GetScreenWidth())
            {
                tankObject.GlobalTransform.z1 = 0;
            }
            if (tankObject.GlobalTransform.z1 < -tankSprite.Width / 2.5)
            {
                tankObject.GlobalTransform.z1 = GetScreenWidth();
            }
            if (tankObject.GlobalTransform.z2 > GetScreenHeight())
            {
                tankObject.GlobalTransform.z2 = 0;
            }
            if (tankObject.GlobalTransform.z2 < -tankSprite.Height / 2.5)
            {
                tankObject.GlobalTransform.z2 = GetScreenHeight();
            }

            TankAABBHitBoxMin = new Calculations.Vector3(tankObject.GlobalTransform.z1 - tankSprite.Width / 2 - (TankCenter * Math.Abs(tankObject.GlobalTransform.x1 * tankObject.GlobalTransform.y1)), tankObject.GlobalTransform.z2 - tankSprite.Height / 2 - (TankCenter * Math.Abs(tankObject.GlobalTransform.x2 * tankObject.GlobalTransform.y2)), 0);
            TankAABBHitBoxMax = new Calculations.Vector3(tankSprite.Width + 1 + (2 * TankCenter * Math.Abs(tankObject.GlobalTransform.x1 * tankObject.GlobalTransform.y1)), tankSprite.Height + 1 + (2 * TankCenter * Math.Abs(tankObject.GlobalTransform.x2 * tankObject.GlobalTransform.y2)), 0);

            tankObject.Update(deltaTime);
            lastTime = currentTime;
        }
        public void BulletDelete(int x)
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
        public void AmmoPackDelete(int x)
        {
            for (int i = 0; i <= AmmoPackCounter; i++)
            {
                AmmoPack CloneAmmo = ammopack[x];
                AmmoPack CloneAmmo2 = ammopack[x + 1];
                CloneAmmo.location = CloneAmmo2.location;
                ammopack[x] = CloneAmmo2;
                x++;
            }
            AmmoPackCounter--;
        }

        public void Draw()
        {
            BeginDrawing();
            ClearBackground(Color.WHITE);
            DrawText(fps.ToString(), 10, 10, 12, Color.RED);
            DrawText(ammo.ToString(), 50, GetScreenHeight() - 100, 100, Color.GREEN);
            for (int i = 0; i < AmmoPackCounter; i++)
            {
                AmmoAABBHitBoxMin = new Calculations.Vector3(ammopack[i].location.z1, ammopack[i].location.z2, 0);
                AmmoAABBHitBoxMax = new Calculations.Vector3(AmmoPack.width + 1 + (2 * AmmoPackCenter * Math.Abs(ammopack[i].location.x1 * ammopack[i].location.y1)), AmmoPack.height + 1 + (2 * AmmoPackCenter * Math.Abs(ammopack[i].location.x2 * ammopack[i].location.y2)), 0);
                Calculations.Vector3 Tankmax = new Calculations.Vector3(TankAABBHitBoxMax.x + TankAABBHitBoxMin.x, TankAABBHitBoxMax.y + TankAABBHitBoxMin.y, 0);
                Calculations.Vector3 Ammomax = new Calculations.Vector3(AmmoAABBHitBoxMax.x + AmmoAABBHitBoxMin.x, AmmoAABBHitBoxMax.y + AmmoAABBHitBoxMin.y, 0);
                DrawRectangleLines((int)AmmoAABBHitBoxMin.x, (int)AmmoAABBHitBoxMin.y, (int)AmmoAABBHitBoxMax.x, (int)AmmoAABBHitBoxMax.y, Color.RED);
                DrawTextureEx(AmmoPack, new Vector2(ammopack[i].location.z1, ammopack[i].location.z2), 0, 1f, Color.WHITE);
                if (Tankmax.x > AmmoAABBHitBoxMin.x && TankAABBHitBoxMin.x < AmmoAABBHitBoxMin.x)
                {
                    if (Tankmax.y > AmmoAABBHitBoxMin.y && TankAABBHitBoxMin.y < AmmoAABBHitBoxMin.y)
                    {
                        ammo += 100;
                        AmmoPackDelete(i);
                    }
                    else if (Tankmax.y > Ammomax.y && TankAABBHitBoxMin.y < Ammomax.y)
                    {
                        ammo += 100;
                        AmmoPackDelete(i);
                    }
                }
                else if (Tankmax.x > Ammomax.x && TankAABBHitBoxMin.x < Ammomax.x)
                {
                    if (Tankmax.y > Ammomax.y && TankAABBHitBoxMin.y < Ammomax.y)
                    {
                        ammo += 100;
                        AmmoPackDelete(i);
                    }
                    else if (Tankmax.y > AmmoAABBHitBoxMin.y && TankAABBHitBoxMin.y < AmmoAABBHitBoxMin.y)
                    {
                        ammo += 100;
                        AmmoPackDelete(i);
                    }
                }
            }
            for (int i = 0; i < Targetcount; i++)
            {
                DrawTextureEx(TargetText, new Vector2(target[i].location.z1, target[i].location.z2), 0, 1f, Color.WHITE);
            }
            for (int i = 0; i < BulletCount; i++)
            {
                BulletAABBHitBoxMin = new Calculations.Vector3(bullet[i].Transforms.z1 - bulletText.width / 2 - (1 * Math.Abs(bullet[i].Transforms.x1 * bullet[i].Transforms.y1)), bullet[i].Transforms.z2 - bulletText.height / 2 - (1 * Math.Abs(bullet[i].Transforms.x2 * bullet[i].Transforms.y2)), 0);
                BulletAABBHitBoxMax = new Calculations.Vector3(bulletText.width + 1 + (2 * 1 * Math.Abs(bullet[i].Transforms.x1 * bullet[i].Transforms.y1)), bulletText.height + 1 + (2 * 1 * Math.Abs(bullet[i].Transforms.x2 * bullet[i].Transforms.y2)), 0);
                DrawRectangleLines((int)BulletAABBHitBoxMin.x, (int)BulletAABBHitBoxMin.y, (int)BulletAABBHitBoxMax.x, (int)BulletAABBHitBoxMax.y, Color.RED);
                float rotation = (float)Math.Atan2(bullet[i].Transforms.x2, bullet[i].Transforms.x1);
                Rectangle hitBox = new Rectangle(bullet[i].Transforms.z1, bullet[i].Transforms.z2, bulletText.width / 2, bulletText.height / 2);
                DrawRectanglePro(hitBox, new Vector2(0, 0), (rotation * (float)(180.0f / Math.PI)) + 90, Color.RED);
                DrawTextureEx(bulletText, new Vector2(bullet[i].Transforms.z1, bullet[i].Transforms.z2), (rotation * (float)(180.0f / Math.PI)) + 90, bullet[i].bulletSize, Color.RAYWHITE);
                bullet[i].Transforms.z1 += bulletSpeed * bullet[i].Transforms.x1 * deltaTime;
                bullet[i].Transforms.z2 += bulletSpeed * bullet[i].Transforms.x2 * deltaTime;
                if (bullet[i].Transforms.z1 > GetScreenWidth())
                {
                    bullet[i].Transforms.z1 = 0;
                }
                if (bullet[i].Transforms.z1 < -tankSprite.Width / 2.5)
                {
                    bullet[i].Transforms.z1 = GetScreenWidth();
                }
                if (bullet[i].Transforms.z2 > GetScreenHeight())
                {
                    bullet[i].Transforms.z2 = 0;
                }
                if (bullet[i].Transforms.z2 < -tankSprite.Height / 2.5)
                {
                    bullet[i].Transforms.z2 = GetScreenHeight();
                }
                if (bullet[i].bulletLifeTime < 0)
                {
                    AmmoPackDelete(i);
                }
                bullet[i].bulletLifeTime -= deltaTime;
            }
            //DrawRectangleLines((int)TankAABBHitBoxMin.x,(int)TankAABBHitBoxMin.y,(int)TankAABBHitBoxMax.x, (int)TankAABBHitBoxMax.y,Color.RED);
            DrawLine((int)TankAABBHitBoxMin.x, (int)TankAABBHitBoxMin.y, (int)TankAABBHitBoxMin.x, (int)TankAABBHitBoxMin.y + (int)TankAABBHitBoxMax.y, Color.RED);
            DrawLine((int)TankAABBHitBoxMin.x, (int)TankAABBHitBoxMin.y + (int)TankAABBHitBoxMax.y, (int)TankAABBHitBoxMin.x + (int)TankAABBHitBoxMax.x, (int)TankAABBHitBoxMin.y + (int)TankAABBHitBoxMax.y, Color.RED);
            DrawLine((int)TankAABBHitBoxMin.x + (int)TankAABBHitBoxMax.x, (int)TankAABBHitBoxMin.y + (int)TankAABBHitBoxMax.y, (int)TankAABBHitBoxMin.x + (int)TankAABBHitBoxMax.x, (int)TankAABBHitBoxMin.y, Color.RED);
            DrawLine((int)TankAABBHitBoxMin.x + (int)TankAABBHitBoxMax.x, (int)TankAABBHitBoxMin.y, (int)TankAABBHitBoxMin.x, (int)TankAABBHitBoxMin.y, Color.RED);
            //for (int i = 0; i < TankHitBoxCorners.Count; i++)
            //{
            //    if (i != 3)
            //    {
            //        DrawLine((int)TankHitBoxCorners[i].x, (int)TankHitBoxCorners[i].y, (int)TankHitBoxCorners[i + 1].x, (int)TankHitBoxCorners[i + 1].y, Color.RED);
            //    }
            //    else
            //    {
            //        DrawLine((int)TankHitBoxCorners[3].x, (int)TankHitBoxCorners[3].y, (int)TankHitBoxCorners[0].x, (int)TankHitBoxCorners[0].y, Color.RED);
            //    }
            //}

            tankObject.Draw();
            EndDrawing();
        }


    }
    public class Program
    {

        public static void Main()
        {
            Game game = new Game();
            InitWindow(1900, 1000, "Tank Game");
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

