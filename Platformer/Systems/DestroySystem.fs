[<AutoOpen>]
module Systems
open Microsoft.Xna.Framework
(** Destroy System *)
let DestroySystem (game:EcsGame) entity = 
    if entity.Destroy then
        game.RemoveEntity(entity)
    entity

