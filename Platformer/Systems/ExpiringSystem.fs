[<AutoOpen>]
module ExpiringSystem
open Microsoft.Xna.Framework
(** 
 * Expiring System 
 *
 *)
let ExpiringSystem (delta:float32) entity =
    match entity.Expires with
    | Some(v) ->
        let exp = v - delta
        { 
            entity with 
                Expires = Some(exp);
                Destroy = if exp > 0.f then false else true;
        }
    | None -> entity

