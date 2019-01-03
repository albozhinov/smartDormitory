
# ASP.NET MVC Final Project

This document describes the **project assignment** for the **ASP.NET Core MVC** course at Telerik Academy.

## Project Description

Design and implement an **ASP.NET Core MVC application**. This application should utilize and extend the already existing business logic from the Databases course.

The application should have:

* **public part** (Must) (accessible without authentication)
* **private part** (Must) (available for registered users)
* **administrative part** (Should) (available for administrators only)

### Public Part

The **public part** of your projects should be **visible without authentication**.

This public part could be the application start page, the user login and user registration forms, as well as the public data of the users, e.g. the blog posts in a blog system, the public offers in a bid system, the products in an e-commerce system, etc.

### Private Part (Users only)

**Registered users** should have private part in the web application accessible after **successful login**.

This part could hold for example the user's profiles management functionality, the user's offers in a bid system, the user's posts in a blog system, the user's photos in a photo sharing system, the user's contacts in a social network, etc.

### Administration Part

**System administrators** should have administrative access to the system and permissions to administer all major information objects in the system, e.g. to create/edit/delete users and other administrators, to edit/delete offers in a bid system, to edit/delete photos and album in a photo sharing system, to edit/delete posts in a blogging system, edit/delete products and categories in an e-commerce system, etc.

## General Requirements

* Completely finished project is not obligatory required.
  * This team work project is for educational purposes
  * Always remember, quality over quantity!  

## Must Requirements (60 points)

* Must have **Public** and **Private** parts
* Must use **ASP.NET Core MVC 2.1**
* Must use **Razor** template engine for generating the UI
* Must use **MS SQL Server** as database back-end
  * Must use **Entity Framework Core** to access your database
* Must have pages with **tables with data** with **paging and sorting** for a model entity
  * You can use Kendo UI grid, jqGrid, any other library or generate your own HTML tables
* Must apply proper **data validation** (both client-side and server-side)
* Must apply proper **error handling** (both client-side and server-side)
* Write **unit tests** for the majority of your application's features. Unisolated tests are not considered valid.
* Follow the SOLID principles and the OOP principles. The lack of SRP or DI will be punished by death.

## Should Requirements (25 points)

* Should have **Administrative** part
  * Use the standard **ASP.NET Identity System** for managing users and roles
  * Your registered users should have at least one of the two roles: **user** and **administrator**
* Should use at least **1 area** in your project (e.g. for administration)
* Should use **caching** of data where it makes sense (e.g. starting page)

## Could Requirements (15 points)

* Chould have **usable and responsive UI**
  * You may use **Bootstrap** or **Materialize**
  * You may change the standard theme and modify it to apply own web design and visual styles

## Challenges

These extra requirements can give bonus points if everything in **Must** and **Should** is completed

* Research and use (simple) gitflow (master and development branches)
* Host your application in Azure (or any other public hosting provider)

## Project Defense

Each team member will have around 30 minutes to:

* Present the project overall
* Explain how they have contributed to the project
* Explain the source code of their teammates
* Answer some theoretical questions related to this course and all the previous ones.

## Give Feedback about Your Teammates

You will be invited to provide feedback about all your teammates, their attitude to this project, their technical skills, their team working skills, their contribution to the project, etc. **The feedback is important part of the project evaluation so take it seriously and be honest.**
