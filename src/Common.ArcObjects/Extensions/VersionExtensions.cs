using ESRI.ArcGIS.Geodatabase;

// ReSharper disable once CheckNamespace
namespace Common.ArcObjects
{
    public static class VersionExtensions
    {
        public static IFeatureClass OpenFeatureClass(this IVersion version, string featureClassName)
        {
            return ((IWorkspace) version).OpenFeatureClass(featureClassName);
        }


        public static void Reconcile(this IWorkspace workspace, string versionName)
        {
            var versionEdit = (IVersionEdit) workspace;

            versionEdit.Reconcile(versionName);
        }

        public static IVersion ReconcileVersion(this IWorkspace workspace)
        {
            var versionEdit = (IVersionEdit)workspace;

            return versionEdit.ReconcileVersion;
        }

        public static void Post(this IWorkspace workspace, string versionName)
        {
            var versionEdit = (IVersionEdit)workspace;

            versionEdit.Post(versionName);
        }
    }
}