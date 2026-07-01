namespace Osrm.Client
{
    internal interface IGeometryFormatAware
    {
        void SetGeometryFormat(string geometryFormat);
    }

    internal interface IHasGeometryFormat
    {
        string Geometries { get; }
    }
}
