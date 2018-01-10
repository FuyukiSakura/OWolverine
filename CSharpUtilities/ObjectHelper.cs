using System;
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
    }
}
