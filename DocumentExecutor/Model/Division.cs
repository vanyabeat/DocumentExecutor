using System;

namespace DocumentExecutor.Model
{
    public class Division : IComparable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Info { get; set; }


        public override string ToString()
        {
            return $"{Name} ({Info})";
        }

        public int CompareTo(object obj)
        {
            return obj switch
            {
                null => 1,
                Division otherDivision => this.Id.CompareTo(otherDivision.Id),
                _ => throw new ArgumentException("Object is not a Division")
            };
        }
    }
}