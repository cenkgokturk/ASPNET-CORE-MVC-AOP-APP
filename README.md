# Untitled

## Description

Basic [ASP.NET](http://asp.net/) Core Model-View-Controller (MVC) app built on C# with Aspect Oriented Programming (AOP).

## Features

- User authentication
- User authorization with predefined roles and user specific permissions
- Session controls using cookies

All of the features above are implemented in AOP without using any third-party APIs or services (such as Identity).

## Pages

- UserLogin works by asking user for mail and password. If an entry is found with the given mail address and the password matches the one in the database, user is redirected to the profile page. Otherwise, an error appears.
- UserRegister asks users for username, mail and password
- UserProfile is a restricted page. When user clicks on profile menu on the home page, isAuthenticated Aspect checks whether the current user logged in in the past 20 minutes. If there is an active cookie, user is admitted. Otherwise, an exception is thrown out.

## Necessary Programs/Libraries

Program was written in C#, therefore, a special environment for the aforementioned language is needed (Visual Studio 2019 was used for the project). Net Core 5.0 was used for the application and no prior version was tested. If you have an older version, libraries and APIs might result in errors. Finally, in order to store user data including first and last name, email address and password, a local SQL server is needed. SQL Server Management Studio (Version 18.8) was used to initialize a database. For the Aspect side, PostSharp was used. 

## How to Install

1. Download the app from GitHub Repository 
2. Open it in Visual Studio 
3. Open Microsoft SQL Server Managment Studio
4. Connect to your local SQL Server
5. Create a new database named as "AccountDB"
6. Execute the given queries to create necessary tables

    ```java
    CREATE TABLE AccountRoles (
    	Roleid INT IDENTITY PRIMARY KEY,
    	Rolename nvarchar(100) NOT NULL,
    	Roleallow nvarchar(100), 
    	Roledeny nvarchar(100)
    );

    CREATE TABLE AccountInfo(
    	UserID INT IDENTITY PRIMARY KEY,
    	Username nvarchar(100),
    	Usermail nvarchar(100) NOT NULL, 
    	Userpassword nvarchar(100) NOT NULL,
    	Userallow nvarchar(100),
    	Userdeny nvarchar(100)
    );

    CREATE TABLE UserRoles(
    	ID INT IDENTITY PRIMARY KEY,
    	UserID INT FOREIGN KEY REFERENCES AccountInfo(UserID),
    	Roleid INT FOREIGN KEY REFERENCES AccountRoles(Roleid)
    );

    CREATE TABLE AccountSessions (
    	ID INT IDENTITY PRIMARY KEY,
    	Usermail nvarchar(100),
    	LoginDate nvarchar(100),
    	IsLoggedIn INT
    );
    ```

7. Go back to Visual Studio and open Server Explorer (can be found in view)
8. Select Add Connection after right clicking on Data Connections

    Data Source: Microsoft SQL Server (SqlClient)

    Server Name: Your local SQL Server's name 

9. Select AccountDB database, created in step 5
10. Under Data Connections, select properties after right clicking the server added in the previous step
11. Copy connection String and update the connection variables in UserRegistrationController, UserLoginController and UserProfileControlle
12. Go to Tools → NuGet Package Manager → Package Manager Console
13. Search & Install Postgres (necessary for AOP)
14. Finally, to run the application, click Ctrl+F5 (Start without debugging)
15. Program will be opened in a new tab in your default browser