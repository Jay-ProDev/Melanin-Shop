using System.Reflection;

namespace Melanin.Tests.TestHelpers
{
    public static class TestHelper
    {
        // Force la propriété "Id" d'une entité (private set géré par EF Core)
        public static void SetId(object entity, int id)
        {
            PropertyInfo? property = entity.GetType().GetProperty("Id");
            property!.SetValue(entity, id);
        }

        // Force n'importe quelle propriété privée d'une entité (pour les tests)
        public static void SetProperty(object entity, string propertyName, object? value)
        {
            PropertyInfo? property = entity.GetType().GetProperty(propertyName);
            property!.SetValue(entity, value);
        }
    }
}