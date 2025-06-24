namespace Autoclicker
{
    internal class Program
    {
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new UI());
        }
    }
}