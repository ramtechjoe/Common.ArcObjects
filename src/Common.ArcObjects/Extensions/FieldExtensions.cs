using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geodatabase;
// ReSharper disable CheckNamespace
namespace Common.ArcObjects
{
    [ComVisible(false)]
    public static class FieldExtensions
    {
        public static bool HasDomain(this IField field)
        {
            if ( field.Domain == null ) return false;

            return true;
        }

        public static bool HasCodedValueDomain(this IField field)
        {
            if ( !field.HasDomain() ) return false;

            return field.Domain is ICodedValueDomain;
        }
    }
}