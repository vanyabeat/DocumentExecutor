namespace DocumentExecutor.Model
{
    public class IdentifierType 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }

        public override string ToString()
        {
            return $"({Name})";
        }

        public object ToObject()
        {
            return new
            {
                Id = Id,
                Info = Info,
                Name = Name
            };
        }
    }
}
