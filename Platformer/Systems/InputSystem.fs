[<AutoOpen>]
module InputSystem
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

let mutable timeToFire = 0.0f
(** Get Player Input *)
let InputSystem (kbState:KeyboardState, msState:MouseState, delta:float32, game:EcsGame) entity =

    let rec HandleKeys keys =
        match keys with
        | [] -> 
            0
        | x :: xs ->
            match x with
            | Keys.Z -> 
                timeToFire <- timeToFire - delta
                if timeToFire <= 0.0f then
                    game.AddBullet((Vector2(entity.Position.X-27.f, entity.Position.Y)))
                    game.AddBullet((Vector2(entity.Position.X+27.f, entity.Position.Y)))
                    timeToFire <- 0.1f
                HandleKeys xs 
            | _ -> 
                HandleKeys xs 

    match entity.EntityType with
    | Player -> 
        HandleKeys(kbState.GetPressedKeys() |> Array.toList) |> ignore
        let position = 
            let pos = game.Window.Position
            let newPosition = Vector2(float32 (msState.X-pos.X), float32 (msState.Y-pos.Y))
            match msState.LeftButton with
            | ButtonState.Pressed ->
                timeToFire <- timeToFire - delta
                if timeToFire <= 0.0f then
                    game.AddBullet(Vector2(float32 (msState.X-pos.X-27), float32 (msState.Y-pos.Y)))
                    game.AddBullet(Vector2(float32 (msState.X-pos.X+27), float32 (msState.Y-pos.Y)))
                    timeToFire <- 0.1f
                newPosition

            | ButtonState.Released ->
                newPosition

            | _ ->
                newPosition

        (* Set Player Position *)
        { entity with Position = position;  }

    | _ -> entity
