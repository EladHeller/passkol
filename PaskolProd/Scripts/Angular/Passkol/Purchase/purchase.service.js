(function () {
    'use strict';

    angular
        .module('passkolApp')
        .factory('purchaseService', purchaseService);

    purchaseService.$inject = ['$http'];

    function purchaseService($http) {
        var service = {
            getPermissionCategories: getPermissionCategories,
            permissionCategoryDetails: permissionCategoryDetails,
            getPurchaseUrl: getPurchaseUrl,
            getPermissionCost: getPermissionCost,
            phonePurchase: phonePurchase,
            getCustomerReports: getCustomerReports,
            getArtistReports: getArtistReports,
            getArtistAgntReports: getArtistAgntReports
        };

        return service;

        function getPurchaseUrl(req,v) {
            return $http({
                method: 'POST',
                url: '/Transact/GetUrl',
                data: req,
                headers: {
                    'RequestVerificationToken': v,
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });
        }

        function phonePurchase(req, v) {
            return $http({
                method: 'POST',
                url: '/Purchase/PhonePurchase',
                data: req,
                headers: {
                    'RequestVerificationToken': v,
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });
        }

        function getPermissionCost(req, v) {
            return $http({
                method: 'POST',
                url: '/Purchase/CheckPermissionCost',
                data: req,
                headers: {
                    'RequestVerificationToken': v,
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });
        }

        function getCustomerReports(req, v) {
            return $http({
                method: 'POST',
                url: '/Purchase/GetCustomerReports',
                data: req,
                headers: {
                    'RequestVerificationToken': v,
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });
        }

        function getArtistAgntReports(req, v) {
            return $http({
                method: 'POST',
                url: '/Purchase/GetArtistAgntReports',
                data: req,
                headers: {
                    'RequestVerificationToken': v,
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });
        }

        function getArtistReports(req, v) {
            return $http({
                method: 'POST',
                url: '/Purchase/GetArtistReports',
                data: req,
                headers: {
                    'RequestVerificationToken': v,
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });
        }

        function getPermissionCategories() { 
            return $http.post('/Purchase/PermissionsCategory');
        }

        function permissionCategoryDetails(categoryId) {
            return $http.post('/Purchase/PermissionCategoryDetails?id=' + categoryId);
        }
    }
})();