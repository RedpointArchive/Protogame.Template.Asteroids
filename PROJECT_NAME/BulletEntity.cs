using Microsoft.Xna.Framework;
using Protogame;
using Protoinject;

namespace PROJECT_SAFE_NAME
{
    public class BulletEntity : ComponentizedEntity
    {
        private readonly INode _node;
        private readonly IHierarchy _hierarchy;
        private readonly PerPixelCollisionTextureComponent _perPixelCollisionTextureComponent;

        public Vector2 Velocity { get; set; }

        public int TicksLeft { get; set; }

        public BulletEntity(
            INode node,
            IHierarchy hierarchy,
            IAssetManager assetManager,
            Render2DTextureComponent render2DTextureComponent,
            WrapEntityComponent wrapEntityComponent,
            PerPixelCollisionTextureComponent perPixelCollisionTextureComponent)
        {
            _node = node;
            _hierarchy = hierarchy;
            _perPixelCollisionTextureComponent = perPixelCollisionTextureComponent;

            render2DTextureComponent.Texture = assetManager.Get<TextureAsset>("texture.Bullet");
            perPixelCollisionTextureComponent.Texture = render2DTextureComponent.Texture;
            perPixelCollisionTextureComponent.RotationAnchor = render2DTextureComponent.RotationAnchor;

            RegisterComponent(render2DTextureComponent);
            RegisterComponent(perPixelCollisionTextureComponent);
            RegisterComponent(wrapEntityComponent);
        }

        public override void Update(IGameContext gameContext, IUpdateContext updateContext)
        {
            base.Update(gameContext, updateContext);

            if (TicksLeft <= 0)
            {
                DeleteBullet();
                return;
            }

            TicksLeft -= 1;

            Transform.LocalPosition += new Vector3(Velocity, 0);
        }

        public void DeleteBullet()
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
