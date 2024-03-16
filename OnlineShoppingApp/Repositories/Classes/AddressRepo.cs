﻿using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Context;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Interfaces;

namespace OnlineShoppingApp.Repositories.Classes
{
	public class AddressRepo : IAddressRepo
	{
		private readonly ShoppingContext _shoppingContext;

		public AddressRepo(ShoppingContext shoppingContext)
		{
			_shoppingContext = shoppingContext;
		}
		public void Delete(int id)
		{
			var removeAdrs = _shoppingContext.Addresses.Find(id);
			if (removeAdrs != null)
			{
				_shoppingContext.Addresses.Remove(removeAdrs);
				_shoppingContext.SaveChanges();
			}
		}

		public Address GetAddressById(int id)
		{
			return _shoppingContext.Addresses.Find(id);
		}

		public List<Address> GetAddresses()
		{
			return _shoppingContext.Addresses.Include(A => A.Buyer).ToList();
		}

		public void Insert(Address address)
		{
			if (address != null)
			{
                if (address.IsMain == 1)
                {
                    var oldMainAddress = _shoppingContext.Addresses.FirstOrDefault(a => a.IsMain == 1);
                    if (oldMainAddress != null)
                    {
                        oldMainAddress.IsMain = 0;
                    }
                }
                _shoppingContext.Addresses.Add(address);
				_shoppingContext.SaveChanges();
			}
		}

		public void Update(int id, Address address)
		{
			var adrs = _shoppingContext.Addresses.Find(id);
			if (adrs != null)
			{
				adrs.BuildingNumber = address.BuildingNumber;
				adrs.Street = address.Street;
				adrs.City = address.City;
				adrs.Country = address.Country;
				if (adrs.IsMain == 1)
				{
					var oldMainAddress = _shoppingContext.Addresses.FirstOrDefault(a => a.IsMain == 1);
					if (oldMainAddress != null)
					{
						oldMainAddress.IsMain = 0;
					}

				}
				adrs.IsMain = address.IsMain;
			}
		}

		public AppUser GetUser(int id)
		{
			return _shoppingContext.Users.Find(id);
		}
	}
}