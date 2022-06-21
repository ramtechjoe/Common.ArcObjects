using System.Collections.Generic;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
// ReSharper disable CheckNamespace
namespace Common.ArcObjects
{
    /// <summary>
    /// Extension methods for ArcObjects IEnum types
    /// </summary>
    [ComVisible(false)]
    public static class EnumExtensions
    {
        /// <summary>
        /// Converts IEnumBSTR to List type
        /// </summary>
        /// <param name="esriEnum"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToList(this IEnumBSTR esriEnum)
        {
            esriEnum.Reset();
            string s = esriEnum.Next();

            while ( s != null )
            {
                yield return s;
                s = esriEnum.Next();
            }
        }

        /// <summary>
        /// Converts IEnumName to List type
        /// </summary>
        /// <param name="esriEnum"></param>
        /// <returns></returns>
        public static IEnumerable<IName> ToList(this IEnumName esriEnum)
        {
            esriEnum.Reset();
            IName name = esriEnum.Next();

            while ( name != null )
            {
                yield return name;
                name = esriEnum.Next();
            }
        }

        /// <summary>
        /// Converts IEnumFeature to List type
        /// </summary>
        /// <param name="features"></param>
        /// <returns></returns>
        public static IEnumerable<IFeature> ToList(this IEnumFeature features)
        {
            features.Reset();
            IFeature feature = features.Next();

            while ( feature != null )
            {
                yield return feature;
                feature = features.Next();
            }
        }

        /// <summary>
        /// Converts Enum to List
        /// </summary>
        /// <param name="datasets"></param>
        /// <returns></returns>
        public static IEnumerable<IDataset> ToList(this IEnumDataset datasets)
        {
            datasets.Reset();
            IDataset dataset = datasets.Next();

            while ( dataset != null )
            {
                yield return dataset;
                dataset = datasets.Next();
            }
        }

        /// <summary>
        /// Converts Enum to List
        /// </summary>
        /// <param name="datasets"></param>
        /// <returns></returns>
        public static IEnumerable<IDatasetName> ToList(this IEnumDatasetName datasets)
        {
            datasets.Reset();
            IDatasetName dataset = datasets.Next();

            while ( dataset != null )
            {
                yield return dataset;
                dataset = datasets.Next();
            }
        }

        public static IDictionary<int, string> ToDictionary(this IEnumSubtype subtypes)
        {
            int code;
            string description = subtypes.Next(out code);

            var dictionary = new Dictionary<int, string>();

            while (description != null)
            {
                dictionary.Add(code, description);
                description = subtypes.Next(out code);
            }

            return dictionary;
        }
    }
}