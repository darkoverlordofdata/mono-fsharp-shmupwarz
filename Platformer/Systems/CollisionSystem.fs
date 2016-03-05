[<AutoOpen>]
module CollisionSystem
open Microsoft.Xna.Framework

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
let CollisionSystem (game:EcsGame) entities =
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
        match a.EntityType, b.EntityType with
        | Enemy, Bullet -> 
            game.RemoveEntity(a)
            game.AddEntity(CreateSmallExplosion(game.Content, b.Position))
            game.RemoveEntity(b)
            match a.Health with
            | Some(h) ->
                let health = h.CurHealth-1
                printfn "health %d" health
                if health <= 0 then
                    game.RemoveEntity(a)
                    game.AddEntity(CreateBigExplosion(game.Content, b.Position))
                    a
                else
                    {
                        a with 
                            Health = Some(CreateHealth(health, h.MaxHealth));
                    }

            | None -> a
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


