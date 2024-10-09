# GameHub Management System
### **Table of Contents**
1. [Project Overview](#project-overview)
2. [Features](#features)
3. [Technologies Used](#technologies-used)
4. [Setup and Installation](#setup-and-installation)
5. [Contributing](#contributing)
6. [License](#license)

---

## Project Overview

The **GameHub Management System** is a C# Windows Forms application, a comprehensive digital distribution platform and game server management software. It provides administrators with an interface to add, edit and delete games from the system and offers users a platform to download and view game details. The system is built to be a centralized hub where multiple games can be managed, with features for categorization, user management and detailed tracking of download history.

This a university-level project and a practical exercise in database management, form-based user interfaces. It is built with user-friendly controls and provides a smooth UI experience for both users and administrators.

---
<details>
   <summary><h2>Features</h2></summary>

### **Admin Panel**
- **Add, Update and Delete Games**: Administrators can manage games in the system database by CRUD operations.
- **Add and Delete Admin**: Administrator can add new admin to the system to operate and can remove the ex-admins.
- **User Database**: Admins can see the provided user informations from the database.
- **View Download History**: Admins can track user downloads, including timestamps and game details. [under development]
- **Game Categories**: Games are sorted into categories for easier management and user browsing. And if necessary admins can add new category, update category and even delete the irrelevent one.

### **User Panel**
- **Game List & Details**: Users can browse through the list of games and view detailed descriptions, including the game genre, release date and pricing.
- **Dynamic Loading**: The interface dynamically loads content from the database, enhancing scalability and performance.

### **Data Handling**
- **Database Connectivity**: SQL server management system is used to ensure data persistence for game details, users and admin actions.
- **CRUD Operations**: Built-in functionality for Create, Read, Update, Delete operations on the database tables.

### **User Account Management**
- **Sign Up & Login**: Users can create accounts and log in to manage their downloads and access exclusive games.
- **Admin Login**: A separate log in form is added for the admins to manage the system database.
---
</details>

<details>
   <summary><h2>Technologies Used</h2></summary>

- **Programming Language**: C# (.NET Framework v4.7.2)
- **Database**: SQL Server management system
- **UI Components**: Krypton.Toolkit, Bunifu.UI, CuoreUI for custom controls and enhanced UI experience
- **IDE**: Visual Studio
- **Version Control**: GitHub

---
</details>

## Setup and Installation

### **Prerequisites**
- **.NET Framework 4.7.2**
- **Microsoft SQL Server**: Make sure SQL local server is connected and all the queries from the Query directory was executed properly.
- **Visual Studio IDE**

### **Steps to Setup**
1. Clone the repository:
   ```bash
   git clone https://github.com/navinxqz/GameHub.git
2. Open the solution file in Visual Studio.
3. Configure the SQL database connection in the `DBconnect.cs` file:
   ```bash
   public static readonly string cs = @"Data Source=your_server_name;Initial Catalog=GameServerDB;Integrated Security=True";
4. Build the solution and run the project.

---

## Contributing

### **Bug Reports and Feature Requests**
If you encounter any bugs or have feature requests, feel free to create an issue in the GitHub repository. Contributions are welcome!

<h2 align="left">Contributors</h2>

####
<table>
   <div style = "display: flex; align-item: flex-start; align: center">
      <table align= "center">
         <tr>
            <td align = "center" width = "200"><img src= "https://avatars.githubusercontent.com/u/169520102?v=4" width="auto" height= "auto"/></td>
            <td align = "center" width = "200"><img src= "https://avatars.githubusercontent.com/u/170220890?v=4" width="auto" height= "auto"/></td>
            <td align = "center" width = "200"><img src= "https://instagram.fdac139-1.fna.fbcdn.net/v/t51.2885-19/458258248_501968952591249_368144769352137847_n.jpg?_nc_ht=instagram.fdac139-1.fna.fbcdn.net&_nc_cat=109&_nc_ohc=BTg9FuVehW8Q7kNvgEaCgLk&_nc_gid=ef62e151457f400ab4a882d4fe716df5&edm=AP4sbd4BAAAA&ccb=7-5&oh=00_AYAO2ShgUVZZipyCdf2RY7PawB7BHTdllpuQ3q29GyXO8Q&oe=670CAF3A&_nc_sid=7a9f4b" width="auto" height= "auto"/></td>
            <td align = "center" width = "200"><img src= "https://scontent.fdac139-1.fna.fbcdn.net/v/t39.30808-6/457326681_1413995893282857_8881534873357543322_n.jpg?_nc_cat=105&ccb=1-7&_nc_sid=6ee11a&_nc_eui2=AeHVThPSNPr73LehJn4JNDVaMCAncwM5RlIwICdzAzlGUh88GifBixUU87q93wsOkVywWPtFxV-_FDCJAgeD2KuU&_nc_ohc=ZwTiQ7aFVmEQ7kNvgHkDDWS&_nc_ht=scontent.fdac139-1.fna&_nc_gid=AiM37e9q6_gkjiKrLdISrV1&oh=00_AYDcVnPiptZ89JKNO2pEPDtrmO0cU8wBgJDzP2d-b26seg&oe=670CA5EE" width="auto" height= "auto"/></td>
         </tr><tr>
            <td align = "center" width = "200"><a href="https://github.com/navinxqz" target="_blank">Navin, Md Nawshin</td>
            <td align = "center" width = "200"><a href="https://github.com/SADMANTANZIM" target="_blank">Sadman Shabab</td>
            <td align = "center" width = "200"><a href="https://www.instagram.com/podder_durjoy" target="_blank" alt="durjoy insta acc">Durjoy Podder</td>
            <td align = "center" width = "200"><a href="https://www.facebook.com/siftialmahmud.sifti" alt="Sifti fb acc" target="_blank">Sifti Al Mahmud</td>
         </tr></table>

---
## License

This project is licensed under the MIT License.


