using System.Collections.Generic;

namespace Imast.Yagen.Cli.Ext;

/// <summary>
/// The dictionary extensions class
/// </summary>
public static class Dict
{
    /// <summary>
    /// Deep merge given dictionary into current one
    /// </summary>
    /// <param name="current">The current dictionary</param>
    /// <param name="other">The other dictionary</param>
    /// <returns></returns>
    public static IDictionary<object, object> DeepApply(this IDictionary<object, object> current, IDictionary<object, object> other)
    {
        // traverse each entry in the other object
        foreach (var entry in other)
        {
            // get the key
            var key = entry.Key;

            // get the value
            var value = entry.Value;

            // the value as a complex value
            var complexValue = value as IDictionary<object, object>;
                
            // try get existing value
            var existingValue = current.TryGetValue(key, out var existing) ? existing : null;

            // the existing value as a complex value
            var complexExistingValue = existingValue as IDictionary<object, object>;

            // if not a complex value just override
            if (complexValue == null)
            {
                current[key] = value;
                continue;
            }

            // in case if new value by key is a complex value but existing value does not exist or is a primitive value
            if (complexExistingValue == null)
            {
                current[key] = value;
                continue;
            }

            // if both values are there and are complex objects merge
            complexExistingValue.DeepApply(complexValue);
        }

        // return result for chaining
        return current;
    }
}