[<AutoOpen>]
module Systems
open Microsoft.Xna.Framework
(** Destroy System *)
let DestroySystem (game:EcsGame) entity = 
    match entity.Destroy with
    | true ->
        { 
            entity with 
                Active = false;
        }
    | false -> entity

