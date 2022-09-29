namespace MCFBuilder.Type.Info
{
    public struct Distance
    {
        public float? Min { get; set; }
        public float? Max { get; set; }
        public Distance(float? min, float? max = null)
        {
            Min = min;
            Max = max;
        }
    }
}
