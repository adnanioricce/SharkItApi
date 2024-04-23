using System;
using System.Reflection;
namespace Shark.Domain.Extensions;
public static class ObjectExtensions
{
    public static bool CopyDirtyPropsToDestination<T>(this T source, T destination)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (destination == null)
            throw new ArgumentNullException(nameof(destination));
        if (source.GetType() != destination.GetType())
            throw new ArgumentException("Objects must be of the same type");

        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties();
        bool changed = false;
        foreach (PropertyInfo property in properties)
        {
            object sourceValue = property.GetValue(source);
            object destinationValue = property.GetValue(destination);

            if (!Equals(sourceValue, destinationValue))
            {
                property.SetValue(destination, sourceValue);
                changed = true;
            }
        }
        return changed;
    }
}