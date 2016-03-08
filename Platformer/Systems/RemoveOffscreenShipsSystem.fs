[<AutoOpen>]
module RemoveOffscreenShipsSystem
open Microsoft.Xna.Framework

let RemoveOffscreenShipsSystem (game:EcsGame) entity =
    match entity.EntityType, entity.Active with
    | Enemy, true when int entity.Position.Y > ScreenHeight ->
        { 
            entity with 
                Active = false;
        }
    | _ -> entity
