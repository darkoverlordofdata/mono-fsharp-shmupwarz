[<AutoOpen>]
module EntitySystem
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input
(**
 * Activate / Deactiveate Entities as needed 
 *)

let EntitySystem (game:EcsGame) entity =


    match entity.Active with
    | true -> 
        let mutable removed = false
        let rec remove_if l predicate =
            match l with
            | [] -> []
            | x::rest -> 
                if predicate(x) then 
                    removed <- true
                    (remove_if rest predicate) 
                else 
                    x::(remove_if rest predicate)

        let len = game.Deactivate.Length
        game.Deactivate <- (remove_if game.Deactivate (fun id -> id = entity.Id))
        {
            entity with 
                Active = not removed;
        }

    | false -> 
        match entity.Layer with
        | BULLET ->
            match game.Bullets with
            | [] -> entity
            | bullet :: rest ->
                game.Bullets <- rest
                {            
                    entity with
                        Active = true;
                        Expires = Some(0.5f);                        
                        Position = Vector2(bullet.Position.X, bullet.Position.Y);
                }
        | ENEMY1 ->
            match game.Enemies1 with
            | [] -> entity
            | enemy :: rest ->
               game.Enemies1 <- rest
               {
                    entity with 
                        Active = true;
                        Position = Vector2(float32(rnd.Next(ScreenWidth)), 100.f);
                        Health = Some(CreateHealth(10, 10));
                }
        | ENEMY2 ->
            match game.Enemies2 with
            | [] -> entity
            | enemy :: rest ->
                game.Enemies2 <- rest
                {
                    entity with 
                        Active = true;
                        Position = Vector2(float32(rnd.Next(ScreenWidth)), 200.f);
                        Health = Some(CreateHealth(20, 20));                
                }
        | ENEMY3 ->
            match game.Enemies3 with
            | [] -> entity
            | enemy :: rest ->
                game.Enemies3 <- rest
                {
                    entity with 
                        Active = true;
                        Position = Vector2(float32(rnd.Next(ScreenWidth)), 300.f);
                        Health = Some(CreateHealth(60, 60));                
                }
        | EXPLOSION ->
            match game.Explosions with
            | [] -> entity
            | exp :: rest ->
                game.Explosions <- rest
                {
                    entity with 
                        Active = true;
                        Expires = Some(0.2f);                        
                        Scale = Some(Vector2(exp.Scale, exp.Scale));
                        Position = Vector2(exp.Position.X, exp.Position.Y);
                }
        | _ -> entity


