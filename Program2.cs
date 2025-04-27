using System;
using System.Collections.Generic;
using System.Linq;

public enum IncidentSeverity { Minor, Moderate, Severe }

public class Incident
{
    public string Type { get; }
    public string Location { get; }
    public DateTime ReportedTime { get; }
    public IncidentSeverity Severity { get; }

    public Incident(string type, string location, IncidentSeverity severity)
    {
        Type = type;
        Location = location;
        ReportedTime = DateTime.Now;
        Severity = severity;
    }

    public override string ToString() =>
        $"{Type.ToUpper()} at {Location} (Severity: {Severity})";
}

public abstract class EmergencyUnit
{
    public string Name { get; protected set; }
    public int Speed { get; protected set; }
    public bool IsAvailable { get; set; } = true;
    public int SuccessfulResponses { get; private set; }

    public abstract bool CanHandle(string incidentType);
    public abstract void RespondToIncident(Incident incident);

    public virtual void CompleteResponse()
    {
        IsAvailable = true;
        SuccessfulResponses++;
    }

    public virtual string GetUnitStatus() =>
        $"{Name} - Speed: {Speed} mph | Available: {IsAvailable} | Missions: {SuccessfulResponses}";
}

public class Police : EmergencyUnit
{
    public int OfficersCount { get; }

    public Police(string name, int speed, int officersCount = 2)
    {
        Name = name;
        Speed = speed;
        OfficersCount = officersCount;
    }

    public override bool CanHandle(string incidentType) =>
        incidentType.Equals("Crime", StringComparison.OrdinalIgnoreCase);

    public override void RespondToIncident(Incident incident)
    {
        IsAvailable = false;
        Console.WriteLine($"{Name} ({OfficersCount} officers) responding to {incident.Location} at {Speed} mph.");
        Console.WriteLine($"Handling {incident.Severity} crime scene...");
    }

    public override string GetUnitStatus() =>
        base.GetUnitStatus() + $" | Officers: {OfficersCount}";
}

public class Firefighter : EmergencyUnit
{
    public bool HasLadderTruck { get; }

    public Firefighter(string name, int speed, bool hasLadderTruck = false)
    {
        Name = name;
        Speed = speed;
        HasLadderTruck = hasLadderTruck;
    }

    public override bool CanHandle(string incidentType) =>
        incidentType.Equals("Fire", StringComparison.OrdinalIgnoreCase);

    public override void RespondToIncident(Incident incident)
    {
        IsAvailable = false;
        string equipment = HasLadderTruck ? "with ladder truck" : "with standard equipment";
        Console.WriteLine($"{Name} {equipment} responding to {incident.Location} at {Speed} mph.");
        Console.WriteLine($"Fighting {incident.Severity} fire...");
    }

    public override string GetUnitStatus() =>
        base.GetUnitStatus() + $" | Ladder Truck: {HasLadderTruck}";
}

public class Ambulance : EmergencyUnit
{
    public bool IsMobileICU { get; }

    public Ambulance(string name, int speed, bool isMobileICU = false)
    {
        Name = name;
        Speed = speed;
        IsMobileICU = isMobileICU;
    }

    public override bool CanHandle(string incidentType) =>
        incidentType.Equals("Medical", StringComparison.OrdinalIgnoreCase);

    public override void RespondToIncident(Incident incident)
    {
        IsAvailable = false;
        string type = IsMobileICU ? "Mobile ICU" : "Standard";
        Console.WriteLine($"{type} ambulance {Name} responding to {incident.Location} at {Speed} mph.");
        Console.WriteLine($"Treating {incident.Severity} medical emergency...");
    }

    public override string GetUnitStatus() =>
        base.GetUnitStatus() + $" | Mobile ICU: {IsMobileICU}";
}

public class EmergencyDispatchSystem
{
    private List<EmergencyUnit> units = new List<EmergencyUnit>();
    private int score = 0;
    private int totalIncidents = 0;

    public void ConfigureSystem()
    {
        Console.WriteLine("-- Emergency Dispatch System Configuration --");
        AddUnits("Police", units);
        AddUnits("Firefighter", units);
        AddUnits("Ambulance", units);
    }

    public void RunSimulation(int turns = 5)
    {
        for (int i = 0; i < turns; i++)
        {
            Console.WriteLine($"\n--- Turn {i + 1} ---");
            DisplayAvailableUnits();

            var incident = GenerateIncident();
            Console.WriteLine($"\nNew Incident: {incident}");
            totalIncidents++;

            var responder = FindResponder(incident);
            ProcessResponse(responder, incident);
        }

        Console.WriteLine($"\nSimulation Complete!\nFinal Score: {score}/{totalIncidents * 10}");
        DisplayUnitStatistics();
    }

    private void AddUnits(string unitType, List<EmergencyUnit> units)
    {
        Console.Write($"Number of {unitType} units: ");
        if (!int.TryParse(Console.ReadLine(), out int count) || count < 0)
        {
            Console.WriteLine("Invalid input. Defaulting to 0.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Console.Write($"{unitType} Unit {i + 1} Name: ");
            string name = Console.ReadLine().Trim();

            Console.Write($"Speed (mph): ");
            if (!int.TryParse(Console.ReadLine(), out int speed) || speed <= 0)
            {
                Console.WriteLine("Invalid speed. Using default 50 mph.");
                speed = 50;
            }

            EmergencyUnit unit = unitType.ToLower() switch
            {
                "police" => new Police(name, speed, new Random().Next(1, 5)),
                "firefighter" => new Firefighter(name, speed, new Random().Next(0, 2) == 1),
                "ambulance" => new Ambulance(name, speed, new Random().Next(0, 2) == 1),
                _ => throw new ArgumentException("Invalid unit type")
            };

            units.Add(unit);
        }
    }

    private Incident GenerateIncident()
    {
        string[] types = { "Crime", "Fire", "Medical" };
        string[] locations = { "Downtown", "Suburbs", "Industrial Zone", "Residential Area", "Highway" };
        var severities = Enum.GetValues(typeof(IncidentSeverity)).Cast<IncidentSeverity>().ToArray();

        string type = types[new Random().Next(types.Length)];
        string location = locations[new Random().Next(locations.Length)];
        var severity = severities[new Random().Next(severities.Length)];

        return new Incident(type, location, severity);
    }

    private EmergencyUnit FindResponder(Incident incident)
    {
        var availableUnits = units.Where(u => u.CanHandle(incident.Type) && u.IsAvailable);
        return availableUnits.OrderByDescending(u => u.Speed).FirstOrDefault();
    }

    private void ProcessResponse(EmergencyUnit responder, Incident incident)
    {
        if (responder != null)
        {
            responder.RespondToIncident(incident);
            score += 10;
            Console.WriteLine($"+10 points\nCurrent Score: {score}");

            // Simulate response completion after delay
            System.Threading.Thread.Sleep(1000);
            responder.CompleteResponse();
        }
        else
        {
            score -= 5;
            Console.WriteLine($"No available units! -5 points\nCurrent Score: {score}");
        }
    }

    private void DisplayAvailableUnits()
    {
        Console.WriteLine("\nAvailable Units:");
        foreach (var unit in units.Where(u => u.IsAvailable))
        {
            Console.WriteLine($"- {unit.GetUnitStatus()}");
        }
    }

    private void DisplayUnitStatistics()
    {
        Console.WriteLine("\nUnit Performance Report:");
        foreach (var unit in units)
        {
            Console.WriteLine(unit.GetUnitStatus());
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var system = new EmergencyDispatchSystem();
        system.ConfigureSystem();
        system.RunSimulation();

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
