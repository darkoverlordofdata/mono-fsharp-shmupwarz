open PlatformerGame

[<EntryPoint>]
let main argv = 
    use game = new Platformer()
    game.Run()
    0
