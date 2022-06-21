using System.Collections.Generic;
using ESRI.ArcGIS.Geodatabase;



// ReSharper disable once CheckNamespace
namespace Common.ArcObjects
{
    public static class RowExtensions
    {
        public static object get_Value(this IRow row, string fieldName)
        {
            int index = row.Fields.FindField(fieldName);

            return row.Value[index];
        }

        /// <summary>
        /// set_Value extension method using field name and value
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName">field name</param>
        /// <param name="value">value</param>
        public static void set_Value(this IRow row, string fieldName, object value)
        {
            int index = row.Fields.FindField(fieldName);

            row.Value[index] = value;
        }

        /// <summary>
        /// set_Value extension method using field name and value
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName">field name</param>
        /// <param name="value">value</param>
        public static void set_Value(this IRowBuffer row, string fieldName, object value)
        {
            int index = row.Fields.FindField(fieldName);

            row.Value[index] = value;
        }

        public static IEnumerable<IField> Fields(this IRow row)
        {
            for ( int i = 0; i < row.Fields.FieldCount; i++ )
            {
                yield return row.Fields.Field[i];
            }
        }

        public static IWorkspace Workspace(this IRow row)
        {
            return ((IDataset) row.Table).Workspace;
        }

        public static string TableName(this IRow row)
        {
            return ((IDataset) row.Table).Name;
        }
    }
}