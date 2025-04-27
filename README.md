# Emergency Dispatch System
Author:Kalkidan Anberbir
##  Project Description
This is a simple C# console-based simulation of an **Emergency Dispatch System**.  
Users can configure emergency units (Police, Firefighters, Ambulances) and then simulate a series of random emergency incidents.  
Units respond based on their ability and availability, with points awarded for successful responses.

---

#  Report

##  Applied OOP Concepts

- **Encapsulation:**  
  Each class manages its own data and behavior internally (e.g., `Incident`, `EmergencyUnit` and its children).
  
- **Inheritance:**  
  `Police`, `Firefighter`, and `Ambulance` inherit from the abstract class `EmergencyUnit`.

- **Polymorphism:**  
  The method `RespondToIncident()` behaves differently depending on the type of `EmergencyUnit`.

- **Abstraction:**  
  `EmergencyUnit` is an abstract class that provides a common interface for different emergency services.

---

## Class Diagram (Text-Based)
+-------------------+ | Incident | +-------------------+ | - Type | | - Location | | - ReportedTime | | - Severity | +-------------------+ | + ToString() | +-------------------+
+----------------------------+ | EmergencyUnit (abstract) | +----------------------------+ | - Name | | - Speed | | - IsAvailable | | - SuccessfulResponses | +----------------------------+ | + CanHandle() (abstract) | | + RespondToIncident() (abstract) | | + CompleteResponse() | | + GetUnitStatus() | +----------------------------+ ▲ ▲ ▲ │ │ │ +------+ +---------+----------+ |Police| |Firefighter|Ambulance| +------+ +-----------+---------+ | OfficersCount | HasLadderTruck | IsMobileICU | +---------------+----------------+------------+ | override CanHandle() | | override RespondToIncident() | | override GetUnitStatus() | +----------------------------------+

+-------------------------+ | EmergencyDispatchSystem | +-------------------------+ | - units: List<EmergencyUnit> | | - score | | - totalIncidents | +-------------------------+ | + ConfigureSystem() | | + RunSimulation() | | + AddUnits() | | + GenerateIncident() | | + FindResponder() | | + ProcessResponse() | | + DisplayAvailableUnits()| | + DisplayUnitStatistics()| +-------------------------+

+-----------+ | Program | +-----------+ | + Main() | +-----------+ 

---

##  Lessons Learned / Challenges Faced

- **Randomness Management:**  
  It was important to manage random number generation properly, or units and incidents would not vary correctly.

- **Abstraction Balance:**  
  Choosing what to include in the `EmergencyUnit` base class vs. what to put in the derived classes (e.g., Police, Firefighter) required careful planning.

- **Extensibility Consideration:**  
  Building the structure in a way that allows adding new types of units (e.g., Rescue Team) in the future was a good design practice.

- **User Input Validation:**  
  Handling invalid user input (non-integer values, negative speeds) was an important detail to ensure program stability.

---

 End of Report
