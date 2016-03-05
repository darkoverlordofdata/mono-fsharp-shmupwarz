[<AutoOpen>]
module ScaleAnimationSystem
open Microsoft.Xna.Framework

let ScaleAnimationSystem (delta:float32) entity =
    match (entity.ScaleAnimation, entity.Scale) with
    | Some(sa), Some(scale)  ->        
        let mutable x = scale.X + sa.Speed * delta
        let mutable y =  scale.Y + sa.Speed * delta
        let mutable active = sa.Active
        if x > sa.Max then
            x <- sa.Max
            y <- scale.X
            active <- false
        elif x < sa.Min then
            x <- sa.Min
            y <- scale.X
            active <- false

        {
            entity with
                Scale = Some(Vector2(x, y));
                ScaleAnimation = Some(CreateScaleAnimation(sa.Min, sa.Max, sa.Speed, sa.Repeat, sa.Active));
        }

    | _, _ -> 
        entity


