using System.Linq;
using Microsoft.Xna.Framework;
using Protoinject;
using Protogame;

namespace PROJECT_SAFE_NAME
{
    
    public class AsteroidsWorld : IWorld
    {
        private readonly IHierarchy _hierarchy;
        private readonly I2DRenderUtilities _renderUtilities;
        private readonly IAssetManager _assetManager;
        private readonly IAssetReference<FontAsset> _asteroidsFont;

        private bool _isEnding;
        
        public AsteroidsWorld(
            INode worldNode,
            IHierarchy hierarchy,
            I2DRenderUtilities renderUtilities,
            IAssetManager assetManager,
            IEntityFactory entityFactory)
        {
            _hierarchy = hierarchy;
            _renderUtilities = renderUtilities;
            _assetManager = assetManager;
            _asteroidsFont = this._assetManager.Get<FontAsset>("font.Asteroids");

            var playerShipEntity = entityFactory.CreatePlayerShipEntity();
            playerShipEntity.Transform.LocalPosition = new Vector3(100, 100, 0);
            var asteroidEntity = entityFactory.CreateAsteroidEntity(0);
            asteroidEntity.Transform.LocalPosition = new Vector3(300, 300, 0);

            // Don't forget to add your entities to the world!
            hierarchy.MoveNode(worldNode, hierarchy.Lookup(playerShipEntity));
            hierarchy.MoveNode(worldNode, hierarchy.Lookup(asteroidEntity));

            Score = 0;
            Lives = 3;
        }
        
        /// <summary>
        /// The player's score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// The player's lives.
        /// </summary>
        public int Lives { get; set; }

        public void Dispose()
        {
            // Unused.
        }

        public void RenderAbove(IGameContext gameContext, IRenderContext renderContext)
        {
            if (renderContext.IsCurrentRenderPass<I2DBatchedRenderPass>())
            {
                // Render the title text.
                _renderUtilities.RenderText(
                    renderContext,
                    new Vector2(
                        renderContext.GraphicsDevice.PresentationParameters.BackBufferWidth / 2f,
                        10),
                    "Asteroids!",
                    _asteroidsFont,
                    HorizontalAlignment.Center);

                // Render the lives text.
                _renderUtilities.RenderText(
                    renderContext,
                    new Vector2(
                        10,
                        10),
                    "Lives: " + Lives,
                    _asteroidsFont,
                    HorizontalAlignment.Left);

                // Render the score text.
                _renderUtilities.RenderText(
                    renderContext,
                    new Vector2(
                        renderContext.GraphicsDevice.PresentationParameters.BackBufferWidth - 10f,
                        10),
                    "Score: " + Score,
                    _asteroidsFont,
                    HorizontalAlignment.Right);
            }
        }

        public void RenderBelow(IGameContext gameContext, IRenderContext renderContext)
        {
            if (renderContext.IsFirstRenderPass())
            {
                // Render the black background.
                renderContext.GraphicsDevice.Clear(Color.Black);
            }
        }

        public void Update(IGameContext gameContext, IUpdateContext updateContext)
        {
            // If the player has no lives left, restart the world.
            if (Lives <= 0)
            {
                // Restart the world.
                if (!_isEnding)
                {
                    gameContext.SwitchWorld<AsteroidsWorld>();
                    _isEnding = true;
                }
                return;
            }

            // If there are no asteroids left, restart the world.
            if (!gameContext.World.GetEntitiesForWorld(_hierarchy).OfType<AsteroidEntity>().Any())
            {
                if (!_isEnding)
                {
                    gameContext.SwitchWorld<AsteroidsWorld>();
                    _isEnding = true;
                }
                return;
            }
        }
    }
}
