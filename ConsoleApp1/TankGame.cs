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
    /// <summary>
    /// The struct for the bullets.
    /// </summary>
    struct Bullet
    {
        //the Matrix3 that is used for placing and moving the bullet.
        public Calculations.Matrix3 Transforms;
        //The bullets size so I can change it for the diffrent barrels.
        public float bulletSize;
        //How long the bullet will remain in the game.
        public float bulletLifeTime;
    }
    /// <summary>
    /// The enemy tank I was going to try and use if I had time.
    /// </summary>
    struct EnemyTank
    {
        //The parts of the enemy tank.
        public SceneObject EnemyBody;
        public SceneObject EnemyTurret;
        public SceneObject EnemyBulletSpawn;
        public SpriteObject EnemyBodyText;
        public SpriteObject EnemyTurretText;

    }
    /// <summary>
    /// 
    /// </summary>
    struct AmmoPack
    {
        public Calculations.Matrix3 location;
    }
    /// <summary>
    /// The Target struct.
    /// </summary>
    struct Target
    {
        //The targets location
        public Calculations.Matrix3 location;
    }

    class Game
    {
        //All the sceneObjects, SpriteObjects, and Textures with a Camera I was trying to mess with.
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
        //Timer
        private float timer = 0;
        //Your FPS.
        private int fps = 1;
        //Your frames.
        private int frames;
        //DeltaTime.
        private float deltaTime = 0.005f;
        //How long the reload should be. 
        float reloadSpeed = .2f;
        //how long untill you can shoot again.
        float reload = 0;
        //The speed of the bullet.
        public float bulletSpeed = 400;
        //saving the current speed of the bullet.
        float bulletSpeedSaved = 0;
        //How fast you rotate.
        float RotateSpeed = 1;
        //How much the change in the bullet should be.
        int SpeedChange = 100;
        //What barrel you are using.
        int barrel = 0;
        //What turret you currently have.
        int CurrentTurret = 0;
        //How much ammo you have.
        int ammo = 1000;
        //how long it should take for a Ammopack to spawn.
        float AmmoPackSpawn = 15;
        //How lomng untll the next ammo pack should spawn.
        float AmmoPackTimer = 0;
        //how long it should take for a target to spawn.
        float TargetSpawnTime = 3;
        //How long untill the next target spawns
        float TargetTimer = 0;
        //The bullet width for the bullets size(since it changes with the turret type).
        public static float BulletWidth = 0.5f;
        //Number of player bullets in the air.
        public int BulletCount = 0;
        //Number of enemy bullets in the air(WIP).
        public int EnemyBulletCount = 0;
        //Number of ammopacks in the game.
        public int AmmoPackCounter = 0;
        //Number of enemys alive(WIP).
        public int NumberOfEnemys = 0;
        //Used for loading the diffrent barrel types.
        public string[] Turrets = new string[5] { "resources/tankGreen_Double1.png", "resources/specialBarrel4Green.png", "resources/tankGreen_barrel2.png", "resources/tankGreen_barrel2_outline.png", "resources/tankGreen_barrel3.png" };
        //All the arrays for the bullets, enemybullets(WIP), Ammopacks, and targets.
        Bullet[] bullet = new Bullet[10000];
        Bullet[] EnemyBullets = new Bullet[10000];
        AmmoPack[] ammopack = new AmmoPack[100000];
        Target[] target = new Target[1000];
        //Making all the hit boxes variables.
        public Calculations.Vector3 TankAABBHitBoxMin;
        public Calculations.Vector3 TankAABBHitBoxMax;
        public Calculations.Vector3 AmmoAABBHitBoxMin;
        public Calculations.Vector3 AmmoAABBHitBoxMax;
        float AmmoPackCenter = 18;
        float TankCenter = 18;
        //How many Targets there are.
        int Targetcount = 0;
        public void Init()
        {
            //Initilizes(sets its position and rotation in center of the screen) the tank and all of its sprites.
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
            //Messing with camera. (Didn't figure out how to use it and I didnt have time to spare to actually do any research into how to use it.)
            camera.target = new Vector2(tankObject.GlobalTransform.x3 + 20, tankObject.GlobalTransform.y3 + 20);
            camera.offset = new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2);
            camera.zoom = 10000f;

        }

        public void Shutdown()
        { }
        /// <summary>
        /// Updates all the stuff if things happen.
        /// </summary>
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
            //Movement.
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
                tankObject.LocalTransform.y1, 1) * RotateSpeed * deltaTime * 100;
                tankObject.Translate(facing.x, facing.y);
            }
            if (IsKeyDown(KeyboardKey.KEY_S))
            {
                Vector3 facing = new Vector3(
               tankObject.LocalTransform.x1,
               tankObject.LocalTransform.y1, 1) * RotateSpeed * deltaTime * -100;
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
            //used for changing the tanks turn and move speed for testing reasons
            if (IsKeyDown(KeyboardKey.KEY_LEFT_CONTROL))
            {
                RotateSpeed = 2;
            }
            else
            {
                RotateSpeed = 1;
            }
            //used to help controll the bullet speed changing.
            if (IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT))
            {
                SpeedChange = 10;
            }
            else
            {
                SpeedChange = 100;
            }
            //Used for shooting.
            if (IsKeyDown(KeyboardKey.KEY_SPACE))
            {
                //Checks to make sure you have ammo.
                if (ammo > 0)
                {
                    //Checks to see if the tank is still reloading a shell.
                    if (reload <= 0)
                    {
                        //this part of the code is to check if you are on the dual turret or not since the dual barrel turret act diffrently
                        if (CurrentTurret == 0)
                        {
                            //switches between the barrels
                            if (barrel == 0)
                            {
                                //makes the bullet and sets all of its variables
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
            //used to increase the speed of the bullets for testing reasons
            if (IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
            {
                bulletSpeed += SpeedChange;
            }
            //used to decrease the speed of the bullets for testing reasons
            if (IsMouseButtonPressed(MouseButton.MOUSE_RIGHT_BUTTON))
            {
                bulletSpeed -= SpeedChange;
            }
            //used to pause the speed of the bullets for testing reasons
            if (IsMouseButtonPressed(MouseButton.MOUSE_MIDDLE_BUTTON))
            {
                bulletSpeedSaved = bulletSpeed;
                bulletSpeed = 0;
            }
            //used to pause the speed of the bullets for testing reasons
            else if (IsMouseButtonReleased(MouseButton.MOUSE_MIDDLE_BUTTON))
            {
                bulletSpeed = bulletSpeedSaved;

            }
            camera.target = new Vector2(tankObject.GlobalTransform.x3 + 20, tankObject.GlobalTransform.y3 + 20);
            camera.offset = new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2);
            camera.zoom = 1.0f;
            //spawns a AmmoPack about every 15 seconds.
            if (AmmoPackTimer <= 0)
            {
                ammopack[AmmoPackCounter].location = new Calculations.Matrix3(0, 0, GetRandomValue(0, GetScreenWidth() - 30), 0, 0, GetRandomValue(0, GetScreenHeight()- 30), 0, 0, 0);
                AmmoPackCounter++;
                AmmoPackTimer = AmmoPackSpawn;
            }
            //spawns a Target about every 3 seconds.
            //Targets currently have no sort of collision.
            if (TargetTimer <= 0)
            {
                target[Targetcount].location = new Calculations.Matrix3(0, 0, GetRandomValue(0, GetScreenWidth() - 30), 0, 0, GetRandomValue(0, GetScreenHeight() - 30), 0, 0, 0);
                Targetcount++;
                TargetTimer = TargetSpawnTime;
            }
            //Wraps the tank around the screen.
            if (tankObject.GlobalTransform.x3 > GetScreenWidth())
            {
                tankObject.GlobalTransform.x3 = 0;
            }
            if (tankObject.GlobalTransform.x3 < -tankSprite.Width / 2.5)
            {
                tankObject.GlobalTransform.x3 = GetScreenWidth();
            }
            if (tankObject.GlobalTransform.y3 > GetScreenHeight())
            {
                tankObject.GlobalTransform.y3 = 0;
            }
            if (tankObject.GlobalTransform.y3 < -tankSprite.Height / 2.5)
            {
                tankObject.GlobalTransform.y3 = GetScreenHeight();
            }
            //makes the values i use to make the hit box around the tank.
            TankAABBHitBoxMin = new Calculations.Vector3(tankObject.GlobalTransform.x3 - tankSprite.Width / 2 - (TankCenter * Math.Abs(tankObject.GlobalTransform.x1 * tankObject.GlobalTransform.x2)), tankObject.GlobalTransform.y3 - tankSprite.Height / 2 - (TankCenter * Math.Abs(tankObject.GlobalTransform.y1 * tankObject.GlobalTransform.y2)), 0);
            TankAABBHitBoxMax = new Calculations.Vector3(tankSprite.Width + 1 + (2 * TankCenter * Math.Abs(tankObject.GlobalTransform.x1 * tankObject.GlobalTransform.x2)), tankSprite.Height + 1 + (2 * TankCenter * Math.Abs(tankObject.GlobalTransform.y1 * tankObject.GlobalTransform.y2)), 0);
            //updates the tank.
            tankObject.Update(deltaTime);
            lastTime = currentTime;
        }
        /// <summary>
        /// "Deletes" the bullet.
        /// </summary>
        /// <param name="x"></param>
        public void BulletDelete(int x)
        {
            for (int i = 0; i <= BulletCount; i++)
            {
                Bullet CloneBullet = bullet[x];
                Bullet CloneBullet2 = bullet[x + 1];
                CloneBullet.Transforms = CloneBullet2.Transforms;
                CloneBullet.bulletSize = CloneBullet2.bulletSize;
                CloneBullet.bulletLifeTime = CloneBullet2.bulletLifeTime;
                bullet[x] = CloneBullet2;
                x++;
            }
            BulletCount--;
        }
        /// <summary>
        /// "Deletes" the AmmoPack.
        /// </summary>
        /// <param name="x"></param>
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
        /// <summary>
        /// Draws all the bullets and stuff plus some math.
        /// </summary>
        public void Draw()
        {
            BeginDrawing();
            //makes the background.
            ClearBackground(Color.WHITE);
            //Draws the fps.
            DrawText(fps.ToString(), 10, 10, 12, Color.RED);
            //Draws how much ammo you have.(migh move it to the end so it draws over the bullets and such but since I couldent figure out the camera it s kinda of usless to do that.)
            DrawText(ammo.ToString(), 50, GetScreenHeight() - 100, 100, Color.GREEN);
            //Draws the AmmoPacks at the position they are at.
            for (int i = 0; i < AmmoPackCounter; i++)
            {
                AmmoAABBHitBoxMin = new Calculations.Vector3(ammopack[i].location.x3, ammopack[i].location.y3, 0);
                AmmoAABBHitBoxMax = new Calculations.Vector3(AmmoPack.width + 1 + (2 * AmmoPackCenter * Math.Abs(ammopack[i].location.x1 * ammopack[i].location.x2)), AmmoPack.height + 1 + (2 * AmmoPackCenter * Math.Abs(ammopack[i].location.y1 * ammopack[i].location.y2)), 0);
                Calculations.Vector3 Tankmax = new Calculations.Vector3(TankAABBHitBoxMax.x + TankAABBHitBoxMin.x, TankAABBHitBoxMax.y + TankAABBHitBoxMin.y, 0);
                Calculations.Vector3 Ammomax = new Calculations.Vector3(AmmoAABBHitBoxMax.x + AmmoAABBHitBoxMin.x, AmmoAABBHitBoxMax.y + AmmoAABBHitBoxMin.y, 0);
                DrawRectangleLines((int)AmmoAABBHitBoxMin.x, (int)AmmoAABBHitBoxMin.y, (int)AmmoAABBHitBoxMax.x, (int)AmmoAABBHitBoxMax.y, Color.RED);
                DrawTextureEx(AmmoPack, new Vector2(ammopack[i].location.x3, ammopack[i].location.y3), 0, 1f, Color.WHITE);
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
            //Draws the Targets at the location they are at. (Again targets are a wip that I probably don't have time to do.)
            for (int i = 0; i < Targetcount; i++)
            {
                DrawTextureEx(TargetText, new Vector2(target[i].location.x3, target[i].location.y3), 0, 1f, Color.WHITE);
            }
            for (int i = 0; i < BulletCount; i++)
            {

                //The rotation of the bullet.
                float rotation = (float)Math.Atan2(bullet[i].Transforms.y1, bullet[i].Transforms.x1);
                //The rectangle that is used in the rectanglePro.
                //Rectangle hitBox = new Rectangle(bullet[i].Transforms.x3, bullet[i].Transforms.y3, bulletText.width / 2, bulletText.height / 2);
                //Was messing with RectanglePro to see if I can use it for hit detection(I probably can it just isnt what the project asks for so I stopped.)
                //DrawRectanglePro(hitBox, new Vector2(0, 0), (rotation * (float)(180.0f / Math.PI)) + 90, Color.RED);
                DrawTextureEx(bulletText, new Vector2(bullet[i].Transforms.x3 , bullet[i].Transforms.y3), (rotation * (float)(180.0f / Math.PI)) + 90, bullet[i].bulletSize, Color.RAYWHITE);

                //little blue dot for the location of the bullet. (used for testing and debugging.)
                DrawRectangle((int)bullet[i].Transforms.x3,(int)bullet[i].Transforms.y3, 1,1, Color.BLUE);
                //Changes the bullets position.
                bullet[i].Transforms.x3 += bulletSpeed * bullet[i].Transforms.x1 * deltaTime;
                bullet[i].Transforms.y3 += bulletSpeed * bullet[i].Transforms.y1 * deltaTime;
                //Bullet wraping (insn't perfect offsets the bullets slightly.)
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
                    AmmoPackDelete(i);
                }
                bullet[i].bulletLifeTime -= deltaTime;
            }
            //DrawRectangleLines((int)TankAABBHitBoxMin.x,(int)TankAABBHitBoxMin.y,(int)TankAABBHitBoxMax.x, (int)TankAABBHitBoxMax.y,Color.RED);
            DrawLine((int)TankAABBHitBoxMin.x, (int)TankAABBHitBoxMin.y, (int)TankAABBHitBoxMin.x, (int)TankAABBHitBoxMin.y + (int)TankAABBHitBoxMax.y, Color.RED);
            DrawLine((int)TankAABBHitBoxMin.x, (int)TankAABBHitBoxMin.y + (int)TankAABBHitBoxMax.y, (int)TankAABBHitBoxMin.x + (int)TankAABBHitBoxMax.x, (int)TankAABBHitBoxMin.y + (int)TankAABBHitBoxMax.y, Color.RED);
            DrawLine((int)TankAABBHitBoxMin.x + (int)TankAABBHitBoxMax.x, (int)TankAABBHitBoxMin.y + (int)TankAABBHitBoxMax.y, (int)TankAABBHitBoxMin.x + (int)TankAABBHitBoxMax.x, (int)TankAABBHitBoxMin.y, Color.RED);
            DrawLine((int)TankAABBHitBoxMin.x + (int)TankAABBHitBoxMax.x, (int)TankAABBHitBoxMin.y, (int)TankAABBHitBoxMin.x, (int)TankAABBHitBoxMin.y, Color.RED);
            //The old draw line.
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
            //Draws the tank.
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
                //Calls Update to do all the math and stuff for the game.
                game.Update();
                //Calls Draw and draws all the stuff to make the game be.
                game.Draw();

            }
            game.Shutdown();

            CloseWindow();
        }
    }
}

