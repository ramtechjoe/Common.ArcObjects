using System.Collections.Generic;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geodatabase;


// ReSharper disable once CheckNamespace
namespace Common.ArcObjects
{
    [ComVisible(false)]
    public static class TableExtensions
    {
        public static IEnumerable<IRow> GetRows(this ICursor cursor)
        {
            IRow row = cursor.NextRow();
            while (row != null)
            {
                yield return row;
                row = cursor.NextRow();
            }

            while ( Marshal.ReleaseComObject(cursor) > 0 ) { }
        }

        public static IEnumerable<IRow> GetAllRows(this ITable table)
        {
            IQueryFilter filter = new QueryFilterClass();
            filter.WhereClause = "1 = 1";
            ICursor cursor = table.Search(filter, false);

            return cursor.GetRows();
        }



        public static IRow SingleRowSearch(this ITable table, IQueryFilter queryFilter)
        {
            ICursor cursor = table.Search(queryFilter, false);

            IRow result = cursor.NextRow();

            while (Marshal.ReleaseComObject(cursor) > 0) { }

            return result;
        }

        public static IRow SingleRowSearch(this ITable table, string whereClause)
        {
            IQueryFilter2 queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = whereClause;

            return table.SingleRowSearch(queryFilter);
        }

        /// <summary>
        /// Search extension which searches based on the provided where clause
        /// </summary>
        /// <param name="table"></param>
        /// <param name="whereClause">The where clause to apply</param>
        /// <returns>IEnumerable of Rows found</returns>
        public static IEnumerable<IRow> Search(this ITable table, string whereClause)
        {
            IQueryFilter2 queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = whereClause;

            return table.Search(queryFilter);
        }

        /// <summary>
        /// Search extension which searches based on the provided where clause
        /// </summary>
        /// <param name="table"></param>
        /// <param name="whereClause">The where clause to apply</param>
        /// <param name="subfields">The subfields to return</param>
        /// <returns>IEnumerable of Rows found</returns>
        public static IEnumerable<IRow> Search(this ITable table, string whereClause, string subfields)
        {
            IQueryFilter2 queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = whereClause;
            queryFilter.SubFields = subfields;

            return table.Search(queryFilter);
        }

        /// <summary>
        /// Extension Search which searches table and return IEnumerable instead of
        /// ICursor
        /// </summary>
        /// <param name="table">Table being searched</param>
        /// <param name="queryFilter">The QueryFilter</param>
        /// <returns>IEnumerable of features found</returns>
        public static IEnumerable<IRow> Search(this ITable table, IQueryFilter queryFilter)
        {
            ICursor cursor = table.Search(queryFilter, false);
            if ( cursor == null ) return new List<IRow>();

            return cursor.GetRows();
        }

        /// <summary>
        /// The Table's name
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string Name(this ITable table)
        {
            return ((IDataset)table).Name;
        }

        /// <summary>
        /// The Table's IWorkspace
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IWorkspace Workspace(this ITable table)
        {
            return ((IDataset)table).Workspace;
        }
    }
}