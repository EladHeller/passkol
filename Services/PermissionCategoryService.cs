using Model;
using Model.Helper;
using Model.PriceModel;
using Model.Purchase;
using Repository;
using Repository.Interfaces;
using Repository.UnitOfWork;
using Services.Messaging.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PermissionCategoryService : GenericServiceUOW<PermissionsCategory, Guid>
    {
        private PermissionService _permissionService;
        private MusicsRepository _mscRep;

        public PermissionCategoryService(IPermmsionCategoryRepository rep, IUnitOfWork uow, PermissionService prmService, MusicsRepository mscRep) : base(rep, uow)
        {
            _permissionService = prmService;
            _mscRep = mscRep;
        }

        public EntityResponse<Permission> GetOrAdd(IEnumerable<Guid> valueIds, Guid catId)
        {
            var res = new EntityResponse<Permission>();

            try
            {
                var categoryRes = GetByID(catId, PermissionNavigationOptions.IncludeAll);
                IEnumerable<PermissionPropertyValue> dbValues = null;
                if (categoryRes.Success)
                {
                    dbValues = categoryRes.Entity?.PermissionProperties?.SelectMany(prop => prop.PropertyValues);

                    res.Entity = dbValues?.SelectMany(val => val.PermissionPrices)
                                    ?.FirstOrDefault(perm =>
                                            perm.PropertyValues.Count == valueIds.Count() &&
                                            perm.PropertyValues.All(dbVal => valueIds.Any(val => val == dbVal.ID)));
                    res.Success = res.Entity != null;
                }
                else
                {
                    res.Success = false;
                    res.Message = categoryRes.Message;
                }

                if (res.Entity == null)
                {
                    var newPerm = new Permission(catId, null)
                    {
                        PropertyValues = (from valId in valueIds
                                          select dbValues.First(dbVal => dbVal.ID == valId)).ToList()
                    };
                    var addRes = _permissionService.Add(newPerm);
                    res.Success = addRes.Success;
                    res.Message = addRes.Message;
                    if (addRes.Success)
                    {
                        res.Entity = newPerm;
                    }
                }
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = "נכשלה פעולת שליפת או הוספת רישיון";
            }
            return res;
        }

        public EntityResponse<PurchaseDetails> CheckPermissionCost(IEnumerable<Guid> valueIds, int musicId, Guid categoryId, bool createPermissionIfNotExist =false)
        {
            var res = new EntityResponse<PurchaseDetails>();
            res.Success = true;
            res.Entity = new PurchaseDetails();
            res.Entity.Cost = 0;

            try
            {
                var music = _mscRep.GetByID(musicId);
                var getCatRes = GetByID(categoryId, PermissionNavigationOptions.IncludeAll);
                PermissionComparer permissionComperer = new PermissionComparer();
                res.Success = getCatRes.Success;
                if (getCatRes.Success)
                {
                    var properties = getCatRes.Entity.PermissionProperties.Where(prop => prop.IsActive);
                    var permissions = properties
                            .SelectMany(prop => prop.PropertyValues.Where(val => val.IsActive)
                                .SelectMany(val => val.PermissionPrices))
                                    .Distinct(permissionComperer);
                    var costProp = properties.Where(prop => prop.IsCostLevel);
                    var nonCostProp = properties.Except(costProp);
                    var nonCostValues = 
                        nonCostProp.Select(prop =>
                        prop.PropertyValues
                            .Single(val => val.IsActive && 
                                valueIds.Any(valId => valId == val.ID)));
                    var costValues = GetCostLevelValuesFromMusic(music, costProp);
                    if (costValues.Any())
                    {
                        var allValues = costValues.Select(costVal => costVal.ID).Union(valueIds);
                        var permission =
                            permissions.FirstOrDefault(perm =>
                            allValues.All(valId =>
                                perm.PropertyValues.Any(permVal => valId == permVal.ID)));

                        var cost = permission?.PermissionCost;
                        res.Entity.Cost = cost.HasValue ? cost.Value : 0;
                        res.Entity.Music = music;
                        res.Entity.PermissionValueNames = nonCostValues.Select(val => val.Name);
                        res.Entity.Permission = permission;
                        res.Entity.IsExistPermission = permission == null && !createPermissionIfNotExist;
                        if (res.Entity.Permission == null)
                        {
                            if (createPermissionIfNotExist)
                            {
                                var getOrAddRes = GetOrAdd(costValues.Union(nonCostValues).Select(val => val.ID), categoryId);
                                res.Success = getOrAddRes.Success;
                                res.Entity.Permission = getOrAddRes.Entity;
                            }
                            else
                            {
                                res.Entity.Permission = new Permission(categoryId, null);
                                res.Entity.Permission.PropertyValues =
                                    costValues.Union(nonCostValues).ToList();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                res.Message = "אירעה שגיאה בעת הבאת מחיר הרשיון";
                res.Success = false;
            }

            return res;
        }

        private List<PermissionPropertyValue> GetCostLevelValuesFromMusic(Music music, IEnumerable<PermissionProperty> costProp)
        {
            List<PermissionPropertyValue> res = new List<PermissionPropertyValue>();
            var artistLevelProp = costProp.FirstOrDefault(prop => prop.Name == "רמת אמן");
            var musicLevelProp = costProp.FirstOrDefault(prop => prop.Name == "רמת יצירה");
            if (artistLevelProp != null && musicLevelProp != null)
            {
                var artistLevelVal = artistLevelProp.PropertyValues.FirstOrDefault(val => val.Name == music.CostLevel.ToDescription());
                var musicLevelVal = musicLevelProp.PropertyValues.FirstOrDefault(val => val.Name == music.Artist.PriceLevel.ToDescription());
                if (artistLevelVal != null && musicLevelVal != null)
                {
                    res.Add(artistLevelVal);
                    res.Add(musicLevelVal);
                }
            }

            return res;
        }

        public EntityAllResponse<PermissionsCategory> GetAll(PermissionNavigationOptions options)
        {
            EntityAllResponse<PermissionsCategory> res = 
                new EntityAllResponse<PermissionsCategory>();
            res.Success = false;
            try
            {
                res.Entities = (_rep as IPermmsionCategoryRepository).GetAll(options);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Message = e.ToString();
            }

            return res;
        }

        public EntityAllResponse<KeyValuePair<Guid, string>> CategoriesAsKeyValue()
        {
            EntityAllResponse<KeyValuePair<Guid, string>> res
                = new EntityAllResponse<KeyValuePair<Guid, string>>();

            try
            {
                res.Entities = this.GetAll()
                    .Entities.Select(p => new KeyValuePair<Guid, string>(p.ID, p.Name));
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Success = false;
            }

            return res;
        }

        public EntityResponse<PermissionsCategory> GetWithPermissionList(Guid id)
        {
            EntityResponse<PermissionsCategory> res = 
                GetByID(id, PermissionNavigationOptions.IncludeAll);
            try
            {
                var permissionComparer = new PermissionComparer();
                if (res.Success)
                {
                    var category = res.Entity;
                    var combinRes = GetAllPermissionsCombinations(category);

                    if (combinRes.Success)
                    {
                        var currPerms = category.PermissionProperties
                        .SelectMany(prop => prop.PropertyValues
                            .SelectMany(val => val.PermissionPrices))
                                .Distinct(permissionComparer);
                        //.Where(perm => category.PermissionProperties
                        //    .All(prop => perm.PropertyValues
                        //        .Any(val => val.PermissionsCategoryID == prop.ID)))
                        //            .ToList();
                        res.Entity.Permissions = combinRes.Entities.ToList();
                        for (int i = 0; i < res.Entity.Permissions.Count; i++)
                        {
                            var perm = res.Entity.Permissions[i];
                            var existPerm =
                                currPerms.FirstOrDefault(currPerm =>
                                perm.PropertyValues.All(val =>
                                    currPerm.PropertyValues.Any(currVal => currVal.ID == val.ID)));
                            if (existPerm != null)
                            {
                                perm.ID = existPerm.ID;
                                perm.PermissionCategoryID = existPerm.PermissionCategoryID;
                                perm.PermissionCost = existPerm.PermissionCost;
                            }
                        }
                        
                        res.Success = true;
                    }
                    else
                    {
                        res.Success = false;
                        res.Message = combinRes.Message;
                    }
                }
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.ToString();
            }

            return res;
        }

        public EntityAllResponse<Permission> GetAllPermissionsCombinations
            (PermissionsCategory category, bool getCostValue = true)
        {
            EntityAllResponse<Permission> res = new EntityAllResponse<Permission>();
            try
            {
                var propValuesLst = (from prop in category.PermissionProperties
                                     orderby prop.ViewOrder
                                     where getCostValue || !prop.IsCostLevel
                                     select prop.PropertyValues);

                List<List<PermissionPropertyValue>> valCombinationLst = 
                    new List<List<PermissionPropertyValue>>() { new List<PermissionPropertyValue>() };

                foreach (var propValues in propValuesLst)
                {
                    valCombinationLst = propValues.SelectMany(val => valCombinationLst.Select(valLst =>
                    {
                        List<PermissionPropertyValue> lst = new List<PermissionPropertyValue>(valLst);
                        lst.Add(val);

                        return lst;
                    })).ToList();
                }

                res.Entities = valCombinationLst.Select(valLst => 
                    new Permission(category.ID, null) { PropertyValues= valLst });
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.ToString();
            }
            return res;
        }
        
        public EntityResponse<PermissionsCategory> GetByID
            (Guid id, PermissionNavigationOptions options)
        {
            EntityResponse<PermissionsCategory> res = new EntityResponse<PermissionsCategory>();
            res.Success = false;
            try
            {
                res.Entity = (_rep as IPermmsionCategoryRepository).GetByID(id, options);
                res.Success = true;
            }
            catch (Exception e)
            {
                res.Message = e.ToString();
            }

            return res;
        }

        public override ResponseBase Update(PermissionsCategory item)
        {
            var getRes = this.GetByID(item.ID);
            if (!getRes.Success)
            {
                return getRes;
            }

            var dbItem = getRes.Entity;
            dbItem.IsActive = item.IsActive;
            dbItem.Name = item.Name;
            dbItem.ViewOrder= item.ViewOrder;

            return base.Update(dbItem);
        }

        public ResponseBase UpdatePermissions(PermissionsCategory category)
        {
            ResponseBase res = new ResponseBase();

            try
            {
                var getRes = GetByID(category.ID, PermissionNavigationOptions.IncludeAll);
                PermissionComparer permissionComparer = new PermissionComparer();
                List<Permission> lstAdd = new List<Permission>();
                if (getRes.Success)
                {
                    var dbVals = getRes.Entity.PermissionProperties
                        .SelectMany(prop => prop.PropertyValues);
                    var dbPerms = dbVals
                                .SelectMany(val => val.PermissionPrices)
                                .Distinct(permissionComparer);
                    foreach (var perm in category.Permissions)
                    {
                        var dbPerm = dbPerms.FirstOrDefault(currPerm => currPerm.ID == perm.ID);
                        if (dbPerm == null)
                        {
                            if (perm.PermissionCost.HasValue)
                            {
                                perm.PropertyValues =
                                perm.PropertyValues.Select(val => dbVals.First(dbVal => dbVal.ID == val.ID)).ToList();
                                lstAdd.Add(perm);
                            }
                        }
                        else
                        {
                            dbPerm.PermissionCost = perm.PermissionCost;
                        }
                    }

                    res = _permissionService.UpdateAndAddList(dbPerms, lstAdd);
                }
                else
                {
                    res = getRes;
                }
            }
            catch (Exception e)
            {
                res.Success = false;
                res.Message = e.ToString();
            }

            return res;
        }
    }

    public class PermissionComparer : IEqualityComparer<Permission>
    {
        public bool Equals(Permission x, Permission y)
        {
            return x.ID == y.ID;
        }

        public int GetHashCode(Permission obj)
        {
            return obj.GetHashCode();
        }
    }
}