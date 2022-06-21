using System.Collections.Generic;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geodatabase;

// ReSharper disable once CheckNamespace
namespace Common.ArcObjects
{
    [ComVisible(false)]
    public static class ObjectClassExtensions
    {


        public static IEnumerable<IField> Fields(this IObjectClass objectClass)
        {
            for (int i = 0; i < objectClass.Fields.FieldCount; i++)
            {
                yield return objectClass.Fields.Field[i];
            }
        }

        public static string Name(this IObjectClass objectClass)
        {
            return ((IDataset)objectClass).Name;
        }

        public static IWorkspace Workspace(this IObjectClass objectClass)
        {
            return ((IDataset) objectClass).Workspace;
        }
    }
}