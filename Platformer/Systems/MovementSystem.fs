[<AutoOpen>]
module MovementSystem
open Microsoft.Xna.Framework

(** Movement System *)
let MovementSystem (delta:float32) entity =
    match entity.Velocity, entity.Active with
    | Some(v), true ->
        let x = entity.Position.X + v.X * delta
        let y = entity.Position.Y + v.Y * delta
        { 
            entity with 
                Position = Vector2(float32 x, float32 y);
        }
    | _ -> entity


