using Nocrastination.Core.Entities;
using Nocrastination.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Nocrastination.Data;
using Nocrastination.Models;
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

        public IEnumerable<Purchase> GetItems(string childId)
        {
            return _purchaseRepo.Get(x => x.ChildId == childId);
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
                        Data = new []{ _purchaseRepo.Add(new Purchase()
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
