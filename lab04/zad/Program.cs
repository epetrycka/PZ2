using System;
using System.IO;
using System.Globalization;

using lab4;

class Program{
    public static void Main(string[] args){
        var employee_territories = 
            new Loader<Employee_territories>()
            .LoadData("data/employee_territories.csv",
            x => new Employee_territories(x[0], x[1]));

        var employees = 
            new Loader<Employees>()
            .LoadData("data/employees.csv", 
            x => new Employees(x[0], x[1], x[2], x[3], x[4], x[5],
                                x[6], x[7], x[8], x[9], x[10], x[11], 
                                x[12], x[13], x[14], x[15], x[16], x[17]));
        
        var orders_details = 
            new Loader<Orders_details>()
            .LoadData("data/Orders_details.csv",
            x => new Orders_details(x[0], x[1], x[2], x[3], x[4]));

        var orders = 
            new Loader<Orders>()
            .LoadData("data/Orders.csv",
            x => new Orders(x[0], x[1], x[2], x[3], x[4], x[5], x[6], 
                            x[7], x[8], x[9], x[10], x[11], x[12], x[13]));

        var regions = 
            new Loader<Regions>()
            .LoadData("data/regions.csv",
            x => new Regions(x[0], x[1]));

        var territories = 
            new Loader<Territories>()
            .LoadData("data/territories.csv",
            x => new Territories(x[0], x[1], x[2]));

        Console.WriteLine("Nazwiska wszystkich pracowników: ");
        var employees_lastnames = from employee in employees
                                  select employee.lastname;
        foreach (var employee in employees_lastnames){
            Console.WriteLine(employee);
        }

        Console.WriteLine("\n Nazwiska pracowników, nazwa regionu, terytorium gdzie pracuje: ");
        var employees_workregions = from employee in employees
                                    join employee_territory in employee_territories on employee.employeeid equals employee_territory.employeeid
                                    join territory in territories on employee_territory.territoryid equals territory.territoryid
                                    select new {
                                        lastName = employee.lastname,
                                        territory = employee_territory.territoryid,
                                        region = territory.regionid,
                                    };
        foreach (var employee in employees_workregions){
            Console.WriteLine($"{employee.lastName}, {employee.territory}, {employee.region}");
        }

        Console.WriteLine("\nNazwy regionów oraz nazwiska pracowników, którzy pracują w tych regionach");
        var regions_employees = from employee in employees
                                join employee_territory in employee_territories on employee.employeeid equals employee_territory.employeeid
                                join territory in territories on employee_territory.territoryid equals territory.territoryid
                                group employee.lastname by territory.regionid into regionGroup
                                select new
                                {
                                    RegionID = regionGroup.Key,
                                    Employees = regionGroup.Distinct()
                                };
        foreach (var region in regions_employees)
        {
            Console.Write($"\n{region.RegionID}: ");
            foreach (var employee in region.Employees)
            {
                Console.Write($"{employee}, ");
            }
        }

        Console.WriteLine("\n\nNazwy regionów oraz liczba pracowników, którzy pracują w tych regionach");
        var regions_count = from employee in employees
                            join employee_territory in employee_territories on employee.employeeid equals employee_territory.employeeid
                            join territory in territories on employee_territory.territoryid equals territory.territoryid
                            group employee.lastname by territory.regionid into regionGroup
                            select new
                            {
                                RegionID = regionGroup.Key,
                                Employees = regionGroup.Count()
                            };
        foreach (var region in regions_count)
        {
            Console.Write($"\n{region.RegionID}: {region.Employees}");
        }

        Console.WriteLine($"\n\nDla każdego pracownika liczba dokonanych przez niego zamówień, średnia wartość zamówienia oraz maksymalna wartość zamówienia.");
        var orders_employee =   from employee in employees
                                join order in orders on employee.employeeid equals order.employeeid
                                join details in orders_details on order.orderid equals details.orderid
                                group details by employee.employeeid into order_detail
                                select new
                                {
                                    Employee = order_detail.Key,
                                    OrderCount = order_detail.Count(),
                                    AverageOrderValue = order_detail.Average(order => decimal.TryParse(order.unitprice, NumberStyles.Any, CultureInfo.InvariantCulture, out var value) ? value : 0),
                                    MaxOrderValue = order_detail.Max(order => decimal.TryParse(order.unitprice, NumberStyles.Any, CultureInfo.InvariantCulture, out var value) ? value : 0)
                                };
        foreach (var value in orders_employee)
        {
            Console.WriteLine($"Pracownik {value.Employee}:");
            Console.WriteLine($"  Liczba zamówień: {value.OrderCount}");
            Console.WriteLine($"  Średnia wartość zamówienia: {value.AverageOrderValue:C}");
            Console.WriteLine($"  Maksymalna wartość zamówienia: {value.MaxOrderValue:C}");
        }
    }
}