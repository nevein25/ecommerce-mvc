﻿using Microsoft.CodeAnalysis;
using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;

namespace OnlineShoppingApp.Repositories.Classes
{
	public class RateRepo : IRateRepo
	{
		ShoppingContext _context { get; }
		public RateRepo(ShoppingContext context)
		{
			_context = context;
		}

		public bool ProductExist(int productId)
		{
			return _context.Products.Any(p => p.Id == productId);
		}
		public void Rate(int ProductId, int UserId, int NumOfStars)
		{
			var rate = _context.Rate.Find(ProductId, UserId);
			if (rate != null)
			{
				if (rate.NumOfStars == NumOfStars)
				{
					_context.Rate.Remove(rate);
					_context.SaveChanges();
					return;
				}
				_context.Rate.Remove(rate);
			//	_context.SaveChanges();
			}

			if (NumOfStars >= 1 && NumOfStars <= 5)
			{

				rate = new Rate
				{
					BuyerId = UserId,
					ProductId = ProductId,
					NumOfStars = NumOfStars
				};

				_context.Rate.Add(rate);
                _context.SaveChanges();

            }
        }
	}
}
