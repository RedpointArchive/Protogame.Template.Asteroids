using System;
using Protoinject;

namespace PROJECT_SAFE_NAME
{
    public interface IEntityFactory : IGenerateFactory
    {
        ExampleEntity CreateExampleEntity(string name);
    }
}
