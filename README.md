# Pierre's Flavor & Treat App

#### By [Will W.](https://wjwat.com/)

#### Allow a user on Pierre's site to create Treats & Flavors, but only if they've created an account.

## :computer: Technologies Used

* C#
* .NET 5.0
* ASP.NET Core
* Razor
* Entity Framework Core
* MySQL
* dotnet script REPL
* HTML
* CSS | [YACCK](https://github.com/sphars/yacck)
* JQuery

## :memo: Description

Use Authentication with Identity to limit the data a user of Pierre's app can
access, create, update, and destroy.

## :gear: Setup/Installation & Usage Instructions

- [Install the MySQL Community Server & MySQL Workbench](https://dev.mysql.com/downloads/mysql/)
- [Install the .NET 5 SDK](https://www.learnhowtoprogram.com/c-and-net/getting-started-with-c/installing-c-and-net)
- Install the [dotnet-ef](https://docs.microsoft.com/en-us/ef/core/cli/dotnet) tool with this command: `dotnet tool install --global dotnet-ef`
- [Clone this
  repository](https://docs.github.com/en/repositories/creating-and-managing-repositories/cloning-a-repository)
  to your device
- Create `appsettings.json` in the top level of this repo
  - Copy the contents of the code below into this file. *Make sure to change the password to the password you used to setup your MySQL server*
  ```JSON
  {
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Port=3306;database=will_watkins;uid=root;pwd=<PASSWORD>;"
    }
  }
  ```
- [Using your
  terminal](https://www.freecodecamp.org/news/how-you-can-be-more-productive-right-now-using-bash-29a976fb1ab4/)
  navigate to the directory where you have cloned this repo.
- Run `dotnet build Bakery` in the top level directory of this repo.
- Once the project has been built run `dotnet ef database update --project Bakery` to create the database necessary to run the app.
- Run `dotnet run --project Bakery` to get the program running, and the site hosted locally.
- Visit `http://localhost:5000` and register an account
- Log in with your newly created account and create Treats & Flavors

## :page_facing_up: Notes

- I've done the best with my limited skills and the time I've allotted for this project to code defensively. Some of the choices made in order to limit access to routes are probably questionable.
- Enable more complex types of information/functionality
    - Orders
    - Pricing for treats
- Currently only users that have created a flavor/treat are allowed to Edit/Delete them. Outside of directly modifying our database there is no way to remove those items. It would be nice to have roles where an admin is able to edit all pieces of information currently being tracked, and users are only allowed to edit their own pieces of information.
- I'd like to have a firmer understanding of how everything works, and how all the pieces work together. At the moment it feels a bit duct taped together.
- Error messages. We silently fail far too often. If a user tries to access a route they aren't supposed to we should tell them that as well as doing a redirect. Maybe an interstitial error page that redirects after X seconds to give the user a change to read why they're being redirected.
- There is a lot of duplicate code in both the Treat & Flavor controller, and I'm unsure of how much of it should be DRY'ed up, and what the best path forward on doing so is.

## :lady_beetle: Known Bugs

* If any are found please feel free to open an issue or email me at wjwat at
  onionslice dot org

## :warning: License

[MIT License](https://opensource.org/licenses/MIT)

Copyright (c) 2022 Will W. ┬┴┬┴┤ ͠° ͟ʖ ͡°)
