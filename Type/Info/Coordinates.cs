namespace MCFBuilder.Type.Info
{
    public struct Coordinates
    {
        public double? X { get; set; }
        public double? Y { get; set; }
        public double? Z { get; set; }
        public Coordinates(double? x = null, double? y = null, double? z = null)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
