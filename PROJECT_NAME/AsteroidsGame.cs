using Microsoft.Xna.Framework;
using Protoinject;
using Protogame;

namespace PROJECT_SAFE_NAME
{
    public class AsteroidsGame : CoreGame<AsteroidsWorld>
    {
        public AsteroidsGame(IKernel kernel) : base(kernel)
        {
        }

        protected override void ConfigureRenderPipeline(IRenderPipeline pipeline, IKernel kernel)
        {
            var graphicsFactory = kernel.Get<IGraphicsFactory>();

            // This render pass is for 2D sprite batched rendering.  In this pass, calls
            // to I2DRenderUtilities will work.
            pipeline.AddFixedRenderPass(graphicsFactory.Create2DBatchedRenderPass());

#if DEBUG
            // IDebugRenderPass inherits the view and projection of the previous render pass.
            // Currently for 2D games, the 2D batched render pass doesn't set up the global
            // view and projection to an orthographic, which means the debug render pass
            // will assume a 3D system by default.  By including a 2D direct render pass
            // here (which does set up the view and projection for direct rendering),
            // the debug render pass will work correctly in a 2D game.  If you're disabling
            // the debug render pass, you can also safely remove the 2D direct render pass
            // if you're not using it.
            pipeline.AddFixedRenderPass(graphicsFactory.Create2DDirectRenderPass());

            // Add a debug render pass which shows the bounding boxes and per-pixel collision
            // states of all entities on screen.
            var debugRenderPass = graphicsFactory.CreateDebugRenderPass();
            pipeline.AddFixedRenderPass(debugRenderPass);

            // NOTE: Uncomment this line if you want to see the per-pixel collision bounding boxes.
            //debugRenderPass.EnabledLayers.Add(new PerPixelCollisionDebugLayer());
#endif
        }

        protected override void PrepareGraphicsDeviceManager(GraphicsDeviceManager graphicsDeviceManager)
        {
            // Set the window / game area size to 800x600.
            graphicsDeviceManager.PreferredBackBufferWidth = 800;
            graphicsDeviceManager.PreferredBackBufferHeight = 600;
        }
    }
}
