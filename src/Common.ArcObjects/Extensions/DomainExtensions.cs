using ESRI.ArcGIS.Geodatabase;

// ReSharper disable once CheckNamespace
namespace Common.ArcObjects
{
    public static class DomainExtensions
    {
        public static string ValueFromCode(this IDomain domain, int code)
        {
            var cvDomain = (ICodedValueDomain2) domain;

            for (int i = 0; i < cvDomain.CodeCount; i++)
            {
                if (cvDomain.Value[i].ToString() == code.ToString())
                {
                    return cvDomain.Name[i].ToString();
                }
            }

            return null;
        }
    }
}