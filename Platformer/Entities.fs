[<AutoOpen>]
module EntitiesModule
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content
(**
 * Entity Factory
 *
 *)
let CreateTEnemy(enemy : Enemies) : TEnemy =
    {
        Enemy = enemy;
    }

let CreateTExplosion(position: Vector2, scale : float32) : TExplosion =
    {
        Position = position;
        Scale = scale;
    }

let CreateTBullet(position : Vector2) : TBullet =
    {
        Position = position;
    }
    

let CreateEntities(content) = 
    [
        CreatePlayer(content, Vector2(float32(ScreenWidth/2), float32 (ScreenHeight-80)));
        CreateExplosion(content, Vector2(0.f, 0.f), 1.f);
        CreateExplosion(content, Vector2(0.f, 0.f), 1.f);
        CreateExplosion(content, Vector2(0.f, 0.f), 1.f);
        CreateExplosion(content, Vector2(0.f, 0.f), 1.f);
        CreateExplosion(content, Vector2(0.f, 0.f), 1.f);
        CreateExplosion(content, Vector2(0.f, 0.f), 1.f);
        CreateExplosion(content, Vector2(0.f, 0.f), 1.f);
        CreateExplosion(content, Vector2(0.f, 0.f), 1.f);
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateBullet(content, Vector2(0.f, 0.f));
        CreateEnemy1(content);
        CreateEnemy1(content);
        CreateEnemy1(content);
        CreateEnemy1(content);
        CreateEnemy1(content);
        CreateEnemy1(content);
        CreateEnemy1(content);
        CreateEnemy1(content);
        CreateEnemy1(content);
        CreateEnemy2(content);
        CreateEnemy2(content);
        CreateEnemy2(content);
        CreateEnemy2(content);
        CreateEnemy2(content);
        CreateEnemy2(content);
        CreateEnemy3(content);
        CreateEnemy3(content);
        CreateEnemy3(content);
        CreateEnemy3(content);

    ]


