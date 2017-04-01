using Protoinject;

namespace PROJECT_SAFE_NAME
{
    public interface IEntityFactory : IGenerateFactory
    {
        AsteroidEntity CreateAsteroidEntity(int size);
        PlayerShipEntity CreatePlayerShipEntity();
        BulletEntity CreateBulletEntity();
    }
}
