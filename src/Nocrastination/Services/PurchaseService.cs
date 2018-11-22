using Nocrastination.Core.Entities;
using Nocrastination.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private IRepository<Tasks> _tasksRepo;
        private IStoreService _storeSrv;
        private IConfiguration _cfg;

        public PurchaseService
            (IRepository<Purchase> purchaseRepo,
            IRepository<Tasks> tasksRepo,
            IStoreService storeSrv,
            IConfiguration cfg)
        {
            _purchaseRepo = purchaseRepo;
            _tasksRepo = tasksRepo;
            _storeSrv = storeSrv;
            _cfg = cfg;
        }

        public UserStatusDTO GetStatus(string childId)
        {
            var selectedItem = GetItems(childId).FirstOrDefault(x => x.IsSelected == true);
            var score = GetChildsEarnedPoints(childId) - GetChildsSpentPoints(childId);
            var item = _storeSrv.GetAllItemsInStore().FirstOrDefault(x => x.Id == selectedItem.Id);

            return new UserStatusDTO()
            {
                Score = score,
                ItemName = item.Name,
                ItemImageURL = item.Picture
            };
        }

        public IEnumerable<Purchase> GetItems(string childId)
        {
            return _purchaseRepo.Get(x => x.ChildId == childId);
        }

        public OperationResult SelectItem(string childId, string itemId)
        {
            var isItemId = Guid.TryParse(itemId, out var id);

            if (isItemId)
            {
                var item = _purchaseRepo.Get(x => x.ChildId == childId &&
                                                  x.ItemId == id).FirstOrDefault();

                if (item != null)
                {
                    var chosenItem = _purchaseRepo.Get(x => x.ChildId == childId &&
                                                            x.IsSelected == true).FirstOrDefault();

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
                        Messages = new[] { "You don`t have enough points." },
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
            var result = _tasksRepo.Data.Where(x => x.IsFinished == true &&
                                                    x.ChildId == childId)
                .Select(x => new
                {
                    Points = (x.EndDate - x.StartDate).Minutes * StoreSettings.PointsMultiplier
                })
                .Sum(x => x.Points);

            return (int)result;
        }

        private int GetChildsSpentPoints(string childId)
        {
            var result = GetItems(childId).GroupBy(x => x.Points)
                .Select(g => new
                {
                    Points = g.Key * g.Count()
                })
                .Sum(x => x.Points);

            return result;
        }
    }
}
