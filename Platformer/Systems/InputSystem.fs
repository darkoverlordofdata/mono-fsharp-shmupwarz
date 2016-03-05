[<AutoOpen>]
module InputSystem
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input

let mutable timeToFire = 0.0f
(** Get Player Input *)
let InputSystem (kbState:KeyboardState, msState:MouseState, delta:float32, game:EcsGame) entity =

    let rec HandleKeys keys (currentVelocity:Vector2) =
        match keys with
        | [] -> 
            Vector2(currentVelocity.X, currentVelocity.Y*0.5f)
        | x :: xs ->
            match x with
            | Keys.Z -> 
                timeToFire <- timeToFire - delta
                if timeToFire <= 0.0f then
                    game.AddEntity((CreateBullet game.Content) (Vector2(entity.Position.X-27.f, entity.Position.Y)))
                    game.AddEntity((CreateBullet game.Content) (Vector2(entity.Position.X+27.f, entity.Position.Y)))
                    timeToFire <- 0.1f


                HandleKeys xs (currentVelocity)
            | _ -> 
                HandleKeys xs (currentVelocity)


    match entity.EntityType with
    | Player -> 
        let initialVelocity = 
            match entity.BodyType with
            | Dynamic(v) -> v
            | _ -> Vector2()
        let position = 
            match msState.LeftButton with
            | ButtonState.Pressed ->
                let pos = game.Window.Position
                Vector2(float32 (msState.X-pos.X), float32 (msState.Y-pos.Y))

            | ButtonState.Released ->
                entity.Position          
            | _ ->
                entity.Position

        let velocity = HandleKeys(kbState.GetPressedKeys() |> Array.toList) (initialVelocity)
        { 
            entity with 
                BodyType = Dynamic(velocity); 
                Position = position; 
        }

    | _ -> entity
