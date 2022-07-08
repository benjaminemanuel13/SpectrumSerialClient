namespace SpectrumSerialClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Services.SerialService svc = new Services.SerialService();
            svc.Create("COM1");

            Console.WriteLine("Connected, press Enter to exit.");
            Console.ReadLine();
        }
    }
}