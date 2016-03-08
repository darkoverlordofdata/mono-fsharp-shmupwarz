[<AutoOpen>]
module EntityModule
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content
(**
 * Entity Factory
 *
 *)

let ScreenWidth = 640
let ScreenHeight = 480
let rnd = System.Random()
let mutable UniqueId = 0
let NextUniqueId() = 
    UniqueId <- UniqueId + 1
    UniqueId



(** Create an Animation Component *)
let CreateSprite(layer:Layer, texture:Texture2D) =
    { 
        Texture = texture; 
        Width = texture.Width;
        Height = texture.Height;
    }

(** Create a Health Component *)
let CreateHealth(curHealth: int, maxHealth : int) =
    {
        CurHealth = curHealth;
        MaxHealth = maxHealth;
    }

let CreateScaleAnimation(min: float32, max: float32, speed: float32, repeat: bool, active: bool) =
    {
        Min = min;
        Max = max;
        Speed = speed;
        Repeat = repeat;
        Active = active;
    } 


(** Create a Player Entity *)
let CreatePlayer (content:ContentManager, position) =
    let sprite = CreateSprite(Layer.PLAYER, content.Load<Texture2D>("images/fighter.png"))
    {
        Id = NextUniqueId();
        Name = "Player";
        Active = true;
        EntityType = Player; 
        Position = position; 
        Size = Vector2(float32 sprite.Width, float32 sprite.Height);
        Layer = Layer.PLAYER;

        Bounds = Some(43);
        Expires = None;
        Health = Some(CreateHealth(100, 100));
        Velocity = Some(Vector2(0.f, 0.f));
        Sprite = Some(sprite);
        Scale = None;
        ScaleAnimation = None;
    }
     
(** Create a Bullet Entity *)
let CreateBullet (content:ContentManager, position) =
    let sprite = CreateSprite(Layer.BULLET, content.Load<Texture2D>("images/bullet.png"))
    {
        Id = NextUniqueId();
        Name = "Bullet";
        Active = false;
        EntityType = Bullet; 
        Position = position; 
        Size = Vector2(float32 sprite.Width, float32 sprite.Height);
        Layer = Layer.BULLET;

        Bounds = Some(5);
        Expires = Some(0.1f);
        Health = None;
        Velocity = Some(Vector2(0.f, -800.f));
        Sprite = Some(sprite);
        Scale = None;
        ScaleAnimation = None;
    }

(** Create Enemy *)
let CreateEnemy1 (content:ContentManager)  =
    let position = Vector2(float32(rnd.Next(ScreenWidth)), 100.f)
    let sprite = CreateSprite(Layer.ENEMY1, content.Load<Texture2D>("images/enemy1.png"))
    {
        Id = NextUniqueId();
        Name = "Enemy1";
        Active = false;
        EntityType = Enemy; 
        Position = position; 
        Size = Vector2(float32 sprite.Width, float32 sprite.Height);
        Layer = Layer.ENEMY1;

        Bounds = Some(20);
        Expires = None
        Health = Some(CreateHealth(10, 10));
        Velocity = Some(Vector2(0.f, 40.f));
        Sprite = Some(sprite);
        Scale = None;
        ScaleAnimation = None;
    }

(** Create Enemy *)
let CreateEnemy2 (content:ContentManager) =
    let position = Vector2(float32(rnd.Next(ScreenWidth)), 200.f)
    let sprite = CreateSprite(Layer.ENEMY2, content.Load<Texture2D>("images/enemy2.png"))
    {
        Id = NextUniqueId();
        Name = "Enemy2";
        Active = false;
        EntityType = Enemy; 
        Position = position; 
        Size = Vector2(float32 sprite.Width, float32 sprite.Height);
        Layer = Layer.ENEMY2;

        Bounds = Some(40);
        Expires = None
        Health = Some(CreateHealth(20, 20));
        Velocity = Some(Vector2(0.f, 30.f));
        Sprite = Some(sprite);
        Scale = None;
        ScaleAnimation = None;
    }

(** Create Enemy *)
let CreateEnemy3 (content:ContentManager)  =
    let position = Vector2(float32(rnd.Next(ScreenWidth)), 300.f)
    let sprite = CreateSprite(Layer.ENEMY3, content.Load<Texture2D>("images/enemy3.png"))
    {
        Id = NextUniqueId();
        Name = "Enemy3";
        Active = false;
        EntityType = Enemy; 
        Position = position; 
        Size = Vector2(float32 sprite.Width, float32 sprite.Height);
        Layer = Layer.ENEMY3;

        Bounds = Some(70);
        Expires = None
        Health = Some(CreateHealth(60, 60));
        Velocity = Some(Vector2(0.f, 20.f));
        Sprite = Some(sprite);
        Scale = None;
        ScaleAnimation = None;
    }


(** Create Big Explosion *)
let CreateExplosion (content:ContentManager, position:Vector2, scale:float32) =
    let sprite = CreateSprite(Layer.EXPLOSION, content.Load<Texture2D>("images/explosion.png"))
    {
        Id = NextUniqueId();
        Name = "Explosion";
        Active = false;
        EntityType = Explosion; 
        Position = position; 
        Size = Vector2(float32 sprite.Width, float32 sprite.Height);
        Layer = Layer.EXPLOSION;

        Bounds = None;
        Expires = Some(0.2f);
        Health = None;
        Velocity = None;
        Sprite = Some(sprite);
        Scale = Some(Vector2(scale, scale))
        ScaleAnimation = Some(CreateScaleAnimation(scale/100.f, scale, -3.f, false, true));
    }


    