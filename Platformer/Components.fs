[<AutoOpen>]
module ComponentsModule
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content

(** Layer - All entities need a display layer *)
type Layer =
    | DEFAULT
    | BACKGROUND
    | TEXT
    | LIVES
    | ENEMY1
    | ENEMY2
    | ENEMY3
    | PLAYER
    | BULLET
    | PARTICLE
    | HUD


type Effect =
    | PEW 
    | ASPLODE
    | SMALLASPLODE 


(** EntityType Component *)
type EntityType =
    | Background
    | Bullet
    | Enemy
    | Explosion
    | Particle
    | Player

type Enemies =
    | Enemy1
    | Enemy2
    | Enemy3


(** Sprite Component *)
type Sprite =
    {
        Width: int;
        Height: int;
        Texture : Texture2D;
    }

(** Health Component *)
type Health =
    {
        CurHealth: int;
        MaxHealth: int;
    }

type ScaleAnimation =
    {
        Min : float32;
        Max : float32;
        Speed : float32;
        Repeat : bool;
        Active : bool;
    }

(** Entity is a record of components *)
type Entity =
    {
        Id : int; (* Unique sequential id *)
        Name : string; (* Display name *)

        (* All entities are required to have: *)
        EntityType  : EntityType;
        Size        : Vector2;
        Position    : Vector2;
        Layer       : Layer;
        Destroy     : bool;

        (* Optional components - used for match by systems *)
        Sprite          : Sprite option;
        Bounds          : int option;
        Expires         : float32 option;
        Health          : Health option;
        Velocity        : Vector2 option;
        Scale           : Vector2 option;
        ScaleAnimation  : ScaleAnimation option;
    }

(** IGame Interface *)
[<AbstractClass>]
type EcsGame()=
    inherit Game()
    abstract member AddEntity: Entity -> unit
    abstract member RemoveEntity: Entity -> unit
    abstract member CreatePlayer : unit -> Entity
    abstract member AddBullet : Vector2 -> unit
    abstract member AddEnemy : Enemies -> unit 
    abstract member AddExplosion : Vector2 * float32 -> unit
