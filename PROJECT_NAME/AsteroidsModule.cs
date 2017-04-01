using Protogame;
using Protoinject;

namespace PROJECT_SAFE_NAME
{
    /// <summary>
    /// The dependency injection configuration for the game.  This contains additional bindings
    /// specific to your game that you want to set up.
    /// </summary>
    public class AsteroidsModule : IProtoinjectModule
    {
        public void Load(IKernel kernel)
        {
            // Binds the entity factory as an automatic factory.
            kernel.Bind<IEntityFactory>().ToFactory();

            // Binds the event binder; this instructs Protogame to bind events according
            // to AsteroidsInputBinder.  You can bind multiple IEventBinder's and events will
            // propagate through all of them.
            kernel.Bind<IEventBinder<IGameContext>>().To<AsteroidsInputBinder>();
        }
    }
}

