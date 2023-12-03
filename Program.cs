using System;
using System.Collections.Generic;

// Інтерфейс для товару
public interface IProduct
{
    string Name { get; set; }
    double Price { get; set; }
    double CalculateDiscount();
    double CalculateCost();
}

// Абстрактний клас для товару
public abstract class Product : IProduct
{
    public string Name { get; set; }
    public double Price { get; set; }

    public abstract double CalculateCost();

    public virtual double CalculateDiscount()
    {
        // Загальна логіка для обчислення знижки
        return 0.1 * Price;
    }
}

// Класи, що успадковуються від Product
public class Book : Product
{
    public int PageCount { get; set; }

    public override double CalculateCost()
    {
        // Унікальна логіка для обчислення вартості книги
        return Price * (1 - CalculateDiscount());
    }
}

public class Electronics : Product
{
    public string MemorySize { get; set; }

    public override double CalculateCost()
    {
        // Унікальна логіка для обчислення вартості електроніки
        return Price * (1 - CalculateDiscount());
    }
}

public class Clothing : Product
{
    public string Size { get; set; }

    public override double CalculateCost()
    {
        // Унікальна логіка для обчислення вартості одягу
        return Price * (1 - CalculateDiscount());
    }
}

// Клас Order
public class Order
{
    public int OrderNumber { get; set; }
    public List<IProduct> Products { get; set; } = new List<IProduct>();
    public double TotalCost { get; private set; }
    public string OrderStatus { get; private set; }

    public delegate void OrderStatusChangeHandler(string newStatus);
    public event OrderStatusChangeHandler OrderStatusChanged;

    public void ProcessOrder()
    {
        // Логіка обробки замовлення
        CalculateTotalCost();
        ChangeOrderStatus("Processed");
    }

    private void CalculateTotalCost()
    {
        // Обчислення загальної вартості замовлення
        TotalCost = 0;
        foreach (var product in Products)
        {
            TotalCost += product.CalculateCost();
        }
    }

    private void ChangeOrderStatus(string newStatus)
    {
        // Зміна статусу замовлення та сповіщення про зміну статусу
        OrderStatus = newStatus;
        OrderStatusChanged?.Invoke(newStatus);
    }
}

// Клас OrderProcessor
public class OrderProcessor
{
    public void ProcessOrder(Order order)
    {
        // Обробка замовлення
        order.ProcessOrder();
    }
}

// Клас NotificationService
public class NotificationService
{
    public void SendNotification(string message)
    {
        // Логіка відправлення сповіщення
        Console.WriteLine($"Notification: {message}");
    }
}

// Точка входу
class Program
{
    static void Main()
    {
        // Створення об'єктів товарів
        Book book = new Book { Name = "The Great Gatsby", Price = 20.0, PageCount = 300 };
        Electronics electronics = new Electronics { Name = "Smartphone", Price = 500.0, MemorySize = "128GB" };
        Clothing clothing = new Clothing { Name = "T-shirt", Price = 15.0, Size = "M" };

        // Створення колекції товарів
        List<IProduct> products = new List<IProduct> { book, electronics, clothing };

        // Створення замовлення
        Order order = new Order { OrderNumber = 1, Products = products };

        // Створення служб для обробки замовлення та сповіщення
        OrderProcessor orderProcessor = new OrderProcessor();
        NotificationService notificationService = new NotificationService();

        // Підписка на подію зміни статусу замовлення
        order.OrderStatusChanged += notificationService.SendNotification;

        // Обробка та виведення інформації про замовлення
        orderProcessor.ProcessOrder(order);
        Console.WriteLine($"Order Status: {order.OrderStatus}, Total Cost: ${order.TotalCost}");
    }
}
