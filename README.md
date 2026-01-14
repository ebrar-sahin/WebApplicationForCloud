Department of Computer Engineering, Istanbul Commerce University <br>

ABSTRACT <br> An admin web application for stock receiving, transfers, and pick/pack workflows using ASP.NET Core. 
Use JavaScript for drag-and-drop bin management, CSS for compact data grids, and server-side APIs for inventory 
transactions. Include user-friendly filters, activity logs, and role-based operations. <br>

INDEX TERMS Warehouse Inventory Web Dashboard, Database Management <br>

I. 
INTRODUCTION  <br>

This project is a modern Warehouse Inventory Management System developed based on ASP.NET Core 8.0, 
designed to overcome the clumsiness of traditional and static stock tracking methods. Named "WIMS" (Warehouse 
Inventory Management System), this application is designed not just as a repository where data is stored, but as a 
living ecosystem that accelerates business processes, minimizes user errors, and prioritizes security. The primary 
motivation of the project is to simplify complex stock entry/exit operations using modern UX (User Experience) 
techniques such as Drag-and-Drop and to implement Enterprise Software Architecture standards in the background. <br>

II. TECHNOLOGY AND INFRASTRUCTURE  <br>

The Microsoft ecosystem was preferred for the development of the project. The .NET 8.0 (MVC) framework was used 
on the server side for its high performance and security features, while Microsoft SQL Server was utilized as the 
database management system. To facilitate communication between the database and the application, Entity 
Framework Core (ORM) technology was employed to mitigate security risks associated with raw SQL queries (such 
as SQL Injection) and to overcome management challenges. The Code-First approach was adopted, modeling the 
database schema through C# models, which ensured that database changes (Migrations) were kept under version 
control. On the frontend, the Bootstrap 5 library was used for a user-friendly and responsive experience, along with 
JavaScript (AJAX) technologies for dynamic page interactions. <br>

III. SOFTWARE ARCHITECTURE AND DESIGN PATTERNS  <br>

One of the strongest aspects of the project is its sustainable and extensible code structure. To avoid "spaghetti code" 
complexity, SOLID principles were adopted, and a layered architecture was implemented. The Repository Design 
Pattern was applied in the data access layer, completely abstracting data access codes from business logic 
(Controller). This ensures that Controller classes remain unaware of how the database operates, requesting only data. 
Furthermore, the Unit of Work design pattern was integrated to guarantee Transaction Integrity in operations where 
multiple tables (Products, Orders, Logs) are updated simultaneously. This structure ensures database consistency by 
rolling back all changes if an error occurs during any part of the transaction. Object dependencies are managed via 
Dependency Injection (DI), enhancing the system's modularity. <br>
Database Design and Normalization The database schema is structured in accordance with the 3rd Normal Form 
(3NF) to prevent data redundancy and ensure consistency. The Product table, located at the center of the system, 
relates to Category, Subcontractor, Material, and Invoice tables via Foreign Keys. Beyond mere stock keeping, 
ActivityLogs and Orders tables were designed for system auditability. Every operation performed in the warehouse 
(stock changes, product deletion, etc.) is recorded bi-directionally: as a textual log record and as a mathematical order 
receipt. On the security side, ASP.NET Core Identity tables (AspNetUsers, AspNetRoles) are integrated into the 
database to provide a role-based authorization infrastructure. <br>

 <img width="703" height="383" alt="image" src="https://github.com/user-attachments/assets/128cdc8f-5662-4032-97aa-7114939d128f" /> <br>
 
 IV. 
KEY FUNCTIONAL FEATURES  <br>
Unlike classic admin panels, the application's user interface features an interactive structure. Thanks to the "Kanban" 
style stock management panel, users can update statuses by dragging and dropping products between "In Stock" and 
"Out of Stock" bins. During this process, the page does not reload (AJAX); instead, the system prompts the user with 
smart questions (e.g., "How many items were sold?") and mathematically updates the stock based on the input. 
Validation rules have been applied on both the client and server sides (Data Annotations) to prevent erroneous data 
entries. For instance, entering negative values for stock quantity or saving a product with an empty name is prevented.  
Additionally, a Global Notification System (Toastr) enhances user experience by displaying instant informational 
messages in the bottom-right corner after every user action. <br>
V.  CHALLENGES ENCOUNTERED AND SOLUTIONS  <br>
The technical obstacles encountered during the development process served as educational steps 
that provided a deeper understanding of the architecture: <br>
• Dependency Injection Management: Service errors received after creating Repository and Unit 
of Work classes were resolved by registering these classes into the IoC Container 
(AddScoped) within Program.cs. <br>
• Data Mapping Issues: While snake_case (e.g., Material_Type) was used in the database, 
PascalCase was used in C#, leading to NullReference errors. This was resolved by strictly 
matching the naming conventions in Controller and View layers with the C# models. <br>
• Migration Conflicts: Conflicts arising when EF Core attempted to create tables that were 
manually created in the database were resolved by manipulating the Up() method in the 
Migration files, ensuring database synchronization. <br>
• Authorization Logic: Timing errors during the role assignment of the Admin user were 
resolved by refactoring the application's startup (Seed) codes and bootstrapping the database 
with a clean start.<br>
VI.  
USER INTERFACE AND FUNCTIONAL SCREENS (UI/UX) <br>
The WIMS project presents its complex background business logic and robust architecture to the end-user through a 
user-friendly, modern interface compatible with "Dark Mode". To maximize User Experience (UX), classic static pages 
were avoided in favor of an interactive and dynamic structure. The system's core modules and functional screens are 
detailed below.<br>

1. Security and Access Management (Authentication) <br>
The login process is secured by the ASP.NET Core Identity infrastructure. To prevent unauthorized access, users log 
in to the system using an email and password combination. The interface features a simple and focused design, 
ensuring ease of use with a "Remember Me" option. <br>
In accordance with security protocols, non-logged-in users are prevented from accessing internal system pages via 
URL; such attempts automatically redirect the user to the login screen. Additionally, immediate feedback is provided 
via validation messages in case of failed login attempts.<br>
<img width="699" height="368" alt="image" src="https://github.com/user-attachments/assets/66ee1d82-5604-480b-a806-905a125589b0" /> <br>
2. Admin Panel (Dashboard) <br>
This is the first screen users encounter upon logging in. It supports the manager in making fast decisions by offering 
an instant summary of the warehouse status. Powered by the Unit of Work pattern and optimized queries in the 
background, this screen visualizes critical data via the DashboardViewModel: <br>
General Status: The total number of registered products in the warehouse and the total volume of transactions 
performed in the system. <br>
Critical Stock Alert: The number of products falling below a defined stock limit (e.g., 10 units) is highlighted to grab the 
manager's attention. <br>
Supplier Network: The number of active logistics and supplier companies working with the system.<br>
<img width="697" height="365" alt="image" src="https://github.com/user-attachments/assets/dc64950d-4dd3-4348-86f2-57faf67c4365" /> <br>
<img width="696" height="364" alt="image" src="https://github.com/user-attachments/assets/ec1fed18-ba5d-471d-9f99-b3dbe269d53b" /> <br>
3. Dynamic Stock Management (Kanban Board)  <br>
As the project's most innovative module, this screen replaces classic list views with an interactive Drag-and-Drop 
structure. Products are visually managed between "In Stock" (Green Area) and "Out of Stock" (Red Area) bins based 
on their status. <br>
Smart Operation: When a user drags a product between bins, the page does not reload (AJAX); instead, the system 
prompts the user with smart questions (e.g., "How many items were sold?" or "How many items were added?"). Based 
on the input, the stock quantity is calculated mathematically, and the database is updated instantly.  <br>
Instant Notifications: "Toast" notifications appearing in the bottom right corner after every operation immediately inform 
the user about the success status or any errors. <br>
4. Product Entry and Data Input <br>
The screen for creating new stock cards is designed with a Relational Data Selection (Dropdowns) method to ensure 
data integrity. Users select Supplier, Material, and Invoice information from dynamically populated lists drawn from the 
database instead of manual entry. <br>
The form structure is designed with high-contrast colors compatible with the dark theme and is supported by validation 
rules running on both the client and server sides. For example, leaving the product name empty or entering a negative 
stock value is prevented. Furthermore, the system automatically determines the product's "Available/Unavailable" 
status based on the entered stock quantity, reducing the user's workload.<br>
<img width="693" height="364" alt="image" src="https://github.com/user-attachments/assets/c595d476-0244-4eae-8975-69480bdfdae8" /> <br>
5. Activity and Stock History (Audit Logs) <br>
In line with the principle of corporate transparency, every critical operation performed in the system (Stock entry, 
product sale, update, etc.) is recorded. The "History" screen allows the manager to find answers to "Who did what and 
when?". <br>
Log records list the transaction date, the affected product, the transaction type, and a detailed description text. 
Transaction types (Stock Entry, Out of Stock, Update) are visualized with colored labels (Badges) to improve 
readability. To manage data density and maintain performance, a Pagination structure (Previous/Next Page) is 
integrated into this screen. <br>
<img width="693" height="362" alt="image" src="https://github.com/user-attachments/assets/ef6d7f40-180e-4b0f-a710-3eb68a352277" /> <br>
6. Administrative Tools (Admin Only) <br>
Developed for system sustainability, these modules are accessible only to users with the "Admin" role: 
User Management: Admins can add new personnel to the system or delete existing users. (Admins cannot delete their 
own accounts). <br>
Supplier Management: Information regarding logistics companies supplying products is managed here. The selection 
lists on the product entry screen are dynamically populated from this data.<br>
<img width="693" height="364" alt="image" src="https://github.com/user-attachments/assets/00c9b9e7-cb4f-454b-a4ba-b0c5c0bc647a" /> <br>
7. 
CONCLUSION <br>
The WIMS project is an integrated solution bringing together modern web technologies, secure coding 
standards, and corporate design patterns. It not only meets functional requirements but also establishes a 
flexible, reliable, and open-to-future-development architecture by adhering to Clean Code principles in the 
background.





