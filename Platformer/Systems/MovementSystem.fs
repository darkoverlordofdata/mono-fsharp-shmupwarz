[<AutoOpen>]
module MovementSystem
open Microsoft.Xna.Framework

(** Movement System *)
let MovementSystem (delta:float32) entity =

    match entity.Velocity, entity.Active with

    | Some(velocity), true ->
        let x = entity.Position.X + velocity.X * delta
        let y = entity.Position.Y + velocity.Y * delta
        { entity with Position = Vector2(float32 x, float32 y)}

    | _ -> entity


