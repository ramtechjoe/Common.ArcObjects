using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geodatabase;
// ReSharper disable CheckNamespace


namespace Common.ArcObjects
{
    /// <summary>
    /// Extension methods for IFeature objects
    /// </summary>
    [ComVisible(false)]
    public static class FeatureExtensions
    {

        /// <summary>
        /// The Network a feature is part of
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public static INetwork Network(this IFeature feature)
        {
            return ((INetworkFeature)feature).GeometricNetwork.Network;
        }

        /// <summary>
        /// The Network a feature is part of
        /// </summary>
        /// <param name="features"></param>
        public static INetwork Network(this IEnumerable<IFeature> features)
        {
            IFeature feature = features.ToArray()[0];
            return feature.Network();
        }

        // ReSharper disable InconsistentNaming
        // ReSharper disable UseIndexedProperty
        /// <summary>
        /// Overload of get_Value for an IEdgeFeature type
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static object get_Value(this IEdgeFeature feature, int index)
        {
            return ((IFeature)feature).get_Value(index);
        }

        /// <summary>
        /// Overload of get_Value for an ISimpleEdgeFeature type
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static object get_Value(this ISimpleEdgeFeature feature, int index)
        {
            return ((IFeature)feature).get_Value(index);
        }

        /// <summary>
        /// Overload of get_Value for an IComplexEdgeFeature type
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static object get_Value(this IComplexEdgeFeature feature, int index)
        {
            return ((IFeature)feature).get_Value(index);
        }

        /// <summary>
        /// Overload of get_Value for an INetworkFeature type
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static object get_Value(this INetworkFeature feature, int index)
        {
            return ((IFeature)feature).get_Value(index);
        }

        /// <summary>
        /// Overload of get_Value for an IJunctionFeature type
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static object get_Value(this IJunctionFeature feature, int index)
        {
            return ((IFeature)feature).get_Value(index);
        }

        /// <summary>
        /// Overload of get_Value for an ISimpleJunctionFeature type
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static object get_Value(this ISimpleJunctionFeature feature, int index)
        {
            return ((IFeature)feature).get_Value(index);
        }

        /// <summary>
        /// Overload of get_Value for an ISimpleNetworkFeature type
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static object get_Value(this ISimpleNetworkFeature feature, int index)
        {
            return ((IFeature)feature).get_Value(index);
        }

        public static object get_Value(this IFeature feature, string fieldName)
        {
            int index = feature.Fields.FindField(fieldName);
            
            return feature.Value[index];
        }

        public static Dictionary<string, string> Values(this IFeature feature)
        {
            var values = new Dictionary<string, string>();
            for (int i = 0; i < feature.Fields.FieldCount; i++)
            {
                string value = feature.Value[i].ToString();
                values.Add(feature.Fields.Field[i].Name, value);
            }

            return values;
        }


        //TODO: needs to also do string
        public static string GetCodedDomainValue(this IFeature feature, string fieldName)
        {
            IField field = feature.Fields.Field[feature.Fields.FindField(fieldName)];

            if ( !field.HasCodedValueDomain() ) return null;

            if ( field.Type == esriFieldType.esriFieldTypeInteger )
            {
                int code = int.Parse(feature.get_Value(field.Name).ToString());
                return field.Domain.ValueFromCode(code);
            }

            return null;
        }

        public static void Connect(this IFeature feature)
        {
            var networkFeature = (INetworkFeature)feature;

            networkFeature.Connect();
        }

        // ReSharper restore InconsistentNaming

        public static int EdgeFeatureCount(this IFeature feature)
        {
            var junctionFeature = feature as ISimpleJunctionFeature;

            if ( junctionFeature == null ) return 0;

            return junctionFeature.EdgeFeatureCount;
        }

        public static bool IsNetworkFeature(this IFeature feature)
        {
            return (feature is INetworkFeature);
        }
    }
}