using Domination.Warfare;


namespace Domination.Utils
{
    public static class UnitsNameConstructor
    {
        private const string DefaultUnitClassName = "Militiaman";

        public static string Build(Weapon weapon)
        {
            return $"{DefaultUnitClassName} with a {weapon.Name}";
        }
    }
}
