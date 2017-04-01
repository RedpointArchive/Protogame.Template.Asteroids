using Protogame;

namespace PROJECT_SAFE_NAME
{
    /// <summary>
    /// This is the input binder, which configures how generated inputs map
    /// into the game.
    /// </summary>
    public class AsteroidsInputBinder : StaticEventBinder<IGameContext>
    {
        public override void Configure()
        {
            // In this case, bind all key events to all componentized entities.  This allows
            // the PlayerShipEntity to handle the key events in it's Handle method.
            Bind<KeyboardEvent>(x => true).ToAllComponentizedEntities();

            // All unbound events are ignored and won't be recieved by entities or other event
            // handling code.
        }
    }
}