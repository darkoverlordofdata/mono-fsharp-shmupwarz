[<AutoOpen>]
module EnemySpawningSystem

open Microsoft.Xna.Framework

type Enemies =
    | Enemy1
    | Enemy2
    | Enemy3

type Timers =
    | Timer1 = 2
    | Timer2 = 6
    | Timer3 = 12


let mutable enemyT1 = float32(Timers.Timer1)
let mutable enemyT2 = float32(Timers.Timer2)
let mutable enemyT3 = float32(Timers.Timer3)

let EnemySpawningSystem (gameTime:GameTime, game:EcsGame) entities =

    //let igame = game:>IGame
    let spawnEnemy (t:float32, enemy) =
        let delta = t - float32 gameTime.ElapsedGameTime.TotalSeconds

        if delta < 0.0f then
            match enemy with
            | Enemy1 -> 
                game.AddEntity(CreateEnemy1(game.Content))
                float32(Timers.Timer1)
            | Enemy2 ->
                game.AddEntity(CreateEnemy2(game.Content))
                float32(Timers.Timer2)
            | Enemy3 ->
                game.AddEntity(CreateEnemy3(game.Content))
                float32(Timers.Timer3)

        else delta

    enemyT1 <- spawnEnemy(enemyT1, Enemy1)
    enemyT2 <- spawnEnemy(enemyT2, Enemy2)
    enemyT3 <- spawnEnemy(enemyT3, Enemy3)


    entities

