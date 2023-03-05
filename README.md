# Inventory Control Webpage

### Vide Demo: <URL>
### Description:

Inventory Control WebApp developed in C# using .NET 7 in Linux Mint using Visual Studio Code. Based on an ASP .NET Core WebApp with Individual Authentication and using SQLite Database with Entity Framework Core, Razor Pages, and MigraDocCore for Pdf output. This web application allows the user to access an inventory control system using a username and password, then allows the user to enter information like items, customers, vendors, and sales or purchases orders; each one of this entries can be edited, viewed in details or deleted, according to the user's needs. A history page shows any order that has already been completed, and a company page lets the user set their company information in order to allows the system to output pdf orders.

#### Web application:

This application was developed using one of the templates available in the .NET Core Framework by executing the following command: `dotnet new webapp -o Inventory --auth Individual`. This creates different folders and files as explained in the documentation, and as stated used individual authentication using a SQLite Database. The *Program.cs* file configures and builds the web application, connects to the database, configures the authentication provider and connects with the rest of the code. The *Inventory.csproj* file contains all the referenced libraries used in the application which are added with the `dotnet add package` command. The most used libraries are EntityFrameworkCore, WebCodeGeneration, AspNetCoreIdentity, and MigradocCore. The *Areas* folder contains all the webpages and controllers that are responsible for the authentication process, like the login page, which were modified only to simplify the user interface.

#### Database configuration:

As stated before, Sqlite is used and configured using Entity Framework Core. The first command already configured the database for authentication purposes, and we typed subsequent commands to configure the same database for our inventory data. First, in the *Models* folder, we created all classes needed for the inventory data like items, customers, and vendors information. Besides that other classes were created for purchases and sales and the corresponding relationships between these and the items and the associated customers or vendors. Finally, we created another class for the relevant data of the user of the inventory application.

After creating the models classes, we scaffolded the pages and associated controllers using the `dotnet aspnet-codegenerator` command, the we used Entity Framework to drop the database, add the necessary migrations and rebuild the database. In the *Data* folder, the *ApplicationDbContext* class contains the definition of each scaffolded models class. We only scaffolded the item, customer, vendor, purchaseorder, salesorders and companyinfo classes. Therefore, we modified the *ApplicationDbContext* class to allow the many to many relationships between the purchaseorder and saleorders classes and the corresponding item and customer or vendor classes using the purchaseitems and saleitems classes. This allows us to relate a given purchase or sale order with a corresponding vendor or customer and as many items as necessary.

#### Pages and Controllers:

In the *Pages* folder, we found the corresponding pages (cshtml files) and controllers (cshtml.cs files) as well as the corresponding templates and imports files. We modified the index and template files to fit our application. Then after scaffolding the model classes we proceeded to modify the pages and controllers to fit our application requirements. Each scaffolded model class has a folder in *Pages* containing five different options: index, create, edit, details, and delete. We modified these files as necessary. In the *Items*, *Customers* and *Vendors* folders, we did not modify any files because they were already accomplishig the necessary requirements.

In the *SaleOrders* and *PurchaseOrders* folders, we modified the files to achieve the many to many relationships already stated, to provide the inventory requirements like updating the items after each purchase or sale, and to perform the added functionality of outputting the inventory orders into pdf files.

The *Index* pages and controllers were modified to include only the created orders and to erase any existing pdf file created before. Then we added a *NamePageModel* class to present the items, customers and vendors in selected list formats. The *Create* pages and controllers were modified to include said selected list formats, and to provide different pages depending on the number of items requested. Here the controller ensured the creation of the many to many relationship between each order and each item requested and the corresponding customer or vendor. After that the *Delete* and *Details* pages and controllers were modified to take into account the many to many relationship. Following that, the *Edit* pages and controllers were added with code that takes into account the many to many relationship and if the status were changed to Modified the corresponding order was moved to the History page, meaning it was archived. Finally, the *Print* pages and controllers were added to provide the option to print the order with the help of the PDF classes later described.

The *Company* folder files were scaffolded and modified to allow the user to add and edit their information as it should appear in each outputted pdf order. Only the index, edit and setinfo pages were left and modified to be similar to the items, customers and vendors pages.

The *History* folder was added with only the index page available to provide a view of all the already completed orders.

Once we ended modifying all controllers we added the `[Authorize]` statement to each one of them to ensure only signed users can access the corresponding pages.

#### Pdf Output:

Once added and referenced the MigraDocCore packages, the *PurchaseOrderPdf* and *SalesOrderPdf* classes were added in the *PDF* folder. These create a new pdf document with a unique filename, and the addecuated format for the corresponding order. The documentation and examples given by the developers of MigraDocCore were used to set the style and format of the document and to add all the information provided by the database in the form of a table.