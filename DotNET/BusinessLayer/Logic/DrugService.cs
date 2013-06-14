﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharmacy.BusinessLayer.Data;

namespace Pharmacy.BusinessLayer.Logic
{
    public static class DrugService
    {
        public static Drug CreateDrug(int pzn, string name, string description)
        {
            Util.ConvertEmptyToNull(ref description);

            Drug newDrug = new Drug
            {
                PZN = pzn,
                Name = name,
                Description = description
            };
            return CreateDrug(newDrug);
        }

        private static Drug CreateDrug(Drug newDrug)
        {
            using (PharmacyContainer db = new PharmacyContainer())
            {
                var count = (from d in db.DrugSet where d.PZN == newDrug.PZN select d).Count();
                if (count > 0)
                    throw new ArgumentException(String.Format("Drug with PZN {0} already exists!", newDrug.PZN));
                db.DrugSet.Add(newDrug);
                db.SaveChanges();
                return newDrug;
            }
        }

        public static ICollection<Drug> GetAllDrugs()
        {
            using (PharmacyContainer db = new PharmacyContainer())
                return db.DrugSet.ToList();
        }

        public static Drug GetDrug(Int32 pzn)
        {
            using (PharmacyContainer db = new PharmacyContainer())
            {
                return GetDrug(pzn, db);
            }
        }

        private static Drug GetDrug(Int32 pzn, PharmacyContainer db)
        {
            Drug result = (from d in db.DrugSet where d.PZN == pzn select d).FirstOrDefault();

            if (result == default(Drug))
                throw new ArgumentException(String.Format("Drug with PZN {0} not found", pzn.ToString()));

            return result;
        }

        public static Drug UpdateDrug(Drug drug)
        {
            return UpdateDrug(drug, drug.Name, drug.Description, drug.MinimumInventoryLevel, drug.OptimalInventoryLevel);
        }

        public static Drug UpdateDrug(Drug drug, String name, String description, int minimumInventoryLevel, int optimalInventoryLevel)
        {
            Util.ConvertEmptyToNull(ref description);

            using (PharmacyContainer db = new PharmacyContainer())
            {
                Drug attachedDrug = GetDrug(drug.PZN, db);
                attachedDrug.Name = name;
                attachedDrug.Description = description;
                attachedDrug.MinimumInventoryLevel = minimumInventoryLevel;
                attachedDrug.OptimalInventoryLevel = optimalInventoryLevel;
                db.SaveChanges();
                return attachedDrug;
            }
        }

        public static void Withdraw(Int32 pzn, Int32 quantity, DateTime dateOfAction)
        {
            using (PharmacyContainer db = new PharmacyContainer())
            {
                Drug drug = GetDrug(pzn, db);
                drug.Apply(WithdrawEvent.Create(drug, quantity, dateOfAction));
                db.SaveChanges();
            }
        }

        public static void Restock(Int32 pzn, Int32 quantity, DateTime dateOfAction)
        {
            using (PharmacyContainer db = new PharmacyContainer())
            {
                Drug drug = GetDrug(pzn, db);
                drug.Apply(RestockEvent.Create(drug, quantity, dateOfAction));
                db.SaveChanges();
            }
        }

        public static void Replenish(Int32 pzn, Int32 quantity, DateTime dateOfAction)
        {
            using (PharmacyContainer db = new PharmacyContainer())
            {
                Drug drug = GetDrug(pzn, db);
                drug.Apply(ReplenishEvent.Create(drug, quantity, dateOfAction));
                db.SaveChanges();
            }
        }

        public static void InitiateReplenishment(int pzn, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
