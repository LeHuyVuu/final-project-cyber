﻿namespace cybersoft_final_project.Models.Request;

public class OrderDetailRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}