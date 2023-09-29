# E-commerce Website

Welcome to our e-commerce website! This project is designed to provide a simple e-commerce platform with features like product listing, shopping cart, user authentication, and order creation.

## Table of Contents

- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)

## Features

- Browse and search for products.
- Add products to your shopping cart.
- View and manage items in your shopping cart.
- User authentication for registered users.
- Place orders.
- Integration with a payment gateway (e.g., Stripe).

## Getting Started

### Prerequisites

Before you begin, ensure you have met the following requirements:

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0) installed on your machine.
- A code editor (e.g., Visual Studio, Visual Studio Code).
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)

### Installation

1. Clone this repository:

   ```bash
   git clone https://github.com/yourusername/e-commerce.git
   
2. Navigate to the project directory (yes twice):
   
   ```bash
   cd e-commerce
   cd e-commerce

3. Create or edit the appsettings.json file in the project root and configure your MySQL connection string and StripSettings. Replace <your_connection_string> with your MySQL database connection details and your publickey and secretke from your stripe account:
    ```bash
    {
        "StripeSettings": {
          "PublishableKey": "your_publickey",
          "SecretKey": "your_secretkey"
        },
       "ConnectionStrings": {
           "TodoContext": "server=localhost;port=3306;database=todo;user=root;password=<your_password>"
       },
       // ...
   }

4. Open a terminal or command prompt and run the following commands Restore the project dependencies::
   ```bash
     dotnet restore

5. Open a terminal or command prompt and run the following commands to apply database migrations:
   ```bash
     dotnet ef database update

## Usage
1. Start the application:
   ```bash
   dotnet run

2. Open your web browser and navigate to http://localhost:5087 to access the E-commerce.
3 Visit the website and create an account or log in if you already have one.
4 Browse the product catalog, add items to your shopping cart, and proceed to checkout.
5 Complete the checkout process, including payment if configured. Use this test card to simulate a payment (4242 4242 4242 4242), the other data could be random.
6 Click pay, and yei you made an order.

## short feedback
- was it easy to complete the task using AI?:
In some cases, using the AI was really useful to help generate code and configuration, but sometimes the steps that the AI gave me weren't complete, what I mean is that chatgpt forgot to tell me some previous configuration or packages that I needed to have previously installed.
- How long did task take you to complete?
around 40 hours
- Was the code ready to run after generation? What did you have to change to make it usable?
Most of the time, the code provided wasn't really functional, for example, the unit tests I had to change a lot to make them work.
- Which challenges did you face during completion of the task?
