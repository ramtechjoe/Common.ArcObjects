using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geodatabase;

// ReSharper disable CheckNamespace

namespace Common.ArcObjects
{
    /// <summary>
    /// Extension Methods for IFeatureClass objects
    /// </summary>
    [ComVisible(false)]
    public static class FeatureClassExtensions
    {
        /// <summary>
        /// Extension method to convert IFeatureCursor to .net IEnumerble
        /// </summary>
        /// <param name="featureCursor"></param>
        /// <returns>IEnumerable of IFeatures</returns>
        public static IEnumerable<IFeature> GetFeatures(this IFeatureCursor featureCursor)
        {
            IFeature feature = featureCursor.NextFeature();
            while ( feature != null )
            {
                yield return feature;
                feature = featureCursor.NextFeature();
            }

            while (Marshal.ReleaseComObject(featureCursor) > 0) { }
        }

        /// <summary>
        /// Get an IEnumerable of IFeatures given a set of object ids
        /// </summary>
        /// <param name="featureClass"></param>
        /// <param name="objectIds"></param>
        /// <returns></returns>
        public static IEnumerable<IFeature> GetFeatures(this IFeatureClass featureClass, IEnumerable<int> objectIds)
        {
            IFeatureCursor featureCursor = featureClass.GetFeatures(objectIds.ToArray(), false);

            IEnumerable<IFeature> features = featureCursor.GetFeatures();

            return features;
        }

        /// <summary>
        /// Extension Search which searches feature class for features and return IEnumerable instead of stndard
        /// IFeatureCursor
        /// </summary>
        /// <param name="featureClass">FeatureClass being searched</param>
        /// <param name="queryFilter">The QueryFilter</param>
        /// <returns>IEnumerable of features found</returns>
        public static IEnumerable<IFeature> Search(this IFeatureClass featureClass, IQueryFilter queryFilter)
        {
            IFeatureCursor featureCursor = featureClass.Search(queryFilter, false);
            if ( featureCursor == null ) return new List<IFeature>();

            IEnumerable<IFeature> results = featureCursor.GetFeatures();

            return results;
        }


        /// <summary>
        /// Search extension which searches based on the provided where clause
        /// </summary>
        /// <param name="featureClass"></param>
        /// <param name="whereClause">The where clause to apply</param>
        /// <returns>IEnumerable of features found</returns>
        public static IEnumerable<IFeature> Search(this IFeatureClass featureClass, string whereClause)
        {
            IQueryFilter2 queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = whereClause;

            return featureClass.Search(queryFilter);
        }

        public static IEnumerable<IFeature> Search(this IFeatureClass featureClass, string whereClause, string[] fields)
        {
            IQueryFilter2 queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = whereClause;

            if ( fields.Length >= 1 )
            {
                queryFilter.SubFields = string.Join(",", fields);
            }

            return featureClass.Search(queryFilter);
        }

        /// <summary>
        /// Search method that searches for features based on Object IDs
        /// </summary>
        /// <param name="featureClass"></param>
        /// <param name="objectIDs">The list of ObjectIDs to search for</param>
        /// <returns></returns>
        public static IEnumerable<IFeature> Search(this IFeatureClass featureClass, IEnumerable<int> objectIDs)
        {
            return featureClass.GetFeatures(objectIDs);
        }

        /// <summary>
        /// Extension Search which searches feature class for a single feature
        /// 
        /// </summary>
        /// <param name="featureClass">FeatureClass being searched</param>
        /// <param name="queryFilter">The QueryFilter</param>
        /// <returns>IFeature or null</returns>
        public static IFeature SingleFeatureSearch(this IFeatureClass featureClass, IQueryFilter queryFilter)
        {
            IFeatureCursor featureCursor = featureClass.Search(queryFilter, false);

            IFeature result = featureCursor.NextFeature();
            Marshal.ReleaseComObject(featureCursor);

            return result;
        }


        /// <summary>
        /// Search extension which searches based on the provided where clause
        /// </summary>
        /// <param name="featureClass"></param>
        /// <param name="whereClause">The where clause to apply</param>
        /// <returns>IFeature or null</returns>
        public static IFeature SingleFeatureSearch(this IFeatureClass featureClass, string whereClause)
        {
            IQueryFilter2 queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = whereClause;

            return featureClass.SingleFeatureSearch(queryFilter);
        }

        public static IFeature SingleFeatureSearch(this IFeatureClass featureClass, string whereClause, string[] fields)
        {
            IQueryFilter2 queryFilter = new QueryFilterClass();
            queryFilter.WhereClause = whereClause;
            if ( fields.Length > 0 )
            {
                queryFilter.SubFields = string.Join(",", fields);
            }

            return featureClass.SingleFeatureSearch(queryFilter);
        }

        /// <summary>
        /// The Network of the Feature Class
        /// </summary>
        /// <param name="featureClass"></param>
        /// <returns>The assocaited logical network of the feature class 
        /// or null if feature class does not belong to a network</returns>
        public static INetwork Network(this IFeatureClass featureClass)
        {
            var networkClass = featureClass as INetworkClass;

            if ( networkClass == null ) return null;

            return networkClass.GeometricNetwork.Network;
        }

        public static string Name(this IFeatureClass featureClass)
        {
            return ((IDataset) featureClass).Name;
        }

        public static IWorkspace Workspace(this IFeatureClass featureClass)
        {
            return ((IDataset) featureClass).Workspace;
        }

        public static bool IsNetworkFeatureClass(this IFeatureClass featureClass)
        {
            return (featureClass is INetworkClass);
        }
    }
}