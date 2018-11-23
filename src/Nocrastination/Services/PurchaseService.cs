using Nocrastination.Core.Entities;
using Nocrastination.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nocrastination.Data;
using Nocrastination.Models;
using Nocrastination.Models.DTO;
using Nocrastination.Settings;

namespace Nocrastination.Services
{
	public class PurchaseService : IPurchaseService
	{
		private IRepository<Purchase> _purchaseRepo;
		private IRepository<StoreItem> _storeRepo;
		private IRepository<ChildTask> _tasksRepo;
		private IStoreService _storeSrv;
		private IConfiguration _cfg;

		public PurchaseService
			(IRepository<Purchase> purchaseRepo,
			IRepository<ChildTask> tasksRepo,
			IStoreService storeSrv,
			IRepository<StoreItem> storeRepo,
			IConfiguration cfg)
		{
			_purchaseRepo = purchaseRepo;
			_tasksRepo = tasksRepo;
			_storeRepo = storeRepo;
			_storeSrv = storeSrv;
			_cfg = cfg;
		}

		public UserStatusDTO GetStatus(string childId)
		{
			var selectedItem = GetPurchases(childId).FirstOrDefault(x => x.IsSelected);
			var score = GetChildsEarnedPoints(childId) - GetChildsSpentPoints(childId);
			var item = _storeSrv.GetAllItemsInStore().FirstOrDefault(x => x.Id == selectedItem.ItemId);

			if (item == null)
			{
				item = new StoreItem()
				{
					Name = "default",
					Picture = "/store-items/item0.png"
				};
			}

			return new UserStatusDTO()
			{
				Score = score,
				ItemName = item.Name,
				ItemImageUrl = item.Picture,
				PurchaseId = selectedItem.Id
			};
		}

		public IEnumerable<Purchase> GetPurchases(string childId)
		{
			return _purchaseRepo.Get(x => x.ChildId == childId);
		}

		public IEnumerable<OutfitDTO> GetItems(string childId)
		{
			return _purchaseRepo.Data.Where(x => x.ChildId == childId).GroupJoin(_storeRepo.Data, x => x.ItemId, y => y.Id, (x, y) => new
			{
				x,
				y,
			}).SelectMany(x => x.y.DefaultIfEmpty(),
			(x, y) => new OutfitDTO()
			{
				Name = y != null ? y.Name : "Default",
				ImageUrl = y != null ? y.Picture : "/store-items/item0.png",
				Points = x.x.Points,
				IsSelected = x.x.IsSelected,
				PurchaseId = x.x.Id
			});
		}

		public OperationResult SelectItem(string childId, string purchaseId)
		{
			var isItemId = Guid.TryParse(purchaseId, out var id);

			if (isItemId)
			{
				var item = _purchaseRepo.Get(x => x.ChildId == childId &&
												  x.Id == id).FirstOrDefault();

				if (item != null)
				{
					var chosenItem = _purchaseRepo.Get(x => x.ChildId == childId &&
															x.IsSelected).FirstOrDefault();

					item.IsSelected = true;

					_purchaseRepo.Update(item);

					if (chosenItem != null)
					{
						chosenItem.IsSelected = false;

						_purchaseRepo.Update(chosenItem);
					}

					return new OperationResult()
					{
						Success = true,
						Messages = new[] { "Selected item was successfully changed." }
					};
				}
			}

			return new OperationResult()
			{
				Messages = new[] { "Item ID is invalid." }
			};
		}

		public OperationResult<Purchase> BuyItem(string childId, string itemId)
		{
			if (_storeSrv.IsStoreItemExists(itemId, out var item))
			{
				var earnedPoints = GetChildsEarnedPoints(childId);
				var spentPoints = GetChildsSpentPoints(childId);

				var availablePoints = earnedPoints - spentPoints;

				if (availablePoints > item.Points)
				{
					return new OperationResult<Purchase>()
					{
						Success = true,
						Messages = new[] { "Item is yours." },
						Data = new[]{ _purchaseRepo.Add(new Purchase()
						{
							ChildId = childId,
							ItemId = item.Id,
							Points = item.Points,
						})}
					};
				}

				return new OperationResult<Purchase>()
				{
					Messages = new[] { "You don`t have enough points." }
				};
			}

			return new OperationResult<Purchase>()
			{
				Messages = new[] { "Item doesn`t exist." }
			};
		}

		private int GetChildsEarnedPoints(string childId)
		{
			var result = _tasksRepo.Data.Where(x => x.IsFinished && x.ChildId == childId)
				.Select(x => new
				{
					Points = (x.EndDate - x.StartDate).TotalMinutes * StoreSettings.PointsMultiplier
				})
				.Sum(x => x.Points);

			return (int)result;
		}

		private int GetChildsSpentPoints(string childId)
		{
			var result = GetPurchases(childId).GroupBy(x => x.Points)
				.Select(g => new
				{
					Points = g.Key * g.Count()
				})
				.Sum(x => x.Points);

			return result;
		}

		public OperationResult SetInitialItem(string childId)
		{
			_purchaseRepo.Add(new Purchase()
			{
				ChildId = childId,
				ItemId = Guid.Empty,
				Points = 0,
				IsSelected = true
			});

			return new OperationResult()
			{
				Success = true,
			};
		}
	}
}
