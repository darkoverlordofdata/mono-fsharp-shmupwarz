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
    | Player
    | Status
    | Explosion

(** Sprite Component *)
type Sprite =
    {
        Width: int;
        Height: int;
        Texture : Texture2D;
        Scale : Vector2;
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
        Id : int;

        EntityType : EntityType;
        Size : Vector2;
        Position : Vector2;
        BodyType : BodyType;
        Destroy : bool;
        Layer : Layer;
        // Optional Components
        Sprite : Sprite option;
        Bounds : int option;
        Expires : float32 option;
        Health : Health option;
        Velocity : Vector2 option;
        Scale : Vector2 option;
        ScaleAnimation : ScaleAnimation option;
    }

(** IGame Interface *)
[<AbstractClass>]
type EcsGame()=
    inherit Game()
    abstract member AddEntity: Entity -> unit
    abstract member RemoveEntity: Entity -> unit



