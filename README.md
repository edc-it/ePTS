# Enhanced Performance Tracking System (ePTS)

The Enhanced Performance Tracking System (ePTS) is a comprehensive, user-friendly web application designed to empower schools with efficient collection, management, and analysis of aggregate data on standardized assessments for grades 1 through 4.

## Features

- **Device Compatibility:** Designed with a mobile-first approach, ePTS is responsive across devices:
  - **Phone:** Simplified layout optimized for mobile devices, focusing on essential features.
  - **Tablet:** Exploits the larger screen size to display additional elements without sacrificing usability.
  - **Laptop:** Offers a full-featured experience tailored for bigger displays.
  
  Regardless of the device, users access all functionalities seamlessly, ensuring consistent performance tracking.
  
- **Integration:** Enhances the existing Performance Tracking System (PTS) dashboard, promoting data accuracy, security, and informed decision-making.
  
- **Database Structure:** Adopting a code-first development, ePTS provides a comprehensive structure:

    ```
    Organization
    ├── Application User
    ├── School
    │   ├── Academic Years
    │   │   └── Gradebook
    │   │       ├── Gradebook Assessments
    │   │       │   └── Assessment Results
    │   │       │       ├── Ref Performance Level
    │   │       │       └── Ref Sex
    │   │       ├── Gradebook Enrollment
    │   │       │   └── Ref Participant Type
    │   │       └── Gradebook Period
    │   │           └── Gradebook Assessment Period
    │   │               ├── Ref Assessment Term
    │   │               └── Ref Assessment Week
    │   ├── Ref School Type
    │   ├── Ref School Location
    │   ├── Ref School Administration Type
    │   ├── Ref School Language
    │   └── Ref School Status
    ├── Ref Organization Type
    └── Ref Location
        ├── Ref Location Type
        └── Ref Location
    ```

- **Technology Stack:**
  - **Frontend:** Utilizes ASP.NET Core 7 MVC, enabling dynamic and responsive web pages.
  - **Backend:** Operated by ASP.NET Core, ensuring secure, efficient, and reliable operations.
  - **Database:** Uses Entity Framework Core, designed for MS SQL. This leading ORM for .NET simplifies data access but can also be adapted to work with other databases.

- **Open Source:** ePTS promotes transparency, collaboration, and community-driven development. The source code is freely available for all.

- **User Levels:** Diverse access levels to cater to different user roles:
  - **Ministry of Education (MoE)**: View all data.
  - **Province**: Oversee specific province data.
  - **District**: Access all schools and data under that district.
  - **Zone**: Regional access.
  - **School**: Restricted to their school's data.

---

## Download & Installation

Follow these steps to get ePTS up and running:

1. **Download the Release:**
   - Navigate to the "Releases" section of the repository.
   - Download the latest release which includes the compiled web application and accompanying SQL files.

2. **Database Setup:**
   - Open your MS SQL Server Management Studio or equivalent tool.
   - Run the provided SQL file to create the necessary database and tables.

3. **Web Application Deployment:**
   - Unzip the downloaded web application package.
   - Deploy the application to your preferred web server, ensuring it supports ASP.NET Core 7.
   - Configure the application to connect to the database you set up in the previous step.

4. **Start Using ePTS:**
   - Navigate to the application URL after deployment.
   - Log in or register, and begin tracking school performance effectively!

Note: Always refer to the official documentation and ensure your server meets the necessary requirements for ASP.NET Core 7 applications.

---

## Requirements

- An active internet connection. Note: ePTS is lightweight and primarily text-based, ensuring minimal bandwidth consumption.

## Conclusion

ePTS is a strategic tool for schools, offering an integrated system for effective performance tracking across devices. By simplifying complex data processes, it aids educators and administrators in deriving valuable insights and facilitating informed decisions.

For further queries, please contact [epts@edc.org](mailto:epts@edc.org).

**Join our community and contribute to the ePTS project!**

# License
[![License: AGPL v3](https://img.shields.io/badge/License-AGPL%20v3-blue.svg)](LICENSE)

This program is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License along with this program. If not, see <https://www.gnu.org/licenses/>.


Copyright (C) 2023 Education Development Center, Inc.

---

<img src="Docs/images/EDCLogo.png" width=120px></p>
Education Development Center (EDC) is a global nonprofit that advances lasting solutions to improve education, promote health, and expand economic opportunity. Since 1958, we have been a leader in designing, implementing, and evaluating powerful and innovative programs in more than 80 countries around the world.
300 Fifth Avenue, Suite 2010
Waltham, MA 02451
Phone: 617-969-7100

_This ePTS software is made possible by the support of the American People through the United States Agency for International Development (USAID)._
