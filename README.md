This project is a partial mockup of the LEGO interview case.
The project consist of a frontend application created in React Next.js and a backend application created in .NET8.

The frontend allows workers and supervisors to manage the states of equipment, and consist of three pages:
1. Login page
2. Equipment dashboard
3. Equipment details

The login page is a mockup to simulate authentication and authorization. On this page the user can select the role it wants to access the frontend with.
- None: The user is nog logged in, and is not able to see anything on the pages
- Worker: The worker is able to see the dashboard, and see most of the information on the equipment details screen. Workers are not able to assign specific orders to equipment or see the state history of equipment
- Supervisor: The supervisor is able to do and see everything the worker can. Additionally the supervisor is able to assign a specific order in the equipment order backlog for the equipment to work on. Supervisors are also able to see the history of equipment.
![image](https://github.com/user-attachments/assets/ece3bd8e-1f34-4fc3-b8b6-9fe17b60a13c)

The equipment dashboard shows an overview of all connected equipment.
The dashboard shows the following information:
- List of all registered equipment
- For each equipment:
  - History of states (Mock)
  - Name
  - State (RED, YELLOW or GREEN)
  - Current order the machine is working on
  - Location of the equipment (Area, floor, section)
  - Whether it is operational
  - Operator responsible for the equipment
  - OOE, How efficient the machine is
 Additionally, the dashboard offers filtering based on the machine name, location and whether the machine is operational or not
![image](https://github.com/user-attachments/assets/963cdc34-5837-4dbd-9473-864a717ebd33)

The equipment details shows detailed information on a specific equipment. It shows all the information the dashboard shows, as well as additional information such as:
- Start/Stop buttons to Start/Stop the machine, only possible if the state is on RED/GREEN
- List of assigned orders
- Dropdown select to select a specific order to be started on, only visible when the machine is on RED (Only visible by supervisors)
- Change state buttons
- Show/Hide history button and the state history of the machine (Only visible by supervisors)
![image](https://github.com/user-attachments/assets/9252e192-d941-4980-b014-13b91753c45a)

     
The backend offers the following API Calls:
/Equipment/GetAll: Get all registered equipment
/Equipment/{equipmentId}: Get specific equipment information
/Equipment/{equipmentId}/UpdateState: Updates the equipment state to a desired state.
/Equipment/{equipmentId}/Start: Starts the machine from RED to GREEN (YELLOW as intermediary state)
/Equipment/{equipmentId]/Stop: Stops the machine from GREEN to RED (YELLOW as intermediary state)
/Equipment/{equipmentId}/AddOrders: Adds a list of orders to its assigned orders backlog for a specific equipment
/Equipment/{equipmentId}/AssignOperator: Assigns an operator to a specific equipment

Frontend technologies and libraries used:
Next.js
React Query
Lucide react
React Hot Toast

Backend technologies and libraries used:
.NET8
Microsoft Extensions Logging
AspNetCore Swashbuckle
NUnit
Moq
Moq.AutoMock
FluentAssertions


Starting the frontend:
- Navigate in the EquipmentManagementDashboard/equipment-management-dashboard
- Start the frontend with "npm build" followed by "npm start"
- The app is then running on http://localhost:3000/

Starting the backend:
- Navigate to EquipmentManagementPlatform
- Build the project with "dotnet build"
- Run the project with "dotnet run --project EquipmentManagementPlatform.API
- The backend should be running on https://localhost:5001/
- Swagger endpoint: https://localhost:5001/swagger/index.html
