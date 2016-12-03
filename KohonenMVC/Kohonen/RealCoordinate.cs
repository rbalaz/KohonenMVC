namespace KohonenMVC.Kohonen
{
    public class RealCoordinate
    {
        public double x { get; private set; }
        public double y { get; private set; }

        public RealCoordinate(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public RealCoordinate(double[] coordinates)
        {
            x = coordinates[0];
            y = coordinates[1];
        }
    }
}