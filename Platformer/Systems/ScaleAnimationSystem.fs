[<AutoOpen>]
module ScaleAnimationSystem
open Microsoft.Xna.Framework

let ScaleAnimationSystem (delta:float32, game:EcsGame) entity =
    match (entity.Scale, entity.ScaleAnimation) with
    | Some(scale), Some(sa) ->        
        let mutable x = scale.X + (sa.Speed * delta)
        let mutable y =  scale.Y + (sa.Speed * delta)
        let mutable active = sa.Active
        if x > sa.Max then
            x <- sa.Max
            y <- sa.Max
            active <- false
        elif x < sa.Min then
            x <- sa.Min
            y <- sa.Min
            active <- false

        {
            entity with
                Scale = Some(Vector2(x, y));
                ScaleAnimation = Some(CreateScaleAnimation(sa.Min, sa.Max, sa.Speed, sa.Repeat, active));
        }

    | _, _ -> 
        entity


