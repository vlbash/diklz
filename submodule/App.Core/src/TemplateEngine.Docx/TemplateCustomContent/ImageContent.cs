using System;
using System.Globalization;
using System.Linq;

namespace TemplateEngine.Docx
{
	[ContentItemName("Image")]
	public class ImageContent : IContentItem, IEquatable<ImageContent>
    {
        public ImageContent()
        {
            
        }

        public ImageContent(string name, byte[] binary)
        {
            Name = name;
            Binary = binary;
        }
   
        public string Name { get; set; }
		public byte[] Binary { get; set; }

		#region Equals
		public bool Equals(ImageContent other)
		{
			if (other == null) return false;

#if NETSTANDARD1_6
            var comparer = CultureInfo.InvariantCulture.CompareInfo.GetStringComparer(CompareOptions.IgnoreCase);
#else
		    var comparer = StringComparer.InvariantCultureIgnoreCase;
#endif

            return comparer.Equals(Name, other.Name)
		           && Binary.SequenceEqual(other.Binary);
		}

		public bool Equals(IContentItem other)
		{
			if (!(other is ImageContent)) return false;

			return Equals((ImageContent)other);
		}

		public override int GetHashCode()
		{
			return new {Name, Binary}.GetHashCode();
		}

#endregion
	}
}
