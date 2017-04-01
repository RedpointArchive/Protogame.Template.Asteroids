using Microsoft.Xna.Framework;
using Protogame;
using Protoinject;

// This instructs Protogame that the AsteroidGameConfiguration class should
// be used for configuration.
[assembly: Configuration(typeof(PROJECT_SAFE_NAME.AsteroidGameConfiguration))]

namespace PROJECT_SAFE_NAME
{
    /// <summary>
    /// The main game configuration, which configures the dependency injection kernel
    /// (and loads the modules for it) and constructs the game instance.
    /// </summary>
    public class AsteroidGameConfiguration : IGameConfiguration
    {
        public void ConfigureKernel(IKernel kernel)
        {
            // The core module is a module needed by every game.
            kernel.Load<ProtogameCoreModule>();

            // The asset module allows you to use the Protogame asset system
            // including IAssetManager.  Most other modules depend on this, but if for
            // example, you just wanted a mostly XNA-like game with Protogame's entity
            // and world system, you could omit this, and load content using XNA/MonoGame's
            // LoadContent method.
            kernel.Load<ProtogameAssetModule>();

            // The events module listens for input from hardware (such as keyboard, mouse
            // and gamepads) and propagates them through the event system.  Without this
            // module, you won't be able to respond to input as events.  Instead you'd need
            // to use the XNA/MonoGame APIs for keyboard, mouse and gamepads directly.
            kernel.Load<ProtogameEventsModule>();

            // The collision module handles 2D non-physics collisions.  We use this module
            // to handle the per-pixel collisions required in Asteroids.  In 3D games, you
            // would use the physics module instead.
            kernel.Load<ProtogameCollisionModule>();

            // The debug module allows engine systems to render debug information on the
            // screen through an IDebugRenderPass.  If you don't include this module,
            // IDebugRenderer is mapped to NullDebugRenderer instead, which means debug
            // rendering won't work even if you add the debug render pass.  So you need
            // to include both this module, and the debug render pass in the render pipeline
            // configuration in order to see debug information.
            kernel.Load<ProtogameDebugModule>();

            // This is the module specific to this game.
            kernel.Load<AsteroidsModule>();
        }
        
        public Game ConstructGame(IKernel kernel)
        {
            return new AsteroidsGame(kernel);
        }
    }
}