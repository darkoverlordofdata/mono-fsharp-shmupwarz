module PlatformerGame

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open System.Collections.Generic

type Platformer () as this =
    inherit EcsGame()

    let mutable first = true
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
    do this.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(this)
    do 
        graphics.PreferredBackBufferWidth <- ScreenWidth
        graphics.PreferredBackBufferHeight <- ScreenHeight
        graphics.ApplyChanges()

    (** Define Entities *)
    let mutable Entities = lazy(CreateEntityDB(this.Content))
    let bgdImage = lazy(this.Content.Load<Texture2D>("images/BackdropBlackLittleSparkBlack.png"))
    let mutable fpsRect = Rectangle(0, 0, 16, 24)

    let fntImage = lazy(this.Content.Load<Texture2D>("tom-thumb-white.png"))
    let bgdRect = Rectangle(0, 0, ScreenWidth, ScreenHeight)

    (** Draw the sprite for an Entity *)
    let DrawSprite(spriteBatch:SpriteBatch) entity =
        if entity.Sprite.IsSome then 
            let sprite = entity.Sprite.Value
            let scale =
                match entity.Scale with
                | Some(scale) -> scale
                | None -> Vector2(1.f, 1.f)
            let w = int(float32 sprite.Width * scale.X)
            let h = int(float32 sprite.Height * scale.Y)
            let x = int(entity.Position.X - float32(w/2))
            let y = int(entity.Position.Y - float32(h/2))
            spriteBatch.Draw(sprite.Texture, Rectangle(x, y, w, h), Color.White)    

    (** Draw a FPS in top left corner *)
    let DrawFps(spriteBatch:SpriteBatch, fps:float32)  =
        let ms = int fps
        let d0 = ms / 10        // 9x.xx
        let d1 = ms - d0*10     // x9.xx
        let fp = int((fps - float32 ms) * 100.f)
        let d2 = fp / 10        // xx.9x
        let d3 = fp - d2*10     // xx.x9

        fpsRect.Y <- 24
        fpsRect.X <- 16*(16+d0)
        spriteBatch.Draw(fntImage.Value, Vector2(0.f, 0.f), System.Nullable(fpsRect), Color.White)    
        fpsRect.X <- 16*(16+d1)
        spriteBatch.Draw(fntImage.Value, Vector2(16.f, 0.f), System.Nullable(fpsRect), Color.White)    
        fpsRect.X <- 224
        spriteBatch.Draw(fntImage.Value, Vector2(32.f, 0.f), System.Nullable(fpsRect), Color.White)    
        fpsRect.X <- 16*(16+d2)
        spriteBatch.Draw(fntImage.Value, Vector2(48.f, 0.f), System.Nullable(fpsRect), Color.White)    
        fpsRect.X <- 16*(16+d3)
        spriteBatch.Draw(fntImage.Value, Vector2(64.f, 0.f), System.Nullable(fpsRect), Color.White)    



    (** Initialize MonoGame *)
    override this.Initialize() =
        spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        this.IsMouseVisible <- true
        base.Initialize()


    (** Load Resources *)
    override this.LoadContent() =
        Entities.Force() |> ignore

    (** Game Logic Loop *)
    override this.Update (gameTime) =
        let delta = float32 gameTime.ElapsedGameTime.TotalSeconds
        let current = Entities.Value

        Entities <-  // Everything happens here:
            lazy (current
                 |> List.map(InputSystem(Keyboard.GetState(), Mouse.GetState(), delta, this))
                 |> List.map(EntitySystem(this))
                 |> List.map(MovementSystem(delta))
                 |> List.map(ExpiringSystem(delta))
                 |> List.map(ScaleAnimationSystem(delta, this))
                 |> List.map(RemoveOffscreenShipsSystem(this))
                 |> CollisionSystem(this)
                 |> EnemySpawningSystem(delta, this)
                 )
        Entities.Force() |> ignore

    (** Game Graphic Loop *)
    override this.Draw(gameTime) =
        this.GraphicsDevice.Clear Color.Black
        spriteBatch.Begin()
        spriteBatch.Draw(bgdImage.Value, bgdRect, Color.White)   
        DrawFps(spriteBatch, 1.f / float32 gameTime.ElapsedGameTime.TotalSeconds)
        Entities.Value 
        |> List.filter(fun e -> e.Active) 
        |> List.sortBy(fun e -> e.Layer) 
        |> List.iter(DrawSprite(spriteBatch))
        spriteBatch.End()

    (** Deactivate an Entity *)
    override this.RemoveEntity(entity:Entity)=
        this.Deactivate <- entity.Id :: this.Deactivate

    (** Activate a Bullet *)
    override this.AddBullet(position : Vector2) =
        this.Bullets <- CreateTBullet(position) :: this.Bullets

    (** Activate an Enemy *)
    override this.AddEnemy(enemy : Enemies) =
        match enemy with 
        | Enemy1 -> this.Enemies1 <- CreateTEnemy(enemy) :: this.Enemies1
        | Enemy2 -> this.Enemies2 <- CreateTEnemy(enemy) :: this.Enemies2
        | Enemy3 -> this.Enemies3 <- CreateTEnemy(enemy) :: this.Enemies3

    (** Activate an Explosion *)
    override this.AddExplosion(position : Vector2, scale : float32) =
        this.Explosions <- CreateTExplosion(position, scale)  :: this.Explosions


