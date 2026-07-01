using System.Reflection;

namespace Melanin.Tests.TestHelpers
{
    public static class TestHelper
    {
        // Force la propriété "Id" (private set) d'une entité, uniquement pour les tests
        public static void SetId(object entity, int id)
        {
            PropertyInfo? property = entity.GetType().GetProperty("Id");
            property!.SetValue(entity, id);
        }
    }
}