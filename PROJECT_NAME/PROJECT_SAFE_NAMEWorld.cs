namespace PROJECT_SAFE_NAME
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;

    using Protoinject;

    using Protogame;

    public class PROJECT_SAFE_NAMEWorld : IWorld
    {
        private readonly I2DRenderUtilities _renderUtilities;

        private readonly IAssetManager _assetManager;

        private readonly IAssetReference<FontAsset> _defaultFont;
        
        public PROJECT_SAFE_NAMEWorld(
            INode worldNode,
            IHierarchy hierarchy,
            I2DRenderUtilities twoDRenderUtilities,
            IAssetManager assetManager,
            IEntityFactory entityFactory)
        {
            this.Entities = new List<IEntity>();

            _renderUtilities = twoDRenderUtilities;
            _assetManager = assetManager;
            _defaultFont = this._assetManager.Get<FontAsset>("font.Default");

            // You can also save the entity factory in a field and use it, e.g. in the Update
            // loop or anywhere else in your game.
            var entityA = entityFactory.CreateExampleEntity("EntityA");
            entityA.Transform.LocalPosition = new Vector3(100, 50, 0);
            var entityB = entityFactory.CreateExampleEntity("EntityB");
            entityB.Transform.LocalPosition = new Vector3(120, 100, 0);

            // Don't forget to add your entities to the world!
            hierarchy.MoveNode(worldNode, hierarchy.Lookup(entityA));
            hierarchy.MoveNode(worldNode, hierarchy.Lookup(entityB));
        }

        public IList<IEntity> Entities { get; private set; }

        public void Dispose()
        {
        }

        public void RenderAbove(IGameContext gameContext, IRenderContext renderContext)
        {
        }

        public void RenderBelow(IGameContext gameContext, IRenderContext renderContext)
        {
            gameContext.Graphics.GraphicsDevice.Clear(Color.Purple);

            this._renderUtilities.RenderText(
                renderContext,
                new Vector2(10, 10),
                "Hello PROJECT_NAME!",
                this._defaultFont);

            this._renderUtilities.RenderText(
                renderContext,
                new Vector2(10, 30),
                "Running at " + gameContext.FPS + " FPS; " + gameContext.FrameCount + " frames counted so far",
                this._defaultFont);
        }

        public void Update(IGameContext gameContext, IUpdateContext updateContext)
        {
        }
    }
}
