using System.Collections.Generic;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;


// ReSharper disable once CheckNamespace
namespace Common.ArcObjects
{
    public static class WorkspaceExtensions
    {
        /// <summary>
        /// Opens an existing Feature Class
        /// </summary>
        /// <param name="wksp"></param>
        /// <param name="name">Fully qualified name of the feature class</param>
        /// <returns></returns>
        public static IFeatureClass OpenFeatureClass(this IWorkspace wksp, string name)
        {
            var featureWorkspace = (IFeatureWorkspace)wksp;
            
            return featureWorkspace.OpenFeatureClass(name);
        }

        /// <summary>
        /// Opens an existing table
        /// </summary>
        /// <param name="wksp"></param>
        /// <param name="name">Fully qualified name of the table</param>
        /// <returns></returns>
        public static ITable OpenTable(this IWorkspace wksp, string name)
        {
            var featureWorkspace = (IFeatureWorkspace)wksp;

            return featureWorkspace.OpenTable(name);
        }

        /// <summary>
        /// Creates a query definition object
        /// </summary>
        /// <param name="wksp"></param>
        /// <returns></returns>
        public static IQueryDef CreateQueryDef(this IWorkspace wksp)
        {
            var featureWorkspace = (IFeatureWorkspace)wksp;

            return featureWorkspace.CreateQueryDef();
        }

        public static IEnumerable<IRow> GetRowsUsingQueryDef(this IWorkspace wksp, string tableNames, string query)
        {
            IQueryDef queryDef = wksp.CreateQueryDef();
            queryDef.Tables = tableNames;
            queryDef.WhereClause = query;
            ICursor cursor = queryDef.Evaluate();
            IRow row = cursor.NextRow();

            while (row != null)
            {
                yield return row;
                row = cursor.NextRow();
            }
        }

        /// <summary>
        /// Opens an exsiting relationship class
        /// </summary>
        /// <param name="wksp"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IRelationshipClass OpenRelationshipClass(this IWorkspace wksp, string name)
        {
            var featureWorkspace = (IFeatureWorkspace)wksp;

            return featureWorkspace.OpenRelationshipClass(name);
        }

        /// <summary>
        /// Creates a new connection to the same database as the existing workspace
        /// </summary>
        /// <param name="wksp"></param>
        /// <returns></returns>
        public static IWorkspace Clone(this IWorkspace wksp)
        {
            //TODO:  create new property set and use copied properties
            IWorkspaceFactory factory = new SdeWorkspaceFactoryClass();

            string instance = wksp.ConnectionProperties.GetProperty("INSTANCE").ToString();
            string version = wksp.ConnectionProperties.GetProperty("VERSION").ToString();
            string user = wksp.ConnectionProperties.GetProperty("USER").ToString();
            string password = wksp.ConnectionProperties.GetProperty("PASSWORD").ToString();
            string database = wksp.ConnectionProperties.GetProperty("DATABASE").ToString();

            IPropertySet2 connectionProperties = new PropertySetClass();

            connectionProperties.SetProperty("INSTANCE", instance);
            connectionProperties.SetProperty("VERSION", version);
            connectionProperties.SetProperty("USER", user);
            connectionProperties.SetProperty("PASSWORD", password);
            connectionProperties.SetProperty("DATABASE", database);

            IWorkspace workspace = factory.Open(connectionProperties, 0);

            return workspace;
        }

        /// <summary>
        /// Starts editing the workspace
        /// </summary>
        /// <param name="wksp"></param>
        /// <param name="withUndoRedo"></param>
        public static void StartEditing(this IWorkspace wksp, bool withUndoRedo)
        {
            var wkspEdit = (IWorkspaceEdit)wksp;

            wkspEdit.StartEditing(withUndoRedo);
        }
        /// <summary>
        /// Stops editing the workspace
        /// </summary>
        /// <param name="wksp"></param>
        /// <param name="saveEdits"></param>
        public static void StopEditing(this IWorkspace wksp, bool saveEdits)
        {
            var wkspEdit = (IWorkspaceEdit)wksp;

            wkspEdit.StopEditing(saveEdits);
        }
        /// <summary>
        /// Begins an edit operation
        /// </summary>
        /// <param name="wksp"></param>
        public static void StartEditOperation(this IWorkspace wksp)
        {
            var wkspEdit = (IWorkspaceEdit)wksp;

            wkspEdit.StartEditOperation();
        }
        /// <summary>
        /// Ends an edit operation
        /// </summary>
        /// <param name="wksp"></param>
        public static void StopEditOperation(this IWorkspace wksp)
        {
            var wkspEdit = (IWorkspaceEdit)wksp;
            wkspEdit.StopEditOperation();
        }
        /// <summary>
        /// Gets the names of all the existing versions
        /// </summary>
        /// <param name="wksp"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetVersionNames(this IWorkspace wksp)
        {
            var versionedWksp = (IVersionedWorkspace)wksp;

            IEnumVersionInfo versionInfos = versionedWksp.Versions;
            IVersionInfo versionInfo = versionInfos.Next();

            while ( versionInfo != null )
            {
                yield return versionInfo.VersionName;
                versionInfo = versionInfos.Next();
            }
        }
        /// <summary>
        /// Change Workspace Version
        /// </summary>
        /// <param name="wksp"></param>
        /// <param name="versionName"></param>
        /// <returns></returns>
        public static IWorkspace ChangeVersion(this IWorkspace wksp, string versionName)
        {
            var versionedWksp = wksp as IVersionedWorkspace;
            if ( versionedWksp == null ) return wksp;

            IVersion version = versionedWksp.FindVersion(versionName);

            return (IWorkspace) version;
        }

    }
}