[<AutoOpen>]
module CollisionSystem
open Microsoft.Xna.Framework

(** Return Rect defining the current bounds *)
let BoundingRect(entity) =
    Rectangle((int entity.Position.X),(int entity.Position.Y),(int entity.Size.X),(int entity.Size.Y))

(** Collision Handler for Entities *)
let CollisionSystem (game:EcsGame) entities =

    let FindOptimumCollision a b =
        match a.EntityType, b.EntityType with
        | Enemy, Bullet -> 
            game.RemoveEntity(a)
            game.AddEntity(CreateExplosion(game.Content, b.Position, 0.25f))
            game.RemoveEntity(b)
            match a.Health with
            | Some(h) ->
                let health = h.CurHealth-1
                if health <= 0 then
                    game.RemoveEntity(a)
                    game.AddEntity(CreateExplosion(game.Content, b.Position, 0.5f))
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
            //let a = if (DesiredBounds(entity).Intersects(DesiredBounds(x))) then
            let a = if (BoundingRect(entity).Intersects(BoundingRect(x))) then
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

    FixCollisions entities []


