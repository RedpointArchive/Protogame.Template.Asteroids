using Microsoft.Xna.Framework;
using Protogame;

namespace PROJECT_SAFE_NAME
{
    /// <summary>
    /// This is a component which wraps entities around the screen.  It inherits from
    /// IUpdatableComponent (which means it has an Update method that gets called on
    /// every update), and IRenderableComponent (which means it has a Render method that
    /// gets called every render).  There's many more interfaces for components to
    /// implement, which allow you to hook into many different aspects of the game.
    /// 
    /// You can also make a component derive from ComponentizedObject if you want to
    /// have sub-components, and you can use the [RequireExisting] and [From...]
    /// attributes on constructor parameters to depend on components or entities
    /// elsewhere in the scene.
    /// </summary>
    public class WrapEntityComponent : IUpdatableComponent, IRenderableComponent
    {
        private const float _sceneWidth = 800;
        private const float _sceneHeight = 600;

        private bool _isRendering = false;

        public void Update(ComponentizedEntity entity, IGameContext gameContext, IUpdateContext updateContext)
        {
            var currentPosition = entity.Transform.LocalPosition;

            if (currentPosition.X < 0)
            {
                currentPosition.X += _sceneWidth;
            }
            if (currentPosition.Y < 0)
            {
                currentPosition.Y += _sceneHeight;
            }
            if (currentPosition.X > _sceneWidth)
            {
                currentPosition.X -= _sceneWidth;
            }
            if (currentPosition.Y > _sceneHeight)
            {
                currentPosition.Y -= _sceneHeight;
            }

            entity.Transform.LocalPosition = currentPosition;
        }

        public void Render(ComponentizedEntity entity, IGameContext gameContext, IRenderContext renderContext)
        {
            // We are going to move the entity to the other sides of the screen (as necessary) and 
            // re-call Render on the entity.  Since our Render method will be called again as part
            // of this, we use the _isRendering flag to ensure we don't do anything for these 
            // secondary rendering calls.
            if (_isRendering)
            {
                return;
            }

            _isRendering = true;

            entity.Transform.LocalPosition += new Vector3(-_sceneWidth, 0, 0);
            entity.Render(gameContext, renderContext);
            entity.Transform.LocalPosition -= new Vector3(-_sceneWidth, 0, 0);

            entity.Transform.LocalPosition += new Vector3(_sceneWidth, 0, 0);
            entity.Render(gameContext, renderContext);
            entity.Transform.LocalPosition -= new Vector3(_sceneWidth, 0, 0);
            
            entity.Transform.LocalPosition += new Vector3(0, -_sceneHeight, 0);
            entity.Render(gameContext, renderContext);
            entity.Transform.LocalPosition -= new Vector3(0, -_sceneHeight, 0);

            entity.Transform.LocalPosition += new Vector3(0, _sceneHeight, 0);
            entity.Render(gameContext, renderContext);
            entity.Transform.LocalPosition -= new Vector3(0, _sceneHeight, 0);

            entity.Transform.LocalPosition += new Vector3(-_sceneWidth, -_sceneHeight, 0);
            entity.Render(gameContext, renderContext);
            entity.Transform.LocalPosition -= new Vector3(-_sceneWidth, -_sceneHeight, 0);

            entity.Transform.LocalPosition += new Vector3(_sceneWidth, -_sceneHeight, 0);
            entity.Render(gameContext, renderContext);
            entity.Transform.LocalPosition -= new Vector3(_sceneWidth, -_sceneHeight, 0);

            entity.Transform.LocalPosition += new Vector3(-_sceneWidth, _sceneHeight, 0);
            entity.Render(gameContext, renderContext);
            entity.Transform.LocalPosition -= new Vector3(-_sceneWidth, _sceneHeight, 0);

            entity.Transform.LocalPosition += new Vector3(_sceneWidth, _sceneHeight, 0);
            entity.Render(gameContext, renderContext);
            entity.Transform.LocalPosition -= new Vector3(_sceneWidth, _sceneHeight, 0);

            _isRendering = false;
        }
    }
}
