using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;


// ReSharper disable once CheckNamespace
namespace Common.ArcObjects
{
    //TODO: Maybe need to make this a class so can maintain state
    /// <summary>
    /// Extension methods for IMap type
    /// </summary>
    [ComVisible(false)]
    public static class MapExtensions
    {
        public static IWorkspace MapWorkspace(this IMap map)
        {
            IFeatureLayer featureLayer = map.GetFeatureLayers().First();

            return ((IDataset) featureLayer.FeatureClass).Workspace;
        }

        

        /// <summary>
        /// Extension method to get the list of feature layers from the map
        /// </summary>
        /// <param name="map"></param>
        /// <returns>IEnumerable containing all feature layers</returns>
        public static IEnumerable<IFeatureLayer> GetFeatureLayers(this IMap map)
        {
            //UID to only return feature layers
            UID uid = new UIDClass {Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}"};
            IEnumLayer layers = map.Layers[uid];

            var featureLayer = layers.Next() as IFeatureLayer;

            while ( featureLayer != null )
            {
                yield return featureLayer;
                featureLayer = layers.Next() as IFeatureLayer;
            }
        }

        /// <summary>
        /// Extension method to get the feature layer based from the name of the feature class from the map
        /// </summary>
        /// <param name="map"></param>
        /// <param name="name">Feature class name</param>
        /// <returns>IFeatureLayer</returns>
        public static IFeatureLayer GetFeatureLayerFromFeatureClassName(this IMap map, string name)
        {
            IEnumerable<IFeatureLayer> featureLayers = map.GetFeatureLayers();

            return featureLayers.FirstOrDefault(fl => ((IDataset)fl.FeatureClass).Name.ToUpper() == name.ToUpper());
        }

        /// <summary>
        /// Extension method to retrieve a feature class by name from the map
        /// </summary>
        /// <param name="map"></param>
        /// <param name="name">Feature class name</param>
        /// <returns>IFeatureClass</returns>
        public static IFeatureClass GetFeatureClassFromName(this IMap map, string name)
        {
            IFeatureLayer featureLayer = map.GetFeatureLayerFromFeatureClassName(name);

            return featureLayer.FeatureClass;
        }
        
    }
}