[<AutoOpen>]
module ExpiringSystem
open Microsoft.Xna.Framework
(** 
 * Expiring System 
 *
 * Destroy entities when their time is up
 *)
let ExpiringSystem (delta:float32) entity =
    match entity.Expires, entity.Active with
    | Some(v), true ->
        let exp = v - delta
        let active = if exp > 0.f then true else false
        { 
            entity with 
                Expires = Some(exp);
                Active = active;
        }
    | _ -> entity

