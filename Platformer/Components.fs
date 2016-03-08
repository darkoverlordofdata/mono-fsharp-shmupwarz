[<AutoOpen>]
module ComponentsModule
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content
(** 
 * Metadata to define the entity database
 *)

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
    | EXPLOSION
    | PARTICLE
    | HUD


(** Sound Effect Component *)
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

(** Enemy Type Component *)
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

(** ScaleAnimation Component *)
type ScaleAnimation =
    {
        Min : float32;
        Max : float32;
        Speed : float32;
        Repeat : bool;
        Active : bool;
    }


(** Request an enemy *)
type TEnemy =
    {
        Enemy : Enemies;
    }
(** Request an explosion *)
type TExplosion =
    {
        Position : Vector2;
        Scale : float32;
    }
(** Request a bullet *)
type TBullet =
    {
        Position : Vector2;
    }


(** Entity is a record of components *)
type Entity =
    {
        Id : int; (* Unique sequential id *)
        Name : string; (* Display name *)
        Active : bool; (* In use *)

        (* All entities are required to have: *)
        EntityType  : EntityType;
        Layer       : Layer;
        Size        : Vector2;
        Position    : Vector2;

        (* Optional components - used for match by systems *)
        Sprite          : Sprite option;
        Bounds          : int option;
        Expires         : float32 option;
        Health          : Health option;
        Velocity        : Vector2 option;
        Scale           : Vector2 option;
        ScaleAnimation  : ScaleAnimation option;
    }

(**
 * The abstract EscGame provides interface and lists to
 * use for adding and removing entities
 *)
[<AbstractClass>]
type EcsGame()=
    inherit Game()

    member val Bullets = List.empty<TBullet> with get,set
    member val Deactivate = List.empty<int> with get,set
    member val Enemies1 = List.empty<TEnemy> with get,set
    member val Enemies2 = List.empty<TEnemy> with get,set
    member val Enemies3 = List.empty<TEnemy> with get,set
    member val Explosions = List.empty<TExplosion> with get,set

    abstract member AddBullet : Vector2 -> unit
    abstract member AddEnemy : Enemies -> unit 
    abstract member AddExplosion : Vector2 * float32 -> unit
    abstract member RemoveEntity: Entity -> unit


