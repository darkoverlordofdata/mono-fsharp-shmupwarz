module Components
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content

(** Player State *)
type PlayerState =
    | Nothing
    | Jumping

type Layer =
    | DEFAULT
    | BACKGROUND
    | TEXT
    | LIVES
    | MINES
    | ACTORS_1
    | ACTORS_2
    | ACTORS_3
    | PLAYER
    | BULLET
    | PARTICLE
    | HUD

(** BodyType Component *)
type BodyType =
    | Static
    | Dynamic of Vector2

(** EntityType Component *)
type EntityType =
    | Background
    | Bullet
    | Enemy
    | Mine
    | Player of PlayerState
    | Status

(** Sprite Component *)
type Sprite =
    {
        Width: int;
        Height: int;
        Texture : Texture2D;
        Rect : System.Nullable<Rectangle>;
        Layer : Layer;
    }

(** Health Component *)
type Health =
    {
        CurHealth: int;
        MaxHealth: int;
    }


(** Entity is a record of components *)
type Entity =
    {
        Id : int;
        EntityType : EntityType;
        Size : Vector2;
        Position : Vector2;
        BodyType : BodyType;
        Destroy : bool;
        // Optional Components
        Sprite : Sprite option;
        Bounds : int option;
        Expires : float32 option;
        Health : Health option;
        Velocity : Vector2 option;
        Layer : Layer option;
    }

let ScreenWidth = 640
let ScreenHeight = 480
let FrameWidth = 32
let FrameHeight = 32
let mutable UniqueId = 0
let rnd = System.Random()

(** Create an Animation Component *)
let CreateSprite(layer:Layer, texture:Texture2D) =
    { 
        Texture = texture; 
        Width = texture.Width;
        Height = texture.Height;
        Rect = System.Nullable(Rectangle(0, 0, texture.Width, texture.Height));
        Layer = layer;
    }

(** Create a Health Component *)
let CreateHealth(maxHealth : int) =
    {
        MaxHealth = maxHealth;
        CurHealth = maxHealth;
    }


(** Create a Player Entity *)
let CreatePlayer (content:ContentManager) (position) =
    UniqueId <- UniqueId + 1
    let sprite = CreateSprite(Layer.PLAYER, content.Load<Texture2D>("images/fighter.png"))
    {
        Id = UniqueId;
        EntityType = Player(Nothing); 
        Position = position; 
        Size = Vector2(float32 sprite.Width, float32 sprite.Height);
        Destroy = false;

        Bounds = Some(43);
        Expires = None;
        Health = Some(CreateHealth(100));
        Velocity = Some(Vector2(0.f, 0.f));
        Layer = Some(Layer.PLAYER);
        Sprite = Some(sprite);
        BodyType = Dynamic(Vector2(0.f,0.f)); 
    }
     
(** Create a Bullet Entity *)
let CreateBullet (content:ContentManager) (position) =
    UniqueId <- UniqueId + 1
    let sprite = CreateSprite(Layer.BULLET, content.Load<Texture2D>("images/bullet.png"))
    {
        Id = UniqueId;
        EntityType = Bullet; 
        Position = position; 
        Size = Vector2(float32 sprite.Width, float32 sprite.Height);
        Destroy = false;

        Bounds = Some(5);
        Expires = Some(1000.f);
        Health = None;
        Velocity = Some(Vector2(0.f, -0.8f));
        Layer = Some(Layer.PLAYER);
        Sprite = Some(sprite);
        BodyType = Dynamic(Vector2(0.f,0.f)); 
    }

(** Create Enemy *)
let CreateEnemy1 (content:ContentManager) =
    UniqueId <- UniqueId + 1
    let position = Vector2(float32(rnd.Next(ScreenWidth)), float32(ScreenHeight-100))
    let sprite = CreateSprite(Layer.ACTORS_1, content.Load<Texture2D>("images/enemy1.png"))
    {
        Id = UniqueId;
        EntityType = Enemy; 
        Position = position; 
        Size = Vector2(float32 sprite.Width, float32 sprite.Height);
        Destroy = false;

        Bounds = Some(1);
        Expires = None
        Health = Some(CreateHealth(10));
        Velocity = Some(Vector2(0.f, -0.120f));
        Layer = Some(Layer.ACTORS_1);
        Sprite = Some(sprite);
        BodyType = Dynamic(Vector2(0.f,0.f)); 
    }

(** Create Enemy *)
let CreateEnemy2 (content:ContentManager) =
    UniqueId <- UniqueId + 1
    let position = Vector2(float32(rnd.Next(ScreenWidth)), float32(ScreenHeight-200))
    let sprite = CreateSprite(Layer.ACTORS_2, content.Load<Texture2D>("images/enemy2.png"))
    {
        Id = UniqueId;
        EntityType = Enemy; 
        Position = position; 
        Size = Vector2(float32 sprite.Width, float32 sprite.Height);
        Destroy = false;

        Bounds = Some(2);
        Expires = None
        Health = Some(CreateHealth(20));
        Velocity = Some(Vector2(0.f, -0.09f));
        Layer = Some(Layer.ACTORS_2);
        Sprite = Some(sprite);
        BodyType = Dynamic(Vector2(0.f,0.f)); 
    }

(** Create Enemy *)
let CreateEnemy3 (content:ContentManager) =
    UniqueId <- UniqueId + 1
    let position = Vector2(float32(rnd.Next(ScreenWidth)), float32(ScreenHeight-300))
    let sprite = CreateSprite(Layer.ACTORS_3, content.Load<Texture2D>("images/enemy3.png"))
    {
        Id = UniqueId;
        EntityType = Enemy; 
        Position = position; 
        Size = Vector2(float32 sprite.Width, float32 sprite.Height);
        Destroy = false;

        Bounds = Some(3);
        Expires = None
        Health = Some(CreateHealth(40));
        Velocity = Some(Vector2(0.f, -0.06f));
        Layer = Some(Layer.ACTORS_3);
        Sprite = Some(sprite);
        BodyType = Dynamic(Vector2(0.f,0.f)); 
    }


(** IGame Interface *)
type IGame  =
    abstract member addEntity: Entity -> unit
    abstract member delEntity: Entity -> unit

