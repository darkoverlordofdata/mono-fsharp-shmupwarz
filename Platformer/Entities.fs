[<AutoOpen>]
module EntitiesModule
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
let EFFECT_PEW = 0
let EFFECT_ASPLODE = 1
let EFFECT_SMALLASPLODE = 1
let Scale1 = Vector2(1.f, 1.f)
let ScaleA = Vector2(0.5f, 0.5f)
let ScaleB = Vector2(0.1f, 0.1f)

(** Create an Animation Component *)
let CreateSprite(layer:Layer, texture:Texture2D, scale:Vector2) =
    { 
        Texture = texture; 
        Width = texture.Width;
        Height = texture.Height;
        Scale = Vector2(scale.X, scale.Y)
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
let CreatePlayer (content:ContentManager) (position) =
    UniqueId <- UniqueId + 1
    let sprite = CreateSprite(Layer.PLAYER, content.Load<Texture2D>("images/fighter.png"), Scale1)
    {
        Id = UniqueId;
        EntityType = Player; 
        Position = position; 
        Size = Vector2(float32 sprite.Width, float32 sprite.Height);
        Destroy = false;
        Layer = Layer.PLAYER;

        Bounds = Some(43);
        Expires = None;
        Health = Some(CreateHealth(100, 100));
        Velocity = Some(Vector2(0.f, 0.f));
        Sprite = Some(sprite);
        BodyType = Dynamic(Vector2(0.f,0.f)); 
        Scale = None;
        ScaleAnimation = None;
    }
     
(** Create a Bullet Entity *)
let CreateBullet (content:ContentManager) (position) =
    UniqueId <- UniqueId + 1
    let sprite = CreateSprite(Layer.BULLET, content.Load<Texture2D>("images/bullet.png"), Scale1)
    {
        Id = UniqueId;
        EntityType = Bullet; 
        Position = position; 
        Size = Vector2(float32 sprite.Width, float32 sprite.Height);
        Destroy = false;
        Layer = Layer.PLAYER;

        Bounds = Some(5);
        Expires = Some(1000.f);
        Health = None;
        Velocity = Some(Vector2(0.f, -800.f));
        Sprite = Some(sprite);
        BodyType = Dynamic(Vector2(0.f,0.f)); 
        Scale = None;
        ScaleAnimation = None;
    }

(** Create Enemy *)
let CreateEnemy1 (content:ContentManager) =
    UniqueId <- UniqueId + 1
    let position = Vector2(float32(rnd.Next(ScreenWidth)), 100.f)
    let sprite = CreateSprite(Layer.ACTORS_1, content.Load<Texture2D>("images/enemy1.png"), Scale1)
    {
        Id = UniqueId;
        EntityType = Enemy; 
        Position = position; 
        Size = Vector2(float32 sprite.Width, float32 sprite.Height);
        Destroy = false;
        Layer = Layer.ACTORS_1;

        Bounds = Some(20);
        Expires = None
        Health = Some(CreateHealth(10, 10));
        Velocity = Some(Vector2(0.f, 40.f));
        Sprite = Some(sprite);
        BodyType = Dynamic(Vector2(0.f,0.f)); 
        Scale = None;
        ScaleAnimation = None;
    }

(** Create Enemy *)
let CreateEnemy2 (content:ContentManager) =
    UniqueId <- UniqueId + 1
    let position = Vector2(float32(rnd.Next(ScreenWidth)), 200.f)
    let sprite = CreateSprite(Layer.ACTORS_2, content.Load<Texture2D>("images/enemy2.png"), Scale1)
    {
        Id = UniqueId;
        EntityType = Enemy; 
        Position = position; 
        Size = Vector2(float32 sprite.Width, float32 sprite.Height);
        Destroy = false;
        Layer = Layer.ACTORS_2;

        Bounds = Some(20);
        Expires = None
        Health = Some(CreateHealth(20, 20));
        Velocity = Some(Vector2(0.f, 30.f));
        Sprite = Some(sprite);
        BodyType = Dynamic(Vector2(0.f,0.f)); 
        Scale = None;
        ScaleAnimation = None;
    }

(** Create Enemy *)
let CreateEnemy3 (content:ContentManager) =
    UniqueId <- UniqueId + 1
    let position = Vector2(float32(rnd.Next(ScreenWidth)), 300.f)
    let sprite = CreateSprite(Layer.ACTORS_3, content.Load<Texture2D>("images/enemy3.png"), Scale1)
    {
        Id = UniqueId;
        EntityType = Enemy; 
        Position = position; 
        Size = Vector2(float32 sprite.Width, float32 sprite.Height);
        Destroy = false;
        Layer = Layer.ACTORS_3;

        Bounds = Some(70);
        Expires = None
        Health = Some(CreateHealth(60, 60));
        Velocity = Some(Vector2(0.f, 20.f));
        Sprite = Some(sprite);
        BodyType = Dynamic(Vector2(0.f,0.f)); 
        Scale = None;
        ScaleAnimation = None;
    }


(** Create Big Explosion *)
let CreateBigExplosion (content:ContentManager, position:Vector2) =
    UniqueId <- UniqueId + 1
    let sprite = CreateSprite(Layer.ACTORS_3, content.Load<Texture2D>("images/explosion.png"), ScaleA)
    {
        Id = UniqueId;
        EntityType = Explosion; 
        Position = position; 
        Size = Vector2(float32 sprite.Width, float32 sprite.Height);
        Destroy = false;
        Layer = Layer.PARTICLE;

        Bounds = None;
        Expires = Some(0.5f);
        Health = None;
        Velocity = None;
        Sprite = Some(sprite);
        BodyType = Dynamic(Vector2(0.f,0.f)); 
        Scale = Some(ScaleA)
        ScaleAnimation = Some(CreateScaleAnimation(0.005f, 0.5f, -3.f, false, true));
    }


(** Create Small Explosion *)
let CreateSmallExplosion (content:ContentManager, position:Vector2) =
    UniqueId <- UniqueId + 1
    let sprite = CreateSprite(Layer.ACTORS_3, content.Load<Texture2D>("images/explosion.png"), ScaleB)
    {
        Id = UniqueId;
        EntityType = Explosion; 
        Position = position; 
        Size = Vector2(float32 sprite.Width, float32 sprite.Height);
        Destroy = false;
        Layer = Layer.PARTICLE;

        Bounds = None;
        Expires = Some(0.1f);
        Health = None;
        Velocity = None;
        Sprite = Some(sprite);
        BodyType = Dynamic(Vector2(0.f,0.f)); 
        Scale = Some(ScaleB)
        ScaleAnimation = Some(CreateScaleAnimation(0.001f, 0.1f, -3.f, false, true));
    }


