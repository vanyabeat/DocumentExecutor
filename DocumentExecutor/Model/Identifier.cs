using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DocumentExecutor.Model
{
    public class Identifier : IEquatable<Identifier>, IComparable<Identifier>
    {
        public string Content { get; set; }
        public int IdentifierTypeId => IdentifierType.Id;
        public virtual IdentifierType IdentifierType { get; set; }

        //public int CompareTo(object? obj)
        //{
        //    if (obj == null)
        //    {
        //        return 1;
        //    }
        //    Identifier otherId = obj as Identifier;
        //    if (otherId != null)
        //        return String.Compare(this.Content, otherId.Content, StringComparison.Ordinal);
        //    else
        //        throw new ArgumentException("Object is not a Temperature");
        //}
        public bool Equals(Identifier other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Content == other.Content && IdentifierTypeId == other.IdentifierTypeId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Identifier) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Content, IdentifierTypeId);
        }

        public int CompareTo(Identifier other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var contentComparison = string.Compare(Content, other.Content, StringComparison.Ordinal);
            if (contentComparison != 0) return contentComparison;
            return IdentifierTypeId.CompareTo(other.IdentifierTypeId);
        }
    }
}
