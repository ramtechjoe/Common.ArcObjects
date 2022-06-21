using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
// ReSharper disable CheckNamespace


namespace Common.ArcObjects
{
    /// <summary>
    /// Extension methods for Network Class Interface types
    /// </summary>
    [ComVisible(false)]
    public static class NetworkExtensions
    {
        /// <summary>
        /// Find the ObjectID of an edge from the EID
        /// </summary>
        /// <param name="network"></param>
        /// <param name="edgeEID"></param>
        /// <returns></returns>
        public static int FindEdgeOID(this INetwork network, int edgeEID)
        {
            var netElements = (INetElements)network;
            int userClassId, userId, subId;

            netElements.QueryIDs(edgeEID, esriElementType.esriETEdge, out userClassId, out userId, out subId);

            return userId;
        }

        /// <summary>
        /// Find the ObjectID of a Junction from the EID
        /// </summary>
        /// <param name="network"></param>
        /// <param name="junctionEID"></param>
        /// <returns></returns>
        public static int FindJunctionOID(this INetwork network, int junctionEID)
        {
            var netElements = (INetElements)network;
            int userClassId, userId, subId;

            netElements.QueryIDs(junctionEID, esriElementType.esriETJunction, out userClassId, out userId, out subId);

            return userId;
        }

        /// <summary>
        /// Find the ClassID of an Edge from EID
        /// </summary>
        /// <param name="network"></param>
        /// <param name="eid"></param>
        /// <returns></returns>
        public static int FindEdgeClassId(this INetwork network, int eid)
        {
            return FindClassId(network, eid, esriElementType.esriETEdge);
        }

        /// <summary>
        /// Find the ClassID of a Junction from the EID
        /// </summary>
        /// <param name="network"></param>
        /// <param name="eid"></param>
        /// <returns></returns>
        public static int FindJunctionClassId(this INetwork network, int eid)
        {
            return FindClassId(network, eid, esriElementType.esriETJunction);
        }

        /// <summary>
        /// Find the ClassID of a network element
        /// </summary>
        /// <param name="network"></param>
        /// <param name="eid"></param>
        /// <param name="elementType"></param>
        /// <returns></returns>
        public static int FindClassId(this INetwork network, int eid, esriElementType elementType)
        {
            var netElements = (INetElements)network;
            int userClassId, userId, subId;

            netElements.QueryIDs(eid, elementType, out userClassId, out userId, out subId);

            return userClassId;
        }



        /// <summary>
        /// Convert IEnumNetEID to a .net IEnumerable(List) type
        /// </summary>
        /// <param name="netEIDs"></param>
        /// <returns></returns>
        public static IEnumerable<int> ToList(this IEnumNetEID netEIDs)
        {
            netEIDs.Reset();

            int eid = netEIDs.Next();

            while ( eid != 0 )
            {
                yield return eid;
                eid = netEIDs.Next();
            }
        }

        /// <summary>
        /// Find the EID of a network feature.  If complex edge returns the EID at the start junction
        /// point
        /// </summary>
        /// <param name="feature"></param>
        /// <returns>The network feature EID, 0 if feature is not a network feature</returns>
        public static int EID(this IFeature feature)
        {
            var junctionFeature = feature as ISimpleJunctionFeature;
            if ( junctionFeature != null ) return junctionFeature.EID;

            var edgeFeature = feature as ISimpleEdgeFeature;
            if ( edgeFeature != null ) return edgeFeature.EID;

            var complexEdge = feature as IComplexEdgeFeature;
            if ( complexEdge != null )
            {
                var junction = (IFeature)complexEdge.get_JunctionFeature(0);
                var point = (IPoint)junction.Shape;

                var complexNetFeature = (IComplexNetworkFeature)feature;

                return complexNetFeature.FindEdgeEID(point);
            }

            return 0;
        }

        /// <summary>
        /// Retrieve feature assocaited to a junction EID
        /// </summary>
        /// <param name="network"></param>
        /// <param name="junctionEID">Juncion EID</param>
        /// <returns>The feature</returns>
        public static IFeature JunctionFeatureFromEID(this INetwork network, int junctionEID)
        {
            var dataset = (IDataset)network;

            var featureWkspMange = (IFeatureWorkspaceManage2)dataset.Workspace;
            int classId = network.FindJunctionClassId(junctionEID);
            int oid = network.FindJunctionOID(junctionEID);

            string featureClassName = featureWkspMange.GetObjectClassNameByID(classId);

            IFeatureClass featureClass = ((IFeatureWorkspace)featureWkspMange).OpenFeatureClass(featureClassName);

            IFeature feature = featureClass.GetFeature(oid);

            return feature;
        }

        /// <summary>
        /// Retrieve feature assocaited to a Edge EID
        /// </summary>
        /// <param name="network"></param>
        /// <param name="edgeEID">Edge EID</param>
        /// <returns>The feature</returns>
        public static IFeature EdgeFeatureFromEID(this INetwork network, int edgeEID)
        {
            var dataset = (IDataset)network;

            var featureWkspMange = (IFeatureWorkspaceManage2)dataset.Workspace;
            int classId = network.FindEdgeClassId(edgeEID);
            int oid = network.FindEdgeOID(edgeEID);

            string featureClassName = featureWkspMange.GetObjectClassNameByID(classId);

            IFeatureClass featureClass = ((IFeatureWorkspace)featureWkspMange).OpenFeatureClass(featureClassName);

            IFeature feature = featureClass.GetFeature(oid);

            return feature;
        }

        /// <summary>
        /// Returns the subtype code for the feature represeted by an Edge EID
        /// </summary>
        /// <param name="network"></param>
        /// <param name="edgeEID">Edge EID</param>
        /// <returns>Subtype Code</returns>
        public static int EdgeSubtype(this INetwork network, int edgeEID)
        {
            IFeature feature = network.EdgeFeatureFromEID(edgeEID);
            var substypes = (ISubtypes)feature.Class;

            int subtypeCode = Convert.ToInt32(feature.get_Value(substypes.SubtypeFieldIndex));

            return subtypeCode;
        }
    }
}