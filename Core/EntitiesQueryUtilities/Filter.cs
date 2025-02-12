namespace Core.EntitiesQueryUtilities
{
    public abstract class Filter
    {
        protected Filter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }
    }
}
