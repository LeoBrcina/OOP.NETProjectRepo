FootieProject

FootieProject is an object-oriented programming project developed in .NET, displaying football statistics from the 2018 Men's World Cup and 2019 Women's World Cup using data retrieved from the World Cup API. The project consists of three main components:

Windows Forms Application - for displaying and interacting with data in a user-friendly interface.

Windows Presentation Foundation (WPF) Application - providing a modern, responsive UI.

Data Layer (Class Library) - handling all data manipulation and interaction with the API.

Table of Contents
Project Description
Features
Installation
Usage
Technologies
Project Structure
License

Project Description
The FootieProject allows users to retrieve and visualize football statistics from the 2018 Men's and 2019 Women's World Cup. Users can set preferences, select favorite teams and players, view match details, and display rankings. The project consists of two client applications (Windows Forms and WPF) and a shared data layer that processes and stores the retrieved data.

Features
Windows Forms Application:
Favorite National Team & Players: Allows users to select their favorite team and three favorite players.
Player Stats: Displays player details such as name, shirt number, position, captain status, and favorite status.
Drag & Drop: Users can drag and drop players to mark them as favorite or non-favorite.
Player Pictures: Users can set custom images for players.
Ranking System: Create player and match rankings based on various statistics (e.g., goals, yellow cards).
Printing: Generate and print rankings to PDF.
Language and Preference Settings: Support for Croatian and English localization, and user-defined championship preference (men's or women's).

WPF Application:
Responsive UI: Fully responsive interface for viewing teams and matches.
Match Details: Displays match results between selected teams, with basic information such as goals and line-ups.
Animations: Visual transitions and animations for navigating between different views.
Dynamic Starting Line-up: Display team line-ups on a football field background, based on player positions.
Player Overview: View detailed information about selected players from the starting line-up.

Data Layer:
API Interaction: Fetch data from the World Cup API and handle JSON parsing.
Data Storage: Save and retrieve user preferences, favorite teams, and players from text files.
Error Handling: Ensure robust error handling, including API errors and exceptions.

Installation

Clone the repository:

bash
git clone https://github.com/yourusername/FootieProject.git

Open the solution file (FootieProject.sln) in Visual Studio.

Restore NuGet packages and build the solution:

In Visual Studio, right-click the solution in Solution Explorer and select Restore NuGet Packages.
Build the solution by pressing F6 or selecting Build > Build Solution from the menu.

Run the project:

You can run the Windows Forms or WPF applications directly from Visual Studio.

Usage
Windows Forms Application:

Upon launching, set your preferred championship (men's or women's) and language.
Select your favorite national team from the dropdown.
Choose your favorite players and view their stats.
View ranking lists of players and matches.
Print the rankings to a PDF file.

WPF Application:

Set display mode and language during the initial setup.
Select national teams and view match results.
Explore team and player details through the animated user interface.

Technologies

C#
.NET Framework
Windows Forms (WinForms)
Windows Presentation Foundation (WPF)
REST API Interaction via HttpClient
JSON Parsing
Drag and Drop functionality in WinForms
Localization and Globalization (Croatian and English)

Project Structure
bash

FootieProject/
│
├── DAO/                  # Data Access Object for handling data operations
├── FootieForms/           # Windows Forms project
├── FootieWPF/             # WPF project
└── FootieProject.sln      # Visual Studio Solution file

License
This project is for educational purposes as part of the Object-Oriented Programming course at Visoko Učilište Algebra. No formal license is applied.
