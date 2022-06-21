using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geometry;

// ReSharper disable once CheckNamespace
namespace Common.ArcObjects
{
    public static class DocumentExtensions
    {
        public static IMap FocusMap(this IDocument document)
        {
            return ((IMxDocument) document).FocusMap;
        }
    }
}