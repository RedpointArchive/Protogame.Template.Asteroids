using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Protogame;
using Protoinject;

namespace PROJECT_SAFE_NAME
{
    public class PlayerShipEntity : ComponentizedEntity
    {
        private readonly INode _node;
        private readonly IHierarchy _hierarchy;
        private readonly IEntityFactory _entityFactory;
        private readonly Render2DTextureComponent _render2DTextureComponent;
        private readonly PerPixelCollisionTextureComponent _perPixelCollisionTextureComponent;
        private float _horSpeed;
        private float _vertSpeed;
        private float _rotation;
        private float _invincibleCountdown;

        public PlayerShipEntity(
            INode node,
            IHierarchy hierarchy,
            IEntityFactory entityFactory,
            IAssetManager assetManager,
            Render2DTextureComponent render2DTextureComponent,
            WrapEntityComponent wrapEntityComponent,
            PerPixelCollisionTextureComponent perPixelCollisionTextureComponent)
        {
            _node = node;
            _hierarchy = hierarchy;
            _entityFactory = entityFactory;
            _render2DTextureComponent = render2DTextureComponent;
            _perPixelCollisionTextureComponent = perPixelCollisionTextureComponent;

            render2DTextureComponent.Texture = assetManager.Get<TextureAsset>("texture.Ship");
            render2DTextureComponent.RotationAnchor = new Vector2(16, 16);
            perPixelCollisionTextureComponent.Texture = render2DTextureComponent.Texture;
            perPixelCollisionTextureComponent.RotationAnchor = render2DTextureComponent.RotationAnchor;

            RegisterComponent(render2DTextureComponent);
            RegisterComponent(perPixelCollisionTextureComponent);
            RegisterComponent(wrapEntityComponent);

            _horSpeed = 0;
            _vertSpeed = 0;

            _invincibleCountdown = 120;
        }

        public override void Update(IGameContext gameContext, IUpdateContext updateContext)
        {
            Transform.LocalPosition += new Vector3(_horSpeed, _vertSpeed, 0);
            Transform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, _rotation);

            if (_invincibleCountdown > 0)
            {
                // Toggle visibility each frame to make the player flash.
                _render2DTextureComponent.Enabled = !_render2DTextureComponent.Enabled;

                // Decrement invincibility counter.
                _invincibleCountdown--;
            }
            else
            {
                _render2DTextureComponent.Enabled = true;
            }

            base.Update(gameContext, updateContext);
        }

        public bool IsInvincible => _invincibleCountdown > 0;

        public override bool Handle(IGameContext context, IEventEngine<IGameContext> eventEngine, Event @event)
        {
            var keyHeldEvent = @event as KeyHeldEvent;
            var keyPressEvent = @event as KeyPressEvent;

            if (keyHeldEvent != null)
            {
                if (keyHeldEvent.Key == Keys.Up)
                {
                    var up = new Vector3(0, -1, 0);
                    var dir = Vector3.Transform(up, Transform.LocalRotation);

                    _horSpeed += dir.X * 0.1f;
                    _vertSpeed += dir.Y * 0.1f;

                    var vec = new Vector2(_horSpeed, _vertSpeed);
                    if (vec.Length() > 5)
                    {
                        vec.Normalize();
                        vec *= 5;
                    }

                    _horSpeed = vec.X;
                    _vertSpeed = vec.Y;
                }
                else if (keyHeldEvent.Key == Keys.Left)
                {
                    _rotation -= MathHelper.TwoPi / 360f * 3f;
                    if (_rotation < 0)
                    {
                        _rotation += MathHelper.TwoPi;
                    }
                }
                else if (keyHeldEvent.Key == Keys.Right)
                {
                    _rotation += MathHelper.TwoPi / 360f * 3f;
                    if (_rotation > MathHelper.TwoPi)
                    {
                        _rotation -= MathHelper.TwoPi;
                    }
                }
            }

            if (keyPressEvent != null)
            {
                if (keyPressEvent.Key == Keys.Space)
                {
                    var up = new Vector3(0, -1, 0);
                    var dir = Vector3.Transform(up, Transform.LocalRotation);

                    var vel = new Vector2(dir.X, dir.Y) * 8f;

                    var bullet = _entityFactory.CreateBulletEntity();
                    bullet.Transform.LocalPosition = Transform.LocalPosition + Vector3.Normalize(new Vector3(vel.X, vel.Y, 0)) * 32f;
                    bullet.Velocity = vel;
                    bullet.TicksLeft = 120;

                    // Move the bullet underneath the world.
                    _hierarchy.MoveNode(
                        _hierarchy.Lookup(context.World),
                        _hierarchy.Lookup(bullet));
                }
            }

            return base.Handle(context, eventEngine, @event);
        }

        public void DeletePlayer()
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
