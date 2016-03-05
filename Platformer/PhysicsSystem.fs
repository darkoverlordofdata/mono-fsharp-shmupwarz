module PhysicsSystem
open Microsoft.Xna.Framework
open Components

(** Return Rect defining the current bounds *)
let CurrentBounds(this) =
    Rectangle((int this.Position.X),(int this.Position.Y),(int this.Size.X),(int this.Size.Y))

(** Return Rect defining the desired bounds *)
let DesiredBounds(this) =
    let desiredPos = 
        match this.BodyType with
        | Dynamic(s) -> this.Position + s
        | _-> this.Position
    Rectangle((int desiredPos.X), (int desiredPos.Y), (int this.Size.X), (int this.Size.Y))

(** Is it Static or Moving? *)
let IsEntityStatic entity =
    match entity.BodyType with
    | Static -> true
    | _ -> false

(** Seperate out the static entities *)
let SeperateEntities entities =
    entities
    |> List.partition IsEntityStatic

(** Collision Handler for Entities *)
let CollisionSystem entities =
    let staticEntities, dynamicEntities = SeperateEntities entities
    
    let FindNewVelocity rect1 rect2 velocity =
        let inter = Rectangle.Intersect(rect1,rect2)
        let mutable (newVel:Vector2) = velocity
        if inter.Height > inter.Width then
            newVel.X <- 0.f
        if inter.Width > inter.Height then
            newVel.Y <- 0.f
        newVel

    let FindOptimumCollision a b =
        match a.EntityType,b.EntityType with
        | Player(h), Mine -> 
            match a.BodyType, b.BodyType with
            | Dynamic (s), Static -> 
                let r1 = DesiredBounds(a)
                let r2 = CurrentBounds(b)
                { 
                    a with 
                        BodyType = Dynamic(FindNewVelocity r1 r2 s); 
                        EntityType = Player(Nothing) 
                }
            | _ -> a
        | _ -> a

    let rec FigureCollisions (entity:Entity) (sortedEntities:Entity list) =
        match sortedEntities with
        | [] -> entity
        | x :: xs -> 
            let a = if (DesiredBounds(entity).Intersects(DesiredBounds(x))) then
                        FindOptimumCollision entity x
                    else
                        entity
            FigureCollisions a xs

    let rec FixCollisions (toFix:Entity list) (alreadyFixed:Entity list) =
        match toFix with
        | [] -> alreadyFixed
        | x :: xs -> 
            let a = FigureCollisions x alreadyFixed
            FixCollisions xs (a::alreadyFixed)

    FixCollisions dynamicEntities staticEntities


(** Gravity *)
let GravitySystemExecute (gameTime:GameTime) entity =
    let ms = gameTime.ElapsedGameTime.TotalMilliseconds
    let g = ms * 0.01
    match entity.BodyType with
    | Dynamic(s) -> 
        let d = Vector2(s.X, s.Y + (float32 g))
        { 
            entity with 
                BodyType = Dynamic(d); 
        }
    | _ -> entity

(** Friction *)
let FrictionSystemExecute entity = 
    match entity.BodyType with
    | Dynamic (v) -> 
        let newV = Vector2(v.X*0.95f, v.Y)
        { 
            entity with 
                BodyType = Dynamic(newV) 
        }
    | _ -> entity

(** Movement System *)
let VelocitySystemExecute entity =
    match entity.BodyType with
    | Dynamic (s) -> 
        { 
            entity with 
                Position = entity.Position + s 
        }
    | _ -> entity


