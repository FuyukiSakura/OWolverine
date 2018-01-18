using System;
using System.Linq;
using System.Reflection;

namespace CSharpUtilities
{
    public static class ObjectHelper
    {
        public static T CloneAndUpcast<T>(Object b) where T : IUpCastable, new()
        {
            T clone = new T();
            var members = b.GetType().GetMembers(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < members.Length; i++)
            {
                if (members[i].MemberType == MemberTypes.Property)
                {
                    clone
                        .GetType()
                        .GetProperty(members[i].Name)
                        .SetValue(clone, b.GetType().GetProperty(members[i].Name).GetValue(b, null), null);
                }
            }
            return clone;
        }

        public static void CopyPropertiesTo<T, TU>(this T source, TU dest)
        {
            var sourceProps = typeof(T).GetProperties().Where(x => x.CanRead).ToList();
            var destProps = typeof(TU).GetProperties()
                    .Where(x => x.CanWrite)
                    .ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (destProps.Any(x => x.Name == sourceProp.Name))
                {
                    var p = destProps.First(x => x.Name == sourceProp.Name);
                    if (p.CanWrite)
                    { // check if the property can be set or no.
                        p.SetValue(dest, sourceProp.GetValue(source, null), null);
                    }
                }
            }
        }
    }
}
