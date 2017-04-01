using System;
using Microsoft.Xna.Framework;
using Protogame;
using Protoinject;

namespace PROJECT_SAFE_NAME
{
    public class AsteroidEntity : ComponentizedEntity
    {
        private readonly INode _node;
        private readonly IHierarchy _hierarchy;
        private readonly IEntityFactory _entityFactory;
        private readonly Render2DTextureComponent _render2DTextureComponent;
        private readonly PerPixelCollisionTextureComponent _perPixelCollisionTextureComponent;
        private IAssetReference<TextureAsset>[] _asteroidTextures;

        private int _size;

        private Vector2 _velocityVector;
        private float _rotationSpeed;
        private float _rotation;
        private static Random _random = new Random();

        public AsteroidEntity(
            INode node,
            IHierarchy hierarchy,
            IEntityFactory entityFactory,
            IAssetManager assetManager,
            Render2DTextureComponent render2DTextureComponent,
            WrapEntityComponent wrapEntityComponent,
            PerPixelCollisionTextureComponent perPixelCollisionTextureComponent,
            int size)
        {
            _node = node;
            _hierarchy = hierarchy;
            _entityFactory = entityFactory;
            _render2DTextureComponent = render2DTextureComponent;
            _perPixelCollisionTextureComponent = perPixelCollisionTextureComponent;
            _asteroidTextures = new[]
            {
                assetManager.Get<TextureAsset>("texture.Asteroid0"),
                assetManager.Get<TextureAsset>("texture.Asteroid1"),
                assetManager.Get<TextureAsset>("texture.Asteroid2"),
                assetManager.Get<TextureAsset>("texture.Asteroid3")
            };

            _size = size;

            render2DTextureComponent.Texture = _asteroidTextures[_size];

            RegisterComponent(render2DTextureComponent);
            RegisterComponent(wrapEntityComponent);

            _velocityVector =
                Vector2.Transform(Vector2.UnitX, Quaternion.CreateFromAxisAngle(Vector3.UnitZ,
                    (float) (_random.NextDouble() * MathHelper.TwoPi))) * 3f;
            _rotationSpeed = (float) (((_random.NextDouble() * 2) - 1) * (MathHelper.TwoPi / 360f * 6f));
        }

        public override void Update(IGameContext gameContext, IUpdateContext updateContext)
        {
            if (_render2DTextureComponent.Texture.IsReady)
            {
                _render2DTextureComponent.RotationAnchor = new Vector2(
                                                               _render2DTextureComponent.Texture.Asset.Texture.Width,
                                                               _render2DTextureComponent.Texture.Asset.Texture.Height) /
                                                           2;
                _perPixelCollisionTextureComponent.RotationAnchor = _render2DTextureComponent.RotationAnchor;
            }

            _perPixelCollisionTextureComponent.Texture = _render2DTextureComponent.Texture;

            _rotation += _rotationSpeed;
            if (_rotation < 0)
            {
                _rotation += MathHelper.TwoPi;
            }
            if (_rotation > 360)
            {
                _rotation -= MathHelper.TwoPi;
            }

            Transform.LocalPosition += new Vector3(_velocityVector, 0);
            Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, _rotation);

            base.Update(gameContext, updateContext);
        }

        public override void PerPixelCollision(IGameContext gameContext, IServerContext serverContext, IUpdateContext updateContext, object obj1,
            object obj2)
        {
            base.PerPixelCollision(gameContext, serverContext, updateContext, obj1, obj2);

            if (obj1 == this && obj2 is BulletEntity)
            {
                // Remove the bullet.
                ((BulletEntity)obj2).DeleteBullet();

                // Remove ourselves.
                DeleteAsteroid();

                var nextSize = _size += 1;
                if (nextSize >= _asteroidTextures.Length)
                {
                    // We don't spawn any more asteroids.
                    return;
                }

                // Spawn smaller asteroids.
                var asteroid1 = _entityFactory.CreateAsteroidEntity(nextSize);
                var asteroid2 = _entityFactory.CreateAsteroidEntity(nextSize);
                var asteroid3 = _entityFactory.CreateAsteroidEntity(nextSize);
                var asteroid4 = _entityFactory.CreateAsteroidEntity(nextSize);

                asteroid1.Transform.LocalPosition = Transform.LocalPosition;
                asteroid2.Transform.LocalPosition = Transform.LocalPosition;
                asteroid3.Transform.LocalPosition = Transform.LocalPosition;
                asteroid4.Transform.LocalPosition = Transform.LocalPosition;

                // Move the smaller asteroids underneath the world.
                var worldNode = _hierarchy.Lookup(gameContext.World);
                _hierarchy.MoveNode(worldNode, _hierarchy.Lookup(asteroid1));
                _hierarchy.MoveNode(worldNode, _hierarchy.Lookup(asteroid2));
                _hierarchy.MoveNode(worldNode, _hierarchy.Lookup(asteroid3));
                _hierarchy.MoveNode(worldNode, _hierarchy.Lookup(asteroid4));

                // Increment the score.
                var world = gameContext.World as AsteroidsWorld;
                if (world != null)
                {
                    world.Score += 1;
                }
            }

            if (obj1 == this && obj2 is PlayerShipEntity)
            {
                // Only hurt the player if they're not invincible.
                if (!((PlayerShipEntity) obj2).IsInvincible)
                {
                    // Remove the player.
                    ((PlayerShipEntity) obj2).DeletePlayer();

                    // Recreate the player.
                    var player = _entityFactory.CreatePlayerShipEntity();
                    player.Transform.LocalPosition = new Vector3(400, 300, 0);
                    var worldNode = _hierarchy.Lookup(gameContext.World);
                    _hierarchy.MoveNode(worldNode, _hierarchy.Lookup(player));

                    // Decrement lives.
                    var world = gameContext.World as AsteroidsWorld;
                    if (world != null)
                    {
                        world.Lives -= 1;
                    }
                }
            }
        }

        public void DeleteAsteroid()
        {
            _hierarchy.RemoveNode(_node);

            // At some point in the future, these kinds of stateful components (physics,
            // collision, etc.) will automatically remove the associated data when
            // the node for the entity is removed from the hierarchy.  However, for now,
            // you need to explicitly disable these kinds of components when deleting
            // the entity.
            _perPixelCollisionTextureComponent.Enabled = false;
        }
    }
}
