using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private List<Task<Car>> cars;

        public Form1()
        {
            InitializeComponent();
            InitializeCars();
            Load += Form1_Load; // Додайте цей рядок
        }

        private void InitializeCars()
        {
            cars = new List<Task<Car>>
            {
                Task.Run(() => new Car("Toyota", 2020, new Dictionary<string, string> { { "FuelConsumptionUrban", "8.5" }, { "FuelConsumptionHighway", "6.0" } }, "Good", 25000)),
                Task.Run(() => new Car("Honda", 2022, new Dictionary<string, string> { { "FuelConsumptionUrban", "7.8" }, { "FuelConsumptionHighway", "5.8" } }, "Excellent", 28000)),
                Task.Run(() => new Car("Ford", 2019, new Dictionary<string, string> { { "FuelConsumptionUrban", "10.0" }, { "FuelConsumptionHighway", "7.5" } }, "Good", 22000))
            };
        }

        private void ProcessInParallel()
        {
            Task.WhenAll(cars).Wait();
            Parallel.ForEach(cars, task =>
            {
                Car car = task.Result;
                car.Price *= 1.1m; // Збільшення ціни на 10%
            });
        }

        private void DisplayCars()
        {
            foreach (var task in cars)
            {
                Console.WriteLine(task.Result);
            }
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            ProcessInParallel();
            DisplayCars();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

    public class Car
    {
        public string Brand { get; set; }
        public int Year { get; set; }
        public Dictionary<string, string> TechnicalSpecifications { get; set; }
        public string TechnicalCondition { get; set; }
        public decimal Price { get; set; }
        public double FuelConsumptionUrban { get; set; }
        public double FuelConsumptionHighway { get; set; }

        public Car() { }

        public Car(string brand, int year, Dictionary<string, string> technicalSpecifications, string technicalCondition, decimal price)
        {
            Brand = brand;
            Year = year;
            TechnicalSpecifications = technicalSpecifications;
            TechnicalCondition = technicalCondition;
            Price = price;

            if (TechnicalSpecifications.ContainsKey("FuelConsumptionUrban"))
                FuelConsumptionUrban = double.Parse(TechnicalSpecifications["FuelConsumptionUrban"]);

            if (TechnicalSpecifications.ContainsKey("FuelConsumptionHighway"))
                FuelConsumptionHighway = double.Parse(TechnicalSpecifications["FuelConsumptionHighway"]);
        }

        public override string ToString()
        {
            return $"{Brand} ({Year}), Price: {Price:C}, Fuel Consumption (Urban/Highway): {FuelConsumptionUrban}/{FuelConsumptionHighway} L/100km";
        }
    }
}
