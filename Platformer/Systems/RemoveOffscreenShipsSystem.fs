[<AutoOpen>]
module RemoveOffscreenShipsSystem
open Microsoft.Xna.Framework

let RemoveOffscreenShipsSystem (game:EcsGame) entity =
    match entity.EntityType with
    | Enemy when int entity.Position.Y > ScreenHeight ->
        game.RemoveEntity(entity)
        entity
    | _ ->
        entity
