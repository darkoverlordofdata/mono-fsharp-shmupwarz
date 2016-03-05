module Systems
open Microsoft.Xna.Framework
open Components

(** Movement System *)
let MovementSystem (gameTime:GameTime) entity =
    match entity.Velocity with
    | Some(v) ->
        let ms = float32 gameTime.ElapsedGameTime.TotalMilliseconds
        let x = entity.Position.X + v.X * ms
        let y = entity.Position.Y + v.Y * ms
        { 
            entity with 
                Position = Vector2(float32 x, float32 y);
        }
    | None -> entity


(** Expiring System *)
let ExpiringSystem (gameTime:GameTime) entity =
    match entity.Expires with
    | Some(v) ->
        let exp = v - float32(gameTime.ElapsedGameTime.TotalMilliseconds)
        { 
            entity with 
                Expires = Some(exp);
                Destroy = if exp > 0.f then false else true;
        }
    | None -> entity


(** Destroy System *)
let DestroySystem (igame:IGame) entity = 
    if entity.Destroy then
        igame.delEntity(entity)
    entity


