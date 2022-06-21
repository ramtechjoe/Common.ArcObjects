using System.Collections.Generic;
using ESRI.ArcGIS.Geodatabase;

namespace Common.ArcObjects.Comparers
{
    /// <summary>
    /// Compares equality of features based on the ObjectID of the feature
    /// </summary>
    public class FeatureEqualityComparer : IEqualityComparer<IFeature>
    {
        #region Implementation of IEqualityComparer<IFeature>

        public bool Equals(IFeature x, IFeature y)
        {
            if ( x == null || y == null )
            {
                return false;
            }

            //first make sure same feature class
            if ( ((IDataset)x.Class).Name != ((IDataset)y.Class).Name ) return false;

            if ( x.OID == y.OID )
            {
                return true;
            }

            return false;
        }

        public int GetHashCode(IFeature obj)
        {
            return obj.OID.GetHashCode();
        }

        #endregion
    }
}