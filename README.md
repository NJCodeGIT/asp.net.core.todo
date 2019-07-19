# asp.net.core.todo
This is a basic asp.net core boilerplate to-do application.

### Technologies
  - **Asp.net Core 2.1** -  is a cross-platform, high-performance, open-source framework for building modern, cloud-based, Internet-connected applications. 
  - **Entity Framework (EF) Core** - is a lightweight, extensible, open source and cross-platform version of the popular Entity Framework data access technology.
  - **Angular** - is a platform for building mobile and desktop web applications.
  - **Bootstrap** - is an open source toolkit for developing with HTML, CSS, and JS.
  - **AutoMapper** - is an object-object mapper. Object-object mapping works by transforming an input object of one type into an output object of a different type.

### Feature
  - Multi way authentication which includes **identity login** and **social logins like** **facebook, twitter and google**.
  - Code-First approach for database.
  - Integrated with Angular (just a placeholder).
  - To-Do functionality without much UX or Styling and **implemented fully on Razor engine**.
  - Web Api implementation (try *https://localhost:xxxx/api/menu*  on runtime)

### Next action plans
- Include role based authorization.
- Include to-do feature on top of angular placeholder

### Steps To Set-Up Source Code

#### cmd:
```shell
git clone https://github.com/NJCodeGIT/asp.net.core.todo
cd .\asp.net.core.todo\
```

- Open miniapp.sln in **Visual Studio 2017 or greater**

- Right click on solution and click on Restore NuGet Packages

#### cmd:
```shell
  cd .\miniapp\
  npm install
```
  
- Build the solution in VS and make sure it complies successfully.

#### cmd:
```shell
  cd ..
  cd .\miniapp.EntityFrameworkCore\
```
  
  
**Note**: Open .\asp.net.core.todo\miniapp\config.json file and apply the changes if you need to change the server and database name.
  
#### cmd:
```shell
  dotnet ef database update -s ..\miniapp\
```
  
Now a database (default name *miniappDB*) will get create on your db server.
In config.json file, you can also change the **AppId** and **AppSecret** for the respective social logins.

##### Setup login providers required by your application
- [Facebook](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/facebook-logins?view=aspnetcore-2.2 "Facebook") instructions
- [Twitter](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/twitter-logins?view=aspnetcore-2.2 "Twitter") instructions
- [Google](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-2.2 "Google") instructions

**[Asp.net Core Identity](https://docs.microsoft.com/en-us/aspnet/identity/overview/getting-started/introduction-to-aspnet-identity "Asp.net Core Identity")**: The default user name and password used for Identity user is `niju.mn@live.com` and `P@ssw0rd!`.


