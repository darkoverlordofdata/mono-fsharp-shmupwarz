module InputHandler
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input
open Components

(** Get Player Input *)
let InputSystem (kbState:KeyboardState, msState:MouseState, game:Game, igame:IGame) entity =
    let rec HandleKeys keys (currentVelocity:Vector2,state) =
        match keys with
        | [] -> 
            Vector2(currentVelocity.X, currentVelocity.Y*0.5f)
            //currentVelocity
        | x :: xs ->
            match x with
            | Keys.Left -> 
                let newSpeed = if (currentVelocity.X - 0.25f) < -10.f then -10.f else currentVelocity.X - 0.25f
                let newV = Vector2(newSpeed, currentVelocity.Y)
                HandleKeys xs (newV,state)
            | Keys.Right ->
                let newSpeed = if (currentVelocity.X + 0.25f) > 10.f then 10.f else currentVelocity.X + 0.25f
                let newV = Vector2(newSpeed, currentVelocity.Y)
                HandleKeys xs (newV,state)
            | Keys.Z -> 
                igame.addEntity((CreateBullet game.Content) (Vector2(entity.Position.X-23.f, entity.Position.Y)))
                igame.addEntity((CreateBullet game.Content) (Vector2(entity.Position.X+23.f, entity.Position.Y)))
                HandleKeys xs (currentVelocity,state)
            | _ -> 
                HandleKeys xs (currentVelocity,state)

    match entity.EntityType with
    | Player(s) -> 
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

        let velocity = HandleKeys(kbState.GetPressedKeys() |> Array.toList) (initialVelocity, s)
        { 
            entity with 
                BodyType = Dynamic(velocity); 
                EntityType = Player(Jumping);
                Position = position; 
        }
    | _ -> entity
