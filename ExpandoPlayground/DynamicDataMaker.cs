using System.Dynamic;
using System.Reflection;

namespace ExpandoPlayground
{
    public static class DynamicDataMaker
    {
        public static dynamic MakeDynamicContact(ContactDTO dto, List<FieldDescription> fieldDescs)
        {
            dynamic dynamicContact = new ExpandoObject();

            foreach (FieldDescription fieldDesc in fieldDescs)
            {

                if (fieldDesc.TypeName is not null)
                {
                    (string? partName, string? partType) = BreakInTwo(fieldDesc.FieldName, ":");
                    if (partType.Contains(":"))
                    {
                        throw new InvalidOperationException("Only one level of nesting is supported");
                    }
                    DoAddToList(dto, dynamicContact, fieldDesc);
                }
                else
                {
                    object? propertyValue = dto.GetType().GetProperty(fieldDesc.DbName)?.GetValue(dto);
                    (string? groupName, string? propName) = BreakInTwo(fieldDesc.FieldName, ":");
                    if (propName is not null)
                    {
                        if (!HasProperty(dynamicContact, groupName))
                        {
                            ((IDictionary<string, object>)dynamicContact)[groupName] = new ExpandoObject();
                        }
                        dynamic group = GetDynamicPropertyByName(dynamicContact, groupName);
                        ((IDictionary<string, object>)group)[propName] = propertyValue;
                    }
                    else
                    {
                        ((IDictionary<string, object>)dynamicContact)[fieldDesc.FieldName] = propertyValue;
                    }
                }
            }

            return dynamicContact;
        }

        private static void DoAddToList(ContactDTO dto, dynamic dynamicContact, FieldDescription fieldDesc)
        {
            var propertyValue = dto.GetType().GetProperty(fieldDesc.DbName)?.GetValue(dto);

            (string? partName, string? partType) = BreakInTwo(fieldDesc.FieldName, ":");


            if (!HasProperty(dynamicContact, partName))
            {
                ((IDictionary<string, object>)dynamicContact)[partName] = new List<ExpandoObject>();
            }
            var partLib = GetDynamicPropertyByName(dynamicContact, partName) as List<ExpandoObject>;
            if (partLib is null)
            {
                throw new InvalidOperationException($"Property {partName} is not a list of ExpandoObject");
            }
            ExpandoObject part = new ExpandoObject();
            part.TryAdd(partType, propertyValue);
            part.TryAdd("Type", fieldDesc.TypeName);
            partLib.Add(part);
        }

        public static dynamic? GetDynamicPropertyByName(dynamic obj, string propertyName)
        {
            if (HasProperty(obj, propertyName))
            {
                return ((IDictionary<string, object>)obj)[propertyName];
            }
            else
            {
                return null;
            }
        }

        public static (string?, string?) BreakInTwo(string? input, string separator)
        {
            if (input is null)
            {
                return ((string?)null, (string?)null);
            }
            int separatorIndex = input.IndexOf(separator);
            if (separatorIndex == -1)
            {
                return (input, (string?)null);
            }

            string part1 = input.Substring(0, separatorIndex);
            string part2 = input[(separatorIndex + separator.Length)..];

            return (part1, part2);
        }

        public static bool HasProperty(dynamic obj, string propertyName)
        {
            bool doesIt = (((IDictionary<string, object>)obj).ContainsKey(propertyName));
            return doesIt;
        }

    }
}
