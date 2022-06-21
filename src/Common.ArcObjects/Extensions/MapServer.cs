using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

// ReSharper disable once CheckNamespace

namespace Common.ArcObjects
{
    public static class MapServer
    {
        /// <summary>
        /// Name of FeatureClass associtaed to a Map Server Layer 
        /// </summary>
        /// <param name="mapServer"></param>
        /// <param name="layerId">The ID of the Map Server layer</param>
        /// <returns>Feature Class Name</returns>
        public static string FeatureClassName(this IMapServer3 mapServer, int layerId)
        {
            IMapServerDataAccess dataAccess = (IMapServerDataAccess)mapServer;

            if (dataAccess.GetDataSource(mapServer.DefaultMapName, layerId) is ITable)
            {
                return ((IDataset) dataAccess.GetDataSource(mapServer.DefaultMapName, layerId)).Name;
            }

            return null;
        }

        /// <summary>
        /// Feature Class associated to Map Server layer
        /// </summary>
        /// <param name="mapServer"></param>
        /// <param name="layerId">ID of the Map Server layer</param>
        /// <returns>Feature Class</returns>
        public static IFeatureClass FeatureClass(this IMapServer3 mapServer, int layerId)
        {
            //IMapServerDataAccess give access to the Services underlying data - new in 10.1
            IMapServerDataAccess dataAccess = (IMapServerDataAccess) mapServer;
            IFeatureClass featureClass = (IFeatureClass) dataAccess.GetDataSource(mapServer.DefaultMapName, layerId);

            return featureClass;
        }


        /// <summary>
        /// Finds the table name of a table layer in service.  Meant to be used when only a single table layer
        /// exists, otherwise returns the first table layer found
        /// </summary>
        /// <param name="mapServer"></param>
        /// <returns>The table name of the first table layer found</returns>
        public static string TableLayerName(this IMapServer3 mapServer)
        {

            IMapServerInfo4 msInfo = (IMapServerInfo4)mapServer.GetServerInfo(mapServer.DefaultMapName);
            IStandaloneTableInfos layerInfos = msInfo.StandaloneTableInfos;
            
            if ( layerInfos.Count >= 1 )
            {
                return layerInfos.Element[0].Name;
            }

            return null;
        }

        public static ITable Table(this IMapServer3 mapServer)
        {
            IMapServerInfo4 msInfo = (IMapServerInfo4)mapServer.GetServerInfo(mapServer.DefaultMapName);
            IStandaloneTableInfos layerInfos = msInfo.StandaloneTableInfos;

            IMapServerDataAccess dataAccess = (IMapServerDataAccess)mapServer;
            ITable table = (ITable)dataAccess.GetDataSource(mapServer.DefaultMapName, layerInfos.Element[0].ID);

            return table;
        }

        /// <summary>
        /// THe MapLayerInfo object for the default Map Frame
        /// </summary>
        /// <param name="mapServer"></param>
        /// <returns>IMapLayerInfos object</returns>
        public static IMapLayerInfos MapLayerInfos(this IMapServer3 mapServer)
        {
            IMapServerInfo msInfo = mapServer.GetServerInfo(mapServer.DefaultMapName);

            IMapLayerInfos layerInfos = msInfo.MapLayerInfos;

            return layerInfos;
        }

        /// <summary>
        /// Workspace associated to a Map Server layer
        /// </summary>
        /// <param name="mapServer"></param>
        /// <param name="layerId">ID of the Map Server layer</param>
        /// <returns>The IWorkspace</returns>
        public static IWorkspace Workspace(this IMapServer3 mapServer, int layerId)
        {
            IFeatureClass featureClass = mapServer.FeatureClass(layerId);
            return featureClass.Workspace();
        }
    }
}