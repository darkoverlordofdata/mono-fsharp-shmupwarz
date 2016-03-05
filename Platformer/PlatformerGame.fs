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
    let mutable Entities = lazy([])
    let mutable newEntities = List.empty<Entity>
    let mutable delEntities = List.empty<Entity>
    let bgdImage = lazy(this.Content.Load<Texture2D>("images/BackdropBlackLittleSparkBlack.png"))
    let mutable fpsRect = Rectangle(0, 0, 16, 24)

    let fntImage = lazy(this.Content.Load<Texture2D>("tom-thumb-white.png"))
    let bgdRect = Rectangle(0, 0, ScreenWidth, ScreenHeight)
    let player = lazy((CreatePlayer this.Content) (Vector2(float32(ScreenWidth/2), float32 (ScreenHeight-80))))

    (** Draw the sprite for an Entity *)
    let DrawSprite(spriteBatch:SpriteBatch) entity =
        if entity.Sprite.IsSome then 
            let sprite = entity.Sprite.Value
            let w = int(float32 sprite.Width * sprite.Scale.X)
            let h = int(float32 sprite.Height * sprite.Scale.Y)
            let x = entity.Position.X - float32(w/2)
            let y = entity.Position.Y - float32(h/2)
            let rect = System.Nullable(Rectangle(0, 0, w, h))
            spriteBatch.Draw(sprite.Texture, Vector2(x, y), rect, Color.White)    

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

    (** Return the difference of two lists *)
    let Difference (left:list< 'a >) (right:list< 'a >) =
        let cache = HashSet< 'a >(right, HashIdentity.Structural)
        left |> List.filter (fun n -> not (cache.Contains n))

    

    //interface IGame with
    override this.AddEntity(entity:Entity)=
        newEntities <- entity :: newEntities

    override this.RemoveEntity(entity:Entity)=
        delEntities <- entity :: delEntities

    (** Initialize MonoGame *)
    override this.Initialize() =
        spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        this.IsMouseVisible <- true
        base.Initialize()
        this.AddEntity(player.Value)


    (** Load Resources *)
    override this.LoadContent() =
        Entities.Force () |> ignore

    (** Game Logic Loop *)
    override this.Update (gameTime) =
        let delta = float32 gameTime.ElapsedGameTime.TotalMilliseconds/1000.f
        let current = (Difference Entities.Value delEntities) @ newEntities
        delEntities <- List.empty<Entity>
        newEntities <- List.empty<Entity>
        Entities <-  // Everything happens here:
            lazy (current
                 |> List.map (InputSystem(Keyboard.GetState(), Mouse.GetState(), delta, this))
                 |> List.map (MovementSystem delta)
                 |> List.map (ExpiringSystem delta)
                 |> List.map (ScaleAnimationSystem delta)
                 |> List.map (RemoveOffscreenShipsSystem this)
                 |> CollisionSystem this
                 |> List.map (DestroySystem this))
                 |> EnemySpawningSystem(gameTime, this)
        Entities.Force() |> ignore

    (** Game Graphic Loop *)
    override this.Draw (gameTime) =

        this.GraphicsDevice.Clear Color.Black
        spriteBatch.Begin()
        spriteBatch.Draw(bgdImage.Value, bgdRect, Color.White)   
        DrawFps(spriteBatch, 1.f / float32 gameTime.ElapsedGameTime.TotalSeconds)
        Entities.Value |> List.sortBy(fun x -> x.Layer) |> List.iter(DrawSprite spriteBatch)
        spriteBatch.End()




